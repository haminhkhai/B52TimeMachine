using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B52TimeMachine.Models;
using B52TimeMachine.ViewModels;
using B52TimeMachine.Dtos;

namespace B52TimeMachine.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var playingRental = from r in _context.Rentals
                                where r.To == null
                                select new { r.RentalId, r.Ps.PsId };


            var psRentalDtos = (from p in _context.Pss
                                where p.IsVisible == true
                                join r in playingRental on p.PsId equals r.PsId into lj
                                from g in lj.DefaultIfEmpty()
                                select  (new PsRentalDto
                                 { 
                                     PsId = p.PsId, 
                                     Name = p.Name, 
                                     IsAvailable = p.IsAvailable, 
                                     Price = p.Price, 
                                     RentalId = (g == null ? 0 : g.RentalId)
                                 })).ToList();


            var indexViewModel = new IndexViewModel 
            {
                PsRentalDtos = psRentalDtos,
                Services = _context.Services.Where(s => s.IsVisible == true).OrderBy(s => s.Order).ToList()
            };
            return View(indexViewModel);
        }
    }
}