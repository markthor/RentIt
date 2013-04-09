using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.SMU;

namespace RentItServer
{
    /// <summary>
    /// This partial class extends functionality of the SMUrental entity used by the entity framework
    /// </summary>
    public partial class SMUrental
    {
        /// <summary>
        /// Gets the rental representation of this object.
        /// </summary>
        /// <returns></returns>
        public Rental GetRental()
        {
            return new Rental(id, userId, bookId, startDate, mediaType);
        }
    }
}