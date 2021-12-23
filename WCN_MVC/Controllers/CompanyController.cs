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
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index(string SearchString, int? page) //, string Sorting_Order
        {
            CompanyDBHandle dbhandle = new CompanyDBHandle();
            GenericClass gc = new GenericClass();
            ModelState.Clear();
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

            return View(dbhandle.GetCompany(SearchString, "company_name", "asc").ToPagedList(page ?? gc.minPage, gc.maxPage));
        }




        // GET: Company/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            CompanyDBHandle cdb = new CompanyDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetCompany(SearchString, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }




        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }




        // POST: Company/Create
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CompanyModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    CompanyDBHandle cdb = new CompanyDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = await cdb.AddCompany(cmodel).ConfigureAwait(true);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Company created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Company code already created.";
                    }
                    else if (retVal == 3)
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }
                }

                return View(cmodel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }




        // GET: Company/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CompanyDBHandle cdb = new CompanyDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetCompany(SearchString, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }



        // POST: Company/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, CompanyModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    CompanyDBHandle cdb = new CompanyDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = await cdb.EditCompany(cmodel).ConfigureAwait(true);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Company edited.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Company not active.";
                    }
                    else if (retVal == 3)
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


        // GET: Company/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            int retVal = 0;

            try
            {
                CompanyDBHandle cdb = new CompanyDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = await cdb.DeleteCompany(id, userId).ConfigureAwait(true);

                if (retVal == 1)
                {
                    ViewBag.Message = "Company deleted.";
                }
                else if (retVal == 2)
                {
                    ViewBag.Message = "Company not active.";
                }
                else
                {
                    ViewBag.Message = "Error. Please contact admin.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // POST: Company/Delete/5
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
            CompanyDBHandle cdb = new CompanyDBHandle();
            GenericClass gc = new GenericClass();
            List<CompanyModel> lst = new List<CompanyModel>();
            lst = cdb.GetCompany(string.Empty, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "CompanyMaster");
            string fileName = "CompanyMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

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


        //public DataTable ConvertToDataTable<T>(IList<T> data)
        //{
        //    PropertyDescriptorCollection properties =
        //       TypeDescriptor.GetProperties(typeof(T));
        //    DataTable table = new DataTable();
        //    table.TableName = "CompanyMaster";
        //    foreach (PropertyDescriptor prop in properties)
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    foreach (T item in data)
        //    {
        //        DataRow row = table.NewRow();
        //        foreach (PropertyDescriptor prop in properties)
        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        table.Rows.Add(row);
        //    }
        //    table.AcceptChanges();
        //    return table;
        //}

    }

}

