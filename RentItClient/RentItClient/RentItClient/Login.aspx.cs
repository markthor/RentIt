using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace RentItClient
{

    public partial class Login : System.Web.UI.Page
    {
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

            // Checks input
            if (username == "" || email == "" || password == "")
            {
                ErrorMessage("Username, Email or password is empty");
                return;
            }
            if (password != passwordConfirm)
            {
                ErrorMessage("Password not confirmed");
                return;
            }
            Match match = Regex.Match(email, @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                ErrorMessage("E-mail not correct");
                return;
            }
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
            if (username == "" ||  password == "")
            {
                ErrorMessage("Username or password is empty");
                return;
            }
        }

        private void ErrorMessage(string msg)
        {
            Response.Write("<script language='javascript'>alert('" + msg + "');</script>");
        }
    }
}