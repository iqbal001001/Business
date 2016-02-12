using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Business.WebApi;
using Business.RepositoryInterface;
using Business.Domain;
using System.Web.Http.Cors;

namespace Business.WebApi.Controllers
{
    [RoutePrefix("api/employee")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        // GET api/Employee
        [Route("")]
        public IEnumerable<Employee> Get()
        {
            var employees = _employeeRepository.Get().ToList();
            return employees;
           

        }

        // GET api/employee/5
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            Employee employee = _employeeRepository.Get().FirstOrDefault(e => e.EmployeeID == id);
            
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse<Employee>(employee);
        }

        // PUT api/employee/5
        [Route("{id}")]
        public HttpResponseMessage Put(string id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != employee.EmployeeID.ToString())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            _employeeRepository.Update(employee);


            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        // POST api/employee
        [Route("")]
        public HttpResponseMessage Post(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(employee);
              
                _unitOfWork.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, employee);
               // response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = employee.EmployeeID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/employee/5
        [Route("{id}")]
        public HttpResponseMessage Delete(string id)
        {
            Employee employee = _employeeRepository.Get().FirstOrDefault(e => e.EmployeeID.ToString() == id);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            _employeeRepository.Delete(employee);

            try
            {
                _unitOfWork.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        //[Route("")]
        //private bool EmployeeExists(int id)
        //{
        //    return _employeeRepository.Get().Count(e => e.EmployeeID == id) > 0;
        //}
    }
}