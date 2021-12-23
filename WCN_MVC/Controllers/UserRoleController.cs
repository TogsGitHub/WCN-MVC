using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WCN_MVC.Models;
using WCN_MVC.DBHandle;
using System.Net;
using PagedList;
using PagedList.Mvc;
using ClosedXML.Excel;
using System.IO;
using System.Web.UI;
using System.Data;
using System.ComponentModel;
using System.Web.UI.WebControls;
using WCN_MVC.Generic;

namespace WCN_MVC.Controllers
{
    public class UserRoleController : Controller
    {
        // GET: UserRole
        public ActionResult Index(string SearchString, int? page)
        {
            UserRoleDBHandle dbhandle = new UserRoleDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            //return View(dbhandle.GetUserRole(SearchString));//, "company_name", "asc").ToPagedList(page ?? gc.minPage, gc.maxPage));
            return View(dbhandle.GetUserRole(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: UserRole/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            UserRoleDBHandle cdb = new UserRoleDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetUserRole(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: UserRole/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: UserRole/Create
        [HttpPost]
        public ActionResult Create(UserRoleModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    UserRoleDBHandle cdb = new UserRoleDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddUserRole(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "User role created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "User role already created.";
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



        // GET: UserRole/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserRoleDBHandle cdb = new UserRoleDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetUserRole(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: UserRole/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserRoleModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    UserRoleDBHandle cdb = new UserRoleDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditUserRole(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "User role edited.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "User role not active.";
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



        // GET: UserRole/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                UserRoleDBHandle cdb = new UserRoleDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteUserRole(id, userId);

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



        // POST: UserRole/Delete/5
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
            UserRoleDBHandle cdb = new UserRoleDBHandle();
            GenericClass gc = new GenericClass();
            List<UserRoleModel> lst = new List<UserRoleModel>();
            lst = cdb.GetUserRole(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "UserRoleMaster");
            string fileName = "UserRoleMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("PageMessage");
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
