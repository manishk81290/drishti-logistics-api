using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace drishtilogistics_api.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        [HttpPost]
        public ActionResult LoginUser(string username, string passwordd)
        {
            try
            {
                string data = string.Empty;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "select * from dl_users where username='" + username + "' and passwordd='" + passwordd + "'";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                SqlDataReader sqlDataReader = sqlCom.ExecuteReader();
                List<user> userData = new List<user>();
                user usr;
                Guid guidcode = Guid.NewGuid();
                int numberOfRow=0;
                while (sqlDataReader.Read())
                {
                    usr = new user();
                    usr.username = sqlDataReader.GetValue(1).ToString();
                    //usr.passwordd = sqlDataReader.GetValue(2).ToString();
                    usr.userrole = sqlDataReader.GetValue(3).ToString();
                    usr.guidcode = guidcode.ToString();
                    userData.Add(usr);
                }
                sqlDataReader.Close();
                if (userData.Count == 1) {
                    sqlCommand = "insert into dl_userlogin(username,code,logintime) values(@v1,@v2,@v3)";
                    SqlCommand sqlComInsert = new SqlCommand(sqlCommand, sqlCon);
                    sqlComInsert.Parameters.AddWithValue("@v1", username);
                    sqlComInsert.Parameters.AddWithValue("@v2", guidcode.ToString());
                    sqlComInsert.Parameters.AddWithValue("@v3", DateTime.Now);
                    sqlComInsert.CommandType = System.Data.CommandType.Text;
                    numberOfRow = sqlComInsert.ExecuteNonQuery();
                }
                sqlCon.Close();
                return Json(new { status = "200", mail = "Sent", message = "Success", userData = JsonConvert.SerializeObject(userData), loggedRows=numberOfRow.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult ValidateUser(string username, string code)
        {
            try
            {
                Boolean userValid = false;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "select * from dl_userlogin where username='" + username + "' and code='" + code + "' and logouttime IS NULL";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                SqlDataReader sqlDataReader = sqlCom.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    userValid = true;
                }
                else {
                    userValid = false;
                }
                return Json(new { status = "200", mail = "Sent", message = "Success", isUserValid = userValid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Logout(string username, string code)
        {
            try
            {
                Boolean loggedOut = false;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "update dl_userlogin set logouttime=@v1 where username='" + username + "' and code='" + code + "' and logouttime IS NULL";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                sqlCom.Parameters.AddWithValue("@v1", DateTime.Now);
                sqlCom.CommandType = System.Data.CommandType.Text;
                int numberOfRow = sqlCom.ExecuteNonQuery();
                if (numberOfRow==1)
                {
                    loggedOut = true;
                }
                else
                {
                    loggedOut = false;
                }
                return Json(new { status = "200", mail = "Sent", message = "Success", loggedOut = loggedOut }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        public class user {
            public string username;
            public string passwordd;
            public string userrole;
            public DateTime createddate;
            public string createdby;
            public DateTime updateddate;
            public string updatedby;
            public string guidcode;
        }
    }
}