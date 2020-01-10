using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trashcollector.Models
{
    public class PickByDayViewModels
    {
        public List<Customer> customers { get; set; }
        [Display(Name ="Day")]
        public string Day { get; set; }
        
    }
}