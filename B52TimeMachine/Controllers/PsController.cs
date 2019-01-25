using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B52TimeMachine.Models;

namespace B52TimeMachine.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class PsController : Controller
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View("Form", new Ps { PsId = 0, IsVisible = true});
        }

        public ActionResult Edit(int Id)
        {
            var ps = _context.Pss.SingleOrDefault(p => p.PsId == Id);

            if (ps == null)
                return HttpNotFound();

            return View("Form", ps);
        }
    }
}