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
    public class TrackingController : Controller
    {
        // GET: Tracking
        [System.Web.Http.HttpPost]
        public ActionResult GetTrackingInfo(string trackingnumber)
        {
            try
            {
                string data = string.Empty;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "select * from dl_tracking where trackingnumber='" + trackingnumber + "'";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                SqlDataReader sqlDataReader = sqlCom.ExecuteReader();
                List<tracking> trackingData = new List<tracking>();
                tracking trckng;
                while (sqlDataReader.Read())
                {
                    trckng = new tracking();
                    trckng.trackingnumber = trackingnumber;
                    trckng.shippmentfrom = sqlDataReader.GetValue(2).ToString();
                    trckng.shippmentto = sqlDataReader.GetValue(3).ToString();
                    trckng.trackingstatus = sqlDataReader.GetValue(4).ToString();
                    trckng.createddate = Convert.ToDateTime(sqlDataReader.GetValue(5));
                    trckng.createdby = sqlDataReader.GetValue(6).ToString();
                    trckng.updateddate = Convert.ToDateTime(sqlDataReader.GetValue(7));
                    trckng.updatedby = sqlDataReader.GetValue(8).ToString();
                    trckng.name = sqlDataReader.GetValue(9).ToString();
                    trckng.email = sqlDataReader.GetValue(10).ToString();
                    trckng.phone = sqlDataReader.GetValue(11).ToString();
                    trckng.weight = sqlDataReader.GetValue(12).ToString();
                    trckng.dimension = sqlDataReader.GetValue(13).ToString();
                    trackingData.Add(trckng);
                }
                return Json(new { status = "200", mail = "Sent", message = "Success", trackingData= JsonConvert.SerializeObject(trackingData) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CreateNewShipment(string trackingnumber, string from, string to, string username, string name, string email, string phone, string weight, string dimension)
        {
            try
            {
                string data = string.Empty;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "insert into dl_tracking values(@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13)";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                sqlCom.Parameters.AddWithValue("@v1", trackingnumber);
                sqlCom.Parameters.AddWithValue("@v2", from);
                sqlCom.Parameters.AddWithValue("@v3", to);
                sqlCom.Parameters.AddWithValue("@v4", "Placed");
                sqlCom.Parameters.AddWithValue("@v5", DateTime.Now);
                sqlCom.Parameters.AddWithValue("@v6", username);
                sqlCom.Parameters.AddWithValue("@v7", DateTime.Now);
                sqlCom.Parameters.AddWithValue("@v8", username);
                sqlCom.Parameters.AddWithValue("@v9", name);
                sqlCom.Parameters.AddWithValue("@v10", email);
                sqlCom.Parameters.AddWithValue("@v11", phone);
                sqlCom.Parameters.AddWithValue("@v12", weight);
                sqlCom.Parameters.AddWithValue("@v13", dimension);
                sqlCom.CommandType = System.Data.CommandType.Text;
                int numberOfRow = sqlCom.ExecuteNonQuery();
                sqlCon.Close();
                return Json(new { status = "200", mail = "Sent", message = "Success", newShipmentAdded=numberOfRow.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult GetShipmentDetails(string trackingnumber)
        {
            try
            {
                Boolean updateStatus = false;
                string data = string.Empty;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "select * from dl_tracking where trackingnumber='" + trackingnumber + "'";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                SqlDataReader sqlDataReader = sqlCom.ExecuteReader();
                List<tracking> trackingData = new List<tracking>();
                tracking trckng;
                while (sqlDataReader.Read())
                {
                    trckng = new tracking();
                    trckng.trackingnumber = trackingnumber;
                    trckng.shippmentfrom = sqlDataReader.GetValue(2).ToString();
                    trckng.shippmentto = sqlDataReader.GetValue(3).ToString();
                    trckng.trackingstatus = sqlDataReader.GetValue(4).ToString();
                    trckng.createddate = Convert.ToDateTime(sqlDataReader.GetValue(5));
                    trckng.createdby = sqlDataReader.GetValue(6).ToString();
                    trckng.updateddate = Convert.ToDateTime(sqlDataReader.GetValue(7));
                    trckng.updatedby = sqlDataReader.GetValue(8).ToString();
                    trckng.name = sqlDataReader.GetValue(9).ToString();
                    trckng.email = sqlDataReader.GetValue(10).ToString();
                    trckng.phone = sqlDataReader.GetValue(11).ToString();
                    trckng.weight = sqlDataReader.GetValue(12).ToString();
                    trckng.dimension = sqlDataReader.GetValue(13).ToString();
                    trackingData.Add(trckng);
                }
                sqlCon.Close();
                return Json(new { status = "200", mail = "Sent", message = "Success", trackingData = JsonConvert.SerializeObject(trackingData) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        public ActionResult UpdateShipment(string trackingnumber, string trackingstatus, string username)
        {
            try
            {
                Boolean updateStatus = false;
                string data = string.Empty;
                string connStr = ConfigurationManager.ConnectionStrings["drishtiLogisticsConfig"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connStr);
                string sqlCommand = "update dl_tracking set trackingstatus=@v1, updateddate=@v2, updatedby=@v3 where trackingnumber='" + trackingnumber + "'";
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand(sqlCommand, sqlCon);
                sqlCom.Parameters.AddWithValue("@v1", trackingstatus);
                sqlCom.Parameters.AddWithValue("@v2", DateTime.Now);
                sqlCom.Parameters.AddWithValue("@v3", username);
                sqlCom.CommandType = System.Data.CommandType.Text;
                int numberOfRow = sqlCom.ExecuteNonQuery();
                if (numberOfRow == 1)
                {
                    updateStatus = true;
                }
                else {
                    updateStatus = false;   
                }
                sqlCon.Close();
                return Json(new { status = "200", mail = "Sent", message = "Success", updateStatus = updateStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "000", mail = "Fail", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }


        public class tracking {
            public string trackingnumber;
            public string shippmentfrom;
            public string shippmentto;
            public string trackingstatus;
            public DateTime createddate;
            public string createdby;
            public DateTime updateddate;
            public string updatedby;
            public string name;
            public string email;
            public string phone;
            public string weight;
            public string dimension;
        }
    }
}