using System;
using System.Collections.Generic;
using System.Configuration;
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

        }

        protected void rblRespondent_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlRespondentDetails.Visible = (rblRespondent.SelectedValue == "Yes");
        }

        protected void submit_form(object sender, EventArgs e)
        {

            if (!Page.IsValid)
            {
                return;
            }

            string connectionStr = ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            string title = "";
            string given_name = txtRespondentID_given_name.Text;
            string last_name = TextBox6.Text;
            string age_range = TextBox2.Text;
            string gender = "M";
            string DOB = TextBox8.Text;
            string email = TextBox1.Text;
            string phone_number = TextBox7.Text;
            string state_territory = TextBox3.Text;
            string suburb = TextBox4.Text;
            string postcode = TextBox5.Text;
            bool is_member = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO respondent (title, given_name, last_name, age_range, gender, email, phone_number, DOB, state_territory, suburb, postcode, is_member)
                    VALUES (@title, @given_name, @last_name, @age_range, @gender, @email, @phone_number, @DOB, @state_territory, @suburb, @postcode, @is_member)
                ";

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

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lbMessage.Text = "Created successfully";
                        }

                    }
                }

            } catch (Exception ex) {
                lbMessage.Text = ex.Message;
            }

        

            Response.Redirect("survey_finished.aspx");
        }
    }
}