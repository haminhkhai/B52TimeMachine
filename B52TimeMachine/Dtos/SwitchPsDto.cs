using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Dtos
{
    public class SwitchPsDto
    {
        public int OldPsId { get; set; }
        public int NewPsId { get; set; }
        public string Case { get; set; }
    }
}