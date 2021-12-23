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
    public class PeriodModel
    {
        public int Id { get; set; } = 0;

        public int MonthId { get; set; } = 0;

        [DisplayName("Month")]
        [StringLength(20)]
        [Required(ErrorMessage = "Month required.")]
        public string Month { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year required.")]
        public int Year { get; set; } = 0;

        public SelectList YearLists { get; set; }

        [DisplayName("Period")]
        [Required(ErrorMessage = "Period required.")]
        public string Period { get; set; } = string.Empty;

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