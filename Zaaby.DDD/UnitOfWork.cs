using System;
using System.Data;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Infrastructure;

namespace Zaaby.DDD
{
    public class UnitOfWork : IUnitOfWork
    {
        public Guid Id { get; }

        public IDbConnection Connection { get; private set; }

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
            Transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            Connection?.Dispose();
            Connection = null;
            Transaction?.Dispose();
            Transaction = null;
        }
    }
}