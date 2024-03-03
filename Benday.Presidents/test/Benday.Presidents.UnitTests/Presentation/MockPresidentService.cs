using Benday.Presidents.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.UnitTests.Presentation
{
    public class MockPresidentService : IPresidentService
    {

        public MockPresidentService()
        {
            GetPresidentsReturnValue = new List<President>();
        }

        public void DeletePresidentById(int id)
        {
            throw new NotImplementedException();
        }

        public President GetPresidentByIdReturnValue { get; set; }
        public President GetPresidentById(int id)
        {
            return GetPresidentByIdReturnValue;
        }

        public IList<President> GetPresidentsReturnValue { get; set; }

        public IList<President> GetPresidents()
        {
            return GetPresidentsReturnValue;
        }

        public President SavePresidentArgumentValue { get; set; }
        public void Save(President saveThis)
        {
            SavePresidentArgumentValue = saveThis;
        }

        public IList<President> SearchReturnValueForStateSearch { get; set; }

        public IList<President> Search(string firstName, string lastName, string birthState, string deathState)
        {
            return SearchReturnValueForStateSearch;
        }

        public IList<President> SearchReturnValue { get; set; }

        public IList<President> Search(string firstName, string lastName)
        {
            return SearchReturnValue;
        }
    }
}
