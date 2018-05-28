using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class PersonsController : ApiController
    {
        private MyRepository _repository = new MyRepository();

        public IHttpActionResult GetPersons()
        {
            var persons = _repository.GetAllPersons();
            return Json(persons);
        }

        [JWTFilter]
        public IHttpActionResult GetPerson(int id)
        {
            var person = _repository.GetAllPersons().FirstOrDefault(p => p.Id == id);
            return Json(person);
        }

        [JWTFilter]
        [HttpPost]
        public HttpResponseMessage SavePerson(Person person)
        {
            var result = _repository.SavePerson(person);
            HttpResponseMessage response;
            if (result)
            {
                response = Request.CreateResponse(HttpStatusCode.Created, person);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, person);
            }

            return response;
        }
    }
}
