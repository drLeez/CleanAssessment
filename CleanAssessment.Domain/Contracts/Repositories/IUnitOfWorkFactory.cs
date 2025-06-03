using CleanAssessment.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        public UnitOfWorkFactory(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentException(nameof(contextFactory));
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_contextFactory.CreateDbContext());
        }
    }
}
