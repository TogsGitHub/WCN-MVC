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
    public class DashboardModel
    {
        [DisplayName("Total Revenue")]
        public decimal TotalRevenue { get; set; } = 0;

        [DisplayName("Total Unbilled")]
        public decimal TotalUnbilled { get; set; } = 0;

        [DisplayName("Created")]
        public Int32 WcnCreated { get; set; } = 0;

        [DisplayName("Approved")]
        public Int32 WcnApproved { get; set; } = 0;

        [DisplayName("Payment")]
        public Int32 WcnPayment { get; set; } = 0;

        //***************************************************************************

        [DisplayName("Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [DisplayName("WCN No.")]
        public string WCNNo { get; set; } = string.Empty;

        [DisplayName("WCN Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> WCNDate { get; set; }
        //public DateTime WCNDate { get; set; }

        
        public decimal Revenue { get; set; }

        public decimal Unbilled { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; } = string.Empty;
    }
}