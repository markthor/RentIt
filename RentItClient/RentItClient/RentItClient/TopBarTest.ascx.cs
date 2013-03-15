using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class TopBarTest : System.Web.UI.UserControl
    {
        public event EventHandler SimpleSearchClick;
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void img_Logo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void bth_searchAdvanced_Click(object sender, EventArgs e)
        {

        }

        protected void btn_createChannel_Click(object sender, EventArgs e)
        {

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (SimpleSearchClick != null)
            {
                SimpleSearchClick(this, e);
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {

        }
    }
}