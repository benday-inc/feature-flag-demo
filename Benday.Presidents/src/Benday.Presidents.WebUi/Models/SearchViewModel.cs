using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Benday.Presidents.WebUI.Models
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            Results = new List<SearchResultRow>();
            FirstName = String.Empty;
            LastName = String.Empty;
        }

        public List<SearchResultRow> Results { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Birth State")]
        public string BirthState { get; set; }

        [Display(Name = "Death State")]
        public string DeathState { get; set; }
    }
}
