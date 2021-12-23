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
    public class ResourceTypeController : Controller
    {
        // GET: ResourceType
        public ActionResult Index(string SearchString, int? page)
        {
            ResourceTypeDBHandle dbhandle = new ResourceTypeDBHandle();
            GenericClass gc = new GenericClass();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            //return View(dbhandle.GetResourceType(SearchString));//, "company_name", "asc").ToPagedList(page ?? gc.minPage, gc.maxPage));
            return View(dbhandle.GetResourceType(SearchString).ToPagedList(page ?? gc.minPage, gc.maxPage));
        }


        
        // GET: ResourceType/Details/5
        public ActionResult Details(int id, string SearchString)
        {
            ResourceTypeDBHandle cdb = new ResourceTypeDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetResourceType(SearchString).Find(cmodel => cmodel.Id == id));//, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }

        
        
        // GET: ResourceType/Create
        public ActionResult Create()
        {
            return View();
        }

        
        
        // POST: ResourceType/Create
        [HttpPost]
        public ActionResult Create(ResourceTypeModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    ResourceTypeDBHandle cdb = new ResourceTypeDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.AddResourceType(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Resource Type created.";
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Resource type already created.";
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

        
        
        // GET: ResourceType/Edit/5
        public ActionResult Edit(int id, string SearchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ResourceTypeDBHandle cdb = new ResourceTypeDBHandle();
            SearchString = String.IsNullOrEmpty(SearchString) ? string.Empty : SearchString;
            return View(cdb.GetResourceType(SearchString).Find(cmodel => cmodel.Id == id)); //, "company_name", "asc").Find(cmodel => cmodel.Id == id));
        }

        
        
        // POST: ResourceType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ResourceTypeModel cmodel)
        {
            int retVal = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    ResourceTypeDBHandle cdb = new ResourceTypeDBHandle();
                    cmodel.UserId = Convert.ToInt16(Session["UserID"]);
                    retVal = cdb.EditResourceType(cmodel);

                    if (retVal == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Message = "Resource type edited.";
                        
                    }
                    else if (retVal == 2)
                    {
                        ViewBag.Message = "Resource type not active.";
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

        
        
        // GET: ResourceType/Delete/5
        public ActionResult Delete(int id)
        {
            int retVal = 0;

            try
            {
                ResourceTypeDBHandle cdb = new ResourceTypeDBHandle();
                int userId = Convert.ToInt16(Session["UserID"]);
                retVal = cdb.DeleteResourceType(id, userId);

                if (retVal == 1)
                {
                    ViewBag.AlertMsg = "Resource type deleted successfully";
                }
                else
                {
                    ViewBag.AlertMsg = "Resource type deleted successfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        
        
        // POST: ResourceType/Delete/5
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
            ResourceTypeDBHandle cdb = new ResourceTypeDBHandle();
            GenericClass gc = new GenericClass();
            List<ResourceTypeModel> lst = new List<ResourceTypeModel>();
            lst = cdb.GetResourceType(string.Empty);//, "company_name", "asc");
            DataTable dt = gc.ConvertToDataTable(lst, "ResourceTypeMaster");
            string fileName = "ResourceTypeMaster_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".xlsx";

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
