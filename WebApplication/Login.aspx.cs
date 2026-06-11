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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CheckCredentials()
        {
            String connectionStr = ConfigurationManager.ConnectionStrings["devConnectionStr"].ConnectionString;

            String email = TextBox_email.Text;
            String password = TextBox_password.Text;


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();

                    string query = @"
            SELECT staffID, given_name, email, roleID
            FROM staff
            WHERE email = @Email
            AND password = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int roleID = Convert.ToInt32(reader["roleID"]);

                                Session["userID"] = reader["staffID"];
                                Session["userEmail"] = reader["email"];
                                Session["userName"] = reader["given_name"];
                                Session["position"] = roleID;

                                if (roleID == 1)
                                {
                                    Response.Redirect("WebForm.aspx");
                                } else
                                {                                  
                                    Response.Redirect("StaffPanel.aspx");
                                }
                            } 
                            else
                            {
                                label_title_login.Text = "Invalid email or password.";
                            }
                        }
                    }
                }

            }
            catch (Exception ex) {
                label_title_login.Text = "Something went wrong while logging in.";
                Console.WriteLine("Exception: " + ex.StackTrace);
            }
        }

        protected void check_credentials(object sender, EventArgs e)
        {
            CheckCredentials();
        }
    }
}