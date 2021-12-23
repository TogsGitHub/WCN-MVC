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
using System.Web.Script.Serialization;
using DocumentFormat.OpenXml.InkML;

namespace WCN_MVC.Controllers
{
    public class WCNController : Controller
    {
        public static WCNModel cMain;
        WCNDBHandle wcnDBH = new WCNDBHandle();
        WCNModel wcnModel = new WCNModel();
        GenericClass gc = new GenericClass();
        static int gblWcnId = 0;
        static string companyName = "Company";
        static string divisionName = "Division";
        static string locationName = "Location";

        // GET: WCN

        public ActionResult Index(string SearchString, int? page)
        {
            WCNDBHandle dbhandle = new WCNDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(dbhandle.GetWCNTracker(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: WCN/Details/5
        public ActionResult Details(int id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            gblWcnId = id;
            ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            ViewBag.Divisions = wcnDBH.GetDivisionList(divisionName);
            ViewBag.Periods = wcnDBH.GetPeriodList();
            ViewBag.Locations = wcnDBH.GetLocationList(locationName);
            ViewBag.Customers = wcnDBH.GetCustomerList();
            //return View();
            //cMain = new WCNModel();
            //wcnModel.CompanyLists = cMain.CompanyLists = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            //wcnModel.DivisionLists = cMain.DivisionLists = wcnDBH.GetDivisionList(divisionName);
            //wcnModel.PeriodLists = cMain.PeriodLists = wcnDBH.GetPeriodList();
            //wcnModel.LocationLists = cMain.LocationLists = wcnDBH.GetLocationList(locationName);
            //wcnModel.CustomerLists = cMain.CustomerLists = wcnDBH.GetCustomerList();
            return View(cdb.GetWCNTracker(string.Empty).Find(cmodel => cmodel.Id == id));
        }



        [HttpPost]
        public JsonResult GetEmployees()
        {

            Employee[] emps = {
                        new Employee { Name = "Mark", Address = "Address1", PhoneNo = "XYZ", Country = "UK" },
                                  new Employee { Name = "John", Address = "Address2", PhoneNo = "XYZ", Country = "Germany" },
                                  new Employee { Name = "Lisa", Address = "Address3", PhoneNo = "XYZ", Country = "UK" },
                                  new Employee { Name = "Mike", Address = "Address4", PhoneNo = "XYZ", Country = "Australia" }
        };

            return Json(emps, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetDataWCNLineItems()
        {
            WCNDBHandle cdb = new WCNDBHandle();
            return Json(cdb.GetDataWCNLineItems(gblWcnId), JsonRequestBehavior.AllowGet);
        }


        // GET: WCN/Create
        public ActionResult Create()
        {
            cMain = new WCNModel();
            wcnModel.CompanyLists = cMain.CompanyLists = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            wcnModel.DivisionLists = cMain.DivisionLists = wcnDBH.GetDivisionList(divisionName);
            wcnModel.PeriodLists = cMain.PeriodLists = wcnDBH.GetPeriodList();
            wcnModel.LocationLists = cMain.LocationLists = wcnDBH.GetLocationList(locationName);
            wcnModel.CustomerLists = cMain.CustomerLists = wcnDBH.GetCustomerList();
            wcnModel.ResourceTypeLists = cMain.ResourceTypeLists = wcnDBH.GetResourceTypeList();
            wcnModel.UnitLists = cMain.UnitLists = wcnDBH.GetUnitList();
            wcnModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);
            wcnModel.Status = "New";
            return View(wcnModel);
        }



        // POST: WCN/Create
        [HttpPost]
        public ActionResult Create(WCNModel cmodel)
        {
            wcnModel.CompanyLists = cMain.CompanyLists = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            wcnModel.DivisionLists = cMain.DivisionLists = wcnDBH.GetDivisionList(divisionName);
            wcnModel.PeriodLists = cMain.PeriodLists = wcnDBH.GetPeriodList();
            wcnModel.LocationLists = cMain.LocationLists = wcnDBH.GetLocationList(locationName);
            wcnModel.CustomerLists = cMain.CustomerLists = wcnDBH.GetCustomerList();
            wcnModel.ResourceTypeLists = cMain.ResourceTypeLists = wcnDBH.GetResourceTypeList();
            wcnModel.UnitLists = cMain.UnitLists = wcnDBH.GetUnitList();
            wcnModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);
            wcnModel.Status = "New";
            ModelState.Clear();
            return View(wcnModel);
        }



        public JsonResult AddWCNTrackerDetails(List<WCNLineItems> wcnLineItems, WCNModel tracker)
        {
            int retVal = 0;
            int insertedRecords = 0;
            WCNDBHandle cdb = new WCNDBHandle();
            WCNLineItems LineItems = new WCNLineItems();
            DataTable dt = new DataTable();

            try
            {
                tracker.UserId = Convert.ToInt16(Session["UserID"]);
                dt = cdb.AddWCNTracker(tracker);
                retVal = Convert.ToInt16(dt.Rows[0]["dbVal"]);

                if (retVal == 1)
                {
                    //Check for NULL.
                    if (wcnLineItems == null)
                    {
                        wcnLineItems = new List<WCNLineItems>();
                    }

                    int retValLines = cdb.DeleteWCNLineItems(dt.Rows[0]["WcnNo"].ToString());

                    foreach (WCNLineItems additem in wcnLineItems)
                    {
                        LineItems.WcnNo = dt.Rows[0]["WcnNo"].ToString();
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
                    }

                    string wcnNo = dt.Rows[0]["WcnNo"].ToString();

                    //ModelState.Clear();
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
                dt = null;
            }

            return Json(insertedRecords);
        }


        // GET: WCN/Edit/5
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


        public JsonResult get_data()
        {
            WCNDBHandle cdb = new WCNDBHandle();
            DataSet ds = cdb.GetWCNLineItems("B1");
            List<WCNLineItems> wcnItems = new List<WCNLineItems>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                wcnItems.Add(new WCNLineItems
                {
                    ResourceType = Convert.ToString(dr["resource_type"]),
                    Resource = Convert.ToString(dr["resource"]),
                    StartDate = Convert.ToDateTime(dr["start_date"]),
                    EndDate = Convert.ToDateTime(dr["end_date"]),
                    Days = Convert.ToInt16(dr["no_of_days"]),
                    Qty = Convert.ToInt16(dr["qty"]),
                    Unit = Convert.ToInt16(dr["unit"]),
                    Rate = Convert.ToInt16(dr["rate"]),
                    Total = Convert.ToInt16(dr["total"])
                });
            }

            return Json(wcnItems, JsonRequestBehavior.AllowGet);
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
                retVal = cdb.EditWCNTracker(tracker);

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



        public ActionResult PopulateDropDownListFor()
        {
            cMain = new WCNModel();
            wcnModel.CompanyLists = cMain.CompanyLists = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            wcnModel.DivisionLists = cMain.DivisionLists = wcnDBH.GetDivisionList(divisionName);
            wcnModel.PeriodLists = cMain.PeriodLists = wcnDBH.GetPeriodList();
            wcnModel.LocationLists = cMain.LocationLists = wcnDBH.GetLocationList(locationName);
            wcnModel.CustomerLists = cMain.CustomerLists = wcnDBH.GetCustomerList();
            wcnModel.ResourceTypeLists = cMain.ResourceTypeLists = wcnDBH.GetResourceTypeList();
            wcnModel.UnitLists = cMain.UnitLists = wcnDBH.GetUnitList();
            wcnModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);
            return View("Edit", wcnModel);
        }




        // POST: WCN/Edit/5
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



        // GET: WCN/Delete/5
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



        // POST: WCN/Delete/5
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



        // POST: WCN/AddDivision
        [HttpPost]
        public ActionResult AddDivision(WCNModel cmodel)
        {
            int retVal = 0;

            try
            {
                WCNDBHandle cdb = new WCNDBHandle();
                cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.AddDivision(cmodel);

                if (retVal == 1)
                {
                    cmodel.PageMessage = "Division added successfully.";
                    wcnModel.DivisionLists = cMain.DivisionLists = wcnDBH.GetDivisionList(divisionName);
                }
                else
                {
                    cmodel.PageMessage = "Division not add.";
                }

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }



        // POST: WCN/AddLocation
        [HttpPost]
        public ActionResult AddLocation(WCNModel cmodel)
        {
            int retVal = 0;

            try
            {
                WCNDBHandle cdb = new WCNDBHandle();
                cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.AddLocation(cmodel);

                if (retVal == 1)
                {
                    cmodel.PageMessage = "Location added successfully.";
                    wcnModel.LocationLists = cMain.LocationLists = wcnDBH.GetLocationList(locationName);
                }
                else
                {
                    cmodel.PageMessage = "Location not add.";
                }

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }



        // POST: WCN/AddCustomer
        [HttpPost]
        public ActionResult AddCustomer(WCNModel cmodel)
        {
            int retVal = 0;

            try
            {
                WCNDBHandle cdb = new WCNDBHandle();
                cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.AddCustomer(cmodel);

                if (retVal == 1)
                {
                    cmodel.PageMessage = "Customer added successfully.";
                    wcnModel.CustomerLists = cMain.CustomerLists = wcnDBH.GetCustomerList();
                }
                else
                {
                    cmodel.PageMessage = "Customer not add.";
                }

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }



        // POST: WCN/AddPeriod
        [HttpPost]
        public ActionResult AddPeriod(WCNModel cmodel)
        {
            int retVal = 0;

            try
            {
                WCNDBHandle cdb = new WCNDBHandle();
                cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.AddPeriod(cmodel);

                if (retVal == 1)
                {
                    cmodel.PageMessage = "Period added successfully.";
                    wcnModel.PeriodLists = cMain.PeriodLists = wcnDBH.GetPeriodList();
                }
                else
                {
                    cmodel.PageMessage = "Period already exists.";
                }

                return RedirectToAction("Create");
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
            string fileName = "WCNTracker_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

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


        public ActionResult ConfirmWCN()
        {
            WCNDBHandle cdb = new WCNDBHandle();
            int userId = Convert.ToInt16(Session["UserId"]);
            int retVal = cdb.ConfirmWCN(gblWcnId, userId);

            if (retVal == 1)
            {
                ModelState.Clear();
                ViewBag.Message = "WCN confirmed.";
                cMain = new WCNModel();
                ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
                ViewBag.Divisions = wcnDBH.GetDivisionList(divisionName);
                ViewBag.Periods = wcnDBH.GetPeriodList();
                ViewBag.Locations = wcnDBH.GetLocationList(locationName);
                ViewBag.Customers = wcnDBH.GetCustomerList();
                SendEmail_ConfirmedWCN(gblWcnId);
            }
            else if (retVal == 2)
            {
                ViewBag.Message = "WCN not confirmed.";
            }
            else if (retVal == 3)
            {
                ViewBag.Message = "Error, please contact system admin.";
            }

            return View("Edit");
        }


        public ActionResult CompleteWCN()
        {
            WCNDBHandle cdb = new WCNDBHandle();
            int userId = Convert.ToInt16(Session["UserId"]);
            DateTime completedDate = DateTime.Now;
            int retVal = cdb.CompleteWCN(gblWcnId, completedDate, userId);

            if (retVal == 1)
            {
                ModelState.Clear();
                ViewBag.Message = "WCN completed.";
                SendEmail_CompletedWCN(gblWcnId);
            }
            else if (retVal == 2)
            {
                ViewBag.Message = "WCN not completed.";
            }
            else if (retVal == 3)
            {
                ViewBag.Message = "Error, please contact system admin.";
            }

            return View("Edit");

        }



        private void SendEmail_Created(int wcn_id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();
            SendEmail objSendEmail = new SendEmail();
            string mailBody = string.Empty;
            string htmlbody = string.Empty;
            int emailStatus = 0;

            wcnList = cdb.GetWCNDetails(wcn_id);

            if (wcnList.Count > 0)
            {
                mailBody += "Dear Approver,<br><br>";
                mailBody += "Please find below created WCN details.<br><br>";
                mailBody += "WCN Number :- " + wcnList[0].WcnNo + "<br>";
                mailBody += "Status :- " + wcnList[0].Status + "<br>";
                mailBody += "WCN Date :- " + wcnList[0].WCNDate + "<br>";
                mailBody += "Company :- " + wcnList[0].CompanyName + "<br>";
                mailBody += "Division :- " + wcnList[0].DivisionName + "<br>";
                mailBody += "Customer :- " + wcnList[0].CustomerName + "<br>";
                mailBody += "Area :- " + wcnList[0].LocationName + "<br>";
                mailBody += "Month :- " + wcnList[0].Period + "<br>";
                mailBody += "Description :- " + wcnList[0].Description + "<br>";
                mailBody += "Rig No. :- " + wcnList[0].RigNo + "<br>";
                mailBody += "Total Revenue :- " + wcnList[0].TotalRevenue + "<br>";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/EMailTemplate.html")))
                {
                    htmlbody = reader.ReadToEnd();
                }

                htmlbody = htmlbody.Replace("{Title}", "Created WCN No." + wcnList[0].WcnNo);

                htmlbody = htmlbody.Replace("{Description}", mailBody);

                emailStatus = objSendEmail.SendMail_GMail("sumit@truckomangroup.com", "Created WCN No." + wcnList[0].WcnNo, htmlbody, "WCN ADMIN<wcn.togs@gmail.com>");

                if (emailStatus == 1)
                {
                    ViewBag.PopMessage = "Email sent";
                }
                else
                {
                    ViewBag.PopMessage = "Email not send.";
                }
            }
        }


        private void SendEmail_ConfirmedWCN(int wcn_id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();
            SendEmail objSendEmail = new SendEmail();
            string mailBody = string.Empty;
            string htmlbody = string.Empty;
            int emailStatus = 0;

            wcnList = cdb.GetWCNDetails(wcn_id);

            if (wcnList.Count > 0)
            {
                mailBody += "Dear Approver,<br><br>";
                mailBody += "Please find below confirmed WCN details.<br><br>";
                mailBody += "WCN Number :- " + wcnList[0].WcnNo + "<br>";
                mailBody += "Status :- " + wcnList[0].Status + "<br>";
                mailBody += "WCN Date :- " + wcnList[0].WCNDate + "<br>";
                mailBody += "Company :- " + wcnList[0].CompanyName + "<br>";
                mailBody += "Division :- " + wcnList[0].DivisionName + "<br>";
                mailBody += "Customer :- " + wcnList[0].CustomerName + "<br>";
                mailBody += "Area :- " + wcnList[0].LocationName + "<br>";
                mailBody += "Month :- " + wcnList[0].Period + "<br>";
                mailBody += "Description :- " + wcnList[0].Description + "<br>";
                mailBody += "Rig No. :- " + wcnList[0].RigNo + "<br>";
                mailBody += "Total Revenue :- " + wcnList[0].TotalRevenue + "<br>";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/EMailTemplate.html")))
                {
                    htmlbody = reader.ReadToEnd();
                }

                htmlbody = htmlbody.Replace("{Title}", "Confirmed WCN No." + wcnList[0].WcnNo);

                htmlbody = htmlbody.Replace("{Description}", mailBody);

                emailStatus = objSendEmail.SendMail_GMail("sumit@truckomangroup.com", "Confirmed WCN No." + wcnList[0].WcnNo, htmlbody, "WCN ADMIN<wcn.togs@gmail.com>");

                if (emailStatus == 1)
                {
                    ViewBag.PopMessage = "Email sent";
                }
                else
                {
                    ViewBag.PopMessage = "Email not send.";
                }
            }
        }


        private void SendEmail_CompletedWCN(int wcn_id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();
            SendEmail objSendEmail = new SendEmail();
            string mailBody = string.Empty;
            string htmlbody = string.Empty;
            int emailStatus = 0;

            wcnList = cdb.GetWCNDetails(wcn_id);

            if (wcnList.Count > 0)
            {
                mailBody += "Dear Approver,<br><br>";
                mailBody += "Please find below completed WCN details.<br><br>";
                mailBody += "WCN Number :- " + wcnList[0].WcnNo + "<br>";
                mailBody += "Status :- " + wcnList[0].Status + "<br>";
                mailBody += "WCN Date :- " + wcnList[0].WCNDate + "<br>";
                mailBody += "WCN Completed Date :- " + wcnList[0].CompletedDate + "<br>";
                mailBody += "Company :- " + wcnList[0].CompanyName + "<br>";
                mailBody += "Division :- " + wcnList[0].DivisionName + "<br>";
                mailBody += "Customer :- " + wcnList[0].CustomerName + "<br>";
                mailBody += "Area :- " + wcnList[0].LocationName + "<br>";
                mailBody += "Month :- " + wcnList[0].Period + "<br>";
                mailBody += "Description :- " + wcnList[0].Description + "<br>";
                mailBody += "Rig No. :- " + wcnList[0].RigNo + "<br>";
                mailBody += "Total Revenue :- " + wcnList[0].TotalRevenue + "<br>";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/EMailTemplate.html")))
                {
                    htmlbody = reader.ReadToEnd();
                }

                htmlbody = htmlbody.Replace("{Title}", "Completed WCN No." + wcnList[0].WcnNo);

                htmlbody = htmlbody.Replace("{Description}", mailBody);

                emailStatus = objSendEmail.SendMail_GMail("sumit@truckomangroup.com", "Completed WCN No." + wcnList[0].WcnNo, htmlbody, "WCN ADMIN<wcn.togs@gmail.com>");

                if (emailStatus == 1)
                {
                    ViewBag.PopMessage = "Email sent";
                }
                else
                {
                    ViewBag.PopMessage = "Email not send.";
                }
            }
        }



       
    }
}
