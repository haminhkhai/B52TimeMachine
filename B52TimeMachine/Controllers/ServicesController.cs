using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B52TimeMachine.Models;

namespace B52TimeMachine.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class ServicesController : Controller
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
        //
        // GET: /Services/
        public ActionResult Index()
        {
            return View("List");
        }

        public ActionResult New()
        {

            var lastService = _context.Services.OrderByDescending(s => s.Order).ToList();

            var service = new Service 
            { 
                ServiceId = 0,
                Order = lastService.Count == 0 ? 1 : lastService[0].Order + 1,
                IsVisible = true
            };
            return View("Form", service);
        }

        public ActionResult Edit(int Id)
        {
            var service = _context.Services.SingleOrDefault(s => s.ServiceId == Id);

            if (service == null)
                return HttpNotFound();

            return View("Form", service);
        }
	}
}