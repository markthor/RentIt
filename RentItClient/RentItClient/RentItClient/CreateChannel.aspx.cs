using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class CreateChannel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_createChannel_Click(object sender, EventArgs e)
        {
            Session["ChannelName"] = tbx_channelName.Text;
            Response.Redirect("EditChannel.aspx");
        }

        protected void btn_cancelCreateChannel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}