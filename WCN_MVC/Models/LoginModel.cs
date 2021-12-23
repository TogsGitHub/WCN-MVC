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
    public class LoginModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username required.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required.")]
        public string Password { get; set; }

        public string PageMessage { get; set; }

        public string ModalMessage { get; set; }
    }
}