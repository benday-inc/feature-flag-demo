using Benday.Presidents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Benday.Presidents.Api.DataAccess.SqlServer
{
    public class SqlEntityFrameworkFeatureRepository :
            SqlEntityFrameworkCrudRepositoryBase<Feature>, IFeatureRepository
    {
        public SqlEntityFrameworkFeatureRepository(
            IPresidentsDbContext context) : base(context)
        {

        }

        protected override DbSet<Feature> EntityDbSet => Context.Features;

        public IList<Feature> GetByUsername(string username)
        {
            return (
                from temp in EntityDbSet
                where (temp.Username == username || temp.Username == String.Empty)
                select temp
                ).ToList();
        }
    }
}
