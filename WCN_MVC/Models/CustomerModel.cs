using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;

namespace WCN_MVC.Models
{
    public class CustomerModel
    {
        public int Id { get; set; } = 0;

        [DisplayName("Customer Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Customer name required.")]
        public string CustomerName { get; set; } = string.Empty;


        [DisplayName("Contact No.")]
        //[StringLength(8)]
        [Required(ErrorMessage = "Contact no. required.")]
        public int ContactNo { get; set; } = 0;


        [DisplayName("Contact Person")]
        [StringLength(50)]
        [Required(ErrorMessage = "Contact person required.")]
        public string ContactPerson { get; set; } = string.Empty;


        [DisplayName("Address")]
        [StringLength(50)]
        [Required(ErrorMessage = "Address required.")]
        public string Address { get; set; } = string.Empty;


        [DisplayName("Email Id")]
        [StringLength(50)]
        [Required(ErrorMessage = "Email id required.")]
        public string Emailid { get; set; } = string.Empty;


        public int UserId { get; set; } = 0;

        public string PageMessage { get; set; }


        [DisplayName("Created By")]
        public string CreatedBy { get; set; } = string.Empty;


        [DisplayName("Created Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CreatedDate { get; set; }


        [DisplayName("Last Modified By")]
        public string ModifiedBy { get; set; } = string.Empty;


        [DisplayName("Modified Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}