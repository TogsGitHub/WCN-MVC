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
    public class PeriodController : Controller
    {
        public static PeriodModel cMain;
        PeriodDBHandle periodDBH = new PeriodDBHandle();
        PeriodModel periodModel = new PeriodModel();
        GenericClass gc = new GenericClass();
        static int gblWcnId = 0;

        // GET: Period
        public ActionResult Index(string SearchString, int? page)//, string Sorting_Order, int? page)
        {
            PeriodDBHandle dbhandle = new PeriodDBHandle();
            GenericClass gc = new GenericClass();
            //ModelState.Clear();
            //ViewBag.SortingCode = Sorting_Order == "code" ? "code_desc" : "code";
            //ViewBag.SortingName = Sorting_Order == "name" ? "name_desc" : "name";
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;

            //switch (Sorting_Order)
            //{
            //    case "code":
            //        return View(dbhandle.GetCompany(SearchString, "code", "asc"));
            //        break;
            //    case "name":
            //        return View(dbhandle.GetCompany(SearchString, "company_name", "asc"));
            //        break;
            //    case "code_desc":
            //        return View(dbhandle.GetCompany(SearchString, "code", "desc"));
            //        break;
            //    case "name_desc":
            //        return View(dbhandle.GetCompany(SearchString, "company_name", "desc"));
            //        break;
            //}

            //return View(dbhandle.GetPeriod(SearchString));//, "company_name", "asc").ToPagedList(page ?? gc.minPage, gc.maxPage));
            return View(dbhandle.GetPeriod(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: Period/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            PeriodDBHandle cdb = new PeriodDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetPeriod(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: Period/Create
        public ActionResult Create()
        {
            cMain = new PeriodModel();
            periodModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);
            return View(periodModel);
        }

        
        
        // POST: Period/Create
        [HttpPost]
        public ActionResult Create(PeriodModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    PeriodDBHandle cdb = new PeriodDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddPeriod(cmodel);

                    if (retVal == 1)
                    {
                        //ModelState.Clear();
                        ViewBag.Message = "Period created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Period already exists.";
                    }
                    else
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }

                    ModelState.Clear();
                    cMain = new PeriodModel();
                    periodModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);
                    return View(periodModel);
                }

                return View(cmodel);
            }
            catch (Exception ex)
            {
                return View(cmodel);
            }
        }

        
        
        // GET: Period/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //cMain = new PeriodModel();
            PeriodDBHandle cdb = new PeriodDBHandle();
            //periodModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);
            ViewBag.Years = gc.GetYears(DateTime.Now.Year);
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetPeriod(SearchString).Find(cmodel => cmodel.Id == id));
        }

        
        
        // POST: Period/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PeriodModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    PeriodDBHandle cdb = new PeriodDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditPeriod(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Period edited.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Period not active.";
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

        
        
        // GET: Period/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                PeriodDBHandle cdb = new PeriodDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeletePeriod(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "Period deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "Period deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        
        
        // POST: Period/Delete/5
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
            PeriodDBHandle cdb = new PeriodDBHandle();
            GenericClass gc = new GenericClass();
            List<PeriodModel> lst = new List<PeriodModel>();
            lst = cdb.GetPeriod(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "PeriodMaster");
            string fileName = "PeriodMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("PageMessage");
                dt.Columns.Remove("CreatedBy");
                dt.Columns.Remove("CreatedDate");
                dt.Columns.Remove("ModifiedBy");
                dt.Columns.Remove("ModifiedDate");
                dt.Columns.Remove("MonthId");
                dt.Columns.Remove("YearLists");

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
