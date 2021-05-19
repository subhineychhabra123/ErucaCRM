using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Repository
{
    public partial class Entities : DbContext
    {
        public override int SaveChanges()//SaveOptions options
        {
            ChangeTracker.DetectChanges(); // Important!

            ObjectContext ctx = ((IObjectContextAdapter)this).ObjectContext;

            List<ObjectStateEntry> objectStateEntryList =
                ctx.ObjectStateManager.GetObjectStateEntries(EntityState.Added
                                                           | EntityState.Modified
                                                           | EntityState.Deleted)
                .ToList();

            foreach (ObjectStateEntry entry in objectStateEntryList)
            {
                if (!entry.IsRelationship)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // write log...
                            break;
                        case EntityState.Deleted:
                            // write log...
                            break;
                        case EntityState.Modified:
                            break;
                    }
                }
            }
            //foreach (ObjectStateEntry entry in
            //    ObjectStateManager.GetObjectStateEntries(
            //    EntityState.Added | EntityState.Modified))
            //{
            //    // Validate the objects in the Added and Modified state
            //    // if the validation fails throw an exeption.
            //}
            return base.SaveChanges();
        }

    }
}
