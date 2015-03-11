using System;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Net;

namespace DDCommon {
    public delegate void LastChanceToDoSomethingWithEmailBeforeItGoesOut(MailMessage mailMessage);
    public delegate void DoSomethingAfterEmailBeforeGoesOut(MyMailMessage mailMessage, Dictionary<String, string> otherStuffYouMightWannaKnow);
    public class MyMailMessage : MailMessage {
        private List<string> _AttachmentsFileSpecs=new List<string>();
        public List<string> AttachmentsfileSpecs {
            get {
                return _AttachmentsFileSpecs;
            }
            set {
                _AttachmentsFileSpecs = value;
            }
        }
        public MyMailMessage()
            : base() {
        }
        public MyMailMessage(string from, string to, string subject, string body)
            : base(from, to, subject, body) {
        }
    }
    public class EmailManager {
        public static void sendMail(string to, string subject, string body, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, Dictionary<string, string> otherStuff) {
            sendMail(to, subject, body, null,DDCommon.CommonRoutines.NULL_INT,hookLastChanceToDoDoSomethingWithEmail,hookAfterSuccessfulEmailTranmission,otherStuff);
        }
        public static void sendMail(string to, string subject, string body, string[] attachmentFiles, int Dumbo, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, Dictionary<string, string> otherStuff) {
            sendMail(to, subject, body, false, attachmentFiles, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission, otherStuff);
        }
        public static void sendMail(string to, string subject, string body, bool isBodyHtml, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, Dictionary<string, string> otherStuff) {
            sendMail(to, subject, body, isBodyHtml, null, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission, otherStuff);
        }
        public static void sendMail(string to, string subject, string body, bool isBodyHtml, string[] attachmentFiles, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, Dictionary<string, string> otherStuff) {
            sendMail(ConfigurationManager.AppSettings["EmailFrom"], to, subject, body, isBodyHtml, attachmentFiles, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission, otherStuff);
        }
        public static void sendMail(string from, string to, string subject, string body, bool isBodyHtml, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, Dictionary<string, string> otherStuff) {
            sendMail(from, to, subject, body, isBodyHtml, null, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission, otherStuff);
        }
        public static void sendMail(string from, string to, string subject, string body, bool isBodyHtml, 
                                    string[] attachmentFiles, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission,
                                    Dictionary<string,string> otherStuff) {
            string[] sa = to.Split(new char[1] { ';' });
            MyMailMessage mailMessage=null;
            if (sa.Length == 1) {
                mailMessage = new MyMailMessage(
                           from,
                           to,
                           subject,
                           body);
            } else {

                mailMessage = new MyMailMessage();
                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                foreach (string str in sa) {
                    mailMessage.To.Add(new MailAddress(str));
                }
            }
            if(!DDCommon.CommonRoutines.isNothing(from)) {
                mailMessage.Sender = new MailAddress(from);
            }
            mailMessage.IsBodyHtml = isBodyHtml;
            //            if (!CommonClientSide.CommonMethods.isTestMode()) {
            if (attachmentFiles != null && attachmentFiles.Length > 0) {
                foreach (string filename in attachmentFiles) {
                    mailMessage.Attachments.Add(new Attachment(filename));
                    mailMessage.AttachmentsfileSpecs.Add(filename);
                }
            }
            try {
                SmtpClient sc = new SmtpClient();
                // sc.EnableSsl = true;
                //sc.Credentials = new NetworkCredential("uic@employersunity.com", "uiclaim123","uic.employersunity.com");
                if (hookLastChanceToDoDoSomethingWithEmail != null) {
                    hookLastChanceToDoDoSomethingWithEmail(mailMessage);
                }
                // for some reason bcc doesn't do it's thing.  So:
                List<MailAddress> bccs=new List<MailAddress>();
                foreach (MailAddress ma in mailMessage.Bcc) {
                    bccs.Add(ma);
                }
                mailMessage.Bcc.Clear();
                sc.Send(mailMessage);
                if (hookAfterSuccessfulEmailTranmission != null) {
                    hookAfterSuccessfulEmailTranmission(mailMessage,otherStuff);
                }
                mailMessage.Subject = "BCC of mail sent to: " + to + " Subject: " + mailMessage.Subject;
                foreach (MailAddress ma2 in bccs) {
                    mailMessage.To.Clear();
                    mailMessage.To.Add(ma2);
                    sc.Send(mailMessage);
                }
            } catch (Exception eieio) {
                if (mailMessage.To[0].Address.ToLower().IndexOf("test") == -1) {
                    throw eieio;
                    //  EmailManager2.sendMail("uic@employersunity.com", "samuelsmichael222@gmail.com", "Hi, does this work?", "Here's the body");
                }
            }
                      
        }
        public static void sendFormattedMail(
            string fileSpec, Dictionary<string, string> substitutions, string to, string subject, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, string isLocalFileSystemFilespec, Dictionary<string, string> otherStuff) {
            sendFormattedMail(fileSpec, substitutions, to, subject, true, null, null, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission, isLocalFileSystemFilespec,otherStuff);
        }
        public static void sendFormattedMail(
            string fileSpec, Dictionary<string, string> substitutions, string to, string subject, string from, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, string isLocalFileSystemFilespec, Dictionary<string, string> otherStuff) {
            sendFormattedMail(fileSpec, substitutions, to, subject, true, null, from, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission, isLocalFileSystemFilespec,otherStuff);
        }

