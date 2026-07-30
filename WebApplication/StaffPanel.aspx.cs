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
    public partial class AdminPanel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            LoadRespondents();
            LoadInsuranceProviders();
            LoadRoomType();
            LoadServices();
        }


        private void LoadRespondents()
        {
            String connectionStr = ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            string query_roles = "SELECT respondentID, title, given_name, last_name, age_range," +
                "gender, email, phone_number, DOB, state_territory, suburb, postcode, is_member, submission_datetime FROM respondent";

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

                    ddlInsurance.DataSource = reader;
                    ddlInsurance.DataTextField = "insurance_name";
                    ddlInsurance.DataValueField = "insuranceProviderID";
                    ddlInsurance.DataBind();
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

                    ddlRoomType.DataSource = reader;
                    ddlRoomType.DataTextField = "room_type_name";
                    ddlRoomType.DataValueField = "roomTypeID";
                    ddlRoomType.DataBind();
                }
            }


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

                    ddlTypeOfService.DataSource = reader;
                    ddlTypeOfService.DataTextField = "service_name";
                    ddlTypeOfService.DataValueField = "serviceID";
                    ddlTypeOfService.DataBind();
                }
            }
        }
    }
}