using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using WCN_MVC.Models;

namespace WCN_MVC.DBHandle
{
    public class DashboardDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();


        // **************** GET DASHBOARD COUNTS *********************
        public List<DashboardModel> GetDashboard(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<DashboardModel> dashboardList = new List<DashboardModel>();

            try
            {
                SqlParameter user_id = new SqlParameter("@user_id", userId);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_dashboard", user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        dashboardList.Add(
                            new DashboardModel
                            {
                                TotalRevenue = Convert.ToDecimal(dr["dashboard_total_revenue"]),
                                TotalUnbilled = Convert.ToDecimal(dr["dashboard_total_unbilled"]),
                                WcnCreated = Convert.ToInt32(dr["dashboard_cnt_created"]),
                                WcnApproved = Convert.ToInt32(dr["dashboard_cnt_approved"]),
                                WcnPayment = Convert.ToInt32(dr["dashboard_cnt_payment"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return dashboardList;
        }



        // **************** GET DASHBOARD TOTAL REVENUE IN DETAILS *********************

        public List<DashboardModel> GetTotalRevenueDetails(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<DashboardModel> dashboardList = new List<DashboardModel>();

            try
            {
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_dashboard_total_revenue_details", user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        dashboardList.Add(
                            new DashboardModel
                            {
                                TotalRevenue = Convert.ToDecimal(dr["dashboard_total_revenue"]),
                                TotalUnbilled = Convert.ToDecimal(dr["dashboard_total_unbilled"]),
                                WcnCreated = Convert.ToInt32(dr["dashboard_cnt_created"]),
                                WcnApproved = Convert.ToInt32(dr["dashboard_cnt_approved"]),
                                WcnPayment = Convert.ToInt32(dr["dashboard_cnt_payment"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WCNNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                Revenue = Convert.ToInt32(dr["total_revenue"]),
                                Unbilled = Convert.ToInt32(dr["total_unbilled"]),
                                CreatedBy = Convert.ToString(dr["created_by"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return dashboardList;
        }



        // **************** GET DASHBOARD TOTAL UNBILLED IN DETAILS *********************
        public List<DashboardModel> GetTotalUnbilledDetails(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<DashboardModel> dashboardList = new List<DashboardModel>();

            try
            {
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_dashboard_total_unbilled_details", user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        dashboardList.Add(
                            new DashboardModel
                            {
                                TotalRevenue = Convert.ToDecimal(dr["dashboard_total_revenue"]),
                                TotalUnbilled = Convert.ToDecimal(dr["dashboard_total_unbilled"]),
                                WcnCreated = Convert.ToInt32(dr["dashboard_cnt_created"]),
                                WcnApproved = Convert.ToInt32(dr["dashboard_cnt_approved"]),
                                WcnPayment = Convert.ToInt32(dr["dashboard_cnt_payment"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WCNNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                Revenue = Convert.ToInt32(dr["total_revenue"]),
                                Unbilled = Convert.ToInt32(dr["total_unbilled"]),
                                CreatedBy = Convert.ToString(dr["created_by"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return dashboardList;
        }



        // **************** GET DASHBOARD TOTAL CREATED COUNT IN DETAILS *********************
        public List<DashboardModel> GetCreatedDetails(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<DashboardModel> dashboardList = new List<DashboardModel>();

            try
            {
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_dashboard_created_details", user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        dashboardList.Add(
                            new DashboardModel
                            {
                                TotalRevenue = Convert.ToDecimal(dr["dashboard_total_revenue"]),
                                TotalUnbilled = Convert.ToDecimal(dr["dashboard_total_unbilled"]),
                                WcnCreated = Convert.ToInt32(dr["dashboard_cnt_created"]),
                                WcnApproved = Convert.ToInt32(dr["dashboard_cnt_approved"]),
                                WcnPayment = Convert.ToInt32(dr["dashboard_cnt_payment"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WCNNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                Revenue = Convert.ToInt32(dr["total_revenue"]),
                                Unbilled = Convert.ToInt32(dr["total_unbilled"]),
                                CreatedBy = Convert.ToString(dr["created_by"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return dashboardList;
        }



        // **************** GET DASHBOARD TOTAL APPROVED COUNT IN DETAILS *********************
        public List<DashboardModel> GetApprovedDetails(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<DashboardModel> dashboardList = new List<DashboardModel>();

            try
            {
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_dashboard_approved_details", user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        dashboardList.Add(
                            new DashboardModel
                            {
                                TotalRevenue = Convert.ToDecimal(dr["dashboard_total_revenue"]),
                                TotalUnbilled = Convert.ToDecimal(dr["dashboard_total_unbilled"]),
                                WcnCreated = Convert.ToInt32(dr["dashboard_cnt_created"]),
                                WcnApproved = Convert.ToInt32(dr["dashboard_cnt_approved"]),
                                WcnPayment = Convert.ToInt32(dr["dashboard_cnt_payment"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WCNNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                Revenue = Convert.ToInt32(dr["total_revenue"]),
                                Unbilled = Convert.ToInt32(dr["total_unbilled"]),
                                CreatedBy = Convert.ToString(dr["created_by"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return dashboardList;
        }



        // **************** GET DASHBOARD TOTAL PAYMENT COUNT IN DETAILS *********************
        public List<DashboardModel> GetPaymentDetails(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<DashboardModel> dashboardList = new List<DashboardModel>();

            try
            {
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_dashboard_payment_details", user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        dashboardList.Add(
                            new DashboardModel
                            {
                                TotalRevenue = Convert.ToDecimal(dr["dashboard_total_revenue"]),
                                TotalUnbilled = Convert.ToDecimal(dr["dashboard_total_unbilled"]),
                                WcnCreated = Convert.ToInt32(dr["dashboard_cnt_created"]),
                                WcnApproved = Convert.ToInt32(dr["dashboard_cnt_approved"]),
                                WcnPayment = Convert.ToInt32(dr["dashboard_cnt_payment"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WCNNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                Revenue = Convert.ToInt32(dr["total_revenue"]),
                                Unbilled = Convert.ToInt32(dr["total_unbilled"]),
                                CreatedBy = Convert.ToString(dr["created_by"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return dashboardList;
        }

    }
}