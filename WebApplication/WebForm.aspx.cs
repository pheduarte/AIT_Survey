using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WebApplication
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadRespondents();

                LoadInsuranceProviders();
                LoadRoomType();
                LoadRehabSurgery();
                LoadGender();
                LoadInRoomServices();
            }


        }


        // Load respondents from db for the gridview
        private void LoadRespondents()
        {
            String connectionStr = ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            string query_roles = "SELECT * FROM vw_StaffRespondentSearch;";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query_roles, connection))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    gvAtendents.DataSource = table;
                    gvAtendents.DataBind();
                }
            }
        }

        // Load Insurance Providers from db for the dropdown list
        private void LoadInsuranceProviders()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
            SELECT option_text
            FROM answer_option
            WHERE questionID = 10";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlInsurance.DataSource = reader;
                    ddlInsurance.DataTextField = "option_text";
                    ddlInsurance.DataValueField = "option_text";
                    ddlInsurance.DataBind();
                }
            }
        }


        // Load Gender from db for the dropdown list
        private void LoadGender()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"
            SELECT option_text
            FROM answer_option
            WHERE questionID = 3";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlGender.DataSource = reader;
                    ddlGender.DataTextField = "option_text";
                    ddlGender.DataValueField = "option_text";
                    ddlGender.DataBind();
                }
            }
        }


        // Load Room Type from db for the dropdown list
        private void LoadRoomType()
        {
            string connectionStr =
              ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
                SELECT option_text
            FROM answer_option
            WHERE questionID = 18";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlRoomType.DataSource = reader;
                    ddlRoomType.DataTextField = "option_text";
                    ddlRoomType.DataValueField = "option_text";
                    ddlRoomType.DataBind();
                }
            }


        }


        //Load Rehab Surgery from db for the dropdown list
        private void LoadRehabSurgery()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
            SELECT option_text
            FROM answer_option
            WHERE questionID = 13";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlSurgery.DataSource = reader;
                    ddlSurgery.DataTextField = "option_text";
                    ddlSurgery.DataValueField = "option_text";
                    ddlSurgery.DataBind();
                }
            }
        }

        //Load In Room Services from db for the dropdown list
        private void LoadInRoomServices()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
                    SELECT option_text
                    FROM answer_option
                    WHERE questionID = 9";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlInRoom.DataSource = reader;
                    ddlInRoom.DataTextField = "option_text";
                    ddlInRoom.DataValueField = "option_text";
                    ddlInRoom.DataBind();
                }
            }
        }

        // Reads the search filters and queries the database for respondents matching the criteria
        protected void Search_clicked(object sender, EventArgs e)
        {


            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"]
                    .ConnectionString;

            string name = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            string gender = ddlGender.SelectedValue;
            string surgery = ddlSurgery.SelectedValue;
            string inRoomService = ddlInRoom.SelectedValue;
            string roomType = ddlRoomType.SelectedValue;
            string insurance = ddlInsurance.SelectedValue;

            string query = @"
        SELECT
            r.respondentID,
            r.title,
            r.given_name,
            r.last_name,
            r.email,
            r.phone_number,
            r.date_of_birth,
            r.is_member,
            r.submission_datetime,
            r.attended_datetime
        FROM respondent r
        WHERE
            (@given_name = ''
                OR r.given_name LIKE '%' + @given_name + '%')

            AND (@last_name = ''
                OR r.last_name LIKE '%' + @last_name + '%')

            AND (@email = ''
                OR r.email LIKE '%' + @email + '%')

            AND (
                @gender = ''
                OR EXISTS
                (
                    SELECT 1
                    FROM respondent_answer ra
                    INNER JOIN answer_option ao
                        ON ra.answer_optionID = ao.answer_optionID
                    WHERE ra.respondentID = r.respondentID
                      AND ra.questionID = 3
                      AND ao.option_text = @gender
                )
            )

            AND (
                @surgery = ''
                OR EXISTS
                (
                    SELECT 1
                    FROM respondent_answer ra
                    INNER JOIN answer_option ao
                        ON ra.answer_optionID = ao.answer_optionID
                    WHERE ra.respondentID = r.respondentID
                      AND ra.questionID = 13
                      AND ao.option_text = @surgery
                )
            )

            AND (
                @in_room_service = ''
                OR EXISTS
                (
                    SELECT 1
                    FROM respondent_answer ra
                    INNER JOIN answer_option ao
                        ON ra.answer_optionID = ao.answer_optionID
                    WHERE ra.respondentID = r.respondentID
                      AND ra.questionID = 9
                      AND ao.option_text = @in_room_service
                )
            )

            AND (
                @room_type = ''
                OR EXISTS
                (
                    SELECT 1
                    FROM respondent_answer ra
                    INNER JOIN answer_option ao
                        ON ra.answer_optionID = ao.answer_optionID
                    WHERE ra.respondentID = r.respondentID
                      AND ra.questionID = 18
                      AND ao.option_text = @room_type
                )
            )

            AND (
                @insurance = ''
                OR EXISTS
                (
                    SELECT 1
                    FROM respondent_answer ra
                    INNER JOIN answer_option ao
                        ON ra.answer_optionID = ao.answer_optionID
                    WHERE ra.respondentID = r.respondentID
                      AND ra.questionID = 10
                      AND ao.option_text = @insurance
                )
            )

        ORDER BY r.last_name ASC;";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@given_name", name);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                cmd.Parameters.AddWithValue("@email", email);

                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@surgery", surgery);
                cmd.Parameters.AddWithValue("@in_room_service", inRoomService);
                cmd.Parameters.AddWithValue("@room_type", roomType);
                cmd.Parameters.AddWithValue("@insurance", insurance);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    gvAtendents.DataSource = table;
                    gvAtendents.DataBind();

                    Label1.Text = table.Rows.Count + " respondent(s) found";
                }
            }
        }

        // Clear the search filters and reload the respondents
        protected void Clear_clicked(object sender, EventArgs e)
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";

            ddlGender.SelectedIndex = 0;
            ddlRoomType.SelectedIndex = 0;
            ddlSurgery.SelectedIndex = 0;
            ddlInRoom.SelectedIndex = 0;
            ddlInsurance.SelectedIndex = 0;

            LoadRespondents();

            Label1.Text = "Result";
        }

    }
}