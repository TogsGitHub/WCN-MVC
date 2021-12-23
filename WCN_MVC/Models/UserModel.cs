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
    public class UserModel
    {
        public int Id { get; set; } = 0;

        [DisplayName("Name")]
        [Required(ErrorMessage = "Name required.")]
        public string Name { get; set; }


        [DisplayName("Username")]
        [Required(ErrorMessage = "Username required.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required.")]
        public string Password { get; set; }


        [DisplayName("User Role")]
        //[Required(ErrorMessage = "User role required.")]
        public string UserRole { get; set; } = string.Empty;

        public int UserRoleID { get; set; } = 0;

        public IEnumerable<SelectListItem> UserRoleLists { get; set; }


        [DisplayName("Contact No.")]
        [Required(ErrorMessage = "Contact no. required.")]
        public int ContactNo { get; set; }


        [DisplayName("Email ID")]
        [Required(ErrorMessage = "Email id required.")]
        public string EmailID { get; set; }

        public int UserID { get; set; } = 0;

        [DisplayName("Image Name")]
        [Required(ErrorMessage = "Image name required.")]
        public string ImageName { get; set; }

        public string DisplayMessage { get; set; }

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