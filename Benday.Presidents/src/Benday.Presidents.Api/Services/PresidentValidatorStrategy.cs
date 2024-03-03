using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.Api.Services
{
    public class PresidentValidatorStrategy : IPresidentValidatorStrategy
    {
        public bool IsValid(President validateThis)
        {
            if (validateThis == null)
            {
                return false;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(validateThis.FirstName) == true)
                {
                    return false;
                }
                else if(String.IsNullOrWhiteSpace(validateThis.LastName) == true)
                {
                    return false;
                }
                else if (validateThis.Terms.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
