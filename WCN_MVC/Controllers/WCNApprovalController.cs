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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace WCN_MVC.Controllers
{
    public class WCNApprovalController : Controller
    {
        public static WCNModel cMain;
        WCNDBHandle wcnDBH = new WCNDBHandle();
        WCNModel wcnModel = new WCNModel();
        GenericClass gc = new GenericClass();
        static int gblWcnId = 0;
        static string companyName = "Company";
        static string divisionName = "Division";
        static string locationName = "Location";

        // GET: WCNApproval
        public ActionResult Index(string SearchString, string Status, int? page)
        {
            WCNDBHandle dbhandle = new WCNDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            Status = String.IsNullOrEmpty(Status) ? "All" : Status;
            return View(dbhandle.GetWCNApproval(SearchString, Status).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: WCNApproval/Details/5
        public ActionResult Details(int id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            ViewBag.Divisions = wcnDBH.GetDivisionList(divisionName);
            ViewBag.Periods = wcnDBH.GetPeriodList();
            ViewBag.Locations = wcnDBH.GetLocationList(locationName);
            ViewBag.Customers = wcnDBH.GetCustomerList();
            return View(cdb.GetWCNTracker(string.Empty).Find(cmodel => cmodel.Id == id));
        }



        // GET: WCNApproval/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: WCNApproval/Create
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



        // GET: WCNApproval/Edit/5
        public ActionResult Edit(int id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            //PopulateDropDownListFor();// ddl = new PopulateDropDownListFor();
            gblWcnId = id;

            cMain = new WCNModel();
            ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            ViewBag.Divisions = wcnDBH.GetDivisionList(divisionName);
            ViewBag.Periods = wcnDBH.GetPeriodList();
            ViewBag.Locations = wcnDBH.GetLocationList(locationName);
            ViewBag.Customers = wcnDBH.GetCustomerList();
            //get_data();

            //wcnModel.ResourceTypeLists = cMain.ResourceTypeLists = wcnDBH.GetResourceTypeList();
            //wcnModel.UnitLists = cMain.UnitLists = wcnDBH.GetUnitList();
            //wcnModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);

            return View(cdb.GetWCNTracker(string.Empty).Find(cmodel => cmodel.Id == id));
        }



        // POST: WCNApproval/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        public JsonResult EditWCNTrackerDetails(List<WCNLineItems> wcnLineItems, WCNModel tracker)
        {
            int retVal = 0;
            int insertedRecords = 0;
            WCNDBHandle cdb = new WCNDBHandle();
            WCNLineItems LineItems = new WCNLineItems();

            try
            {
                tracker.UserId = Convert.ToInt16(Session["UserID"]);
                tracker.Id = gblWcnId;
                retVal = cdb.EditWCNApproval(tracker);

                if (retVal == 1)
                {
                    //Check for NULL.
                    if (wcnLineItems == null)
                    {
                        wcnLineItems = new List<WCNLineItems>();
                    }

                    int retValLines = cdb.DeleteWCNLineItems(tracker.WcnNo);

                    foreach (WCNLineItems additem in wcnLineItems)
                    {
                        LineItems.WcnNo = tracker.WcnNo;
                        LineItems.ResourceType = additem.ResourceType;
                        LineItems.Resource = additem.Resource;
                        LineItems.StartDate = additem.StartDate;
                        LineItems.EndDate = additem.EndDate;
                        LineItems.Days = additem.Days;
                        LineItems.Qty = additem.Qty;
                        LineItems.Unit = additem.Unit;
                        LineItems.Rate = additem.Rate;
                        LineItems.Total = additem.Total;
                        insertedRecords = cdb.AddWCNLineItems(LineItems);
                        ViewBag.Message = "WCN edited.";
                    }
                }
                else if (retVal == 2)
                {
                    insertedRecords = 0;
                }
                else
                {
                    insertedRecords = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //dt = null;
            }

            return Json(insertedRecords);
        }



        // GET: WCNApproval/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                WCNDBHandle cdb = new WCNDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteWCNTracker(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "WCN tracker details deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "WCN tracker not deleted";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: WCNApproval/Delete/5
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
            WCNDBHandle cdb = new WCNDBHandle();
            GenericClass gc = new GenericClass();
            List<WCNModel> lst = new List<WCNModel>();
            lst = cdb.GetWCNTracker(string.Empty);
            //(string.Empty, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "WCNTracker");
            string fileName = "WCNApproval_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("CompanyLists");
                dt.Columns.Remove("CompanyId");
                dt.Columns.Remove("DivisionId");
                dt.Columns.Remove("DivisionCode");
                dt.Columns.Remove("DivisionLists");
                dt.Columns.Remove("PeriodId");
                dt.Columns.Remove("PeriodLists");
                dt.Columns.Remove("YearLists");
                dt.Columns.Remove("Month");
                dt.Columns.Remove("Year");
                dt.Columns.Remove("LocationId");
                dt.Columns.Remove("LocationLists");
                dt.Columns.Remove("OtherLocation");
                dt.Columns.Remove("CustomerId");
                dt.Columns.Remove("ContactNo");
                dt.Columns.Remove("ContactPerson");
                dt.Columns.Remove("Address");
                dt.Columns.Remove("Emailid");
                dt.Columns.Remove("CustomerLists");
                dt.Columns.Remove("Description");
                dt.Columns.Remove("ResourceTypeId");
                dt.Columns.Remove("ResourceTypeLists");
                dt.Columns.Remove("UnitId");
                dt.Columns.Remove("UnitLists");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("PageMessage");
                dt.Columns.Remove("CreatedBy");
                dt.Columns.Remove("CreatedDate");
                dt.Columns.Remove("ModifiedBy");
                dt.Columns.Remove("ModifiedDate");
                dt.Columns.Remove("WCNLineItems");

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



        [HttpPost]
        public JsonResult GetDataWCNLineItems()
        {
            WCNDBHandle cdb = new WCNDBHandle();
            return Json(cdb.GetDataWCNLineItems(gblWcnId), JsonRequestBehavior.AllowGet);
        }



        public ActionResult GenerateWCN()
        {
            ReportDocument rd = new ReportDocument();
            ReportDocument sub1 = new ReportDocument();

            DataTable dtMainReport = new DataTable();
            DataTable dtSubReport1 = new DataTable();

            dtMainReport = wcnDBH.GenerateWCN(gblWcnId);
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "WCN.rpt"));
            rd.SetDataSource(dtMainReport);

            sub1 = rd.Subreports["Rpt_WCNLineItems"];
            dtSubReport1 = wcnDBH.GenerateWCNLineItems(gblWcnId);
            sub1.SetDataSource(dtSubReport1);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "WCN_Report_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".pdf");
            }
            catch (Exception ex)
            {
                //throw;
            }

            return File(string.Empty, "application/pdf", "WCN_Report_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".pdf");
        }



        public ActionResult ApproveWCN()
        {
            WCNDBHandle cdb = new WCNDBHandle();
            return View(cdb.ApproveWCN(gblWcnId, Convert.ToInt16(Session["UserID"])));
        }
    }
}
