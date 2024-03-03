using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.Presidents.WebUI.Models
{
    public class SearchResultRow
    {
        public SearchResultRow()
        {
            
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
