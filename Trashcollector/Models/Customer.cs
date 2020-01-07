using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trashcollector.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name ="Zip Code")]
        public int ZipCode { get; set; }
        [Display(Name = "Pick Up Day")]
        public string PickUpDay { get; set; }
        [Display(Name = "Extra Pick Up Date")]
        public string ExtraPickUpDate { get; set; }
        [Display(Name = "Balance")]
        public double Balance { get; set; }
        [Display(Name ="Suspend Date Start")]
        public DateTime SuspendStart { get; set; }
        [Display(Name = "Suspend Date End")]
        public DateTime SuspendEnd { get; set; }
        [Display(Name = "Pick Up Confirmation")]
        public bool PickupConfirmation { get; set; }
    }
}