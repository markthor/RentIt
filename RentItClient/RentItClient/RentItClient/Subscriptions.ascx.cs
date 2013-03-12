using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class Subscriptions : System.Web.UI.UserControl
    {
        private List<string> channelList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitChannelList();
            }
        }

        private void InitChannelList()
        {
            channelList = new List<string>();
            channelList.Add("Andreases super channel");
            channelList.Add("Mikkels techno party");
            lbx_myChannels.DataSource = channelList;
            lbx_myChannels.DataBind();
            Session["ChannelList"] = channelList;
        }

        protected void btn_selectChannel_Click(object sender, EventArgs e)
        {

        }

        protected void btn_editChannel_Click(object sender, EventArgs e)
        {
            string channel = lbx_myChannels.SelectedItem.Text;
            Session["ChannelName"] = channel;
            
            Response.Redirect("EditChannel.aspx");
        }
    }
}