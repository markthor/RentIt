using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class AudioController : Controller
    {
        public ActionResult AudioPlayer(int channelId)
        {
            String uri = "http://rentit.itu.dk:27000/" + channelId.ToString();
            Response.Write("<script language='javascript'>openPlayer("+ uri +")</script>");
            Response.Write("<script language='javascript'>alert('Hello World')</script>");
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        private const string audioPlayerScript =
            "function playAudio() {" +
            "//Retrieve the button and player" +
            "var player = document.getElementById(\"player\");" +
            "var playstopButton = document.getElementById(\"playStop\");" +
    
            "//Retrieve icons" +
            "var playIcon = document.getElementById(\"playIcon\");" +
            "var stopIcon = document.getElementById(\"stopIcon\");" +
    
            "if (playIcon != null) { //Player is not playing" +
                "//Reload the player - effectivly rebuffering" +
                "player.load();" +

                "//Create the stopIcon and append it to the button" +
                "stopIcon = document.createElement(\"i\");" +
                "stopIcon.setAttribute(\"id\", \"stopIcon\");" +
                "stopIcon.setAttribute(\"class\", \"icon-stop icon-white\");" +
                "playstopButton.appendChild(stopIcon);" +

                "//Remove the playIcon" +
                "playstopButton.removeChild(playIcon);" +
        
                "//Start playing" +
                "player.play();" +
        
            "} else { //Player is playing" +
                "//Stop playing" +
                "player.pause();" +
        
                "//Create the playIcon and append it to the button" +
                "playIcon = document.createElement(\"i\");" +
                "playIcon.setAttribute(\"id\", \"playIcon\");" +
                "playIcon.setAttribute(\"class\", \"icon-play icon-white\");" +
                "playstopButton.appendChild(playIcon);" +
        
                "//Remove the stopIcon" +
                "playstopButton.removeChild(stopIcon);" +
            "}" +
        "}" +

        "function updateSlider(newValue) {" +
            "//Retrieve the player" +
            "var player = document.getElementById(\"player\");" +

            "//Set the volume" +
            "player.volume = newValue;" +
        "}" +

        "function openPlayer(uri) {" +
            "window.open(uri, 'playerWindow', 'width = 200, height = 400, left = 100, right = 100');" +
        "}";
    }
}