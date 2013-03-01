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
        }
    }
}