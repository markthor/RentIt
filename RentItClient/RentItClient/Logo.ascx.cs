using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class Logo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void img_Logo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}