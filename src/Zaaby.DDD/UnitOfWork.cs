using System;
using System.Data;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Infrastructure;

namespace Zaaby.DDD
{
    public class UnitOfWork : IUnitOfWork
    {
        public Guid Id { get; }
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbConnection connection)
        {
            Id = SequentialGuidHelper.GenerateComb();
            Connection = connection;
        }

        public void Begin()
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction.Dispose();
            }
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction.Dispose();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}