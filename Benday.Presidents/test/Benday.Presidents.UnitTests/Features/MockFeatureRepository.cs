using Benday.Presidents.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.UnitTests.Features
{
    public class MockFeatureRepository : IFeatureRepository
    {

        public MockFeatureRepository()
        {
            GetByUsernameReturnValue = new List<Feature>();
        }

        public void Delete(Feature deleteThis)
        {
            throw new NotImplementedException();
        }

        public IList<Feature> GetAll()
        {
            throw new NotImplementedException();
        }

        public Feature GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Feature> GetByUsernameReturnValue
        {
            get;
            set;
        }

        public IList<Feature> GetByUsername(string username)
        {
            return GetByUsernameReturnValue;
        }

        public void Save(Feature saveThis)
        {
            throw new NotImplementedException();
        }
    }
}
