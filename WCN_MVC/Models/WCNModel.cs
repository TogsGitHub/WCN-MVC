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

    public class WCNModel
    {
        public int Id { get; set; } = 0;

        [DisplayName("Company Name")]
        [Required(ErrorMessage = "Company name required.")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name required.")]
        public IEnumerable<SelectListItem> CompanyLists { get; set; }
        public int CompanyId { get; set; } = 0;
        //public IEnumerable<SelectListItem> CompanyId { get; set; }

        [DisplayName("WCN No.")]
        public string WcnNo { get; set; } = string.Empty;

        [DisplayName("Status")]
        public string Status { get; set; } = string.Empty;

        public int DivisionId { get; set; } = 0;


        [DisplayName("Division Code")]
        [StringLength(1)]
        [Required(ErrorMessage = "Division code required.")]
        public string DivisionCode { get; set; } = string.Empty;


        [DisplayName("Division Name")]
        [Required(ErrorMessage = "Division name required.")]
        public string DivisionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Division name required.")]
        public List<SelectListItem> DivisionLists { get; set; }


        [Required(ErrorMessage = "WCN date required.")]
        [DisplayName("WCN Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> WCNDate { get; set; }
        //public DateTime WCNDate { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }

        public int PeriodId { get; set; } = 0;

        [DisplayName("Period")]
        [Required(ErrorMessage = "Period required.")]
        public string Period { get; set; } = string.Empty;

        [Required(ErrorMessage = "Period required.")]
        public List<SelectListItem> PeriodLists { get; set; }
        public SelectList YearLists { get; set; }

        [Required(ErrorMessage = "Month required.")]
        public string Month { get; set; } = string.Empty;

        public int Year { get; set; } = 0;


        [Required(ErrorMessage = "Start date required.")]
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Completed Date")]
        public Nullable<System.DateTime> CompletedDate { get; set; }


        public int LocationId { get; set; } = 0;

        [DisplayName("Location Name")]
        [Required(ErrorMessage = "Location name required.")]
        public string LocationName { get; set; } = string.Empty;

        public List<SelectListItem> LocationLists { get; set; }


        [DisplayName("Other Location")]
        public string OtherLocation { get; set; }

        public int CustomerId { get; set; } = 0;

        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Customer name required.")]
        public string CustomerName { get; set; }

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

        [Required(ErrorMessage = "Customer name required.")]
        public List<SelectListItem> CustomerLists { get; set; }


        [DisplayName("Description")]
        [StringLength(200)]
        [Required(ErrorMessage = "Description required.")]
        public string Description { get; set; } = string.Empty;


        [DisplayName("Rig No.")]
        public int RigNo { get; set; } = 0;


        [DisplayName("WCN Sent")]
        public bool IsWCNSent { get; set; }


        [DisplayName("WCN Signed")]
        public bool IsWCNSigned { get; set; }


        [DisplayName("Client Signed")]
        public bool IsClientSigned { get; set; }

        [DisplayName("Discount")]
        public bool IsDiscount { get; set; }


        [DisplayName("Discount Amount")]
        public decimal DiscountAmount { get; set; } = 0;


        [DisplayName("Additional Discount")]
        public decimal AdditionalDiscount { get; set; } = 0;


        [DisplayName("Discount Remark")]
        public string DiscountRemark { get; set; } = string.Empty;


        public int ResourceTypeId { get; set; } = 0;
        public List<SelectListItem> ResourceTypeLists { get; set; }


        public int UnitId { get; set; } = 0;
        public List<SelectListItem> UnitLists { get; set; }


        public int UserId { get; set; } = 0;

        public string PageMessage { get; set; } = string.Empty;

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

        [DisplayName("Total Revenue")]
        public decimal TotalRevenue { get; set; } = 0;


        [DisplayName("Total Billable")]
        public decimal TotalBillable { get; set; } = 0;


        [DisplayName("Invoiced Amount")]
        [Required(ErrorMessage = "Invoiced amount required.")]
        public decimal InvoicedAmount { get; set; } = 0;

        [DisplayName("Billing Status")]
        [Required(ErrorMessage = "Billing status required.")]
        public string BillingStatus { get; set; } = string.Empty;

        [DisplayName("Stake Holder")]
        [Required(ErrorMessage = "Stake holder required.")]
        public string StakeHolder { get; set; } = string.Empty;

        [DisplayName("Invoice Number")]
        [Required(ErrorMessage = "Invoice no. required.")]
        public string InvoiceNumber { get; set; } = string.Empty;

        public WCNLineItems WCNLineItems { get; set; }


        [DisplayName("Additional Asset")]
        public decimal AdditionalAsset { get; set; } = 0;

        [DisplayName("Additional Load")]
        public decimal AdditionalLoad { get; set; } = 0;

        [DisplayName("Standby Charges")]
        public decimal StandbyCharges { get; set; } = 0;

        [DisplayName("Other Charges")]
        public decimal OtherCharges { get; set; } = 0;


        [DisplayName("Demobilisation")]
        public decimal Demobilisation { get; set; } = 0;

        [DisplayName("Mobilisation")]
        public decimal Mobilisation { get; set; } = 0;

        [DisplayName("ROP Admin Charges")]
        public decimal ROPAdminCharges { get; set; } = 0;

        [DisplayName("ROP Charges")]
        public decimal ROPCharges { get; set; } = 0;

        [DisplayName("Twilight Charges")]
        public decimal TwilightCharges { get; set; } = 0;

        [DisplayName("Revenue")]
        public decimal Revenue { get; set; } = 0;

    }


    //[DataContract]
    public class WCNLineItems
    {
        public string WcnNo { get; set; }
        public string ResourceType { get; set; }
        public string Resource { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Days { get; set; }
        public decimal Qty { get; set; }
        public int Unit { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }
    }


    public class Employee
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Country { get; set; }
    }
}