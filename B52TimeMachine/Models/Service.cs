using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B52TimeMachine.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required]
        public float Order { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        public bool IsCig { get; set; }

        public bool IsVisible { get; set; }

    }
}