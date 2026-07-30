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
    public partial class survey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Prevents users to go back and submit the survey again
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            if (Session["SurveyCompleted"] != null)
            {
                Response.Redirect("survey_finished.aspx");
            }
 
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadQuestions();
        }


        protected void submit_form(object sender, EventArgs e)
        {
            /*
            if (!Page.IsValid)
            {
                return;
            }

            string connectionStr = ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            string title = select_title.SelectedValue;
            string given_name = txtRespondentID_given_name.Text;
            string last_name = TextBox6.Text;
            string age_range = AgeRange.SelectedValue;
            string gender = DropDown_gender.SelectedValue;
            string DOB = TextBox8.Text;
            string email = TextBox1.Text;
            string phone_number = TextBox7.Text;
            string state_territory = StateTerritoryRadio.SelectedValue;
            string suburb = TextBox4.Text;
            string postcode = TextBox5.Text;
            bool is_member = (rblRespondent.SelectedValue == "Yes");

            // Reset fields if user selects No for Membership 
            if (rblRespondent.SelectedValue == "No")
            {
                title = "";
                given_name = "Anonymous";
                last_name = "";
                phone_number = "";
                DOB = "";
            }

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO respondent 
                        (title, given_name, last_name, age_range, gender, email, phone_number, DOB, state_territory, suburb, postcode, is_member)
                    OUTPUT INSERTED.respondentID
                    VALUES 
                        (@title, @given_name, @last_name, @age_range, @gender, @email, @phone_number, @DOB, @state_territory, @suburb, @postcode, @is_member);
                    ";

                    int respondentID;

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@given_name", given_name);
                        cmd.Parameters.AddWithValue("@last_name", last_name);
                        cmd.Parameters.AddWithValue("@age_range", age_range);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone_number", phone_number);
                        cmd.Parameters.AddWithValue("@DOB", DOB);
                        cmd.Parameters.AddWithValue("@state_territory", state_territory);
                        cmd.Parameters.AddWithValue("@suburb", suburb);
                        cmd.Parameters.AddWithValue("@postcode", postcode);
                        cmd.Parameters.AddWithValue("@is_member", is_member);

                        respondentID = (int)cmd.ExecuteScalar();

                    }

                    //Insert values to respective tables in db
                    
                }   

            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message;
            }


            Session["SurveyCompleted"] = true;

            Response.Redirect("survey_finished.aspx");

            */
        }


        private DataTable GetQuestions()
        {
            string connectionStr =
              ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            const string query = @"
        SELECT
            questionID,
            question_text,
            answer_type
        FROM question
        WHERE is_active = 1 AND is_main_question = 1
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
            questions_rep.DataSource = GetQuestions();
            questions_rep.DataBind();
        }

        private DataTable GetAnswerOptions(int questionID)
        {
            string connectionStr =
              ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            const string sql = @"
        SELECT
            answer_optionID,
            questionID,
            option_text
        FROM answer_option
        WHERE questionID=@questionID
        ORDER BY answer_optionID";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(
                    "@questionID",
                    SqlDbType.Int
                ).Value = questionID;

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }


        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            DataRowView row = (DataRowView)e.Item.DataItem;

            int questionID = Convert.ToInt32(row["questionID"]);
            string answerType = row["answer_type"].ToString();

            PlaceHolder placeHolder = (PlaceHolder)e.Item.FindControl("answerPlaceholder");

            DataTable options = GetAnswerOptions(questionID);

            if (answerType == "radio")
            {
                RadioButtonList radioList = new RadioButtonList
                {
                    ID = $"answer_{questionID}",
                    CssClass = "form-check"
                };

                radioList.DataSource = options;
                radioList.DataTextField = "option_text";
                radioList.DataValueField = "answer_optionID";
                radioList.DataBind();

                placeHolder.Controls.Add(radioList);
            }
            else if (answerType == "dropdown")
            {
                DropDownList dropdown = new DropDownList()
                {
                    ID = $"answer_{questionID}",
                    CssClass = "form-control"
                };

                dropdown.DataSource = options;
                dropdown.DataTextField = "option_text";
                dropdown.DataValueField = "answer_optionID";
                dropdown.DataBind();

                placeHolder.Controls.Add(dropdown);
            }
            else if (answerType == "text")
            {
                TextBox textBox = new TextBox
                {
                    ID = $"answer_{questionID}",
                    CssClass = "form-control"
                };
                placeHolder.Controls.Add(textBox);
            }
            else if (answerType == "check")
            {
                CheckBoxList checkbox = new CheckBoxList
                {
                    ID = $"answer_{questionID}",
                    CssClass = "form-control"
                };

                checkbox.DataSource = options;
                checkbox.DataTextField = "option_text";
                checkbox.DataValueField = "answer_optionID";
                checkbox.DataBind();

                placeHolder.Controls.Add(checkbox);
            }
        }

       

    }
}