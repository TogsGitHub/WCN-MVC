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
    public class PeriodDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();


        // **************** ADD NEW PERIOD *********************
        public int AddPeriod(PeriodModel cmodel)
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



        // **************** EDIT PERIOD *********************
        public int EditPeriod(PeriodModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter month = new SqlParameter("@month", cmodel.Month);
                SqlParameter year = new SqlParameter("@year", cmodel.Year);
                SqlParameter period = new SqlParameter("@period", cmodel.Period);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_period", id, month, year, period, userid);

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



        // **************** DELETE PERIOD *********************
        public int DeletePeriod(int periodid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", periodid);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_period", id, deleteuserid);

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



        // **************** GET/VIEW PERIOD *********************
        public List<PeriodModel> GetPeriod(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<PeriodModel> periodList = new List<PeriodModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_period", search_string); //, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        periodList.Add(
                            new PeriodModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Month = Convert.ToString(dr["month"]),
                                Year = Convert.ToInt16(dr["year"]),
                                Period = Convert.ToString(dr["month_text"]),
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

            return periodList;
        }



    }
}