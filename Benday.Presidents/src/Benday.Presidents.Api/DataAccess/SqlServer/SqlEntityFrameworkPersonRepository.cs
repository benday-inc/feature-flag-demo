using Benday.Presidents.Common;
using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Benday.Presidents.Api.DataAccess.SqlServer
{
    public class SqlEntityFrameworkPersonRepository : 
        SqlEntityFrameworkCrudRepositoryBase<Person>, IRepository<Person>
    {
        public SqlEntityFrameworkPersonRepository(
            IPresidentsDbContext context) : base(context)
        {

        }

        protected override DbSet<Person> EntityDbSet
        {
            get
            {
                return Context.Persons;
            }
        }

        public override IList<Person> GetAll()
        {
            return (
                from temp in EntityDbSet
                    .Include(x => x.Relationships)
                    .Include(p => p.Facts)
                orderby temp.LastName, temp.FirstName
                select temp
                ).ToList();
        }

        public override Person GetById(int id)
        {
            return (
                from temp in EntityDbSet
                    .Include(x => x.Relationships)
                        .ThenInclude(r1 => r1.ToPerson)
                    .Include(x => x.Relationships)
                        .ThenInclude(r => r.FromPerson)
                    .Include(p => p.Facts)
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }
    }
}
