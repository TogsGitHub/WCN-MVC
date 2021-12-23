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
using System.ComponentModel;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace WCN_MVC.Controllers
{
    public class UnbilledController : Controller
    {
        public static WCNModel cMain;
        WCNDBHandle wcnDBH = new WCNDBHandle();
        WCNModel wcnModel = new WCNModel();

        // GET: Unbilled
        public ActionResult Index()
        {
            cMain = new WCNModel();
            ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            return View();
        }


        public ActionResult GenerateUnbilledReport(FormCollection form)
        {
            ReportDocument rd = new ReportDocument();
            WCNDBHandle wcnDBH = new WCNDBHandle();
            DataTable dtMainReport = new DataTable();
            ViewBag.Message = string.Empty;

            if (form["CompanyId"] == string.Empty || form["CompanyId"] == "0")
            {
                ViewBag.Message = "Company required.";
                ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));
            }
            else
            {
                int companyID = Convert.ToInt16(form["CompanyId"]);
                dtMainReport = wcnDBH.UnbilledReport(companyID);

                if (dtMainReport.Rows.Count <= 0)
                {
                    ViewBag.Companies = wcnDBH.GetUserCompanyList(Convert.ToInt16(Session["UserID"]));

                    for (int i = 0; i < ViewBag.Companies.Count; i++)
                    {
                        if (Convert.ToInt16(ViewBag.Companies[i].Value) == Convert.ToInt16(companyID))
                        {
                            ViewBag.Message = "No data found for company - " + ViewBag.Companies[i].Text;
                        }
                    }
                }
                else
                {
                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "UnbilledReport.rpt"));
                    rd.SetDataSource(dtMainReport);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    try
                    {
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/pdf", "Unbilled_Report_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        //throw;
                    }
                }
            }

            return View("Index");
        }
    }
}