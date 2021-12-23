using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using WCN_MVC.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Runtime.Remoting.Contexts;
using Newtonsoft.Json;
//using DocumentFormat.OpenXml.InkML;

namespace WCN_MVC.DBHandle
{
    public class WCNDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        // **************** GET/VIEW WCN *********************
        public List<WCNModel> GetWCNTracker(string searchstring)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                //ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_company", search_string, sorting_Fleid, sort_Order);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_wcn_tracker", search_string);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        wcnList.Add(
                            new WCNModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Status = Convert.ToString(dr["status"]),
                                CompanyId = Convert.ToInt16(dr["company_id"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WcnNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                StartDate = Convert.ToDateTime(dr["start_date"]),
                                CompletedDate = Convert.ToDateTime(dr["completed_date"]),
                                PeriodId = Convert.ToInt16(dr["period_id"]),
                                Period = Convert.ToString(dr["month_text"]),
                                DivisionId = Convert.ToInt16(dr["division_id"]),
                                DivisionName = Convert.ToString(dr["division_name"]),
                                CustomerId = Convert.ToInt16(dr["customer_id"]),
                                CustomerName = Convert.ToString(dr["customer_name"]),
                                LocationId = Convert.ToInt16(dr["location_id"]),
                                LocationName = Convert.ToString(dr["location_name"]),
                                Description = Convert.ToString(dr["description"]),
                                RigNo = Convert.ToInt16(dr["rig_no"]),
                                OtherLocation = Convert.ToString(dr["other_location"]),
                                IsDiscount = Convert.ToBoolean(dr["is_discount"]),
                                DiscountAmount = Convert.ToDecimal(dr["discount_amount"]),
                                DiscountRemark = Convert.ToString(dr["discount_remark"]),
                                IsWCNSent = Convert.ToBoolean(dr["is_wcn_sent"]),
                                IsWCNSigned = Convert.ToBoolean(dr["is_wcn_signed"]),
                                IsClientSigned = Convert.ToBoolean(dr["is_client_sign"]),
                                TotalRevenue = Convert.ToDecimal(dr["total_revenue"]),
                                TotalBillable = Convert.ToDecimal(dr["total_billable"]),
                                InvoiceNumber = Convert.ToString(dr["invoice_no"]),
                                InvoicedAmount = Convert.ToDecimal(dr["invoiced_amount"]),
                                BillingStatus = Convert.ToString(dr["billing_status"]),
                                StakeHolder = Convert.ToString(dr["stake_holder"]),

                                Revenue = Convert.ToDecimal(dr["Revenue"]),
                                TwilightCharges = Convert.ToDecimal(dr["TwilightCharges"]),
                                ROPCharges = Convert.ToDecimal(dr["ROPCharges"]),
                                ROPAdminCharges = Convert.ToDecimal(dr["ROPAdminCharges"]),
                                Mobilisation = Convert.ToDecimal(dr["Mobilisation"]),
                                Demobilisation = Convert.ToDecimal(dr["Demobilisation"]),
                                AdditionalLoad = Convert.ToDecimal(dr["AdditionalLoad"]),
                                AdditionalAsset = Convert.ToDecimal(dr["AdditionalAsset"]),
                                StandbyCharges = Convert.ToDecimal(dr["StandbyCharges"]),
                                OtherCharges = Convert.ToDecimal(dr["OtherCharges"]),
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

            return wcnList;
        }



        // **************** ADD NEW WCN TRACKER *********************
        public DataTable AddWCNTracker(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter company_id = new SqlParameter("@company_id", cmodel.CompanyId);
                SqlParameter status = new SqlParameter("@Status", cmodel.Status);
                SqlParameter period_id = new SqlParameter("@period_id", cmodel.PeriodId);
                SqlParameter wcn_date = new SqlParameter("@wcn_date", cmodel.WCNDate);
                SqlParameter start_date = new SqlParameter("@start_date", cmodel.StartDate);
                SqlParameter complete_date = new SqlParameter("@complete_date", cmodel.CompletedDate);
                SqlParameter division_id = new SqlParameter("@division_id", cmodel.DivisionId);
                SqlParameter customer_id = new SqlParameter("@customer_id", cmodel.CustomerId);
                SqlParameter location_id = new SqlParameter("@location_id", cmodel.LocationId);
                SqlParameter description = new SqlParameter("@description", cmodel.Description);
                SqlParameter rig_no = new SqlParameter("@rig_no", cmodel.RigNo);
                SqlParameter other_location = new SqlParameter("@other_location", cmodel.OtherLocation);
                SqlParameter is_wcn_sent = new SqlParameter("@is_wcn_sent", cmodel.IsWCNSent);
                SqlParameter is_wcn_signed = new SqlParameter("@is_wcn_signed", cmodel.IsWCNSigned);
                SqlParameter is_client_signed = new SqlParameter("@is_client_signed", cmodel.IsClientSigned);
                SqlParameter total_billable = new SqlParameter("@total_billable", cmodel.TotalBillable);
                SqlParameter user_id = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_wcn_tracker",
                company_id, status, period_id, wcn_date, start_date, complete_date, division_id, customer_id, location_id,
                description, rig_no, other_location, is_wcn_sent, is_wcn_signed, is_client_signed, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return dt;
        }



        // **************** EDIT WCN TRACKER *********************
        public int EditWCNTracker(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter company_id = new SqlParameter("@company_id", cmodel.CompanyId);
                SqlParameter status = new SqlParameter("@Status", cmodel.Status);
                SqlParameter period_id = new SqlParameter("@period_id", cmodel.PeriodId);
                SqlParameter wcn_date = new SqlParameter("@wcn_date", cmodel.WCNDate);
                SqlParameter start_date = new SqlParameter("@start_date", cmodel.StartDate);
                SqlParameter complete_date = new SqlParameter("@complete_date", cmodel.CompletedDate);
                SqlParameter division_id = new SqlParameter("@division_id", cmodel.DivisionId);
                SqlParameter customer_id = new SqlParameter("@customer_id", cmodel.CustomerId);
                SqlParameter location_id = new SqlParameter("@location_id", cmodel.LocationId);
                SqlParameter description = new SqlParameter("@description", cmodel.Description);
                SqlParameter rig_no = new SqlParameter("@rig_no", cmodel.RigNo);
                SqlParameter other_location = new SqlParameter("@other_location", cmodel.OtherLocation);
                SqlParameter is_wcn_sent = new SqlParameter("@is_wcn_sent", cmodel.IsWCNSent);
                SqlParameter is_wcn_signed = new SqlParameter("@is_wcn_signed", cmodel.IsWCNSigned);
                SqlParameter is_client_signed = new SqlParameter("@is_client_signed", cmodel.IsClientSigned);
                SqlParameter user_id = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_wcn_tracker", id,
                company_id, status, period_id, wcn_date, start_date, complete_date, division_id, customer_id, location_id,
                description, rig_no, other_location, is_wcn_sent, is_wcn_signed, is_client_signed, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        // **************** DELETE WCN TRACKER *********************
        public int DeleteWCNTracker(int wcnid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", wcnid);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_wcn_tracker", id, deleteuserid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }




        // **************** GET WCN LINE ITEMS *********************
        public DataSet GetWCNLineItems(string wcnNo)
        {
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //List<WCNLineItems> wcnItems = new List<WCNLineItems>();

            try
            {
                SqlParameter wcn_no = new SqlParameter("@wcn_no", wcnNo);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_wcn_line_item", wcn_no);

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    dt = ds.Tables[0];

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        wcnItems.Add(
                //            new WCNLineItems
                //            {
                //                ResourceType = Convert.ToString(dr["resource_type"]),
                //                Resource = Convert.ToString(dr["resource"]),
                //                StartDate = Convert.ToDateTime(dr["start_date"]),
                //                EndDate = Convert.ToDateTime(dr["end_date"]),
                //                Days = Convert.ToInt16(dr["no_of_days"]),
                //                Qty = Convert.ToInt16(dr["qty"]),
                //                Unit = Convert.ToInt16(dr["unit"]),
                //                Rate = Convert.ToInt16(dr["rate"]),
                //                Total = Convert.ToInt16(dr["total"]),
                //            });
                //    }
                //}
            }
            catch (Exception ex)
            { }
            finally
            {
                //ds = null;
                //dt = null;
            }

            return ds;
        }


        // **************** DELETE WCN LINE ITEMS *********************
        public int DeleteWCNLineItems(string wcnNo)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter wcnno = new SqlParameter("@wcn_no", wcnNo);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_wcn_line_item", wcnno);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        // **************** ADD NEW WCN LINE ITEMS *********************
        public int AddWCNLineItems(WCNLineItems cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter wcn_no = new SqlParameter("@wcn_no", cmodel.WcnNo);
                SqlParameter resource_type = new SqlParameter("@resource_type", cmodel.ResourceType);
                SqlParameter resource = new SqlParameter("@resource", cmodel.Resource);
                SqlParameter start_date = new SqlParameter("@start_date", cmodel.StartDate);
                SqlParameter end_date = new SqlParameter("@end_date", cmodel.EndDate);
                SqlParameter days = new SqlParameter("@days", cmodel.Days);
                SqlParameter qty = new SqlParameter("@qty", cmodel.Qty);
                SqlParameter unit = new SqlParameter("@unit", cmodel.Unit);
                SqlParameter rate = new SqlParameter("@rate", cmodel.Rate);
                SqlParameter total = new SqlParameter("@total", cmodel.Total);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_wcn_line_item",
                wcn_no, resource_type, resource, start_date, end_date, days, qty, unit, rate, total);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        // **************** LOAD USER COMPANY MAPPING DROPDOWN *********************
        public List<SelectListItem> GetUserCompanyList(int UserId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_user_company_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UserId;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["company_name"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }



        // **************** LOAD DIVISION DROPDOWN *********************
        public List<SelectListItem> GetDivisionList(string typeName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                //string query = "get_division_list";
                string query = "get_lookup_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@type_name", SqlDbType.NVarChar).Value = typeName;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["description"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }


        // **************** LOAD PERIOD DROPDOWN *********************
        public List<SelectListItem> GetPeriodList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_period_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["month_text"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }



        // **************** LOAD LOCATION DROPDOWN *********************
        public List<SelectListItem> GetLocationList(string typeName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                //string query = "get_location_list";
                string query = "get_lookup_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@type_name", SqlDbType.NVarChar).Value = typeName;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["description"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }



        // **************** LOAD CUSTOMER DROPDOWN *********************
        public List<SelectListItem> GetCustomerList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_customer_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["customer_name"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }



        // **************** LOAD RESOURCE TYPES DROPDOWN *********************
        public List<SelectListItem> GetResourceTypeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_resource_type_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["resource_type"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }


        // **************** LOAD UNIT DROPDOWN *********************
        public List<SelectListItem> GetUnitList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_unit_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["unit_name"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }



        // **************** ADD NEW DIVISION *********************
        public int AddDivision(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter code = new SqlParameter("@code", cmodel.DivisionCode);
                SqlParameter divisionname = new SqlParameter("@divisionname", cmodel.DivisionName);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_division", code, divisionname, userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        // **************** ADD NEW LOCATION *********************
        public int AddLocation(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter locationname = new SqlParameter("@locationname", cmodel.LocationName);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_location", locationname, userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        // **************** ADD NEW CUSTOMER *********************
        public int AddCustomer(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter customer_name = new SqlParameter("@customer_name", cmodel.CustomerName);
                SqlParameter contact_no = new SqlParameter("@contact_no", cmodel.ContactNo);
                SqlParameter contact_person = new SqlParameter("@contact_person", cmodel.ContactPerson);
                SqlParameter address = new SqlParameter("@address", cmodel.Address);
                SqlParameter email_id = new SqlParameter("@email_id", cmodel.Emailid);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_customer", customer_name, contact_no, contact_person, address, email_id, userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        // **************** ADD NEW PERIOD *********************
        public int AddPeriod(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter month = new SqlParameter("@month", cmodel.Month);
                SqlParameter year = new SqlParameter("@year", cmodel.Year);
                SqlParameter period = new SqlParameter("@period", cmodel.Period);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_period", month, year, period, userid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }


        //****************************** WCN APPROVAL ****************************************************

        // **************** GET WCN TRACKER *********************
        public List<WCNModel> GetWCNApproval(string searchstring, string wcnStatus)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                SqlParameter status = new SqlParameter("@Status", wcnStatus);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                //ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_company", search_string, sorting_Fleid, sort_Order);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_wcn_approval", search_string, status);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        wcnList.Add(
                            new WCNModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Status = Convert.ToString(dr["status"]),
                                CompanyId = Convert.ToInt16(dr["company_id"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                WcnNo = Convert.ToString(dr["wcn_no"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                StartDate = Convert.ToDateTime(dr["start_date"]),
                                CompletedDate = Convert.ToDateTime(dr["completed_date"]),
                                PeriodId = Convert.ToInt16(dr["period_id"]),
                                Period = Convert.ToString(dr["month_text"]),
                                DivisionId = Convert.ToInt16(dr["division_id"]),
                                DivisionName = Convert.ToString(dr["division_name"]),
                                CustomerId = Convert.ToInt16(dr["customer_id"]),
                                CustomerName = Convert.ToString(dr["customer_name"]),
                                LocationId = Convert.ToInt16(dr["location_id"]),
                                LocationName = Convert.ToString(dr["location_name"]),
                                Description = Convert.ToString(dr["description"]),
                                RigNo = Convert.ToInt16(dr["rig_no"]),
                                OtherLocation = Convert.ToString(dr["other_location"]),
                                IsDiscount = Convert.ToBoolean(dr["is_discount"]),
                                DiscountAmount = Convert.ToDecimal(dr["discount_amount"]),
                                DiscountRemark = Convert.ToString(dr["discount_remark"]),
                                IsWCNSent = Convert.ToBoolean(dr["is_wcn_sent"]),
                                IsWCNSigned = Convert.ToBoolean(dr["is_wcn_signed"]),
                                IsClientSigned = Convert.ToBoolean(dr["is_client_sign"])
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

            return wcnList;
        }



        // **************** EDIT WCN TRACKER *********************
        public int EditWCNApproval(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter company_id = new SqlParameter("@company_id", cmodel.CompanyId);
                SqlParameter status = new SqlParameter("@Status", cmodel.Status);
                SqlParameter period_id = new SqlParameter("@period_id", cmodel.PeriodId);
                SqlParameter wcn_date = new SqlParameter("@wcn_date", cmodel.WCNDate);
                SqlParameter start_date = new SqlParameter("@start_date", cmodel.StartDate);
                SqlParameter complete_date = new SqlParameter("@complete_date", cmodel.CompletedDate);
                SqlParameter division_id = new SqlParameter("@division_id", cmodel.DivisionId);
                SqlParameter customer_id = new SqlParameter("@customer_id", cmodel.CustomerId);
                SqlParameter location_id = new SqlParameter("@location_id", cmodel.LocationId);
                SqlParameter description = new SqlParameter("@description", cmodel.Description);
                SqlParameter rig_no = new SqlParameter("@rig_no", cmodel.RigNo);
                SqlParameter other_location = new SqlParameter("@other_location", cmodel.OtherLocation);
                SqlParameter is_wcn_sent = new SqlParameter("@is_wcn_sent", cmodel.IsWCNSent);
                SqlParameter is_wcn_signed = new SqlParameter("@is_wcn_signed", cmodel.IsWCNSigned);
                SqlParameter is_client_signed = new SqlParameter("@is_client_signed", cmodel.IsClientSigned);
                SqlParameter is_discount = new SqlParameter("@is_discount", cmodel.IsDiscount);
                SqlParameter discount_amount = new SqlParameter("@discount_amount", cmodel.DiscountAmount);
                SqlParameter discount_remark = new SqlParameter("@discount_remark", cmodel.DiscountRemark);
                SqlParameter user_id = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_wcn_approval", id,
                company_id, status, period_id, wcn_date, start_date, complete_date, division_id, customer_id, location_id,
                description, rig_no, other_location, is_wcn_sent, is_wcn_signed, is_client_signed, is_discount,
                discount_amount, discount_remark, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }


        //****************************** WCN PAYMENT ****************************************************
        public int EditWCNPayment(WCNModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter billing_status = new SqlParameter("@billing_status", cmodel.BillingStatus);
                SqlParameter stake_holder = new SqlParameter("@stake_holder", cmodel.StakeHolder);
                SqlParameter invoice_no = new SqlParameter("@invoice_no", cmodel.InvoiceNumber);
                SqlParameter invoiced_amount = new SqlParameter("@invoiced_amount", cmodel.InvoicedAmount);
                SqlParameter user_id = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_wcn_payment", id,
                billing_status, stake_holder, invoice_no, invoiced_amount, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        public List<WCNModel> GetWCNPayment(string searchstring)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter status = new SqlParameter("@Status", wcnStatus);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                //ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_company", search_string, sorting_Fleid, sort_Order);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_wcn_payment", search_string);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        wcnList.Add(
                            new WCNModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                WcnNo = Convert.ToString(dr["wcn_no"]),
                                Status = Convert.ToString(dr["status"]),
                                BillingStatus = Convert.ToString(dr["billing_status"]),
                                StakeHolder = Convert.ToString(dr["stake_holder"]),
                                InvoiceNumber = Convert.ToString(dr["invoice_no"]),
                                InvoicedAmount = Convert.ToDecimal(dr["invoiced_amount"]),
                                PeriodId = Convert.ToInt16(dr["period_id"]),
                                Period = Convert.ToString(dr["month_text"]),
                                CustomerId = Convert.ToInt16(dr["customer_id"]),
                                CustomerName = Convert.ToString(dr["customer_name"]),
                                TotalRevenue = Convert.ToDecimal(dr["total_revenue"]),
                                TotalBillable = Convert.ToDecimal(dr["total_billable"]),
                                DivisionName = Convert.ToString(dr["division_name"]),
                                StartDate = Convert.ToDateTime(dr["start_date"]),
                                LocationName = Convert.ToString(dr["location_name"]),
                                Description = Convert.ToString(dr["description"])
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

            return wcnList;
        }



        public DataTable GenerateWCN(int wcnId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlParameter wcn_id = new SqlParameter("@wcn_id", wcnId);

            ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "generate_wcn", wcn_id);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }



        public DataTable GenerateWCNLineItems(int wcnId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            SqlParameter wcn_id = new SqlParameter("@wcn_id", wcnId);

            ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "generate_wcn_line_item", wcn_id);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }



        //****************************** GENERATE REVENUE REPORT *************************************
        public DataTable RevenueReport(DateTime fromDate, DateTime toDate, int companyId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                SqlParameter from_date = new SqlParameter("@from_date", fromDate);
                SqlParameter to_date = new SqlParameter("@to_date", toDate);
                SqlParameter company_id = new SqlParameter("@company_id", companyId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "revenue_report",
                from_date, to_date, company_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds = null;
            }

            return dt;
        }



        //****************************** GENERATE UNBILLED REPORT *************************************
        public DataTable UnbilledReport(int companyID)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                SqlParameter Company_id = new SqlParameter("@company_id", companyID);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "unbilled_report", Company_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds = null;
            }

            return dt;
        }


        //****************************** CONFIRM WCN *************************************
        public int ConfirmWCN(int id, int userId)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter wcn_id = new SqlParameter("@wcn_id", id);
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "confirm_wcn", wcn_id, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }



        //****************************** COMPLETE WCN *************************************
        public int CompleteWCN(int id, DateTime CompleteDate, int userId)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter wcn_id = new SqlParameter("@wcn_id", id);
                SqlParameter wcn_complete_date = new SqlParameter("@wcn_complete_date", CompleteDate);
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "complete_wcn", wcn_id, wcn_complete_date, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }


        public List<WCNModel> GetWCNDetails(int wcnId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<WCNModel> wcnList = new List<WCNModel>();

            try
            {
                SqlParameter wcn_id = new SqlParameter("@wcn_id", wcnId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_wcn_Details", wcn_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        wcnList.Add(
                            new WCNModel
                            {
                                WcnNo = Convert.ToString(dr["wcn_no"]),
                                Status = Convert.ToString(dr["status"]),
                                WCNDate = Convert.ToDateTime(dr["wcn_date"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                DivisionName = Convert.ToString(dr["division_name"]),
                                CustomerName = Convert.ToString(dr["customer_name"]),
                                LocationName = Convert.ToString(dr["location_name"]),
                                Period = Convert.ToString(dr["month_text"]),
                                CompletedDate = Convert.ToDateTime(dr["completed_date"]),
                                Description = Convert.ToString(dr["description"]),
                                RigNo = Convert.ToInt16(dr["rig_no"]),
                                TotalRevenue = Convert.ToDecimal(dr["total_revenue"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                ds = null;
            }

            return wcnList;
        }


        public List<WCNLineItems> GetDataWCNLineItems(int id)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<WCNLineItems> wcnItems = new List<WCNLineItems>();

            try
            {
                SqlParameter wcn_id = new SqlParameter("@id", id);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_data_wcn_lines", wcn_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        wcnItems.Add(
                            new WCNLineItems
                            {
                                WcnNo = Convert.ToString(dr["wcn_no"]),
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
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

            return wcnItems;
        }


        public int ApproveWCN(int id, int userId)
        {
            DataSet ds = new DataSet();
            int retVal = 0;
            DataTable dt = new DataTable();

            try
            {
                SqlParameter wcn_id = new SqlParameter("@wcn_id", id);
                SqlParameter user_id = new SqlParameter("@user_id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "approve_wcn", wcn_id, user_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                }
            }
            catch (Exception ex)
            {
                retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return retVal;
        }
    }
}