using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using B52TimeMachine.Models;
using B52TimeMachine.Dtos;

namespace B52TimeMachine.App_Start
{
	public class MappingProfile : Profile
	{
		public MappingProfile ()
		{
			CreateMap<ServiceRental, ServiceRentalDto>();
			CreateMap<Service, ServiceDto>();
		}
		 
	}
}