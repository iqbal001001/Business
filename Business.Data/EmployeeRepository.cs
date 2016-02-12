using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Domain;
using Business.RepositoryInterface;

namespace Business.Data
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDbContextFactory contextFactory) : base(contextFactory) { }
    }

}
