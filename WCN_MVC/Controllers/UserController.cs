using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WCN_MVC.Models;
using WCN_MVC.DBHandle;
using WCN_MVC.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using PagedList;
using PagedList.Mvc;
using ClosedXML.Excel;
using System.IO;
using System.Web.UI;
using System.Data;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace WCN_MVC.Controllers
{
    public class UserController : Controller
    {
        public static UserModel cMain;
        UserDBHandle userDBH = new UserDBHandle();
        UserModel userModel = new UserModel();
        GenericClass gc = new GenericClass();
        static int gblWcnId = 0;
        
        
        // GET: User
        public ActionResult Index(string SearchString, int? page)
        {
            UserDBHandle dbhandle = new UserDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            //return View(dbhandle.GetUser(SearchString));//, "company_name", "asc").ToPagedList(page ?? gc.minPage, gc.maxPage));
            return View(dbhandle.GetUser(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: User/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            UserDBHandle cdb = new UserDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetUser(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: User/Create
        public ActionResult Create()
        {
            cMain = new UserModel();
            userModel.UserRoleLists = cMain.UserRoleLists = userDBH.GetUserRoleList();
            //ViewBag.UserRoles = userDBH.GetUserRoleList();
            return View(userModel);
        }



        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    UserDBHandle cdb = new UserDBHandle();
                    cmodel.UserID = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddUser(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "User created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "User already created.";
                    }
                    else
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }
                }
                else
                {
                    //ViewBag.UserRoles = userModel.UserRoleLists = userDBH.GetUserRoleList();
                    //return View(userModel);
                    //cMain = new UserModel();
                    userModel.UserRoleLists = cMain.UserRoleLists = userDBH.GetUserRoleList();
                    //ViewBag.UserRoles = userDBH.GetUserRoleList();
                    return View(userModel);
                }

                ModelState.Clear();
                cMain = new UserModel();
                userModel.UserRoleLists = cMain.UserRoleLists = userDBH.GetUserRoleList();
                return View(userModel);
            }
            catch
            {
                return View();
            }
        }



        // GET: User/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //cMain = new UserModel();
            //userModel.UserRoleLists = cMain.UserRoleLists = userDBH.GetUserRoleList();

            UserDBHandle cdb = new UserDBHandle();
            ViewBag.UserRoles = userDBH.GetUserRoleList();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetUser(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    UserDBHandle cdb = new UserDBHandle();
                    cmodel.UserID = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditUser(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "User edited.";
                        ViewBag.UserRoles = userDBH.GetUserRoleList();
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "User not active.";
                    }
                    else
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                UserDBHandle cdb = new UserDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteUser(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "User role deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "User role deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Export()
        {
            UserDBHandle cdb = new UserDBHandle();
            GenericClass gc = new GenericClass();
            List<UserModel> lst = new List<UserModel>();
            lst = cdb.GetUser(string.Empty);
            //(string.Empty, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "UserMaster");
            string fileName = "UserMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("password");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("UserRoleId");
                dt.Columns.Remove("UserRoleLists");
                dt.Columns.Remove("DisplayMessage");
                dt.Columns.Remove("CreatedBy");
                dt.Columns.Remove("CreatedDate");
                dt.Columns.Remove("ModifiedBy");
                dt.Columns.Remove("ModifiedDate");

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }

            return View("Index");
        }
    }
}