        public static void sendFormattedMail(
            string fileSpec, Dictionary<string,string> substitutions, string to,
                    string subject, bool useWebRequest, System.Web.HttpServerUtility server, string from, LastChanceToDoSomethingWithEmailBeforeItGoesOut hookLastChanceToDoDoSomethingWithEmail, DoSomethingAfterEmailBeforeGoesOut hookAfterSuccessfulEmailTranmission, string localFileSystemFileSpec, Dictionary<string, string> otherStuff) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                string text = string.Empty;
                bool makeItThrowAnException = false; // so's I can test
                if (useWebRequest) {
                    StreamReader sr = null;
                    StringBuilder sa = new StringBuilder();
                    try {
                        byte[] ba = new byte[1000];
                        char[] ca = new char[1000];
                        int nbrRead = 0;
                        System.Net.WebRequest wr = null;
                        Stream str = null;
                        bool isLocalFileSystem = false;
                        try {
                            if (makeItThrowAnException) {
                                throw new Exception("dummy");
                            }
                            wr = System.Net.WebRequest.Create(fileSpec);
                            str = wr.GetResponse().GetResponseStream();
                        } catch {
                            isLocalFileSystem = true;
                            sr = new StreamReader(localFileSystemFileSpec);
                        }

                        if (!isLocalFileSystem) {
                            nbrRead = str.Read(ba, 0, 1000);
                            // kludge to get rid o weird characters
                            System.Text.Decoder dec = Encoding.Default.GetDecoder();
                            while (nbrRead > 0) {
                                for (int c = 0; c < nbrRead; c++) {
                                    if (ba[c] == 239 || ba[c] == 187 || ba[c] == 191) {
                                        ba[c] = 32;
                                    }
                                }
                                dec.GetChars(ba, 0, nbrRead, ca, 0);
                                string str2 = new string(ca, 0, nbrRead).Trim();
                                sa.Append(str2);
                                nbrRead = str.Read(ba, 0, 1000);
                            }
                        } else {
                            nbrRead = sr.Read(ca, 0, 1000);
                            while (nbrRead > 0) {
                                // kludge to get rid o weird characters
                                for (int c = 0; c < nbrRead; c++) {
                                    if (ca[c] == 239 || ca[c] == 187 || ca[c] == 191) {
                                        ca[c] = ' ';
                                    }
                                }
                                sa.Append(ca, 0, nbrRead);
                                nbrRead = sr.Read(ca, 0, 1000);
                            }
                        }
                        text = sa.ToString();
                    } catch {
                    } finally {
                        try {
                            sr.Close();
                        } catch { }
                    }
                } else {
                    string path=server.MapPath(".");
                    string newFileSpec = path + "\\" + Path.GetFileName(fileSpec);
                    StreamReader sr = null;
                    try {
                        sr = new StreamReader(newFileSpec);
                        string line = sr.ReadLine();
                        bool breakOut = false;
                        if (line == null) {
                            breakOut = true;
                        }
                        while (!breakOut) {
                            text += line;
                            try {
                                line = sr.ReadLine();
                            } catch {
                                breakOut = true;
                                break;
                            }
                            if (line == null) {
                                breakOut = true;
                                break;
                            }
                        }
                    } finally {
                        text.Replace("’", "&rsquo;");
                        text.Replace("‘", "&lsquo;");
                        text.Replace("'", "&apos;");
                        text.Replace("\"", "&quot;");
                        text.Replace("“", "&ldquo;");
                        text.Replace("”", "&rdquo;");
                        text.Replace("Â", " ");
                        sr.Close();
                    }
                }
                foreach (string substitutionKey in substitutions.Keys) {
                    text = text.Replace("&&" + substitutionKey, substitutions[substitutionKey]);
                }
                if (from != null) {
                    sendMail(from, to, subject, text, true, hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission,otherStuff);
                } else {
                    sendMail(to, subject, text, true,hookLastChanceToDoDoSomethingWithEmail, hookAfterSuccessfulEmailTranmission,otherStuff);
                }
            }
        }
    }
}