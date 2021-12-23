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

namespace WCN_MVC.DBHandle
{
    public class CompanyUserMappingDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        // **************** ADD NEW COMPANY USER MAPPING *********************
        public int AddCompanyUserMapping(CompanyUserMappingModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter company_id = new SqlParameter("@company_id", cmodel.CompanyMasterId);
                SqlParameter user_id = new SqlParameter("@user_id", cmodel.UserMasterId);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_company_user_mapping", company_id, user_id, userid);

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



        // **************** EDIT COMPANY USER MAPPING *********************
        public int EditCompanyUserMapping(CompanyUserMappingModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter company_id = new SqlParameter("@company_id", cmodel.CompanyMasterId);
                SqlParameter user_id = new SqlParameter("@user_id", cmodel.UserMasterId);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_company_user_mapping", id, company_id, user_id, userid);

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



        // **************** DELETE COMPANY USER MAPPING *********************
        public int DeleteCompanyUserMapping(int mappingid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", mappingid);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_company_user_mapping", id, deleteuserid);

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



        // **************** GET/VIEW COMPANY USER MAPPING *********************
        public List<CompanyUserMappingModel> GetCompanyUserMapping(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<CompanyUserMappingModel> mappingList = new List<CompanyUserMappingModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_company_user_mapping", search_string); //, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        mappingList.Add(
                            new CompanyUserMappingModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                CompanyMasterId = Convert.ToInt16(dr["company_id"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
                                UserMasterId = Convert.ToInt16(dr["user_id"]),
                                UserName = Convert.ToString(dr["user_name"]),
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

            return mappingList;
        }



        // **************** LOAD COMPANY DROPDOWN *********************
        public List<SelectListItem> GetCompanyList(string typeName)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                //string query = "get_company_list";
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


        // **************** LOAD USER DROPDOWN *********************
        public List<SelectListItem> GetUserList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_user_list";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UserId;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["user_name"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
        }
    }
}