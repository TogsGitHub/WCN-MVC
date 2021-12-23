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
    public class DivisionController : Controller
    {
        // GET: Division
        public ActionResult Index(string SearchString, int? page)
        {
            DivisionDBHandle dbhandle = new DivisionDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(dbhandle.GetDivision(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: Division/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            DivisionDBHandle cdb = new DivisionDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetDivision(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: Division/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: Division/Create
        [HttpPost]
        public ActionResult Create(DivisionModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    DivisionDBHandle cdb = new DivisionDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddDivision(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Division created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Division code already created.";
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



        // GET: Division/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionDBHandle cdb = new DivisionDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetDivision(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: Division/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DivisionModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    DivisionDBHandle cdb = new DivisionDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditDivision(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Division edited.";
                    }
                    else if (retVal == 2)
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



        // GET: Division/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                DivisionDBHandle cdb = new DivisionDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteDivision(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "Division deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "Division deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: Division/Delete/5
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
            DivisionDBHandle cdb = new DivisionDBHandle();
            GenericClass gc = new GenericClass();
            List<DivisionModel> lst = new List<DivisionModel>();
            lst = cdb.GetDivision(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "DivisionMaster");
            string fileName = "DivisionMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

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
