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
            String connectionStr = ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            if (!IsPostBack)
            {
                if (Session["userID"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

                label_welcome.Text = "Welcome, " + Session["UserName"].ToString();

                DatabaseConnection(connectionStr);
                LoadRoles(connectionStr);
                LoadUsers(connectionStr);
            }
            
        }

        private void DatabaseConnection(String connectionStr)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionStr);

                connection.Open();

                connection.Close();
            }
            catch (Exception error)
            {
                label_welcome.Text = "Could not connect to database";
                Console.WriteLine(error.StackTrace);
            }
        }

        private void LoadRoles(String connectionStr) {
            string query_roles = "SELECT roleID, position FROM role";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query_roles, connection))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    GridViewRoles.DataSource = table;
                    GridViewRoles.DataBind();
                }
            }
        }

        private void LoadUsers(String connectionStr)
        {

            string query_users = "SELECT u.staffID, r.roleID AS Role, u.given_name AS FirstName, u.last_name AS LastName, u.email FROM " +
                "staff u INNER JOIN role r ON u.roleID = r.roleID";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query_users, connection))
                {
                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    GridViewUsers.DataSource = table;
                    GridViewUsers.DataBind();
                }
            }
        }

        public class Roles
        {
            public int Id { get; set; }
            public string Position {  get; set; }
        }
    }
}