using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepositoryInterface;

namespace Business.Data
{
    public class BusinessUnitOfWork : IUnitOfWork
    {
        private IDbContextFactory _contextFactory;
        private BusinessEntities _context;

        public BusinessUnitOfWork(IDbContextFactory contextFactory)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }

            _contextFactory = contextFactory;
        }

        protected BusinessEntities Context
        {
            get { return _context ?? (_context = _contextFactory.Get()); }
        }

        public void SaveChanges()
        {
            //Context.Commit();
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

}
