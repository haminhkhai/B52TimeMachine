using B52TimeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B52TimeMachine.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class WarehouseController : Controller
    {
        ApplicationDbContext _context;

        public WarehouseController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Warehouse
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Restock()
        {
            return View();
        }

        public ActionResult CheckQuantity()
        {
            return View();
        }
    }
}