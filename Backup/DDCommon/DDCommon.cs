using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Collections.Specialized;
using System.Collections;


namespace DDCommon {
    public class CommonRoutines {
        public static string getDayOfWeekShort(int dayOfWeek) {
            string jdret="";
            switch (dayOfWeek) {
                case 0:
                    jdret = "Sun";
                    break;
                case 1:
                    jdret = "Mon";
                    break;
                case 2:
                    jdret = "Tue";
                    break;
                case 3:
                    jdret = "Wed";
                    break;
                case 4:
                    jdret = "Thu";
                    break;
                case 5:
                    jdret = "Fri";
                    break;
                case 6:
                    jdret = "Sat";
                    break;
                default:
                    break;
            }
            return jdret;
        }
        public static string getMonthNameShort(int month) {
            string retval = "";
            switch (month) {
                case 1:
                    retval = "Jan";
                    break;
                case 2:
                    retval = "Feb";
                    break;
                case 3:
                    retval = "Mar";
                    break;
                case 4:
                    retval = "Apr";
                    break;
                case 5:
                    retval = "May";
                    break;
                case 6:
                    retval = "Jun";
                    break;
                case 7:
                    retval = "Jul";
                    break;
                case 8:
                    retval = "Aug";
                    break;
                case 9:
                    retval = "Sep";
                    break;
                case 10:
                    retval = "Oct";
                    break;
                case 11:
                    retval = "Nov";
                    break;
                case 12:
                    retval = "Dec";
                    break;
                default:
                    break;
            }
            return retval;
        }
        public enum PAD_DIRECTION {
            LEFT, RIGHT
        }
        public static int NULL_INT = 40404040;
        public static string RegEditForCommaDelimitedFilesWithMaybeQuotes = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
        public static decimal NULL_DECIMAL = 40404040m;
        public static DateTime NULL_DATETIME = DateTime.MinValue;
        public static DateTime SQL_NULL_DATETIME = new DateTime(1900, 1, 1);
        public static int BYTE_SIZE = 1048576;
        public static object _lockingObject = new object();
        public static int countOfIn(char chr, string str) {
            int i = 0;
            if (str != null) {
                for (int c = 0; c < str.Length; c++) {
                    if (str[c] == chr) {
                        i++;
                    }
                }
            }
            return i;
        }
        public static string formatNameLFM(string lastName, string firstName, string mi) {
            string jdString=
                ObjectToStringV2(lastName) +
                (DDCommon.CommonRoutines.isNothing(ObjectToStringV2(lastName))?"":", ") +
                ObjectToStringV2(firstName) +
                " " +
                ObjectToStringV2(mi);
                return jdString.Trim();
        }

        public static string phonify(string phonenbr) {
            if (!(phonenbr is string)) {
                return phonenbr;
            } else {
                string phonenumberwithoutdashes = phonenbr.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Trim();
                if (phonenumberwithoutdashes.Length == 10) {
                    return "(" + phonenumberwithoutdashes.Substring(0, 3) + ")" + " " + phonenumberwithoutdashes.Substring(3, 3) + "-" + phonenumberwithoutdashes.Substring(6);
                } else {
                    return phonenbr;
                }
            }
        }

