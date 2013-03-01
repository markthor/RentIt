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
        //genres assosiated with channel
        private List<string> genres;
        //available genres
        private List<string> genreList;
        // content of the description textbox
        private string channelDescription;
        
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!Page.IsPostBack)
            {
                tbx_channelName.Text = Request.QueryString["name"];
                tbx_channelName.ReadOnly = true;
                InitGenres();      
                InitDatabindings();
                Session["GenreList"] = genreList;
                Session["Genres"] = genres; 
            }    
        }

        protected void btn_addGenre_Click(object sender, EventArgs e)
        {
            string s = lb_genrelist.SelectedItem.Text;
            genres = (List<string>)Session["Genres"];
            genres.Add(s);
            lb_genres.DataSource = genres;
            lb_genres.DataBind();
            genreList = (List<string>)Session["GenreList"];
            genreList.Remove(s);
            lb_genrelist.DataSource = genreList;
            lb_genrelist.DataBind();
        }

        protected void btn_deleteGenre_Click(object sender, EventArgs e)
        {
            string s = lb_genres.SelectedItem.Text;
            genres = (List<string>)Session["Genres"];
            genres.Remove(s);
            lb_genres.DataSource = genres;
            lb_genres.DataBind();
            genreList = (List<string>)Session["GenreList"];
            genreList.Add(s);
            lb_genrelist.DataSource = genreList;
            lb_genrelist.DataBind();
        }

        private void InitGenres()
        {
            genreList = new List<string>();
            genres = new List<string>();
            genreList.Add("Rock");
            genreList.Add("Pop");
            genreList.Add("Dancehall");
            genreList.Add("Techno");
            genreList.Add("Reggae");
        }

        private void InitDatabindings()
        {
            lb_genrelist.DataSource = genreList;
            lb_genrelist.DataBind();
            lb_genres.DataSource = genres;
            lb_genres.DataBind();
        }
    }
}