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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(string SearchString, int? page)
        {
            CustomerDBHandle dbhandle = new CustomerDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(dbhandle.GetCustomer(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }



        // GET: Customer/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            CustomerDBHandle cdb = new CustomerDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetCustomer(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    CustomerDBHandle cdb = new CustomerDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddCustomer(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Customer created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Customer already created.";
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



        // GET: Customer/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerDBHandle cdb = new CustomerDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetCustomer(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CustomerModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    CustomerDBHandle cdb = new CustomerDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditCustomer(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Customer edited.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Customer not active.";
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



        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                CustomerDBHandle cdb = new CustomerDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteCustomer(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "Company deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "Company deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: Customer/Delete/5
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
            CustomerDBHandle cdb = new CustomerDBHandle();
            GenericClass gc = new GenericClass();
            List<CustomerModel> lst = new List<CustomerModel>();
            lst = cdb.GetCustomer(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "CustomerMaster");
            string fileName = "CustomerMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

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
