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
    public class LookUpValueDBHandle
    {

        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        // **************** ADD NEW LOOKUPVALUE *********************
        public int AddLookUpValue(LookUpValueModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter code = new SqlParameter("@code", cmodel.Code);
                SqlParameter description = new SqlParameter("@description", cmodel.Description);
                SqlParameter typeid = new SqlParameter("@type_id", cmodel.TypeId);
                SqlParameter sequenceno = new SqlParameter("@sequence_no", cmodel.SequenceNo);
                SqlParameter userid = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_lookup_value", code, description, typeid, sequenceno, userid);

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


        // **************** EDIT LOOKUPVALUE *********************
        public int EditLookUpValue(LookUpValueModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter code = new SqlParameter("@code", cmodel.Code);
                SqlParameter description = new SqlParameter("@description", cmodel.Description);
                SqlParameter typeid = new SqlParameter("@type_id", cmodel.TypeId);
                SqlParameter sequenceno = new SqlParameter("@sequence_no", cmodel.SequenceNo);
                SqlParameter userid = new SqlParameter("@user_id", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_lookup_value", id, code, description, typeid, sequenceno, userid);

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



        // **************** DELETE LOOKUPVALUE *********************
        public async Task<int> DeleteLookUpValue(int lookupvalueid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", lookupvalueid);
                SqlParameter deleteuserid = new SqlParameter("@user_id", userid);

                ds = await nsDataHelper.SqlHelper.ExecuteDatasetAsync(constring, CommandType.StoredProcedure, "delete_lookup_value", id, deleteuserid);

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


        // **************** GET/VIEW LOOKUPVALUE *********************
        public List<LookUpValueModel> GetLookUpValue(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<LookUpValueModel> lookupvalueList = new List<LookUpValueModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_lookup_value", search_string);//, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        lookupvalueList.Add(
                            new LookUpValueModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Code = Convert.ToString(dr["code"]),
                                Description = Convert.ToString(dr["description"]),
                                TypeId = Convert.ToInt16(dr["type_id"]),
                                TypeName = Convert.ToString(dr["type"]),
                                SequenceNo = Convert.ToInt16(dr["sequence_no"]),
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

            return lookupvalueList;
        }



        // **************** LOAD TYPE DROPDOWN *********************
        public List<SelectListItem> GetTypeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_type_list";
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
                                Text = sdr["type"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }

                    con.Close();
                }
            }

            return items;
            //return new SelectList(items, "Value", "Text");
        }

    }
}