using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class RentalStatisticsDto
    {
        public int RentalId { get; set; }
        public int PsId { get; set; }
        public string PsName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string RentalSpan { get; set; }
        public string ServiceFee { get; set; }
        public string RentalFee { get; set; }
        public string Total { get; set; }
    }
}