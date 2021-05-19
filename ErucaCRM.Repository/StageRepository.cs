using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;

namespace ErucaCRM.Repository
{
    public class StageRepository : BaseRepository<Stage>
    {
        public StageRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public bool DeleteStage(int oldstageId, int newStageId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            entities.SSP_DeleteStageAfterMoveLeads(oldstageId, newStageId);
            return true;
        }
    }
}