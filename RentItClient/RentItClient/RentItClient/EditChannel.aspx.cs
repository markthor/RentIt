using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RentItClient
{
    public partial class EditChannel : System.Web.UI.Page
    {
        //genres assosiated with channel
        private List<string> genres;
        //available genres
        private List<string> genreList;
        // tracklist assosiated with channel
        private List<string> trackList;
        // content of the description textbox
        private string channelDescription;
        
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!Page.IsPostBack)
            {
                string channelName = (string)Session["ChannelName"];
                lbl_channelName.Text = channelName;
                InitGenres();
                InitTrackList();
                InitChannelDescription();
            }    
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
            // sets databindings
            lbx_availableGenres.DataSource = genreList;
            lbx_availableGenres.DataBind();
            lbx_chosenGenres.DataSource = genres;
            lbx_chosenGenres.DataBind();
            // saves session data
            Session["GenreList"] = genreList;
            Session["Genres"] = genres;
        }

        private void InitTrackList()
        {
            trackList = new List<string>();
            trackList.Add("Test Track1");
            trackList.Add("Test Track2");
            lbx_trackList.DataSource = trackList;
            lbx_trackList.DataBind();
            Session["TrackList"] = trackList;
        }


        private void InitChannelDescription()
        {
            channelDescription = "Enter description here...";
            tbx_description.Text = channelDescription;
        }

        /// <summary>
        /// Adds selected genre from avaliable genres to the list of genres assosiated with the channel.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs</param>
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

        /// <summary>
        /// Deletes a genre from the genrelist assosiated with the channel
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Eventargs</param>
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

        /// <summary>
        /// Saves changes made to the channel
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Eventargs</param>
        protected void btn_saveChanges_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Discards changes made to the channel
        /// </summary>
        /// <param name="sender">Objects sender</param>
        /// <param name="e">Eventargs</param>
        protected void btn_discardChanges_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        /// <summary>
        /// Uploads a selected song to the server and adds it to the list of tracks assosiated with the channel
        /// </summary>
        /// <param name="sender">Objects sender</param>
        /// <param name="e">Eventargs</param>
        protected void btn_uploadSong_Click(object sender, EventArgs e)
        {
            if (flu_trackUpload.HasFile)
            {
                string extensions = System.IO.Path.GetExtension(flu_trackUpload.FileName);
                if (extensions == ".mp3")
                {
                    using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                    {
                        Stream track = flu_trackUpload.FileContent;
                        string s = proxy.GetData(10);
                    }
                }
            }
            
        }
    }
}