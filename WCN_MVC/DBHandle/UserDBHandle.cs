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
    public class UserDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();


        // **************** ADD NEW USER *********************
        public int AddUser(UserModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter name = new SqlParameter("@name", cmodel.Name);
                SqlParameter user_name = new SqlParameter("@user_name", cmodel.UserName);
                SqlParameter password = new SqlParameter("@password", cmodel.Password);
                SqlParameter contact_no = new SqlParameter("@contact_no", cmodel.ContactNo);
                SqlParameter user_role_id = new SqlParameter("@user_role_id", cmodel.UserRoleID);
                SqlParameter email_id = new SqlParameter("@email_id", cmodel.EmailID);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserID);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "add_user", name, user_name, password, contact_no, user_role_id, email_id, userid);

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



        // **************** EDIT USER *********************
        public int EditUser(UserModel cmodel)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", cmodel.Id);
                SqlParameter name = new SqlParameter("@name", cmodel.Name);
                SqlParameter user_name = new SqlParameter("@user_name", cmodel.UserName);
                SqlParameter password = new SqlParameter("@password", cmodel.Password);
                SqlParameter contact_no = new SqlParameter("@contact_no", cmodel.ContactNo);
                SqlParameter user_role_id = new SqlParameter("@user_role_id", cmodel.UserRoleID);
                SqlParameter email_id = new SqlParameter("@email_id", cmodel.EmailID);
                SqlParameter userid = new SqlParameter("@userid", cmodel.UserID);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "edit_user", id, name, user_name, password, contact_no, user_role_id, email_id, userid);

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



        // **************** DELETE USER *********************
        public int DeleteUser(int user_id, int userid)
        {
            DataSet ds = new DataSet();
            int retVal = 0;

            try
            {
                SqlParameter id = new SqlParameter("@id", user_id);
                SqlParameter deleteuserid = new SqlParameter("@userid", userid);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "delete_user", id, deleteuserid);

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




        // **************** GET/VIEW USER *********************
        public List<UserModel> GetUser(string searchstring)//, string sortingFleid, string sortOrder)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<UserModel> userList = new List<UserModel>();

            try
            {
                SqlParameter search_string = new SqlParameter("@search_string", searchstring);
                //SqlParameter sorting_Fleid = new SqlParameter("@sorting_Fleid", sortingFleid);
                //SqlParameter sort_Order = new SqlParameter("@sort_Order", sortOrder);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_user", search_string); //, sorting_Fleid, sort_Order);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        userList.Add(
                            new UserModel
                            {
                                Id = Convert.ToInt16(dr["id"]),
                                Name = Convert.ToString(dr["name"]),
                                UserName = Convert.ToString(dr["user_name"]),
                                Password = Convert.ToString(dr["password"]),
                                UserRoleID = Convert.ToInt16(dr["user_role_id"]),
                                UserRole = Convert.ToString(dr["user_role"]),
                                ImageName = Convert.ToString(dr["image_name"]),
                                ContactNo = Convert.ToInt32(dr["contact_no"]),
                                EmailID = Convert.ToString(dr["email_id"]),
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

            return userList;
        }



        // **************** LOAD USER ROLE DROPDOWN *********************
        public List<SelectListItem> GetUserRoleList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "get_user_role_list";
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
                                Text = sdr["user_role"].ToString(),
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