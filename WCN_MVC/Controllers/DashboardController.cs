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
    public class DashboardController : Controller
    {
        DashboardDBHandle dbhandle = new DashboardDBHandle();
        int userId = 0;

        // GET: Dashboard
        public ActionResult Index()
        {
            userId = Convert.ToInt16(Session["UserId"]);
            ViewBag.Dashboard = "Total Revenue Details";
            return View(dbhandle.GetDashboard(userId));
        }


        public ActionResult GetTotalRevenueDetails(DashboardModel model)
        {
            userId = Convert.ToInt16(Session["UserId"]);
            ViewBag.Dashboard = "Total Revenue Details";
            return View("Index", dbhandle.GetTotalRevenueDetails(userId));
        }


        public ActionResult GetTotalUnbilledDetails(DashboardModel model)
        {
            userId = Convert.ToInt16(Session["UserId"]);
            ViewBag.Dashboard = "Total Unbilled Revenue";
            return View("Index", dbhandle.GetTotalUnbilledDetails(userId));
        }


        public ActionResult GetCreatedDetails(DashboardModel model)
        {
            userId = Convert.ToInt16(Session["UserId"]);
            ViewBag.Dashboard = "Created/Confirmed/Completed Details";
            return View("Index", dbhandle.GetCreatedDetails(userId));
        }


        public ActionResult GetApprovedDetails(DashboardModel model)
        {
            userId = Convert.ToInt16(Session["UserId"]);
            ViewBag.Dashboard = "Approved Details";
            return View("Index", dbhandle.GetApprovedDetails(userId));
        }


        public ActionResult GetPaymentDetails(DashboardModel model)
        {
            userId = Convert.ToInt16(Session["UserId"]);
            ViewBag.Dashboard = "Payment Details";
            return View("Index", dbhandle.GetPaymentDetails(userId));
        }
    }
}