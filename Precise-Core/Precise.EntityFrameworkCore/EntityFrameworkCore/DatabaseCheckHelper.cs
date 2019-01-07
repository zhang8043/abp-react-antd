using System;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Precise.EntityFrameworkCore
{
    public class DatabaseCheckHelper : ITransientDependency
    {
        private readonly IDbContextProvider<PreciseDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DatabaseCheckHelper(
            IDbContextProvider<PreciseDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager
        )
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public bool Exist(string connectionString)
        {
            if (connectionString.IsNullOrEmpty())
            {
                //connectionString is null for unit tests
                return true;
            }

            try
            {
                using (var uow =_unitOfWorkManager.Begin())
                {
                    // Switching to host is necessary for single tenant mode.
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        _dbContextProvider.GetDbContext().Database.OpenConnection();
                        uow.Complete();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
