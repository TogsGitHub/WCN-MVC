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
    public class MyProfileController : Controller
    {
        UserDBHandle cdb = new UserDBHandle();

        // GET: MyProfile
        public ActionResult Index()
        {
            return View(cdb.GetUser(string.Empty).Find(cmodel => cmodel.Id == Convert.ToInt16(Session["UserID"]))); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }


        
        // GET: MyProfile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        
        
        // GET: MyProfile/Create
        public ActionResult Create()
        {
            return View();
        }

        
        
        // POST: MyProfile/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
        
        // GET: MyProfile/Edit/5
        public ActionResult Edit()
        {
            return View(cdb.GetUser(string.Empty).Find(cmodel => cmodel.Id == Convert.ToInt16(Session["UserID"]))); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }

        
        
        // POST: MyProfile/Edit/5
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
                        //ModelState.Clear();
                        ViewBag.Message = "User edited.";
                        //ViewBag.UserRoles = cdb.GetUserRoleList();
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



        // GET: MyProfile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        
        
        // POST: MyProfile/Delete/5
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
    }
}
