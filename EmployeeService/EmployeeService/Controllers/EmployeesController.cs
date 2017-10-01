using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    //[EnableCors("*","*","*")]
    public class EmployeesController : ApiController
    {
        public EmployeeEntities db = new EmployeeEntities();
        //[RequireHttps]
        [BasicAuthentication]
        public HttpResponseMessage Get(string gender="All")
        {

            string userName = Thread.CurrentPrincipal.Identity.Name;

            switch (userName.ToLower())
            {
                case "all":
                    return Request.CreateResponse(HttpStatusCode.OK, db.Employees.ToList());
                case "male":
                    return Request.CreateResponse(HttpStatusCode.OK,
                        db.Employees.Where(x => x.Gender.ToLower() == "male").ToList());
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK,
                        db.Employees.Where(x => x.Gender.ToLower() == "female").ToList());
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetEmployeeById(int id)
        {
            var entitiy = db.Employees.FirstOrDefault(x => x.Id == id);

            if (entitiy != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, entitiy);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id =" + id + " Not foound");
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri + employee.Id.ToString());
                return message;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var entity = db.Employees.FirstOrDefault(x => x.Id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with id = " + id + " not found todelete");
                }
                else
                {
                    db.Employees.Remove(entity);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        public void Put([FromBody]int id, [FromUri] Employee employee)
        {
            var entity = db.Employees.FirstOrDefault(x => x.Id == id);
            entity.FirstName = employee.FirstName;
            entity.LastName = employee.LastName;
            entity.Gender = employee.Gender;
            db.SaveChanges();
        }

    }
}
