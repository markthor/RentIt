using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class Home : System.Web.UI.Page
    {
        List<string> tracks = new List<string>();
        List<string> genrelist = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            tracks.Add("asds645756tdgsdfhse");
            tracks.Add("asdsdg123454768sfasfsdfhse");
            tracks.Add("asdsdfgrg4352324gsdfhse");
            tracks.Add("asdsdgasdasdasfesdfhse");
            tracks.Add("asdsd2342354gsdfhse");

            ListBox tracklist = (ListBox) channelDescription.FindControl("lbx_tracklist");
            tracklist.DataSource = tracks;
            tracklist.DataBind();
        }
    }
}