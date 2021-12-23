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
    public class WCNPaymentController : Controller
    {
        public static WCNModel cMain;
        WCNDBHandle wcnDBH = new WCNDBHandle();
        WCNModel wcnModel = new WCNModel();
        GenericClass gc = new GenericClass();
        static int gblWcnId = 0;


        // GET: WCNPayment
        public ActionResult Index(string SearchString, int? page)
        {
            WCNDBHandle dbhandle = new WCNDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(dbhandle.GetWCNPayment(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: WCNPayment/Details/5
        public ActionResult Details(int id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            return View(cdb.GetWCNPayment(string.Empty).Find(cmodel => cmodel.Id == id));
        }



        // GET: WCNPayment/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: WCNPayment/Create
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



        // GET: WCNPayment/Edit/5
        public ActionResult Edit(int id)
        {
            WCNDBHandle cdb = new WCNDBHandle();
            //PopulateDropDownListFor();// ddl = new PopulateDropDownListFor();
            gblWcnId = id;

            cMain = new WCNModel();
            //ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            //ViewBag.Divisions = wcnDBH.GetDivisionList();
            //ViewBag.Periods = wcnDBH.GetPeriodList();
            //ViewBag.Locations = wcnDBH.GetLocationList();
            //ViewBag.Customers = wcnDBH.GetCustomerList();
            //get_data();

            //wcnModel.ResourceTypeLists = cMain.ResourceTypeLists = wcnDBH.GetResourceTypeList();
            //wcnModel.UnitLists = cMain.UnitLists = wcnDBH.GetUnitList();
            //wcnModel.YearLists = cMain.YearLists = gc.GetYears(DateTime.Now.Year);

            return View(cdb.GetWCNPayment(string.Empty).Find(cmodel => cmodel.Id == id));
        }



        // POST: WCNPayment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WCNModel cmodel)
        {
            int retVal = 0;

            try
            {
                WCNDBHandle cdb = new WCNDBHandle();
                cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.EditWCNPayment(cmodel);

                if (retVal == 1)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Payment details edited.";
                }
                else if (retVal == 1)
                {
                    ViewBag.Message = "Payment details already edited.";
                }
                else
                {
                    ViewBag.Message = "Error. Please contact admin.";
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: WCNPayment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }



        // POST: WCNPayment/Delete/5
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
            lst = cdb.GetWCNPayment(string.Empty);
            //(string.Empty, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "WCNTracker");
            string fileName = "WCNPayment_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("PeriodId");
                dt.Columns.Remove("CustomerId");
                dt.Columns.Remove("CompanyLists");
                dt.Columns.Remove("CompanyId");
                dt.Columns.Remove("DivisionId");
                dt.Columns.Remove("DivisionCode");
                dt.Columns.Remove("DivisionLists");
                dt.Columns.Remove("PeriodLists");
                dt.Columns.Remove("YearLists");
                dt.Columns.Remove("Month");
                dt.Columns.Remove("Year");
                dt.Columns.Remove("LocationId");
                dt.Columns.Remove("LocationLists");
                dt.Columns.Remove("OtherLocation");
                dt.Columns.Remove("ContactNo");
                dt.Columns.Remove("ContactPerson");
                dt.Columns.Remove("Address");
                dt.Columns.Remove("Emailid");
                dt.Columns.Remove("CustomerLists");
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
                dt.Columns.Remove("CompanyName");
                dt.Columns.Remove("DivisionName");
                dt.Columns.Remove("WCNDate");
                dt.Columns.Remove("StartDate");
                dt.Columns.Remove("CompletedDate");
                dt.Columns.Remove("LocationName");
                dt.Columns.Remove("RigNo");
                dt.Columns.Remove("IsWCNSent");
                dt.Columns.Remove("IsWCNSigned");
                dt.Columns.Remove("IsClientSigned");
                dt.Columns.Remove("IsDiscount");
                dt.Columns.Remove("DiscountAmount");
                dt.Columns.Remove("DiscountRemark");

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
