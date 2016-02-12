using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Business.Data;
using Business.RepositoryInterface;

namespace Business.DIResolver
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<EmployeeRepository>()
                .As<IEmployeeRepository>()
                .InstancePerRequest();

            builder
                .RegisterType<BusinessDbContextFactory>()
                .As<IDbContextFactory>()
                .InstancePerRequest();

            builder
                .RegisterType<BusinessUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            base.Load(builder);
        }
    }
}
