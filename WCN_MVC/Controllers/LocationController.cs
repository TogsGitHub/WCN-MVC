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
    public class LocationController : Controller
    {
        // GET: Location
        public ActionResult Index(string SearchString, int? page)
        {
            LocationDBHandle dbhandle = new LocationDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(dbhandle.GetLocation(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: Location/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            LocationDBHandle cdb = new LocationDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetLocation(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        
        
        // POST: Location/Create
        [HttpPost]
        public ActionResult Create(LocationModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    LocationDBHandle cdb = new LocationDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddLocation(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Location created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Location already created.";
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



        // GET: Location/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LocationDBHandle cdb = new LocationDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetLocation(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, LocationModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    LocationDBHandle cdb = new LocationDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditLocation(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Location edited.";
                        
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Location not active.";
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



        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                LocationDBHandle cdb = new LocationDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteLocation(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "Location deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "Location deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: Location/Delete/5
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
            LocationDBHandle cdb = new LocationDBHandle();
            GenericClass gc = new GenericClass();
            List<LocationModel> lst = new List<LocationModel>();
            lst = cdb.GetLocation(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "LocationMaster");
            string fileName = "LocationMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

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
