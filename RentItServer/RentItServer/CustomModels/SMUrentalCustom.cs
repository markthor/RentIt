﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.SMU;

namespace RentItServer
{
    public partial class SMUrental
    {
        public Rental GetRental()
        {
            return new Rental(userId, bookId, startDate, mediaType, SMUbook, SMUuser);
        }
    }
}