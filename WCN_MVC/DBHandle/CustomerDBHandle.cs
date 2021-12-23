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
    public class CustomerDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();


        // **************** ADD NEW CUSTOMER *********************
        public int AddCustomer(CustomerModel cmodel)
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


        // **************** EDIT CUSTOMER *********************
        public int EditCustomer(CustomerModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter customer_name = new SqlParameter("@customer_name", cmodel.CustomerName);
                SqlParameter contact_no = new SqlParameter("@contact_no", cmodel.ContactNo);
                SqlParameter contact_person = new SqlParameter("@contact_person", cmodel.ContactPerson);
                SqlParameter address = new SqlParameter("@address", cmodel.Address);
                SqlParameter email_id = new SqlParameter("@email_id", cmodel.Emailid);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_customer", id, customer_name, contact_no, contact_person, address, email_id, userid);

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


        // **************** DELETE CUSTOMER *********************
        public int DeleteCustomer(int customerid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", customerid);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_customer", id, deleteuserid);

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


        // **************** GET/VIEW CUSTOMER *********************
        public List<CustomerModel> GetCustomer(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<CustomerModel> customerList = new List<CustomerModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_customer", search_string); //, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        customerList.Add(
                            new CustomerModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                CustomerName = Convert.ToString(dr["customer_name"]),
                                ContactNo = Convert.ToInt32(dr["contact_no"]),
                                ContactPerson = Convert.ToString(dr["contact_person"]),
                                Address = Convert.ToString(dr["address"]),
                                Emailid = Convert.ToString(dr["email_id"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]),
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

            return customerList;
        }

    }
}