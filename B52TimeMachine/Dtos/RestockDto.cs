using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class RestockDto
    {
        public int RestockId { get; set; }

        public string Total { get; set; }

        public string RestockDate { get; set; }
    }
}