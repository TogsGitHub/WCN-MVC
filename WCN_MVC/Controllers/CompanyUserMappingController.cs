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
    public class CompanyUserMappingController : Controller
    {
        public static CompanyUserMappingModel cMain;
        CompanyUserMappingDBHandle cumDBH = new CompanyUserMappingDBHandle();
        CompanyUserMappingModel cumModel = new CompanyUserMappingModel();
        GenericClass gc = new GenericClass();
        static int gblWcnId = 0;
        static string companyName = "Company";

        // GET: CompanyUserMapping
        public ActionResult Index(string SearchString, int? page)
        {
            CompanyUserMappingDBHandle dbhandle = new CompanyUserMappingDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(dbhandle.GetCompanyUserMapping(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
            //return View(dbhandle.GetCompany(SearchString, "company_name", "asc").ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: CompanyUserMapping/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            CompanyUserMappingDBHandle cdb = new CompanyUserMappingDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetCompanyUserMapping(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: CompanyUserMapping/Create
        public ActionResult Create()
        {
            cMain = new CompanyUserMappingModel();
            cumModel.CompanyLists = cMain.CompanyLists = cumDBH.GetCompanyList(companyName);
            cumModel.UserLists = cMain.UserLists = cumDBH.GetUserList();
            return View(cumModel);
        }



        // POST: CompanyUserMapping/Create
        [HttpPost]
        public ActionResult Create(CompanyUserMappingModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    CompanyUserMappingDBHandle cdb = new CompanyUserMappingDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddCompanyUserMapping(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Company and user mapping created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Company and user mapping already created.";
                    }
                    else
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }
                }
                else
                {
                    cumModel.UserLists = cMain.UserLists = cumDBH.GetCompanyList(companyName);
                    cumModel.UserLists = cMain.UserLists = cumDBH.GetUserList();
                    return View(cumModel);
                }

                ModelState.Clear();
                cMain = new CompanyUserMappingModel();
                cumModel.CompanyLists = cMain.CompanyLists = cumDBH.GetCompanyList(companyName);
                cumModel.UserLists = cMain.UserLists = cumDBH.GetUserList();
                return View(cumModel);
            }
            catch
            {
                return View();
            }
        }



        // GET: CompanyUserMapping/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CompanyUserMappingDBHandle cdb = new CompanyUserMappingDBHandle();
            ViewBag.Companies = cumDBH.GetCompanyList(companyName);
            ViewBag.Users = cumDBH.GetUserList();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetCompanyUserMapping(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: CompanyUserMapping/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CompanyUserMappingModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    CompanyUserMappingDBHandle cdb = new CompanyUserMappingDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditCompanyUserMapping(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Company and user mapping edited.";
                        ViewBag.Companies = cumDBH.GetCompanyList(companyName);
                        ViewBag.Users = cumDBH.GetUserList();
                    }
                    else if(retVal==2)
                    {
                        ViewBag.Message = "Not active.";
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



        // GET: CompanyUserMapping/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                CompanyUserMappingDBHandle cdb = new CompanyUserMappingDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteCompanyUserMapping(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "Company User Mapping deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "Company User Mapping deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: CompanyUserMapping/Delete/5
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
            CompanyUserMappingDBHandle cdb = new CompanyUserMappingDBHandle();
            GenericClass gc = new GenericClass();
            List<CompanyUserMappingModel> lst = new List<CompanyUserMappingModel>();
            lst = cdb.GetCompanyUserMapping(string.Empty);
            //(string.Empty, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "CompanyUserMappingMaster");
            string fileName = "CompanyUserMappingMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("PageMessage");
                dt.Columns.Remove("CreatedBy");
                dt.Columns.Remove("CreatedDate");
                dt.Columns.Remove("ModifiedBy");
                dt.Columns.Remove("ModifiedDate");
                dt.Columns.Remove("CompanyMasterId");
                dt.Columns.Remove("CompanyLists");
                dt.Columns.Remove("UserMasterId");
                dt.Columns.Remove("UserLists");
                dt.Columns.Remove("UserId");

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
