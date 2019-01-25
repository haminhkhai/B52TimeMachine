using B52TimeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace B52TimeMachine.Controllers.API
{
    public class PsController : ApiController
    {
        ApplicationDbContext _context;

        public PsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public IHttpActionResult GetPss()
        {
            var pss = _context.Pss.OrderBy(p => p.PsId).ToList();
            return Ok(pss);
        }

        [HttpPost]
        public IHttpActionResult CreatePs(Ps ps)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data invalid");

            ps.IsAvailable = true;
            _context.Pss.Add(ps);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult EditPs(int Id, Ps ps)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data invalid");

            var psInDb = _context.Pss.SingleOrDefault(p => p.PsId == Id);

            if (psInDb == null)
                return BadRequest("Ps Id invalid");

            psInDb.Name = ps.Name;
            psInDb.Price = ps.Price;
            psInDb.IsVisible = ps.IsVisible;

            _context.SaveChanges();

            return Ok();
        }
    }
}
