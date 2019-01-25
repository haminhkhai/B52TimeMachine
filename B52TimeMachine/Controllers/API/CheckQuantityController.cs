using B52TimeMachine.Dtos;
using B52TimeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace B52TimeMachine.Controllers.API
{
    public class CheckQuantityController : ApiController
    {
        ApplicationDbContext _context;

        public CheckQuantityController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        [Route("Api/CheckQuantity/GetCheckQuantities/{month?}")]
        public IHttpActionResult GetCheckQuantities(int? month)
        {
            month = month == null ? 0 : month;
            var checkQuantities = _context.CheckQuantities.Where(c => c.CheckDate.Month == month).ToList();

            if (checkQuantities.Count < 1)
                return BadRequest("There are 0 check quantity in this month");

            var checkQuantityDtos = new List<CheckQuantityDto>();

            foreach (var item in checkQuantities)
            {
                checkQuantityDtos.Add(new CheckQuantityDto {
                    CheckQuantityId = item.CheckQuantityId,
                    Margin = item.Margin,
                    TotalQuantity = item.TotalQuantity,
                    CheckDate = item.CheckDate.ToString("dd MMM yy HH:mm")
                });
            }

            return Ok(checkQuantityDtos);
        }

        [HttpGet]
        public IHttpActionResult GetCheckQuantityDetailByCheckQuantityId(int Id)
        {
            var checkQuantityDetail = _context.CheckQuantityDetails.Include(c => c.CheckQuantity).Where(c => c.CheckQuantity.CheckQuantityId == Id);

            if (checkQuantityDetail.Count() < 1)
                return BadRequest("Details not found");

            var checkQuantityDetailDtos = new List<CheckQuantityDetailDto>();

            foreach (var item in checkQuantityDetail)
            {
                checkQuantityDetailDtos.Add(new CheckQuantityDetailDto {
                    ServiceId = item.ServiceId,
                    OldQuantity = item.OldQuantity,
                    CurrentQuantity = item.CurrentQuantity,
                    Margin = item.Margin
                });
            }

            return Ok(checkQuantityDetailDtos);
        }

        [HttpPost]
        public IHttpActionResult CreateCheckQuanties(List<CheckQuantityDetailDto> checkQuantityDetailDtos)
        {
            var warehouses = _context.Warehouses.Include(s => s.Service).ToList();
            var checkQuantity = new CheckQuantity {
                CheckDate = DateTime.Now
            };
            //entity framework auto add this checkQuantity object to CheckQuantities table when you add checkQuantityDetail.
            //_context.CheckQuantities.Add(checkQuantity);
            int totalMargin = 0, totalQuantity = 0;

            foreach (var item in checkQuantityDetailDtos)
            {
                var margin = 0;
                totalQuantity += item.CurrentQuantity;
                margin = item.CurrentQuantity - warehouses.Single(w => w.Service.ServiceId == item.ServiceId).Quantity;
                totalMargin += margin;

                warehouses.Single(w => w.Service.ServiceId == item.ServiceId).Quantity = item.CurrentQuantity;

                var checkQuantityDetail = new CheckQuantityDetail {
                    CheckQuantity = checkQuantity,
                    ServiceId = item.ServiceId,
                    OldQuantity = item.OldQuantity,
                    CurrentQuantity = item.CurrentQuantity,
                    Margin = margin
                };
                _context.CheckQuantityDetails.Add(checkQuantityDetail);
            }

            //change checkQuantity Total Quantity and Margin
            checkQuantity.TotalQuantity = totalQuantity;
            checkQuantity.Margin = totalMargin;
            _context.SaveChanges();

            //pass dto back to view to append new row
            var checkQuantityDto = new CheckQuantityDto {
                CheckQuantityId = checkQuantity.CheckQuantityId,
                CheckDate = checkQuantity.CheckDate.ToString("dd MMM yy HH:mm"),
                Margin = checkQuantity.Margin,
                TotalQuantity = checkQuantity.TotalQuantity
            };

            return Ok(checkQuantityDto);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCheckQuantity(int Id)
        {
            var checkQuantityDetails = _context.CheckQuantityDetails.Where(c => c.CheckQuantity.CheckQuantityId == Id).ToList();
            var checkQuantity = _context.CheckQuantities.SingleOrDefault(c => c.CheckQuantityId == Id);

            if (checkQuantity == null)
                return BadRequest("Invalid Check Id");

            foreach (var item in checkQuantityDetails)
            {
                _context.CheckQuantityDetails.Remove(item);
            }
            _context.CheckQuantities.Remove(checkQuantity);

            _context.SaveChanges();

            return Ok("Check form deleted");
        }
    }
}
