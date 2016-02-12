using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcContrib;
using MvcContrib.TestHelper;
using Business.Domain;
using Business.RepositoryInterface;
using Business.MVCWeb.Controllers;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace NorthwindSimple.WebApp.Tests.Unit
{
    /// <summary>
    /// Summary description for EmployerControllerTest
    /// </summary>
    [TestClass]
    public class EmployerControllerTest
    {
        private IQueryable<Employee> _data;
        private Mock<DbSet<Employee>> _mockSet;
        private Mock<IEmployeeRepository> _mockER;
        private Mock<IUnitOfWork> _mockUW;
        private Mock<HttpContextBase> _mockHttpContext;
        private Mock<HttpRequestBase> _mockRequest;
        private NameValueCollection _FormKeys;


        public EmployerControllerTest()
        {
            _data = new List<Employee> 
            { 
                new Employee {EmployeeID = 1,  Name = "Aimen", Address = "Address" , Age = 1, Employment = 1, Active = 1}, 
                new Employee {EmployeeID = 2,  Name = "Max", Address = "Address" , Age = 1, Employment = 1, Active = 1},
                new Employee {EmployeeID = 3,  Name = "Alex", Address = "Address" , Age = 1, Employment = 1, Active = 1},
            }.AsQueryable();


            _mockSet = new Mock<DbSet<Employee>>();
            _mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(_data.Provider);
            _mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(_data.Expression);
            _mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(_data.ElementType);
            _mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());


            _mockER = new Mock<IEmployeeRepository>();
            _mockER.Setup(t => t.Get()).Returns(_mockSet.Object);
            _mockER.Setup(t => t.Get(x => x.EmployeeID == It.IsAny<int>())).Returns(_mockSet.Object);
            _mockER.Setup(t => t.Add(It.IsAny<Employee>())).Callback((Employee employee) =>
            {
                var newListEmployee = new List<Employee> { employee };
                _data = _data.Concat(newListEmployee);
            }).Verifiable();
            _mockER.Setup(t => t.Delete(It.IsAny<Employee>())).Callback((Employee employee) =>
            {
                //var newListEmployee = new List<Employee> { employee };
                var list = new List<Employee>();
                list = _data.ToList();
                list.Remove(employee);
                _data = list.AsQueryable();
            }).Verifiable();
            //_mockER.Verify(mr => mr.Update(It.IsAny<Employee>()), Times.Once());
            //_mockER.Setup(t => t.Delete(It.IsAny<int>()));
            //_mockER.Setup(t => t.Delete(It.IsAny<Employee>()));
            _mockER.Setup(t => t.Update(It.IsAny<Employee>()));

            _mockUW = new Mock<IUnitOfWork>();
            _mockUW.Setup(t => t.SaveChanges());

            _mockHttpContext = new Mock<HttpContextBase>();
            _mockRequest = new Mock<HttpRequestBase>();
            _FormKeys = new NameValueCollection();
            _mockHttpContext.Setup(ctxt => ctxt.Request).Returns(_mockRequest.Object);
            _mockRequest.Setup(r => r.Form).Returns(_FormKeys);


        }


        //reference : http://msdn.microsoft.com/en-au/data/dn314429.aspx
        //reference : http://www.codeproject.com/Articles/47603/Mock-a-database-repository-using-Moq
        //reference : http://stackoverflow.com/questions/15805812/mock-an-update-method-returning-a-void-with-moq

        [TestMethod]
        public void Default_Action_Returns_Index_View()
        {
            // Arrange     
            var employeesController = new EmployeesController(_mockER.Object, null);

            const string expectedViewName = "Index";
            //Act
            var result = employeesController.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            result.AssertViewRendered().WithViewData<IEnumerable<Employee>>();
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
        }


        [TestMethod]
        public void Default_Action_Returns_Index_View_Using_MvcContrib_TestHelper()
        {
            // Arrange              
            var employeesController = new EmployeesController(_mockER.Object, null);
            var routeData = new RouteData();

            // Act
            var result = employeesController.Index();

            // Assert
            result.AssertViewRendered().ForView("Index");
            result.AssertViewRendered().WithViewData<IEnumerable<Employee>>();
        }


        [TestMethod]
        public void The_Add_Employee_Action_Returns_RedirectToRouteResult_When_The_Employee_Model_Is_Valid()
        {
            // Arrange  
            const string expectedRouteName = "EmployeeCreated";
            var employee = new Employee()
            {
                EmployeeID = 1,
                Name = "Aimen",
                Address = "Address",
                Age = 1,
                Employment = 1,
                Active = 1
            };

            var employeesController = new EmployeesController(_mockER.Object, _mockUW.Object);
            var routeData = new RouteData();
            _FormKeys.Add("Create", "Create");
            employeesController.ControllerContext =
                new ControllerContext(_mockHttpContext.Object, routeData, employeesController);
            employeesController.ModelState.Clear();
            // Act
            var result = employeesController.Create(employee) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(4, _data.Count());
            Assert.IsNotNull(result, "Should have returned a RedirectToRouteResult");
            // Assert.AreEqual(expectedRouteName, result.RouteName, "Route name should have been {0}", expectedRouteName);

        }

        [TestMethod]
        public void The_Add_Employee_Action_Returns_ViewResult_When_The_Employee_Model_Is_Invalid()
        {
            // Arrange  
            const string expectedRouteName = "EmployeeCreated";
            var employee = new Employee()
            {
                EmployeeID = 1,
                Name = "Aimen",
                Address = "Address",
                Age = 1,
                Employment = 1,
                Active = 1
            };

            var employeesController = new EmployeesController(_mockER.Object, _mockUW.Object);
            employeesController.ModelState.AddModelError("A Error", "Message");
            // Act
            var result = employeesController.Create(employee) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(3, _data.Count());
            Assert.IsNull(result, "Should have returned a RedirectToRouteResult");
        }


        [TestMethod]
        public void The_Delete_Employee_Action_Returns_RedirectToRouteResult_When_The_Employee_Model_Is_Valid()
        {
            // Arrange  
            const string expectedRouteName = "EmployeeDeleted";
            var employee = new Employee()
            {
                EmployeeID = 1,

            };

            var employeesController = new EmployeesController(_mockER.Object, _mockUW.Object);
            var routeData = new RouteData();
            employeesController.ControllerContext =
                new ControllerContext(_mockHttpContext.Object, routeData, employeesController);

            employeesController.ModelState.Clear();
            // Act
            var result = employeesController.DeleteConfirmed(1) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(2, _data.Count());
            Assert.IsNotNull(result, "Should have returned a RedirectToRouteResult");
            // Assert.AreEqual(expectedRouteName, result.RouteName, "Route name should have been {0}", expectedRouteName);

        }






    }
}
