

using ErucaCRM.Repository.Infrastructure.Contract;
using System.Data.Entity;
using System.Data.Objects;
using System.Transactions;
using ErucaCRM.Repository;

namespace ErucaCRM.Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transaction;
        private readonly Entities _db;


        public UnitOfWork()
        {
            _db = new Entities();
          
        }

        public void Dispose()
        {
           
        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _db.SaveChanges();
            _transaction.Complete();
        }

        public DbContext Db
        {
            get { return _db; }
        }


        
    }
}
