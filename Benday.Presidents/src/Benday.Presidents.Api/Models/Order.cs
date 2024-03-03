using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.Api.Models
{
    public class Order : Int32Identity
    {
        public string ShipToName { get; set; }
        public string ShipToAddress { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToPostalCode { get; set; }
                
        public string InternalOrderNumber { get; set; }
        public string ApprovedBy { get; set; }
        public double ProfitAmount { get; set; }
    }
}
