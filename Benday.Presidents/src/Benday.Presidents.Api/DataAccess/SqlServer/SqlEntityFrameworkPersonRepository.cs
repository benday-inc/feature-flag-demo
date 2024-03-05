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

        public override void Save(Person saveThis)
        {
            if (saveThis.Id != 0)
            {
                var knownRelationships = (
                    from temp in Context.Relationships
                    where 
                        temp.FromPersonId == saveThis.Id ||
                        temp.ToPersonId == saveThis.Id
                    select temp).ToList();

                // delete relationships that are no longer in the list
                var relationshipsToDelete = (
                    from temp in knownRelationships
                    where
                        saveThis.Relationships.Any(
                            r => r.Id == temp.Id) == false
                    select temp).ToList();

                foreach (var temp in relationshipsToDelete)
                {
                    this.Context.Relationships.Remove(temp);
                    saveThis.Relationships.Remove(temp);
                }
            }

            base.Save(saveThis);
        }

        public override Person GetById(int id)
        {
            return (
                from temp in EntityDbSet
                    .Include(x => x.Relationships)
                        .ThenInclude(r1 => r1.ToPerson)
                    .Include(x => x.Relationships)

                    .Include(p => p.Facts)
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }
    }
}
