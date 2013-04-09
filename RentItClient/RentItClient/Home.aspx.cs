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
            tracks.Add("Arctic Monkeys - You Probably Couldn't See for the Lights but You Were Looking Straight at Me");
            tracks.Add("Bal Sagoth - The Dark Liege of Chaos is unleashed at the Ensorcelled Shrine of A'Zura Kai (The Splendour of a Thousand Swords Gleaming Beneath the Blazon of the Hyperborean Empire Part II)");
            tracks.Add("asdsdfgrg4352324gsdfhse");
            tracks.Add("asdsdgasdasdasfesdfhse");
            tracks.Add("asdsd2342354gsdfhse");

            ListBox tracklist = (ListBox)channelDescription.FindControl("lbx_tracklist");
            tracklist.DataSource = tracks;
            tracklist.DataBind();

            /*
            Label lbl_track1 = (Label)streamPlayer.FindControl("uc_track1").FindControl("lbl_trackName");
            lbl_track1.Text = "Bal Sagoth - The Dark Liege of Chaos is unleashed at the Ensorcelled Shrine of A'Zura Kai (The Splendour of a Thousand Swords Gleaming Beneath the Blazon of the Hyperborean Empire Part II)".Substring(0, 60);

            Label lbl_track2 = (Label)streamPlayer.FindControl("uc_track2").FindControl("lbl_trackName");
            lbl_track2.Text = "Arctic Monkeys - You Probably Couldn't See for the Lights but You Were Looking Straight at Me".Substring(0, 60);
            */
        }

        public void SimpleSearch()
        {

        }

        protected void topBarTest_SimpleSearchClick(object sender, EventArgs e)
        {
            string msg = "Search Clicked!";
            Response.Write("<script language='javascript'>alert('" + msg + "');</script>");
        }
    }
}