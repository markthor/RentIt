﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using RentItMvc.Models;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    public class AudioController : Controller
    {
        public ActionResult AudioPlayer(int channelId, int userId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.IncrementChannelPlays(channelId);
            }
            Audio audio = new Audio();
            audio.StreamUri = "http://rentit.itu.dk:27000/" + channelId.ToString();
            audio.ChannelId = channelId;
            return View(new Tuple<Audio, int>(audio, userId));
        }

        public PartialViewResult Last5Tracks(int channelId, int userId)
        {
            List<GuiTrack> guiTracks = new List<GuiTrack>();
            for (int i = 0; i < 5; i++)
            {
                GuiTrack track = new GuiTrack
                {
                    ArtistName = "Artist" + i,
                    TrackName = "Title" + i,
                    Id = 542+i,
                    ChannelId = channelId
                };
                guiTracks.Add(track);
            }

            return PartialView(new Tuple<List<GuiTrack>, int>(guiTracks, userId));
        }
    }
}