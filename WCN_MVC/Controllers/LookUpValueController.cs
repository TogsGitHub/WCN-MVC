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
    public class LookUpValueController : Controller
    {
        public static LookUpValueModel cMain;
        LookUpValueDBHandle lookUpValueDBH = new LookUpValueDBHandle();
        LookUpValueModel lookupValueModel = new LookUpValueModel();

        // GET: LookUpValue
        public ActionResult Index(string SearchString, int? page) //, string Sorting_Order
        {
            LookUpValueDBHandle dbhandle = new LookUpValueDBHandle();
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

            return View(dbhandle.GetLookUpValue(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage)); //, "company_name", "asc"
        }



        // GET: LookUpValue/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            LookUpValueDBHandle cdb = new LookUpValueDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetLookUpValue(SearchString).Find(cmodel => cmodel.Id == id));
        }



        // GET: LookUpValue/Create
        public ActionResult Create()
        {
            cMain = new LookUpValueModel();
            //lookupValueModel.TypeLists = cMain.TypeLists = lookUpValueDBH.GetTypeList();
            ViewBag.TypeLists = lookUpValueDBH.GetTypeList();
            return View(lookupValueModel);
        }

        
        
        // POST: LookUpValue/Create
        [HttpPost]
        public ActionResult Create(LookUpValueModel cmodel)
        {
            int retVal = 0;

            try
            {
                //if (ModelState.IsValid)
                //{
                    LookUpValueDBHandle cdb = new LookUpValueDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddLookUpValue(cmodel);

                    if (retVal == 1)
                    {
                        //ModelState.Clear();
                        ViewBag.Message = "Look-up value created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Look-up value already created.";
                    }
                    else if (retVal == 3)
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }
                //}

                ModelState.Clear();
                cMain = new LookUpValueModel();
                //lookupValueModel.TypeLists = cMain.TypeLists = lookUpValueDBH.GetTypeList();
                ViewBag.TypeLists = lookUpValueDBH.GetTypeList();
                return View(cmodel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: LookUpValue/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LookUpValueDBHandle cdb = new LookUpValueDBHandle();
            ViewBag.TypeLists = lookUpValueDBH.GetTypeList();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetLookUpValue(SearchString).Find(cmodel => cmodel.Id == id));
        }



        // POST: LookUpValue/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, LookUpValueModel cmodel)
        {
            int retVal = 0;

            try
            {
                //if (ModelState.IsValid)
                //{
                    LookUpValueDBHandle cdb = new LookUpValueDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditLookUpValue(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Look-up value edited.";
                        ViewBag.TypeLists = lookUpValueDBH.GetTypeList();
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Look-up value not active.";
                    }
                    else if (retVal == 3)
                    {
                        ViewBag.Message = "Error. Please contact admin.";
                    }
                //}

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: LookUpValue/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            int retVal = 0;

            try
            {
                LookUpValueDBHandle cdb = new LookUpValueDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = await cdb.DeleteLookUpValue(id, userId).ConfigureAwait(true);

                if (retVal == 1)
                {
                    ViewBag.Message = "Look-up value deleted.";
                }
                else if (retVal == 2)
                {
                    ViewBag.Message = "Look-up value not active.";
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



        // POST: LookUpValue/Delete/5
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
            LookUpValueDBHandle cdb = new LookUpValueDBHandle();
            GenericClass gc = new GenericClass();
            List<LookUpValueModel> lst = new List<LookUpValueModel>();
            lst = cdb.GetLookUpValue(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "LookUpValue");
            string fileName = "LookUpValue_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("UserId");
                dt.Columns.Remove("TypeId");
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
