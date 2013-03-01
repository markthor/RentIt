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
                lbl_channelName.Text = (string)Session["ChannelName"];
                tbx_description.Text = (string)Session["ChannelDescription"];
                InitGenres();      
                InitDatabindings();
                Session["GenreList"] = genreList;
                Session["Genres"] = genres; 
            }    
        }

        protected void btn_addGenre_Click(object sender, EventArgs e)
        {
            string s = lbx_availableGenres.SelectedItem.Text;
            genres = (List<string>)Session["Genres"];
            genres.Add(s);
            lbx_chosenGenres.DataSource = genres;
            lbx_chosenGenres.DataBind();
            genreList = (List<string>)Session["GenreList"];
            genreList.Remove(s);
            lbx_availableGenres.DataSource = genreList;
            lbx_availableGenres.DataBind();
        }

        protected void btn_deleteGenre_Click(object sender, EventArgs e)
        {
            string s = lbx_chosenGenres.SelectedItem.Text;
            genres = (List<string>)Session["Genres"];
            genres.Remove(s);
            lbx_chosenGenres.DataSource = genres;
            lbx_chosenGenres.DataBind();
            genreList = (List<string>)Session["GenreList"];
            genreList.Add(s);
            lbx_availableGenres.DataSource = genreList;
            lbx_availableGenres.DataBind();
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
            lbx_availableGenres.DataSource = genreList;
            lbx_availableGenres.DataBind();
            lbx_chosenGenres.DataSource = genres;
            lbx_chosenGenres.DataBind();
        }

        protected void btn_saveChanges_Click(object sender, EventArgs e)
        {

        }

        protected void btn_discardChanges_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}