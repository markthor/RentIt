﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace RentItMvc.Models
{
    public class ChannelList
    {
        public IEnumerable<Channel> Children { get; set; }
    }
}