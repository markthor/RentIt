using System;
using System.Text.RegularExpressions;

namespace RentItClient
{
    public partial class Login : System.Web.UI.Page
    {
        private const string EmailRe = @"[\w\d\._-]+@[\w\d\._-]+\.[\w]+";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Creates a new user in the system
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EnevtArgs</param>
        protected void btn_createUser_Click(object sender, EventArgs e)
        {
            // collects data from text fields
            string username = tbx_createUsername.Text;
            string email = tbx_email.Text;
            string password = tbx_createPassword.Text;
            string passwordConfirm = tbx_confirmPassword.Text;

            using (var proxy = new RentItService.RentItServiceClient())
            {
                bool validInputs = ValidateInputs(username, email, password, passwordConfirm);
                if (!validInputs)
                {
                    return;
                }
                int userId = proxy.CreateUser(username, password, email);
                if (userId == -1)
                {
                    PrintErrorMessage("An error occured when trying to create the user");
                }
                else
                {
                    Session.Add("UserId", userId);
                    Response.Redirect("Home.aspx");
                }
            }
        }

        private bool ValidateInputs(string username, string email, string password, string passwordConfirm)
        {
            //Check that the user has entered all information
            if (username.Equals("") || email.Equals("") || password.Equals(""))
            {
                PrintErrorMessage("Please fill all information");
                return false;
            }
            //Check that the email is an email
            if (Regex.Match(password, EmailRe).Success)
            {
                PrintErrorMessage("Please enter a valid email");
                return false;
            }
            //Check that the two passwords are equal
            if (!password.Equals(passwordConfirm))
            {
                PrintErrorMessage("The two passwords are not equal");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Logs the user into the system
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Eventargs</param>
        protected void btn_login_Click(object sender, EventArgs e)
        {
            string username = tbx_username.Text;
            string password = tbx_password.Text;
            if (username.Equals("") ||  password.Equals(""))
            {
                PrintErrorMessage("Username or password is empty");
                return;
            }
            using (var proxy = new RentItService.RentItServiceClient())
            {
                int userId = proxy.Login(username, password);
                if (userId == -1)
                {
                    PrintErrorMessage("Username and email didn't match");
                }
                else
                {
                    Session.Add("UserId", userId);
                    Response.Redirect("Home.aspx");
                }
            }
        }

        private void PrintErrorMessage(string msg)
        {
            Response.Write("<script language='javascript'>alert('" + msg + "');</script>");
        }
    }
}