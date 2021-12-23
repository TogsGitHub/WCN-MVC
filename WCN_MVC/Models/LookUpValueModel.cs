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
    public class LookUpValueModel
    {
        public int Id { get; set; } = 0;

        [DisplayName("Code")]
        [StringLength(5)]
        [Required(ErrorMessage = "Code required.")]
        public string Code { get; set; } = string.Empty;

        [DisplayName("Description")]
        [StringLength(100)]
        [Required(ErrorMessage = "Description required.")]
        public string Description { get; set; } = string.Empty;


        public int TypeId { get; set; } = 0;



        [DisplayName("Type")]
        [Required(ErrorMessage = "Select look-up type.")]
        public string TypeName { get; set; } = string.Empty;


        public List<SelectListItem> TypeLists { get; set; }


        [DisplayName("Sequence No")]
        [Required(ErrorMessage = "Sequence no. required.")]
        public int SequenceNo { get; set; } = 0;

        public int UserId { get; set; } = 0;

        public string Message { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; } = string.Empty;

        [DisplayName("Created Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [DisplayName("Last Modified By")]
        public string ModifiedBy { get; set; } = string.Empty;

        [DisplayName("Last Modified Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}