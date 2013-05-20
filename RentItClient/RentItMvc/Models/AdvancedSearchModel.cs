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

        public static explicit operator ChannelSearchArgs(AdvancedSearchModel model)
        {
            return new ChannelSearchArgs
            {
                SearchString = model.SearchString,
                SortOption = model.SortingKey + model.SortingBy,
                StartIndex = model.StartIndex,
                EndIndex = model.EndIndex,
                MinNumberOfSubscriptions = model.MinAmountOfSubscribers != null ? model.MinAmountOfSubscribers.Value : -1,
                MaxNumberOfSubscriptions = model.MaxAmountOfSubscribers != null ? model.MaxAmountOfSubscribers.Value : int.MaxValue,
                MinNumberOfComments = model.MinAmountOfComments != null ? model.MinAmountOfComments.Value : -1,
                MaxNumberOfComments = model.MaxAmountOfComments != null ? model.MaxAmountOfComments.Value : int.MaxValue,
                MinAmountPlayed = model.MinAmountOfPlays != null ? model.MinAmountOfPlays.Value : -1,
                MaxAmountPlayed = model.MaxAmountOfPlays != null ? model.MaxAmountOfPlays.Value : int.MaxValue,
                MinTotalVotes = model.MinAmountOfVotes != null ? model.MinAmountOfVotes.Value : -1,
                MaxTotalVotes = model.MaxAmountOfVotes != null ? model.MaxAmountOfVotes.Value : int.MaxValue
            };
        }

        public static explicit operator AdvancedSearchModel(ChannelSearchArgs args)
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
                MaxAmountOfPlays = args.MaxAmountPlayed,
                MinAmountOfVotes = args.MinTotalVotes,
                MaxAmountOfVotes = args.MaxTotalVotes
            };
        }
    }
}