        public static string firstLetterCapitalizeTiFy(string str) {
            if (str != null) {
                StringBuilder sb = new StringBuilder();
                int c=0;
                while(c<str.Length) {
                    if(c==0) {
                        sb.Append(str[c].ToString().ToUpper());
                    } else {
                        sb.Append(str[c].ToString().ToLower());
                    }
                    c++;
                }
                return sb.ToString();
            } else {
                return str;
            }
        }
        public static string deSurroundingQuotify(string str) {
            if (str.Length > 1) {
                bool removeFirst = false;
                bool removeLast = false;
                if (str[0] == '"' && str[str.Length - 1] == '"') {
                    return str.Substring(1, str.Length - 2);
                } else {
                    return str;
                }
            } else {
                return str;
            }
        }
        public static string SocialSecurityNumberAtize(string ssn) {
            string jdssn = DDCommon.CommonRoutines.PadString(ssn, PAD_DIRECTION.LEFT, 9, '0');
            try {
                return jdssn.Substring(0, 3) + "-" + jdssn.Substring(3, 2) + "-" + jdssn.Substring(5);
            } catch {
                return ObjectToStringV2(jdssn);
            }
        }
        public static string PersonNamitate(string fn, string mi, string ln) {
            return
                ObjectToStringV2(fn) +
                (!isNothing(ObjectToStringV2(mi)) ? (" " + ObjectToStringV2(mi)) : "") +
                (!isNothing(ObjectToStringV2(ln)) ? (" " + ObjectToStringV2(ln)) : "");
        }
        public static string entityAtize(string str) {
            return CrLfToBRs(str.Replace("&", "&amp").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;"));
        }
        public static string CrLfToBRs(string str) {
            return
                str.Replace("\n\r", "<br>").Replace("\r\n", "<br>").Replace("\n", "<br>");
        }
        public static string PadString(string source, PAD_DIRECTION padDirection, int outputStringSize, char padFillCharacter) {
            if (source.Length >= outputStringSize) {
                return source;
            } else {
                StringBuilder sb = new StringBuilder();
                if (padDirection.Equals(PAD_DIRECTION.LEFT)) {
                    for (int c = 0; c < (outputStringSize - source.Length); c++) {
                        sb.Append(padFillCharacter);
                    }
                    sb.Append(source);
                } else {
                    sb.Append(source);
                    for (int c = 0; c < (outputStringSize - source.Length); c++) {
                        sb.Append(padFillCharacter);
                    }
                }
                return sb.ToString();
            }
        }
        public static DataSet getDataSetWithOnlyNumbers(int first, int last) {
            DataSet ds = new DataSet("Numbers");
            ds.Tables.Add("Numbers");
            ds.Tables[0].Columns.Add(new DataColumn("value", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("desc", typeof(string)));
            for (int c = first; c <= last; c++) {
                DataRow dr = ds.Tables[0].NewRow();
                dr["value"] = c.ToString();
                dr["desc"] = c.ToString();
                ds.Tables[0].Rows.Add(dr);
            }
            return ds;
        }
        public static string FormatToNumberDroppingDecimalsIfTheyArentPresent(object obj) {
            string str = obj.ToString();
            bool gotTheDecimalPoint = false;
            int lastSignificantDigit = -1;
            int locationOfDecimalPointIfAny = -1;
            bool thereAreNonZeroDigitsBeforeDecimalPoint = false;
            // pass 1
            int i = 0;
            for (int c = 0; c < str.Length; c++) {
                char ch=str[c];
                if (ch == '.') {
                    gotTheDecimalPoint = true;
                    locationOfDecimalPointIfAny = i;
                } else {
                    if (gotTheDecimalPoint) {
                        if (Char.IsDigit(ch) && ch != '0') {
                            lastSignificantDigit = i;
                        }
                    } else {
                        if (char.IsDigit(ch) && ch != '0') {
                            thereAreNonZeroDigitsBeforeDecimalPoint = true;
                        }
                        if (thereAreNonZeroDigitsBeforeDecimalPoint) {
                            lastSignificantDigit = i;
                        }
                    }
                }
                i++;
            }
            if (locationOfDecimalPointIfAny == -1 || thereAreNonZeroDigitsBeforeDecimalPoint) {
                return str.Substring(0, lastSignificantDigit + 1);
            } else {  // we've got a decimal point, and there are only 0's before it; so strip them
                if (lastSignificantDigit == -1) {
                    return "0";
                } else {
                    str = str.Substring(0, lastSignificantDigit + 1);
                    str = str.Substring(str.IndexOf('.'));
                    return str;
                }
            }
        }
        public static string removeNonAlphaCharacters(string str) {
            if (isNothing(str)) {
                return string.Empty;
            } else {
                StringBuilder sb = new StringBuilder();
                for (int c = 0; c < str.Length; c++) {
                    char ch = str[c];
                    if (Char.IsLetter(ch) || char.IsDigit(ch) || char.IsWhiteSpace(ch)) {
                        sb.Append(ch);
                    }
                }
                return sb.ToString();
            }
        }
        public static bool forceANewConnection {
            get {
                return ConfigurationManager.AppSettings["ForceANewConnection"] != null &&
                    ConfigurationManager.AppSettings["ForceANewConnection"].ToLower().Equals("true");
            }
        }
        public class ConnectionStringHelper {
            private System.Collections.Specialized.NameValueCollection _values=new NameValueCollection();
            public ConnectionStringHelper(string connectionString) {
                string[] sa = connectionString.Split(new char[1] { ';' });
                foreach (string str in sa) {
                    string[] sa2 = str.Split(new char[1] { '=' });
                    _values.Add(sa2[0],sa2[1]);
                }
            }
                        // Data Source=SERVER01;Initial Catalog=EUDB;User=EUAdmin2;Password=diamond222

            public string DataSource {
                get {
                    return _values["Data Source"];
                }
            }
            public string InitialCatalog {
                get {
                    return _values["Initial Catalog"];
                }
            }
            public string User {
                get {
                    object obj = _values["User"];
                    if (isNothing(obj)) {
                        obj = _values["User ID"];
                    }
                    return (string)obj;
                }
            }
            public string Password {
                get {
                    return _values["Password"];
                }
            }
        }
        public static bool allowMultipleEmailsPerCompany {
            get {
                return ConfigurationManager.AppSettings["AllowMultipleEmailsPerCompany"] != null &&
                    ConfigurationManager.AppSettings["AllowMultipleEmailsPerCompany"].ToLower().Equals("true");
            }
        }
        public static DateTime deBytify(object datetime) {
            DateTime dt = DateTime.MinValue;
            try {
                dt = Convert.ToDateTime(datetime);
            } catch {
                StringBuilder sb = new StringBuilder();
                sb.Append(System.Text.Encoding.ASCII.GetString((byte[])datetime));
                    dt = Convert.ToDateTime(sb.ToString());
            }
            return dt;
        }
        public static string doubleQuotify(string str) {
            if (isNothing(str)) {
                return string.Empty;
            }
            return
                str.Replace("'", "^~^").Replace("’", "^~^").Replace("‘","^~^").Replace("^~^", "''");
        }
        public static bool isNothingNot(object obj) {
            return !isNothing(obj);
        }
        public static bool isNothing(object obj) {
            if (obj == null) {
                return true;
            } else {
                if (obj is string) {
                    return ((string)obj).Trim().Equals(string.Empty);
                } else {
                    if (obj is DBNull) {
                        return true;
                    } else {
                        if (obj is Int32) {
                            return ((int)obj) == NULL_INT;
                        } else {
                            if (obj is DateTime) {
                                return ((DateTime)obj).Equals(NULL_DATETIME) || ((DateTime)obj).Equals(SQL_NULL_DATETIME);
                            } else {
                                if (obj is Decimal) {
                                    return ((Decimal)obj) == NULL_DECIMAL;
                                }
                            return false;
                            }
                        }
                    }
                }
            }
        }
        public static DateTime TimeODayToDateTime(object obj) {
            if (obj == null) {
                return NULL_DATETIME;
            } else {
                if (obj is DBNull) {
                    return NULL_DATETIME;
                } else {
                    try {
                        return Convert.ToDateTime(SQL_NULL_DATETIME.ToShortDateString() + " " + obj.ToString());
                    } catch {
                        return NULL_DATETIME;
                    }
                }
            }
        }

        public static DateTime ObjectToDateTime(object obj) {
            if (obj == null) {
                return NULL_DATETIME;
            } else {
                if (obj is DBNull) {
                    return NULL_DATETIME;
                } else {
                    try {
                        return Convert.ToDateTime(obj);
                    } catch {
                        return NULL_DATETIME;
                    }
                }
            }
        }
        public static string ObjectToStringEmptyForNullDates(object obj) {
            if (obj is DateTime) {
                if (((DateTime)obj).Equals(NULL_DATETIME) || ((DateTime)obj).Equals(SQL_NULL_DATETIME)) {
                    return string.Empty;
                }
            }
            return ObjectToStringV2(obj);
        }
        public static string ObjectToStringForTDz(object obj) {
            string str = ObjectToStringV2(obj);
            if (str.Equals(string.Empty)) {
                str = "&nbsp;";
            }
            return str;
        }
        public static string ObjectToStringV2NoTrimming(object obj) {
            if (obj == null) {
                return "";
            } else {
                if (obj is System.Xml.XmlAttribute) {
                    return ((System.Xml.XmlAttribute)obj).Value.ToString();
                }
                if (obj is string) {
                    return ((string)obj);
                } else {
                    if (obj is DBNull) {
                        return "";
                    } else {
                        return obj.ToString();
                    }
                }
            }
        }
        public static string ObjectToStringV2(object obj) {
            return ObjectToStringV2NoTrimming(obj).Trim();
        }
        public static string ObjectTostring(object obj) {
            return ObjectToStringV2(obj);
        }
        public static decimal ObjectTodecimal(object obj) {
            return ObjectToDecimal(obj);
        }
        public static bool ObjectTobool(object obj) {
            return ObjectToBool(obj);
        }
        public static int ObjectToIntNULLINTIfNull(object obj) {
            if (obj == null || obj is DBNull) {
                return NULL_INT;
            } else {
                return ObjectToInt(obj);
            }
        }
        public static decimal ObjectToDecimal(object obj) {
            try {
                if (obj is DBNull || obj==null) {
                    return DDCommon.CommonRoutines.NULL_DECIMAL;
                } else {
                    return Convert.ToDecimal(obj);
                }
            } catch {
                return 0;
            }
        }
        public static decimal ObjectToDecimal0IfNull(object obj) {
            try {
                if (obj is DBNull || obj == null) {
                    return 0;
                } else {
                    return Convert.ToDecimal(obj);
                }
            } catch {
                return 0;
            }
        }
        public static double ObjectToDouble(object obj) {
            try {
                return Convert.ToDouble(obj);
            } catch {
                return 0d;
            }
        }
        public static int ObjectToInt(object obj) {
            try {
                return Convert.ToInt32(obj);
            } catch {
                return 0;
            }
        }
        public static int ObjectToint(object obj) {
            return ObjectToInt(obj);
        }
        public static int HoursToMinutes(string hours) {
            Decimal hrs=Convert.ToDecimal(hours);
            return (int)(hrs * 60);
        }
        public static double MinutesToHours(int minutes) {
            return Math.Round((double)(((double)minutes) / 60d),2);
        }
        public static DateTime manageDateControl(string textInput) {
            DateTime dtDate = DDCommon.CommonRoutines.NULL_DATETIME;
            try {
                dtDate = Convert.ToDateTime(textInput);
            } catch {
                string mmddyy = textInput.Trim();
                if (mmddyy.Length == 6 || mmddyy.Length==8) {
                    int mm = Convert.ToInt32(mmddyy.Substring(0, 2));
                    int dd = Convert.ToInt32(mmddyy.Substring(2, 2));
                    if (mmddyy.Length == 6) {
                        int yy = Convert.ToInt32(mmddyy.Substring(4, 2));
                        int yyyy =
                            yy > 50 ? (1900 + yy) : (2000 + yy);
                        try {
                            dtDate = new DateTime(yyyy, mm, dd);
                        } catch {
                        }
                    } else {
                        int yyyy = Convert.ToInt32(mmddyy.Substring(4, 4));
                        try {
                            dtDate = new DateTime(yyyy, mm, dd);
                        } catch {
                        }
                    }
                }
            }
            return dtDate;
        }
        public static double ObjectToDouble_RemoveNonNumerics(object obj) {
            string str = obj.ToString();
            StringBuilder sb = new StringBuilder();
            bool haveDoneFirst = false;
            for (int c = 0; c < str.Length; c++) {
                char ch = str[c];
                if (ch == ' ') {
                    continue;
                }
                if (Char.IsDigit(ch) || ch == '.' || (ch == '-' && !haveDoneFirst)) {
                    sb.Append(ch);
                }
                haveDoneFirst = true;
            }
            return ObjectToDouble(sb.ToString());
        }
        public static decimal ObjectToDecimal_RemoveNonNumerics(object obj) {
            string str = obj.ToString();
            StringBuilder sb = new StringBuilder();
            bool haveDoneFirst = false;
            for (int c = 0; c < str.Length; c++) {
                char ch = str[c];
                if (ch == ' ') {
                    continue;
                }
                if (Char.IsDigit(ch) || ch == '.' || (ch=='-' && !haveDoneFirst)) {
                    sb.Append(ch);
                }
                haveDoneFirst = true;
            }
            return ObjectToDecimal(sb.ToString());
        }
        public static int ObjectToInt_RemoveNonNumerics(object obj) {
            string str = obj.ToString();
            StringBuilder sb = new StringBuilder();
            bool haveDoneFirst = false;
            for (int c = 0; c < str.Length; c++) {
                char ch = str[c];
                if (ch == ' ') {
                    continue;
                }
                if (Char.IsDigit(ch) || ch == '.' || (ch == '-' && !haveDoneFirst)) {
                    sb.Append(ch);
                }
                haveDoneFirst = true;
            }
            return ObjectToInt(sb.ToString());
        }
        public static bool ObjectToBool(object obj) {
         
            if (obj is string) {
                string str = ((string)obj).Trim().ToLower();
                if (str.Equals("t")) {
                    return true;
                } else {
                    if ((str.Equals("true"))) {
                        return true;
                    } else {
                        if (str.Equals("y")) {
                            return true;
                        } else {
                            if (str.Equals("yes")) {
                                return true;
                            } else {
                                if (str.Trim().Equals("x")) {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            try {
                return Convert.ToBoolean(obj);
            } catch {
                return false;
            }
        }
        public static DateTime dateTimeFromStrings(string dateString, string timeString) {
            try {
                string[] sa = dateString.Split(new char[] { '/' });
                int mm=Convert.ToInt32(sa[0]);
                int dd=Convert.ToInt32(sa[1]);
                int yy = Convert.ToInt32(sa[2]);
                if (yy < 100) {
                    yy += 2000;
                }
                if (timeString.ToLower().IndexOf("pm") == -1 && timeString.ToLower().IndexOf("am") == -1) {
                    throw new Exception("Time must have an AM or PM designation. Time supplied: " + timeString + ".");
                }
                bool isPM = false;
                if (timeString.ToLower().IndexOf("pm") != -1) {
                    isPM = true;
                }
                if (isPM) {
                    int pmIndex = timeString.ToLower().IndexOf("pm");
                    timeString = timeString.Remove(pmIndex, 2).Trim();
                } else {
                    int amIndex = timeString.ToLower().IndexOf("am");
                    timeString = timeString.Remove(amIndex, 2).Trim();
                }
                sa = timeString.Split(new char[] { ':' });
                int hh = Convert.ToInt32(sa[0]);
                if (hh == 12 && !isPM) {
                    hh = 0;
                } else {
                    if (isPM) {
                        if (hh != 12) {
                            hh += 12;
                        }
                    }
                }
                int mn = Convert.ToInt32(sa[1]);
                return new DateTime(yy, mm, dd, hh, mn, 0);
            } catch {
                throw new Exception("Invalid date or time string. Date supplied: " + dateString + ". Time supplied: " + timeString + ".");
            }
        }
        public static DateTime dateFromString(string dateString) {
            try {
                string[] sa = dateString.Split(new char[] { '/' });
                int mm = Convert.ToInt32(sa[0]);
                int dd = Convert.ToInt32(sa[1]);
                int yy = Convert.ToInt32(sa[2]);
                if (yy < 100) {
                    yy += 2000;
                }
                return new DateTime(yy, mm, dd, 0, 0, 0);
            } catch {
                throw new Exception("Invalid date string. Date supplied: " + dateString + ".");
            }
        }
        public static string dateInCCYYMMDDHHMMSSFormat(DateTime date) {
            return date.ToString("yyyyMMddHHmmss");
        }
        public static DateTime dateFromYYYYMMDD(string yyyymmdd) {
            return new DateTime(
                Convert.ToInt32(yyyymmdd.Substring(0, 4)),
                Convert.ToInt32(yyyymmdd.Substring(4, 2)),
                Convert.ToInt32(yyyymmdd.Substring(6, 2)));
        }
        public static decimal decimalFromCobolString(string data, int nbrODecimalPositions) {
            try {
                char c = data[data.Length - 1];
                bool isNegatory = false;
                char lastChar = c;
                switch (c) {
                    case '{':
                        lastChar = '0';
                        break;
                    case 'A':
                        lastChar = '1';
                        break;
                    case 'B':
                        lastChar = '2';
                        break;
                    case 'C':
                        lastChar = '3';
                        break;
                    case 'D':
                        lastChar = '4';
                        break;
                    case 'E':
                        lastChar = '5';
                        break;
                    case 'F':
                        lastChar = '6';
                        break;
                    case 'G':
                        lastChar = '7';
                        break;
                    case 'H':
                        lastChar = '8';
                        break;
                    case 'I':
                        lastChar = '9';
                        break;

                    case '}':
                        lastChar = '0';
                        isNegatory = true;
                        break;
                    case 'J':
                        lastChar = '1';
                        isNegatory = true;
                        break;
                    case 'K':
                        lastChar = '2';
                        isNegatory = true;
                        break;
                    case 'L':
                        lastChar = '3';
                        isNegatory = true;
                        break;
                    case 'M':
                        lastChar = '4';
                        isNegatory = true;
                        break;
                    case 'N':
                        lastChar = '5';
                        isNegatory = true;
                        break;
                    case 'O':
                        lastChar = '6';
                        isNegatory = true;
                        break;
                    case 'P':
                        lastChar = '7';
                        isNegatory = true;
                        break;
                    case 'Q':
                        lastChar = '8';
                        isNegatory = true;
                        break;
                    case 'R':
                        lastChar = '9';
                        isNegatory = true;
                        break;
                }
                int i = nbrODecimalPositions;
                char[] ca = new char[data.Length];
                for (int j = 0; j < data.Length; j++) {
                    if (j < data.Length - 1) {
                        ca[j] = data[j];
                    } else {
                        ca[j] = lastChar;
                    }
                }
                data = new String(ca);
                double divisior = 1d;
                double fraction = 0d;
                while (i > 0) {
                    string cc = new String(data[data.Length - i],1);
                    double d = Convert.ToDouble(cc);
                    divisior /= 10;
                    fraction += (d * divisior);
                    i--;
                }
                double wholeNumber=Convert.ToDouble(data.Substring(0, data.Length - nbrODecimalPositions));
                wholeNumber+=fraction;
                decimal jdDecimal=Convert.ToDecimal(Math.Round(wholeNumber,nbrODecimalPositions));
                if(isNegatory) {
                    jdDecimal*=-1;
                }
                return jdDecimal;   
            } catch {
                return 0;
            }
        }
        public static DateTime dateFromMMDDYYYY(string mmddyyyy) {
            return new DateTime(
                Convert.ToInt32(mmddyyyy.Substring(4, 4)),
                Convert.ToInt32(mmddyyyy.Substring(0, 2)),
                Convert.ToInt32(mmddyyyy.Substring(2, 2)));
        }
        public static DateTime dayOnly(DateTime dt) {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
        public static string plusify(string str) {
            return str.Replace(" ", "+");
        }
    }
    public class EventArgsArrayList : EventArgs {
        private ArrayList _events = new ArrayList();
        public void AddEvent(object zEvent) {
            _events.Add(zEvent);
        }
        public ArrayList Events {
            get { return _events; }
        }
    }
    public class NotSuppliedException : Exception {
        public NotSuppliedException(string message)
            : base(message) {
        }
    }
}
