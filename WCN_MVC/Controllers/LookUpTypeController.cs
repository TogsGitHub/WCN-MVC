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
    public class LookUpTypeController : Controller
    {
        // GET: LookUpType
        public ActionResult Index(string SearchString, int? page) //, string Sorting_Order
        {
            LookUpTypeDBHandle dbhandle = new LookUpTypeDBHandle();
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

            return View(dbhandle.GetLookUpType(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage)); //, "company_name", "asc"
        }



        // GET: LookUpType/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            LookUpTypeDBHandle cdb = new LookUpTypeDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetLookUpType(SearchString).Find(cmodel => cmodel.Id == id));
        }



        // GET: LookUpType/Create
        public ActionResult Create()
        {
            return View();
        }

        
        
        // POST: LookUpType/Create
        [HttpPost]
        public ActionResult Create(LookUpTypeModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    LookUpTypeDBHandle cdb = new LookUpTypeDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddLookUpType(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Look-up type created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Look-up type already created.";
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



        // GET: LookUpType/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LookUpTypeDBHandle cdb = new LookUpTypeDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetLookUpType(SearchString).Find(cmodel => cmodel.Id == id));
        }



        // POST: LookUpType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, LookUpTypeModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    LookUpTypeDBHandle cdb = new LookUpTypeDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditLookUpType(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Look-up type edited.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Look-up type not active.";
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



        // GET: LookUpType/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            int retVal = 0;

            try
            {
                LookUpTypeDBHandle cdb = new LookUpTypeDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = await cdb.DeleteLookUpType(id, userId).ConfigureAwait(true);

                if (retVal == 1)
                {
                    ViewBag.Message = "Look-Up Type deleted.";
                }
                else if (retVal == 2)
                {
                    ViewBag.Message = "Look-Up Type not active.";
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



        // POST: LookUpType/Delete/5
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
            LookUpTypeDBHandle cdb = new LookUpTypeDBHandle();
            GenericClass gc = new GenericClass();
            List<LookUpTypeModel> lst = new List<LookUpTypeModel>();
            lst = cdb.GetLookUpType(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "LookUpType");
            string fileName = "LookUpType_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("Message");
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
