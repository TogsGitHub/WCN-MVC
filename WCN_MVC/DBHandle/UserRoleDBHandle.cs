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
    public class UserRoleDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        // **************** ADD NEW USER ROLE *********************
        public int AddUserRole(UserRoleModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter user_role = new SqlParameter("@user_role", cmodel.UserRole);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_user_role", user_role, userid);

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



        // **************** EDIT USER ROLE *********************
        public int EditUserRole(UserRoleModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter user_role = new SqlParameter("@user_role", cmodel.UserRole);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_user_role", id, user_role, userid);

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


        // **************** DELETE USER ROLE *********************
        public int DeleteUserRole(int userroleid, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", userroleid);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_user_role", id, deleteuserid);

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


        // **************** GET/VIEW USER ROLE *********************
        public List<UserRoleModel> GetUserRole(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<UserRoleModel> userroleList = new List<UserRoleModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_user_role", search_string); //, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        userroleList.Add(
                            new UserRoleModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                UserRole = Convert.ToString(dr["user_role"]),
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

            return userroleList;
        }

    }
}