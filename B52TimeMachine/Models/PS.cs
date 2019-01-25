using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class Ps
    {
        public int PsId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsVisible { get; set; }
    }
}