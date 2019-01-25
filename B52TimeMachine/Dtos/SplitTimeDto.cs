using B52TimeMachine.Models;
using System;

namespace B52TimeMachine.Dtos
{
    public class SplitTimeDto
    {
        public int RentalId { get; set; }

        public DateTime? TimeSplit { get; set; }

        public string SplitSpan { get; set; }

        public int SplitFee { get; set; }

        public double SplitSpanRatio { get; set; }
    }
}