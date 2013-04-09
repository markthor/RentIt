using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BootstrapAsp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SignUp(object sender, EventArgs e)
        {
            TextBox password = (TextBox)Page.FindControl("inputPassword");
            password.Text = "gogogo";            
        }
    }
}