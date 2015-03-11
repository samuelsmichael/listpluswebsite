using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for Email
/// This is the class used to send the Email alert to a given/ selected registered user
/// Use two methods & smmtp server is smtp.gmail.com 
/// Can use in any application for any purpose 
/// Sends with our Authentication
/// Completely secured. Use SSL V3 & Https & a Encoded message
/// </summary>
public class EmailManager2 {
    public EmailManager2() {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// This is the primery method try from the program
    /// Latest & easiest method of sending an Email using ASP.NET
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    public static void sendMail(string to, string from, string subject, string body) {
        ///Smtp config
        SmtpClient client = new SmtpClient("mail.employersunity.com", 465);
        // Edit password and username
        client.Credentials = new NetworkCredential("uic@employersunity.com", "uiclaim123");
        client.EnableSsl = true;

        ///mail details
        MailMessage msg = new MailMessage();


        try {

            msg.From = new MailAddress(from);
            msg.To.Add(to);
            // msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Subject = subject;
            //msg.CC.Add();
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.Body = body;
            msg.Priority = MailPriority.Normal;

            // Enable one of the following method.

            client.Send(msg);

            // or use the following alternative after enabling send mail asynchronous option in the global.asax 

            //object userState = msg;
            //client.SendAsync(msg, userState);



        } catch (Exception exp) {
            ///This runs the backup plan
            SendMailAlt(to, from, subject, body);
        }

    }

    /// <summary>
    /// This is the Back up plan for the above method
    /// Used Few years back with ASP 
    /// Now obsalete method & substituted by System.Net.Mail namespace
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    private static void SendMailAlt(string to, string from, string subject, string body) {



        System.Web.Mail.MailMessage Mail = new System.Web.Mail.MailMessage();
        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = ("mail.employersunity.com");
        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;

        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = "465";

        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "true";


        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
        // Edit username & password
        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = "uic@employersunity.com";
        Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = "uiclaim123";

        Mail.To = to;
        Mail.From = from;
        Mail.Subject = subject;
        Mail.Body = body;
        Mail.BodyFormat = System.Web.Mail.MailFormat.Html;

        System.Web.Mail.SmtpMail.SmtpServer = "mail.employersunity.com";
        System.Web.Mail.SmtpMail.Send(Mail);
    }
}
