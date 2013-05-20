using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItMvc.RentItService;

namespace RentItMvc.Models
{
    public class AdvancedSearchModel
    {
        public string SearchString { get; set; }
        public string SortingKey { get; set; }
        public string SortingBy { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int? MinAmountOfSubscribers { get; set; }
        public int? MaxAmountOfSubscribers { get; set; }
        public int? MinAmountOfComments { get; set; }
        public int? MaxAmountOfComments { get; set; }
        public int? MinAmountOfPlays { get; set; }
        public int? MaxAmountOfPlays { get; set; }
        public int? MinAmountOfVotes { get; set; }
        public int? MaxAmountOfVotes { get; set; }

        public static AdvancedSearchModel GetAdvancedSearchModel(ChannelSearchArgs args)
        {
            return new AdvancedSearchModel
            {
                SearchString = args.SearchString,
                SortingKey = !args.SortOption.Equals("") ? args.SortOption.Substring(0, args.SortOption.IndexOf(" ")) : "",
                SortingBy = !args.SortOption.Equals("") ? args.SortOption.Substring(args.SortOption.IndexOf(" ") + 1) : "",
                StartIndex = args.StartIndex,
                EndIndex = args.EndIndex,
                MinAmountOfSubscribers = args.MinNumberOfSubscriptions,
                MaxAmountOfSubscribers = args.MaxNumberOfSubscriptions,
                MinAmountOfComments = args.MinNumberOfComments,
                MaxAmountOfComments = args.MaxNumberOfComments,
                MinAmountOfPlays = args.MinAmountPlayed,
                MaxAmountOfPlays = args.MaxAmountPlayed
            };
        }
    }
}