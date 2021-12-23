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
    public class CompanyModel
    {
        public int Id { get; set; } = 0;

        [DisplayName("Code")]
        [StringLength(5)]
        [Required(ErrorMessage = "Code required.")]
        public string Code { get; set; } = string.Empty;

        [DisplayName("Company Name")]
        [StringLength(100)]
        [Required(ErrorMessage = "Company name required.")]
        public string CompanyName { get; set; } = string.Empty;

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