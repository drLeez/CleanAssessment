using CleanAssessment.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public interface IUnitOfWorkFactory<I>
    {
        IUnitOfWork<I> Create();
    }
    public class UnitOfWorkFactory<I> : IUnitOfWorkFactory<I>
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        public UnitOfWorkFactory(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentException(nameof(contextFactory));
        }

        public IUnitOfWork<I> Create()
        {
            return new UnitOfWork<I>(_contextFactory.CreateDbContext());
        }
    }
}
