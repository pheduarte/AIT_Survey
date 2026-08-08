using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadQuestions();
        }

        protected void Start_survey(object sender, EventArgs e)
        {
            string title = null;
            string givenName = null;
            string lastName = null;
            string phoneNumber = null;
            DateTime? dateOfBirth = null;
            string email = null;

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                HiddenField hiddenQuestionId = item.FindControl("hiddenQuestionID") as HiddenField;
                HiddenField hiddenFieldKey = item.FindControl("hiddenFieldKey") as HiddenField;

                if (hiddenQuestionId == null || hiddenFieldKey == null)
                {
                    continue;
                }

                int questionId = Convert.ToInt32(hiddenQuestionId.Value);
                string fieldKey = hiddenFieldKey.Value;

                Control answerControl =
                    item.FindControl($"answer_{questionId}");

                string answer = GetControlValue(answerControl);

                switch (fieldKey)
                {
                    case "title": 
                        title = answer;
                        break;

                    case "given_name":
                        givenName = answer;
                        break;

                    case "last_name":
                        lastName = answer;
                        break;

                    case "phone_number": 
                        phoneNumber = answer;
                        break;

                    case "email": 
                        email = answer;
                        break;

                    case "date_of_birthday": 
                        if (DateTime.TryParse(answer, out DateTime parsedDate))
                        {
                            dateOfBirth = parsedDate;
                        }
                        break;
                }
            }


            Session["isMember"] = true;
            Session["RespondentTitle"] = title;
            Session["RespondentGivenName"] = givenName;
            Session["RespondentLastName"] = lastName;
            Session["RespondentEmail"] = email;
            Session["RespondentPhoneNumber"] = phoneNumber;
            Session["RespondentDateOfBirth"] = dateOfBirth;

            Response.Redirect("survey.aspx");
        }

        protected void Skipped_registration(object sender, EventArgs e)
        {
            Session["RespondentTitle"] = null;
            Session["isMember"] = false;
            Session["RespondentGivenName"] = "Anonymous";

            Response.Redirect("survey.aspx");
        }

        private string GetControlValue(Control control)
        {
            if (control == null)
            {
                return null;
            }

            if (control is TextBox textBox)
            {
                return textBox.Text.Trim();
            }

            if (control is DropDownList dropDown)
            {
                return dropDown.SelectedItem?.Text;
            }

            if (control is RadioButtonList radioList)
            {
                return radioList.SelectedItem?.Text;
            }

            if (control is CheckBoxList checkBoxList)
            {
                List<string> selectedAnswers = new List<string>();

                foreach (ListItem option in checkBoxList.Items)
                {
                    if (option.Selected)
                    {
                        selectedAnswers.Add(option.Text);
                    }
                }

                return string.Join(", ", selectedAnswers);
            }

            return null;
        }

        private DataTable GetQuestions()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"]
                    .ConnectionString;

            const string query = @"
        SELECT
            questionID,
            question_text,
            answer_type,
            field_key
        FROM question
        WHERE is_active = 1 AND is_registration = 1
        ORDER BY display_order;";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable questions = new DataTable();
                adapter.Fill(questions);
                return questions;
            }
        }

        private void LoadQuestions()
        {
            rptQuestions.DataSource = GetQuestions();
            rptQuestions.DataBind();
        }

        protected void rptQuestions_db(
        object sender,
        RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            DataRowView row = (DataRowView)e.Item.DataItem;

            int questionId = Convert.ToInt32(row["questionID"]);
            string questionType = row["answer_type"]
                .ToString()
                .Trim()
                .ToLower();

            PlaceHolder placeholder =
                (PlaceHolder)e.Item.FindControl("phAnswerControl");

            switch (questionType)
            {
                case "radio":
                    RadioButtonList radioList = new RadioButtonList
                    {
                        ID = $"answer_{questionId}",
                        CssClass = "form-check"
                    };

                    BindOptions(radioList, questionId);
                    placeholder.Controls.Add(radioList);
                    break;

                case "check":
                    CheckBoxList checkList = new CheckBoxList
                    {
                        ID = $"answer_{questionId}",
                        CssClass = "form-check"
                    };

                    BindOptions(checkList, questionId);
                    placeholder.Controls.Add(checkList);
                    break;

                case "dropdown":
                    DropDownList dropDown = new DropDownList
                    {
                        ID = $"answer_{questionId}",
                        CssClass = "form-select"
                    };

                    BindOptions(dropDown, questionId);
                    dropDown.Items.Insert(0, new ListItem("-- Select --", ""));
                    placeholder.Controls.Add(dropDown);
                    break;

                case "text":
                    TextBox textBox = new TextBox
                    {
                        ID = $"answer_{questionId}",
                        CssClass = "form-control"
                    };

                    placeholder.Controls.Add(textBox);
                    break;

                case "date":
                    TextBox dateBox = new TextBox
                    {
                        ID = $"answer_{questionId}",
                        TextMode = TextBoxMode.Date,
                        CssClass = "form-control"
                    };

                    placeholder.Controls.Add(dateBox);
                    break;
            }
        }

        private void BindOptions(ListControl control, int questionId)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["devConnectionStr"]
                    .ConnectionString;

            const string query = @"
                SELECT
                answer_optionID,
                option_text
                FROM answer_option
                WHERE questionID = @questionID
                ORDER BY [order];";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add(
                    "@QuestionID",
                    SqlDbType.Int
                ).Value = questionId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    control.DataSource = reader;
                    control.DataTextField = "option_text";
                    control.DataValueField = "answer_optionID";
                    control.DataBind();
                }
            }
        }

      

        private object ToDatabaseValue(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? (object)DBNull.Value
                : value;
        }





    }
}