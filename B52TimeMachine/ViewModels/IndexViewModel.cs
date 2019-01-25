using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using B52TimeMachine.Models;
using B52TimeMachine.Dtos;

namespace B52TimeMachine.ViewModels
{
    public class IndexViewModel
    {
        public List<PsRentalDto> PsRentalDtos { get; set; }
        public List<Service> Services { get; set; }
    }
}