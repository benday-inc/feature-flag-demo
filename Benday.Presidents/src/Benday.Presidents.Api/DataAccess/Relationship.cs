using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.Api.DataAccess
{
    public class Relationship : Int32Identity
    {

        public Relationship()
        {

        }

        public int FromPersonId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("FromPersonId")]
        public virtual Person FromPerson { get; set; }

        public int ToPersonId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ToPersonId")]
        public virtual Person ToPerson { get; set; }

        public string RelationshipType { get; set; }
    }
}
