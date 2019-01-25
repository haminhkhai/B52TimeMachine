using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Controllers.API
{
    public class CalculateMethods
    {
        public static int RentalFeeCalculate(double numberOfMinutes, int price)
        {
            double rentalFee = price / 60 * numberOfMinutes;
            var rentalFeeTemp = rentalFee / 1000;
            rentalFee = Math.Round(rentalFeeTemp, MidpointRounding.AwayFromZero) * 1000;

            return Convert.ToInt32(rentalFee);
        }
    }
}