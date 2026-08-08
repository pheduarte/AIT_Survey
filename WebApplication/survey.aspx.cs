using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class survey : System.Web.UI.Page
    {

        private string ConnectionString =>
            ConfigurationManager
                .ConnectionStrings["devConnectionStr"]
                .ConnectionString;

        private const int EmailQuestionID = 8;
        private int CurrentQuestionIndex
        {
            get
            {
                return ViewState["CurrentQuestionIndex"] == null ? 0
                    : Convert.ToInt32(ViewState["CurrentQuestionIndex"]);
            }

            set
            {
                ViewState["CurrentQuestionIndex"] = value;
            }
        }

        private DataTable SurveyQuestions
        {
            get
            {
                return Session["SurveyQuestions"] as DataTable;
            }

            set
            {
                Session["SurveyQuestions"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["isMember"] == null)
            {
                Response.Redirect("register.aspx");
                return;
            }


            if (!IsPostBack)
            {
                SurveyQuestions = GetSurveyQuestions();

                SurveyDependencies = GetQuestionDependencies();

                CurrentQuestionIndex = 0;

                Session["SurveyAnswers"] =
                    new Dictionary<int, List<string>>();

                DisplayCurrentQuestion();
            }

        }


        private DataTable GetSurveyQuestions()
        {
            const string query = @"
        SELECT
            questionID,
            question_text,
            answer_type,
            is_required,
            min_selection,
            max_selection,
            display_order,
            is_main_question,
            field_key
        FROM question
        WHERE is_active = 1
          AND ISNULL(is_registration, 0) = 0
        ORDER BY display_order, questionID;";

            DataTable table = new DataTable();

            using (SqlConnection connection =
                   new SqlConnection(ConnectionString))
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                adapter.Fill(table);
            }

            return table;
        }


        private void DisplayCurrentQuestion()
        {
            lblError.Text = string.Empty;

            List<int> visibleQuestions =
                GetVisibleQuestionIds();

            RemoveAnswersForHiddenQuestions(
                visibleQuestions
            );

            if (visibleQuestions.Count == 0)
            {
                lblQuestion.Text =
                    "No questions are currently available.";

                DisableAnswerControls();

                return;
            }

            if (CurrentQuestionIndex >=
                visibleQuestions.Count)
            {
                FinishSurvey();
                return;
            }

            int questionID =
                visibleQuestions[
                    CurrentQuestionIndex
                ];

            DataRow question =
                GetQuestionRow(questionID);

            if (question == null)
            {
                lblError.Text =
                    "Question could not be found.";

                return;
            }

            string questionText =
                question["question_text"].ToString();

            string answerType =
                question["answer_type"]
                    .ToString()
                    .Trim()
                    .ToLower();

            bool isRequired = Convert.ToBoolean(question["is_required"]);

            hiddenQuestionID.Value = questionID.ToString();

            hiddenAnswerType.Value = answerType;

            lblQuestion.Text = questionText + (isRequired ? " *" : "");

            lblProgress.Text =
                $"Question {CurrentQuestionIndex + 1} " +
                $"of {visibleQuestions.Count}";

            HideAndClearAnswerControls();

            switch (answerType)
            {
                case "radio":
                    LoadRadioOptions(questionID);
                    rblAnswer.Visible = true;
                    break;

                case "check":
                case "checkbox":
                    LoadCheckboxOptions(questionID);
                    cblAnswer.Visible = true;
                    break;

                case "dropdown":
                    LoadDropdownOptions(questionID);
                    ddlAnswer.Visible = true;
                    break;

                case "text":
                    txtAnswer.Visible = true;
                    break;

                default:
                    lblError.Text = $"Unsupported answer type: {answerType}";
                    break;
            }

            btnPrevious.Visible = CurrentQuestionIndex > 0;

            bool isLastVisibleQuestion = CurrentQuestionIndex == visibleQuestions.Count - 1;

            bool canTriggerChild = QuestionCanTriggerChild(questionID);

            btnNext.Text = isLastVisibleQuestion && !canTriggerChild
                    ? "Finish"
                    : "Next";

            RestoreSavedAnswer(questionID, answerType);
        }

        private void RemoveAnswersForHiddenQuestions(List<int> visibleQuestionIDs)
        {
            HashSet<int> visible = new HashSet<int>(visibleQuestionIDs);

            List<int> savedQuestionIDs = new List<int>(SurveyAnswers.Keys);

            foreach (int questionID in savedQuestionIDs)
            {
                if (!visible.Contains(questionID))
                {
                    SurveyAnswers.Remove(questionID);
                }
            }

            Session["SurveyAnswers"] = SurveyAnswers;
        }

        private void HideAndClearAnswerControls()
        {
            rblAnswer.Visible = false;
            cblAnswer.Visible = false;
            ddlAnswer.Visible = false;
            txtAnswer.Visible = false;


            rblAnswer.Items.Clear();
            cblAnswer.Items.Clear();
            ddlAnswer.Items.Clear();

            txtAnswer.Text = string.Empty;

        }

        private void DisableAnswerControls()
        {
            rblAnswer.Visible = false;
            cblAnswer.Visible = false;
            ddlAnswer.Visible = false;
            txtAnswer.Visible = false;

            btnPrevious.Visible = false;
            btnNext.Visible = false;
        }

        private DataTable GetAnswerOptions(int questionID)
        {
            const string query = @"
                SELECT
                    answer_optionID,
                    option_text
                FROM answer_option
                WHERE questionID = @QuestionID
                ORDER BY [order];";

            DataTable table = new DataTable();

            using (SqlConnection connection =
                   new SqlConnection(ConnectionString))
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@QuestionID",
                    SqlDbType.Int
                ).Value = questionID;

                using (SqlDataAdapter adapter =
                       new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        private void LoadRadioOptions(int questionID)
        {
            rblAnswer.DataSource =
                GetAnswerOptions(questionID);

            rblAnswer.DataTextField = "option_text";
            rblAnswer.DataValueField = "answer_optionID";
            rblAnswer.DataBind();
        }

        private void LoadCheckboxOptions(int questionID)
        {
            cblAnswer.DataSource =
                GetAnswerOptions(questionID);

            cblAnswer.DataTextField = "option_text";
            cblAnswer.DataValueField = "answer_optionID";
            cblAnswer.DataBind();
        }

        private void LoadDropdownOptions(int questionID)
        {
            ddlAnswer.DataSource =
                GetAnswerOptions(questionID);

            ddlAnswer.DataTextField = "option_text";
            ddlAnswer.DataValueField = "answer_optionID";
            ddlAnswer.DataBind();

            ddlAnswer.Items.Insert(
                0,
                new ListItem("-- Select an answer --", "")
            );
        }


        private Dictionary<int, List<string>> SurveyAnswers
        {
            get
            {
                Dictionary<int, List<string>> answers = Session["SurveyAnswers"] as Dictionary<int, List<string>>;

                if (answers == null)
                {
                    answers = new Dictionary<int, List<string>>();

                    Session["SurveyAnswers"] = answers;
                }

                return answers;
            }
        }

        private bool SaveCurrentAnswer()
        {
            int questionID = Convert.ToInt32(hiddenQuestionID.Value);

            string answerType = hiddenAnswerType.Value;

            List<string> selectedAnswers = new List<string>();

            switch (answerType)
            {
                case "radio":
                    if (!string.IsNullOrWhiteSpace(
                        rblAnswer.SelectedValue))
                    {
                        selectedAnswers.Add(
                            rblAnswer.SelectedValue
                        );
                    }
                    break;

                case "dropdown":
                    if (!string.IsNullOrWhiteSpace(
                        ddlAnswer.SelectedValue))
                    {
                        selectedAnswers.Add(
                            ddlAnswer.SelectedValue
                        );
                    }
                    break;

                case "check":
                case "checkbox":
                    foreach (ListItem item in cblAnswer.Items)
                    {
                        if (item.Selected)
                        {
                            selectedAnswers.Add(item.Value);
                        }
                    }
                    break;

                case "text":
                    if (!string.IsNullOrWhiteSpace(
                        txtAnswer.Text))
                    {
                        selectedAnswers.Add(
                            txtAnswer.Text.Trim()
                        );
                    }
                    break;
            }

            if (!ValidateCurrentAnswer(selectedAnswers))
            {
                return false;
            }

            SurveyAnswers[questionID] = selectedAnswers;

            Session["SurveyAnswers"] = SurveyAnswers;

            return true;
        }


        private bool ValidateCurrentAnswer(List<string> selectedAnswers)
        {
            int questionID = Convert.ToInt32(hiddenQuestionID.Value);

            DataRow question =
                GetQuestionRow(questionID);

            if (question == null)
            {
                lblError.Text =
                    "Question information could not be found.";

                return false;
            }

            bool isRequired =
                Convert.ToBoolean(
                    question["is_required"]
                );

            int? minSelection =
                question["min_selection"] ==
                DBNull.Value
                    ? (int?)null
                    : Convert.ToInt32(
                        question["min_selection"]
                    );

            int? maxSelection =
                question["max_selection"] ==
                DBNull.Value
                    ? (int?)null
                    : Convert.ToInt32(
                        question["max_selection"]
                    );

            if (isRequired &&
                selectedAnswers.Count == 0)
            {
                lblError.Text =
                    "Please answer this question " +
                    "before continuing.";

                return false;
            }

            if (minSelection.HasValue &&
                selectedAnswers.Count <
                minSelection.Value)
            {
                lblError.Text =
                    $"Please select at least " +
                    $"{minSelection.Value} options.";

                return false;
            }

            if (maxSelection.HasValue &&
                selectedAnswers.Count >
                maxSelection.Value)
            {
                lblError.Text =
                    $"Please select no more than " +
                    $"{maxSelection.Value} options.";

                return false;
            }

            return true;
        }


        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (!SaveCurrentAnswer())
            {
                return;
            }

            CurrentQuestionIndex++;

            DisplayCurrentQuestion();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            // 
            SaveCurrentAnswer();

            if (CurrentQuestionIndex > 0)
            {
                CurrentQuestionIndex--;
            }

            DisplayCurrentQuestion();
        }


        private void RestoreSavedAnswer(int questionID, string answerType)
        {
            if (!SurveyAnswers.ContainsKey(questionID))
            {
                return;
            }

            List<string> savedAnswers = SurveyAnswers[questionID];

            switch (answerType)
            {
                case "radio":
                    if (savedAnswers.Count > 0)
                    {
                        rblAnswer.SelectedValue =
                            savedAnswers[0];
                    }
                    break;

                case "dropdown":
                    if (savedAnswers.Count > 0)
                    {
                        ddlAnswer.SelectedValue =
                            savedAnswers[0];
                    }
                    break;

                case "check":
                case "checkbox":
                    foreach (ListItem item in cblAnswer.Items)
                    {
                        item.Selected =
                            savedAnswers.Contains(item.Value);
                    }
                    break;

                case "text":
                    if (savedAnswers.Count > 0)
                    {
                        txtAnswer.Text =
                            savedAnswers[0];
                    }
                    break;

                
            }
        }

        private string GetSingleAnswer(Dictionary<int, List<string>> answers, int questionID)
        {
            if (!answers.ContainsKey(questionID))
            {
                return null;
            }

            List<string> values = answers[questionID];

            if (values == null || values.Count == 0)
            {
                return null;
            }

            return values[0];
        }

        private void FinishSurvey()
        {
            Dictionary<int, List<string>> answers =
                Session["SurveyAnswers"]
                as Dictionary<int, List<string>>;

            if (answers == null)
            {
                lblError.Text =
                    "Your survey session has expired. " +
                    "Please restart the survey.";

                return;
            }

            bool isMember =
                Session["isMember"] != null &&
                Convert.ToBoolean(Session["isMember"]);

            string title =
                Session["RespondentTitle"]?.ToString();

            string givenName =
                Session["RespondentGivenName"]?.ToString();

            string lastName =
                Session["RespondentLastName"]?.ToString();

            string phoneNumber =
                Session["RespondentPhoneNumber"]?.ToString();

            DateTime? dateOfBirth = null;

            if (Session["RespondentDateOfBirth"] != null)
            {
                dateOfBirth = Convert.ToDateTime(
                    Session["RespondentDateOfBirth"]
                );
            }

            string email =
                GetSingleAnswer(
                    answers,
                    EmailQuestionID
                );

            if (string.IsNullOrWhiteSpace(email))
            {
                lblError.Text =
                    "An email address is required before " +
                    "the survey can be submitted.";

                return;
            }

            if (!isMember)
            {
                title = null;
                givenName = "Anonymous";
                lastName = null;
                phoneNumber = null;
                dateOfBirth = null;
            }

            try
            {
                int respondentID =
                    SaveCompletedSurvey(
                        isMember,
                        title,
                        givenName,
                        lastName,
                        phoneNumber,
                        dateOfBirth,
                        email,
                        answers
                    );

                Session["SurveyCompleted"] = true;
                Session["CompletedRespondentID"] =
                    respondentID;

                ClearSurveySession();

                Response.Redirect(
                    "survey_finished.aspx",
                    false
                );

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (SqlException ex)
            {
                lblError.Text =
                    "A database error occurred while saving " +
                    "the survey: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Text =
                    "The survey could not be saved: " +
                    ex.Message;
            }
        }

        private int SaveCompletedSurvey(
            bool isMember,
            string title,
            string givenName,
            string lastName,
            string phoneNumber,
            DateTime? dateOfBirth,
            string email,
            Dictionary<int, List<string>> answers)
        {
            const string insertRespondentQuery = @"
        INSERT INTO respondent
        (
            title,
            given_name,
            last_name,
            email,
            phone_number,
            date_of_birth,
            is_member
        )
        OUTPUT INSERTED.respondentID
        VALUES
        (
            @Title,
            @GivenName,
            @LastName,
            @Email,
            @PhoneNumber,
            @DateOfBirth,
            @IsMember
        );";

            const string insertAnswerQuery = @"
        INSERT INTO respondent_answer
        (
            respondentID,
            questionID,
            answer_optionID,
            answer_text
        )
        VALUES
        (
            @RespondentID,
            @QuestionID,
            @AnswerOptionID,
            @AnswerText
        );";

            using (SqlConnection connection =
                   new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction())
                {
                    try
                    {
                        int respondentID;

                        using (SqlCommand respondentCommand =
                               new SqlCommand(
                                   insertRespondentQuery,
                                   connection,
                                   transaction))
                        {
                            respondentCommand.Parameters.Add(
                                "@Title",
                                SqlDbType.VarChar,
                                20
                            ).Value = ToDatabaseValue(title);

                            respondentCommand.Parameters.Add(
                                "@GivenName",
                                SqlDbType.VarChar,
                                100
                            ).Value = ToDatabaseValue(givenName);

                            respondentCommand.Parameters.Add(
                                "@LastName",
                                SqlDbType.VarChar,
                                100
                            ).Value = ToDatabaseValue(lastName);

                            respondentCommand.Parameters.Add(
                                "@Email",
                                SqlDbType.VarChar,
                                255
                            ).Value = email;

                            respondentCommand.Parameters.Add(
                                "@PhoneNumber",
                                SqlDbType.VarChar,
                                20
                            ).Value = ToDatabaseValue(phoneNumber);

                            respondentCommand.Parameters.Add(
                                "@DateOfBirth",
                                SqlDbType.Date
                            ).Value = dateOfBirth.HasValue
                                ? (object)dateOfBirth.Value
                                : DBNull.Value;

                            respondentCommand.Parameters.Add(
                                "@IsMember",
                                SqlDbType.Bit
                            ).Value = isMember;

                            respondentID = Convert.ToInt32(
                                respondentCommand.ExecuteScalar()
                            );
                        }

                        using (SqlCommand answerCommand =
                               new SqlCommand(
                                   insertAnswerQuery,
                                   connection,
                                   transaction))
                        {
                            answerCommand.Parameters.Add(
                                "@RespondentID",
                                SqlDbType.Int
                            );

                            answerCommand.Parameters.Add(
                                "@QuestionID",
                                SqlDbType.Int
                            );

                            answerCommand.Parameters.Add(
                                "@AnswerOptionID",
                                SqlDbType.Int
                            );

                            answerCommand.Parameters.Add(
                                "@AnswerText",
                                SqlDbType.VarChar,
                                1000
                            );

                            foreach (
                                KeyValuePair<int, List<string>> entry
                                in answers)
                            {
                                int questionID = entry.Key;
                                List<string> answerValues = entry.Value;

                                // Ignore optional questions that were skipped.
                                if (answerValues == null ||
                                    answerValues.Count == 0)
                                {
                                    continue;
                                }

                                string answerType =
                                    GetQuestionAnswerType(questionID);

                                foreach (string answerValue
                                         in answerValues)
                                {
                                    answerCommand.Parameters[
                                        "@RespondentID"
                                    ].Value = respondentID;

                                    answerCommand.Parameters[
                                        "@QuestionID"
                                    ].Value = questionID;

                                    if (answerType == "text" ||
                                        answerType == "date")
                                    {
                                        answerCommand.Parameters[
                                            "@AnswerOptionID"
                                        ].Value = DBNull.Value;

                                        answerCommand.Parameters[
                                            "@AnswerText"
                                        ].Value = answerValue;
                                    }
                                    else
                                    {
                                        int answerOptionID;

                                        if (!int.TryParse(
                                            answerValue,
                                            out answerOptionID))
                                        {
                                            throw new InvalidOperationException(
                                                "Invalid option ID for question " +
                                                questionID + "."
                                            );
                                        }

                                        answerCommand.Parameters[
                                            "@AnswerOptionID"
                                        ].Value = answerOptionID;

                                        answerCommand.Parameters[
                                            "@AnswerText"
                                        ].Value = DBNull.Value;
                                    }

                                    answerCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();

                        return respondentID;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private object ToDatabaseValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value;
        }

        private string GetQuestionAnswerType(int questionID)
        {
            if (SurveyQuestions == null)
            {
                throw new InvalidOperationException(
                    "The survey questions are not available."
                );
            }

            foreach (DataRow row in SurveyQuestions.Rows)
            {
                int currentQuestionID =
                    Convert.ToInt32(row["questionID"]);

                if (currentQuestionID == questionID)
                {
                    return row["answer_type"]
                        .ToString()
                        .Trim()
                        .ToLower();
                }
            }

            throw new InvalidOperationException(
                "Question could not be found: " + questionID
            );
        }

        private DataTable SurveyDependencies
        {
            get
            {
                return Session["SurveyDependencies"] as DataTable;
            }

            set
            {
                Session["SurveyDependencies"] = value;
            }
        }

        private DataTable GetQuestionDependencies()
        {
            const string query = @"
        SELECT
            question_dependencyID,
            trigger_answer_optionID,
            child_questionID,
            dependency_condition
        FROM question_dependency
        WHERE is_active = 1
        ORDER BY question_dependencyID;";

            DataTable table = new DataTable();

            using (SqlConnection connection =
                   new SqlConnection(ConnectionString))
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(command))
            {
                adapter.Fill(table);
            }

            return table;
        }


        private DataRow GetQuestionRow(int questionID)
        {
            if (SurveyQuestions == null)
            {
                return null;
            }

            foreach (DataRow row in SurveyQuestions.Rows)
            {
                if (Convert.ToInt32(row["questionID"]) ==
                    questionID)
                {
                    return row;
                }
            }

            return null;
        }

        private List<int> GetVisibleQuestionIds()
        {
            List<int> visibleQuestions =
                new List<int>();

            HashSet<int> addedQuestions =
                new HashSet<int>();

            foreach (DataRow row in SurveyQuestions.Rows)
            {
                bool isMainQuestion =
                    Convert.ToBoolean(row["is_main_question"]);

                if (!isMainQuestion)
                {
                    continue;
                }

                int questionID =
                    Convert.ToInt32(row["questionID"]);

                AddQuestionAndChildren(
                    questionID,
                    visibleQuestions,
                    addedQuestions
                );
            }

            return visibleQuestions;
        }


        private void AddQuestionAndChildren(int questionID, List<int> visibleQuestions, HashSet<int> addedQuestions)
        {
            // Prevent the same question being added twice.
            if (addedQuestions.Contains(questionID))
            {
                return;
            }

            addedQuestions.Add(questionID);
            visibleQuestions.Add(questionID);

            List<string> savedAnswers;

            if (!SurveyAnswers.TryGetValue(
                questionID,
                out savedAnswers))
            {
                return;
            }

            if (savedAnswers == null ||
                savedAnswers.Count == 0)
            {
                return;
            }

            List<int> childQuestionIDs =
                GetTriggeredChildQuestions(
                    questionID,
                    savedAnswers
                );

            foreach (int childQuestionID
                     in childQuestionIDs)
            {
                // Recursive call:
                // the child can itself have children.
                AddQuestionAndChildren(
                    childQuestionID,
                    visibleQuestions,
                    addedQuestions
                );
            }
        }

        private List<int> GetTriggeredChildQuestions(int parentQuestionID, List<string> selectedAnswers)
        {
            List<int> children =
                new List<int>();

            HashSet<int> childIDs =
                new HashSet<int>();

            string answerType =
                GetQuestionAnswerType(parentQuestionID);

            // Dependencies in your current schema are based
            // on answer_optionID, not typed text.
            if (answerType == "text" ||
                answerType == "date")
            {
                return children;
            }

            foreach (string selectedAnswer
                     in selectedAnswers)
            {
                int answerOptionID;

                if (!int.TryParse(
                    selectedAnswer,
                    out answerOptionID))
                {
                    continue;
                }

                foreach (DataRow dependency
                         in SurveyDependencies.Rows)
                {
                    int triggerOptionID =
                        Convert.ToInt32(
                            dependency[
                                "trigger_answer_optionID"
                            ]
                        );

                    if (triggerOptionID !=
                        answerOptionID)
                    {
                        continue;
                    }

                    int childQuestionID =
                        Convert.ToInt32(
                            dependency[
                                "child_questionID"
                            ]
                        );

                    // Same child might be triggered by
                    // multiple selected options.
                    if (childIDs.Add(childQuestionID))
                    {
                        children.Add(childQuestionID);
                    }
                }
            }

            // Keep child questions in their DB display order.
            children.Sort(delegate (int x, int y)
            {
                DataRow questionX = GetQuestionRow(x);
                DataRow questionY = GetQuestionRow(y);

                int orderX =
                    Convert.ToInt32(
                        questionX["display_order"]
                    );

                int orderY =
                    Convert.ToInt32(
                        questionY["display_order"]
                    );

                return orderX.CompareTo(orderY);
            });

            return children;
        }

        private bool QuestionCanTriggerChild(int questionID)
        {
            const string query = @"
                SELECT COUNT(*)
                FROM question_dependency qd
                INNER JOIN answer_option ao
                ON ao.answer_optionID =
                qd.trigger_answer_optionID
                WHERE ao.questionID = @QuestionID
                AND qd.is_active = 1;";

            using (SqlConnection connection =
                   new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionID;

                connection.Open();

                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
        }

        // Clear all survey-related session variables after the survey is completed.
        private void ClearSurveySession()
        {
            Session.Remove("isMember");
            Session.Remove("RespondentTitle");
            Session.Remove("RespondentGivenName");
            Session.Remove("RespondentLastName");
            Session.Remove("RespondentPhoneNumber");
            Session.Remove("RespondentDateOfBirth");
            Session.Remove("SurveyAnswers");
            Session.Remove("SurveyQuestions");
            Session.Remove("SurveyDependencies");
        }


    }
}