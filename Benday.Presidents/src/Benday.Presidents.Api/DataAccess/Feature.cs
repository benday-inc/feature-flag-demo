using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Presidents.Api.DataAccess
{
    public class Feature : Int32Identity
    {
        [Display(Name = "Feature Name")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        [Display(Name = "Is Enabled")]
        public bool IsEnabled { get; set; }

        [Display(Name = "For Username")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Username { get; set; }
    }
}
