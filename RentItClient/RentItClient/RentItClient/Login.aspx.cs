﻿using System;
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

            using (RentItService.RentItServiceClient proxy = new RentItService.RentItServiceClient())
            {
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
                PrintErrorMessage("Username or password is empty");
                return;
            }
            using (RentItService.RentItServiceClient proxy = new RentItService.RentItServiceClient())
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