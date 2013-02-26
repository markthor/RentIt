using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentItClient
{
    public partial class EditChannel : System.Web.UI.Page
    {
        private List<string> genres;
        private List<string> genreList;
        protected void Page_Load(object sender, EventArgs e)
        {
            tbx_channelName.Text = Request.QueryString["name"];
            tbx_channelName.ReadOnly = true;
            genreList = new List<string>();
            genres = new List<string>();
            genreList.Add("Rock");
            genreList.Add("Pop");
            genreList.Add("Dancehall");
            lb_genrelist.DataSource = genreList;
            lb_genrelist.DataBind();
            lb_genres.DataSource = genres;
            lb_genres.DataBind();
        }

        protected void btn_addGenre_Click(object sender, EventArgs e)
        {
            string s = lb_genrelist.SelectedItem.Text;
           // genres.Add(s);
            lb_genres.DataBind();
        }
    }
}