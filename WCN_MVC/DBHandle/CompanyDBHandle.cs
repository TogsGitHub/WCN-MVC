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
    public class CompanyDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();


        // **************** ADD NEW COMPANY *********************
        public async Task<int> AddCompany(CompanyModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter code = new SqlParameter("@code", cmodel.Code);
                SqlParameter companyname = new SqlParameter("@companyname", cmodel.CompanyName);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = await nsDataHelper.SqlHelper.ExecuteDatasetAsync(constring, CommandType.StoredProcedure, "add_company", code, companyname, userid);

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


        // **************** EDIT COMPANY *********************
        public async Task<int> EditCompany(CompanyModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter code = new SqlParameter("@code", cmodel.Code);
                SqlParameter companyname = new SqlParameter("@companyname", cmodel.CompanyName);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = await nsDataHelper.SqlHelper.ExecuteDatasetAsync(constring, CommandType.StoredProcedure, "edit_company", id, code, companyname, userid);

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


        // **************** DELETE COMPANY *********************
        public async Task<int> DeleteCompany(int companyid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", companyid);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = await nsDataHelper.SqlHelper.ExecuteDatasetAsync(constring, CommandType.StoredProcedure, "delete_company", id, deleteuserid);

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


        // **************** GET/VIEW COMPANY *********************
        public List<CompanyModel> GetCompany(string searchstring, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<CompanyModel> companyList = new List<CompanyModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_company", search_string, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        companyList.Add(
                            new CompanyModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Code = Convert.ToString(dr["code"]),
                                CompanyName = Convert.ToString(dr["company_name"]),
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

            return companyList;
        }
        
    }
}