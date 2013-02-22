using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Diagnostics;

namespace RentItClient
{

    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_createUser_Click(object sender, EventArgs e)
        {
            string username = tbx_createUsername.Text;
            string email = tbx_email.Text;
            string password = tbx_createPassword.Text;
            string passwordConfirm = tbx_confirmPassword.Text;

            if (password != passwordConfirm)
                Response.Write("<script language='javascript'>alert('Password not confirmed');</script>");


        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string username = tbx_username.Text;
            string password = tbx_password.Text;
        }
    }
}