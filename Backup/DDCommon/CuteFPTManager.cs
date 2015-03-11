using System;
using System.Collections.Generic;
using System.Text;
using CuteFTPPro;
using System.IO;
using DDCommon.cs;

namespace DDCommon {
    public class CuteFPTManager {        
        public static void trialCode() {
            CuteFTPPro.TEConnection MySite = new CuteFTPPro.TEConnection();
            MySite.set_Option("AutoCloseMethod",1);
            MySite.Protocol = "FTPS_IMPLICIT";
            MySite.Host = "ftp.mhhs.org/UNITY";
            MySite.Login = "55ftpunity";
            MySite.Password = "Tuesday23";
            MySite.Port = 990;
            MySite.Connect();
            StreamReader sr = null;
            try {
                MySite.GetList("", "c:\\temp\\temp_list2.txt", "%NAME");
                MySite.LocalFolder = "c:\\temp";
                sr = new StreamReader("c:\\temp\\temp_list2.txt");
                string line = sr.ReadLine();
                while (line != null) {
                    if (line.Substring(line.Length - 3).ToLower().Equals("txt")) {
                        File.Delete("c:\\temp\\" + line);
                        int okay = MySite.Download(line, line, 1);
                        if (okay == -1) {
                            int okayRemove = MySite.RemoteRemove("/UNITY/" + line);
                            int x = 3;
                        }
                    }
                    line = sr.ReadLine();
                }
                /*
                            MySite.LocalFolder = "c:\\temp";
                            MySite.Download("/UNITY/06202009.txt", "06202009b.txt", 1);
                //            MySite.RemoteRemove "/UNITY/*.txt"
                */
            } finally {
                try { MySite.Disconnect(); } catch { }
                try { MySite.Close("EXIT"); } catch { }
                try { sr.Close(); } catch { }
            }
            /*
'First declare a variable called Mysite. This will hold the reference to the TE COM object.

   Dim MySite

   'Create a connection object and assign it to the variable

   Set MySite = CreateObject("CuteFTPPro.TEConnection")
	
   ' Now set each property for the site connection 
   ' You can omit this section to use the default values, but you should at least specify the Host
   'The default Protocol is FTP, however SFTP (SSH2), FTPS (SSL), HTTP, and HTTPS can also be used)

   MySite.Protocol = "FTP"
   MySite.Host = "ftp.globalscape.com"

   'following lines are optional since the default is anonymous if no login and password are defined

   MySite.Login = "anonymous"
   MySite.Password = "user@user.com"

   'if necessary, use the UseProxy method and ProxyInfo or SocksInfo properties to connect through a proxy server

   MySite.UseProxy = "BOTH"

   'now connect to the site (also called called implicitly when most remote methods are called)

   MySite.Connect

	
   'perform some logic to verify that the connection was made successfully

   If (Not Cbool(MySite.IsConnected)) Then	
      MsgBox "Could not connect to: " & MySite.Host & "!"
      Quit(1)
   End If

   'The script will now check to see if the local folder c:\temp exists and will create it if necessary

   If (Not (MySite.LocalExists("c:\temp"))) Then
      MySite.CreateLocalFolder "c:\temp"
   End If

   'Change TE's local working folder to to c:\temp
   MySite.LocalFolder = "c:\temp"

   'Check for existence of remote folder "/pub/cuteftp"
   b = MySite.RemoteExists("/pub/cuteftp/")

   If (Not CBool(b)) Then
      'Verify existence of remote folder
      MsgBox "Remote folder not found!. Please make sure that the Pub folder exists on the remote site"
      Quit(1)
   End If

   'Now download the index file to the local destination folder
   MySite.Download "/pub/cuteftp/index.txt"

   'Complete.  Show the status of this transfer.
   MsgBox "Task done, final status is '" + MySite.Status + "'"

  MySite.Disconnect
  MySite.Close

'End of sample script. You can save you script and then run it by either selecting it from the Tools > Run Script menu or by double clicking on the script file in Windows
*/
        }
    }
    public class CuteFTPMemorialHermann : CuteFPTManager {
        public CuteFTPMemorialHermann(string outputDir, string host, string userID, string password, ProjectSpecificAbstract psa) {
            string outputDirWithoutSlash = null;
            string outputDirWithSlash = outputDir;
            if (outputDirWithSlash[outputDirWithSlash.Length - 1] != '\\') {
                outputDirWithSlash += "\\";
            }
            outputDirWithoutSlash = outputDirWithSlash.Substring(0, outputDirWithSlash.Length - 1);
            try {
                psa.InstanceLogDebugMessage("Pre-CuteFTPPro.TEConnection", "About to do CuteFTPPro.TEConnection", "CuteFTPMemorialHermann");
            } catch { }
            CuteFTPPro.TEConnection MySite = new CuteFTPPro.TEConnection();
            try {
                psa.InstanceLogDebugMessage("Post-CuteFTPPro.TEConnection", "Did CuteFTPPro.TEConnection", "CuteFTPMemorialHermann");
            } catch { }
            MySite.set_Option("AutoCloseMethod", 1);
            MySite.Protocol = "FTPS_IMPLICIT";
            MySite.Host = host;
            MySite.Login = userID;
            MySite.Password = password;
            MySite.Port = 990;
            MySite.Connect();
            StreamReader sr = null;
            try {
                MySite.GetList("", outputDirWithSlash+"MemorialHermanFileList.txt", "%NAME");
                MySite.LocalFolder = outputDirWithoutSlash;
                sr = new StreamReader(outputDirWithSlash + "MemorialHermanFileList.txt");
                string line = sr.ReadLine();
                while (line != null) {
                    File.Delete(outputDirWithSlash+line);
                    int okay = MySite.Download(line, line, 1);
                    if (okay == -1) {
                        int okayRemove = MySite.RemoteRemove("/UNITY/" + line);
                    }
                    line = sr.ReadLine();
                }
            } finally {
                try { MySite.Disconnect(); } catch { }
                try { MySite.Close("EXIT"); } catch { }
                try { 
                    sr.Close();
                    File.Delete(outputDirWithSlash + "MemorialHermanFileList.txt");
                } catch { }

            }
        }
    }
}
