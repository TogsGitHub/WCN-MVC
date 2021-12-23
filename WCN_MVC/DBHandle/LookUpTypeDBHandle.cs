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
    public class LookUpTypeDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        // **************** ADD NEW LOOKUPTYPE *********************
        public int AddLookUpType(LookUpTypeModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter code = new SqlParameter("@code", cmodel.Code);
                SqlParameter type = new SqlParameter("@type", cmodel.Type);
                SqlParameter userid = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_lookup_type", code, type, userid);

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


        // **************** EDIT LOOKUPTYPE *********************
        public int EditLookUpType(LookUpTypeModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter code = new SqlParameter("@code", cmodel.Code);
                SqlParameter type = new SqlParameter("@type", cmodel.Type);
                SqlParameter userid = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_lookup_type", id, code, type, userid);

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


        // **************** DELETE LOOKUPTYPE *********************
        public async Task<int> DeleteLookUpType(int companyid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", companyid);
                SqlParameter deleteuserid = new SqlParameter("@user_id", userid);

                ds = await nsDataHelper.SqlHelper.ExecuteDatasetAsync(constring, CommandType.StoredProcedure, "delete_lookup_type", id, deleteuserid);

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


        // **************** GET/VIEW LOOKUPTYPE *********************
        public List<LookUpTypeModel> GetLookUpType(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<LookUpTypeModel> lookuptypeList = new List<LookUpTypeModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_lookup_type", search_string);//, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        lookuptypeList.Add(
                            new LookUpTypeModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Code = Convert.ToString(dr["code"]),
                                Type = Convert.ToString(dr["type"]),
                                CreatedBy = Convert.ToString(dr["created_by"]),
                                CreatedDate = Convert.ToDateTime(dr["created_date"]),
                                ModifiedBy = Convert.ToString(dr["modified_by"]),
                                ModifiedDate = Convert.ToDateTime(dr["modified_date"]),
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

            return lookuptypeList;
        }
    }
}