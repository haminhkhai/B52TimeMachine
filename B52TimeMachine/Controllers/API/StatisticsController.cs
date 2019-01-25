using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using B52TimeMachine.Models;
using B52TimeMachine.Dtos;
using System.Data.Entity.SqlServer;

namespace B52TimeMachine.Controllers.API
{
    public class StatisticsController : ApiController
    {
        ApplicationDbContext _context;

        public StatisticsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpPost]
        public IHttpActionResult GetRentalStatistics(RentalStatisticsParams rentalStatisticsParams)
        {
            rentalStatisticsParams.To = rentalStatisticsParams.To.AddHours(23.9999);

            var rentalStatistics = _context.Rentals.Include(r => r.Ps)
                                                   .Where(r => r.From >= rentalStatisticsParams.From && r.From <= rentalStatisticsParams.To && r.To != null)
                                                   .Select(r => new {
                                                       r.RentalId,
                                                       r.From,
                                                       r.To,
                                                       r.Ps.PsId,
                                                       r.Ps.Name,
                                                       r.RentalFee,
                                                       r.ServiceFee
                                                   }).ToList();
            List<RentalStatisticsDto> rentalStatisticsDtos = new List<RentalStatisticsDto>();

            foreach (var item in rentalStatistics)
            {
                var rentalSpan = Convert.ToDateTime(item.To).Subtract(item.From);
                rentalStatisticsDtos.Add(new RentalStatisticsDto {
                    RentalId = item.RentalId,
                    PsId = item.PsId,
                    PsName = item.Name,
                    From = item.From.ToString("d MMM HH:mm"),
                    To = string.Format("{0: d MMM HH:mm}", item.To),
                    RentalSpan = string.Format("{0}:{1}", decimal.Truncate(Convert.ToDecimal(rentalSpan.TotalHours)), rentalSpan.Minutes),
                    RentalFee = item.RentalFee.ToString("n0"),
                    ServiceFee = item.ServiceFee.ToString("n0"),
                    Total = (item.RentalFee + item.ServiceFee).ToString("n0")
                });
            }
            return Ok(rentalStatisticsDtos);
        }
    }

    public class RentalStatisticsParams
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
