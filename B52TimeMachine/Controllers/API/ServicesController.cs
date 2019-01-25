using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using B52TimeMachine.Models;

namespace B52TimeMachine.Controllers.API
{
    public class ServicesController : ApiController
    {
        ApplicationDbContext _context;
        public ServicesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public IHttpActionResult GetServices()
        {
            List<Service> services = _context.Services.Where(s => s.IsVisible == true).OrderBy(s => s.Order).ToList();
            return Ok(services);
        }

        [HttpPost]
        public IHttpActionResult NewService(Service service)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data invalid");
            _context.Services.Add(service);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditService(int Id, Service service)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data invalid");

            var serviceInDb = _context.Services.SingleOrDefault(s => s.ServiceId == Id);

            if (serviceInDb == null)
                return BadRequest("Service invalid");

            var lastOrder = serviceInDb.Order;
            serviceInDb.Order = service.Order;
            serviceInDb.Name = service.Name;
            serviceInDb.Price = service.Price;
            serviceInDb.IsVisible = service.IsVisible;
            serviceInDb.IsCig = service.IsCig;

            _context.SaveChanges();

            //sorting 
            if (lastOrder != service.Order)
            {
                var x = 1;
                List<Service> services = _context.Services.OrderBy(s => s.Order).ToList();
                services.ForEach(s => s.Order = x++);
                _context.SaveChanges();
            }

            

            return Ok();
        }
    }
}
