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
    public class LoginDBHandle
    {
        public static string constring = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        // **************** VALIDATE LOGIN DETAILS *********************
        //public async Task<int> ValidateLogin(LoginModel lmodel)
        public DataTable ValidateLogin(LoginModel lmodel)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //int retVal = 0;

            try
            {
                SqlParameter username = new SqlParameter("@username", lmodel.UserName);
                SqlParameter password = new SqlParameter("@password", lmodel.Password);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "validatelogin", username, password);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                //retVal = 3;
            }
            finally
            {
                ds = null;
            }

            return dt;
        }


        // **************** FORGOT PASSWORD *********************
        public DataTable SendPassword(LoginModel lmodel)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            
            //int retVal = 0;

            try
            {
                SqlParameter username = new SqlParameter("@username", lmodel.UserName);
                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "send_password", username);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //retVal = Convert.ToInt16(ds.Tables[0].Rows[0]["dbVal"]);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                //retVal = 3;
                dt = null;
            }
            finally
            {
                ds = null;
            }

            return dt;
        }


        public List<UserModel> GetUserDetails(int userId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<UserModel> userList = new List<UserModel>();

            try
            {
                SqlParameter user_Id = new SqlParameter("@user_Id", userId);

                ds = nsDataHelper.SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, "get_user_details", user_Id);

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
                                ContactNo = Convert.ToInt32(dr["contact_no"]),
                                EmailID = Convert.ToString(dr["email_id"]),
                                ImageName = Convert.ToString(dr["image_name"]),
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

            return userList;
        }
    }
}