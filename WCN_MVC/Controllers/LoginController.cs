using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WCN_MVC.Models;
using WCN_MVC.DBHandle;
using System.IO;

namespace WCN_MVC.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            int retVal = 0;
            int userId = 0;

            if (!ModelState.IsValid)
            {
                return PartialView("Index", model);
            }

            LoginDBHandle ldb = new LoginDBHandle();
            //int retVal = ldb.ValidateLogin(model);
            DataTable dt = ldb.ValidateLogin(model);

            if (dt.Rows.Count > 0)
            {
                retVal = Convert.ToInt16(dt.Rows[0]["dbVal"]);
                userId = Convert.ToInt16(dt.Rows[0]["userid"]);

                if (retVal == 0)
                {
                    model.PageMessage = "Username is not active.";
                    return View("Index", model);
                }
                else if (retVal == 1)
                {
                    model.PageMessage = "Login success.";
                    ModelState.Clear();
                    List<UserModel> userList = ldb.GetUserDetails(userId);
                    if (userList.Count > 0)
                    {
                        Session["UserID"] = Convert.ToString(userId);
                        Session["UserImage"] = userList[0].ImageName;
                        Session["LoginName"] = userList[0].Name;
                        Session["UserName"] = userList[0].UserName;
                        Session["Password"] = userList[0].Password;
                        Session["UserRoleId"] = userList[0].UserRoleID;
                        Session["UserRole"] = userList[0].UserRole;
                        Session["ContactNo"] = userList[0].ContactNo;
                        Session["EmailId"] = userList[0].EmailID;
                        Session["UserImage"] = userList[0].ImageName;
                        Session["created_by"] = userList[0].CreatedBy;
                        Session["created_date"] = userList[0].CreatedDate;
                        Session["modified_by"] = userList[0].ModifiedBy;
                        Session["modified_date"] = userList[0].ModifiedDate;
                    }

                    return RedirectToAction("Index", "Dashboard");
                }
                else if (retVal == -1)
                {
                    model.PageMessage = "Username and/or password is incorrect.";
                    return View("Index", model);
                }
                else
                {
                    model.PageMessage = "Error, please contact system admin.";
                    return View("Index", model);
                }
            }
            else
            {

            }

            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> SendPassword(LoginModel model)
        {
            LoginDBHandle ldb = new LoginDBHandle();
            string pwd = string.Empty;
            string email = string.Empty;
            string name = string.Empty;
            int retVal = 0;

            DataTable dt = ldb.SendPassword(model);

            if (dt.Rows.Count > 0)
            {
                pwd = dt.Rows[0]["Password"].ToString();
                email = dt.Rows[0]["EmailId"].ToString();
                name = dt.Rows[0]["Name"].ToString();

                if (pwd != string.Empty && email != string.Empty)
                {
                    SendEmail(pwd, email, name);
                }
                else
                {
                    ViewBag.PopMessage = "Invalid username.";
                }
            }

            return View("Index", model);
        }


        private void SendEmail(string Password, string EmailId, string Name)
        {
            SendEmail objSendEmail = new SendEmail();
            string mailBody = string.Empty;
            string htmlbody = string.Empty;
            int emailStatus = 0;

            mailBody += "Dear " + Name + ",<br><br>";
            mailBody += "Please find below your WCN password.<br><br>";
            mailBody += "Password :- " + Password + "<br><br>";

            using (StreamReader reader = new StreamReader(Server.MapPath("~/EMailTemplate.html")))
            {
                htmlbody = reader.ReadToEnd();
            }

            htmlbody = htmlbody.Replace("{Title}", "Forgot Password");

            htmlbody = htmlbody.Replace("{Description}", mailBody);

            emailStatus = objSendEmail.SendMail_GMail(EmailId, "Forgot Password", htmlbody, "WCN ADMIN<wcn.togs@gmail.com>");

            if (emailStatus == 1)
            {
                ViewBag.PopMessage = "Email sent to registered email id.";
            }
            else
            {
                ViewBag.PopMessage = "Email not send.";
            }
        }
    }
}