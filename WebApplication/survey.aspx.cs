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
            //Prevents users to go back and submit the survey again
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            if (Session["SurveyCompleted"] != null)
            {
                Response.Redirect("survey_finished.aspx");
            }

            if (!IsPostBack)
            {
                LoadServices();
                LoadInsuranceProviders();
                LoadDischargePlan();
                LoadRoomType();
                LoadInRoomService();
                LoadWifiService();
            }

            pnlRespondentDetails.Visible = rblRespondent.SelectedValue == "Yes";
            PanelAttendedRehab.Visible = RadioBtnAttendedRehabOP.SelectedValue == "Yes";
            
            
        }

        protected void rblRespondent_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlRespondentDetails.Visible = rblRespondent.SelectedValue == "Yes";
        }

        protected void radioAttendedRehabOP_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanelAttendedRehab.Visible = RadioBtnAttendedRehabOP.SelectedValue == "Yes";
        }

        protected void radioRoomTypeService_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanelInRoomService.Visible = radioRoomType.SelectedItem.Text == "Public Single Room"
                || radioRoomType.SelectedItem.Text == "Private Room";
        }

        protected void radioWifiService_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanelWifiService.Visible =
            CheckInRoomServices.Items
                .Cast<ListItem>()
                .Any(item => item.Selected && item.Text == "Wi-Fi");
        }

        protected void submit_form(object sender, EventArgs e)
        {

            if (!Page.IsValid)
            {
                return;
            }

            // Controls how many providers users can select
            int selectedInsuranceCount = 0;

            foreach (ListItem item in cblInsuranceProviders.Items)
            {
                if (item.Selected)
                {
                    selectedInsuranceCount++;
                }
            }

            if (selectedInsuranceCount > 2)
            {
                lbMessage.Text = "Please select a maximum of 2 insurance providers.";
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

                    // Inserts in db each Insurance Provider user selected (max 2)
                    foreach (ListItem item in cblInsuranceProviders.Items)
                    {
                        if (item.Selected)
                        {
                            string insertInsuranceSql = @"
                             INSERT INTO respondent_insurance
                             (respondentID, insuranceProviderID)
                            VALUES
                            (@respondentID, @insuranceProviderID);
                            ";

                            using (SqlCommand cmd = new SqlCommand(insertInsuranceSql, connection))
                            {
                                cmd.Parameters.AddWithValue("@respondentID", respondentID);
                                cmd.Parameters.AddWithValue("@insuranceProviderID", item.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message;
            }


            Session["SurveyCompleted"] = true;

            Response.Redirect("survey_finished.aspx");
        }

        private void LoadServices()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
            SELECT serviceID,
                   service_name
            FROM service_type
            ORDER BY service_name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    CheckServices.DataSource = reader;
                    CheckServices.DataTextField = "service_name";
                    CheckServices.DataValueField = "serviceID";
                    CheckServices.DataBind();
                }
            }
        }

        private void LoadInsuranceProviders()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
            SELECT insuranceProviderID,
                   insurance_name
            FROM insurance_provider
            ORDER BY insurance_name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    cblInsuranceProviders.DataSource = reader;
                    cblInsuranceProviders.DataTextField = "insurance_name";
                    cblInsuranceProviders.DataValueField = "insuranceProviderID";
                    cblInsuranceProviders.DataBind();
                }
            }
        }

        private void LoadDischargePlan()
        {
            string connectionStr =
                ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
                SELECT dischargePlanID, plan_name
                FROM discharge_plan
                ORDER BY plan_name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    cblDischargePlan.DataSource = reader;
                    cblDischargePlan.DataTextField = "plan_name";
                    cblDischargePlan.DataValueField = "dischargePlanID";
                    cblDischargePlan.DataBind();
                }
            }

        }

        private void LoadRoomType() 
        {
            string connectionStr =
              ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
                SELECT roomTypeID, room_type_name
                FROM room_type
                ORDER BY room_type_name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    radioRoomType.DataSource = reader;
                    radioRoomType.DataTextField = "room_type_name";
                    radioRoomType.DataValueField = "roomTypeID";
                    radioRoomType.DataBind();
                }
            }


        }

        private void LoadInRoomService()
        {
            string connectionStr =
              ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
                SELECT inRoomServiceID, service_name
                FROM inRoom_service";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    CheckInRoomServices.DataSource = reader;
                    CheckInRoomServices.DataTextField = "service_name";
                    CheckInRoomServices.DataValueField = "inRoomServiceID";
                    CheckInRoomServices.DataBind();
                }
            }
        }

        private void LoadWifiService()
        {
            string connectionStr =
              ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();

                string sql = @"
                SELECT wifiServiceID, service_name
                FROM wifi_service
                ORDER BY service_name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    CheckWiFiService.DataSource = reader;
                    CheckWiFiService.DataTextField = "service_name";
                    CheckWiFiService.DataValueField = "wifiServiceID";
                    CheckWiFiService.DataBind();
                }
            }
        }
    }
}