using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace drishtilogistics_api.Controllers
{
    public class CommunicationController : Controller
    {

        [System.Web.Http.HttpGet]
        public ActionResult SendEmail(string name, string email, string phone, string weight, string pickup, string drop, string dimension)
        {
            try
            {
                mailtoClient(name, email);
                mailtoSelf(name, email, phone, weight, pickup, drop, dimension);
                return Json(new { status = "200", mail = "Sent", message="Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "000", mail = "Fail", message=ex.ToString() }, JsonRequestBehavior.AllowGet); 
            }
           
        }

        public void composeEmail(string toEmail, string subject, string body) {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("info@drishtilogistics.com");
                mailMessage.To.Add(new MailAddress(toEmail));

                mailMessage.Subject = subject;

                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("info@drishtilogistics.com", "adanupdas2");
                client.Host = "relay-hosting.secureserver.net";
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void mailtoClient(string name, string email) {
            try
            {
                string subject = "Thanks for the request";
                string body = string.Empty;
                StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/emailtoClient.html"));
                body = reader.ReadToEnd();
                body = body.Replace("{name}", name);
                body = body.Replace("{content}", "Thank you for submitting your request our team will get back to you shortly.");
                composeEmail(email, subject, body);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public void mailtoSelf(string name, string email, string phone, string weight, string pickup, string drop, string dimension) {
            try
            {
                string subject = "New query for Free Quote";
                string body = string.Empty;
                StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/emailtoSelf.html"));
                body = reader.ReadToEnd();
                body = body.Replace("{name}", name);
                body = body.Replace("{email}", email);
                body = body.Replace("{phone}", phone);
                body = body.Replace("{weight}", weight);
                body = body.Replace("{pickup}", pickup);
                body = body.Replace("{drop}", drop);
                body = body.Replace("{dimension}", dimension);
                composeEmail("info@drishtilogistics.com", subject, body);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}