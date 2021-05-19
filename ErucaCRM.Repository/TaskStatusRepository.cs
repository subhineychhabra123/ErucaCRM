using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;

namespace ErucaCRM.Repository
{
    public class TaskStatusRepository : BaseRepository<TaskStatu>
    {
        public TaskStatusRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
