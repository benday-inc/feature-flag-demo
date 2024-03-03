using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Benday.Presidents.Api.DataAccess
{
    public class PersonFact : Int32Identity
    {
        public PersonFact()
        {

        }

        public int PersonId { get; set; }
        public Person Person { get; set; }
        public string FactType { get; set; }
        public string FactValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
