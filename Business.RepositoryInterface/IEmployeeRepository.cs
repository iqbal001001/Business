using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Domain;

namespace Business.RepositoryInterface
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
    }
}
