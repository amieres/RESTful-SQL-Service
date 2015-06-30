

namespace REST.Service {
    public class SQLWebService {
        private string vConnectionString;

        public string ConnectionString {
            get { return vConnectionString; }
            set { vConnectionString = value; }
        }
        public string JsonToXml(string json) {
            using (System.Xml.XmlReader reader = System.Runtime.Serialization.Json.JsonReaderWriterFactory
                    .CreateJsonReader(System.Text.Encoding.ASCII.GetBytes(json), new System.Xml.XmlDictionaryReaderQuotas())) {
                return System.Xml.Linq.XElement.Load(reader).ToString();
            }
        }
        public string XmlToJson(string XML) {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(XML);
            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        }
        public void Connect() {
            if (Connection.State != System.Data.ConnectionState.Open) {
                Connection.ConnectionString = vConnectionString;
                Connection.Open();
            }
        }
        public System.Data.SqlClient.SqlConnection Connection {
            get {
                if (vConnection == null) {
                    vConnection = new System.Data.SqlClient.SqlConnection();
                    vConnection.ConnectionString = vConnectionString;
                }
                return vConnection;
            }
        }
        [System.NonSerialized]
        protected System.Data.SqlClient.SqlConnection vConnection = null;
        public System.Data.SqlClient.SqlCommand GetCommand(string Query) {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
            System.Data.SqlClient.SqlCommand SqlCommandC = Connection.CreateCommand();
            SqlCommandC.CommandText = Query;
            SqlCommandC.CommandTimeout = CommandTimeoutInSeconds;
            return SqlCommandC;
        }
        public int CommandTimeoutInSeconds = 400;
        public string ProcessSqlXmlToJson(string Query) {
            using (System.Data.SqlClient.SqlCommand Command = GetCommand(Query)) {
                using (System.Xml.XmlReader reader = Command.ExecuteXmlReader()) {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(reader);
                    foreach (System.Xml.XmlNode Node in doc.SelectNodes("//*"))
                        (Node as System.Xml.XmlElement).RemoveAllAttributes();
                    return Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                }
            }
        }
        public string Query;
        public string ProcessQueryToJson() {
            return ProcessSqlXmlToJson(Query);
        }
        public System.Net.HttpListener HttpListener = null;
        public void StartWebServer() {
            HttpListener = new System.Net.HttpListener();
            HttpListener.Prefixes.Clear();
            HttpListener.Prefixes.Add(ListenerPrefix);
            KeepListening = true;
            ListenerThread = new System.Threading.Thread(new System.Threading.ThreadStart(WebServerListener));
            ListenerThread.IsBackground = true;
            ListenerThread.Start();
        }
        public void StopWebServer() {
            try {
                KeepListening = false;
                HttpListener.Stop();
                HttpListener = null;
                ListenerThread = null;
            }
            catch { }
        }
        ~SQLWebService() {
            try {
                StopWebServer();
                Connection.Close();
            }
            catch { }
        }
        public void ListenerCallback(System.IAsyncResult GetContextHandle) {
            System.Net.HttpListenerContext context = HttpListener.EndGetContext(GetContextHandle);
            try {
                //context.Request;
                string result = ProcessSqlXmlToJson(Query);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(context.Response.OutputStream)) {
                    writer.Write(result);
                    context.Response.ContentLength64 = context.Response.OutputStream.Length;
                    writer.Close();
                }
            }
            catch (System.Exception E) {
                context.Response.StatusCode = 500;
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(context.Response.OutputStream)) {
                    writer.Write(E.ToString());
                    context.Response.ContentLength64 = context.Response.OutputStream.Length;
                    writer.Close();
                }
            }

        }
        public string ListenerPrefix;
        public bool KeepListening;
        public void WebServerListener() {
            try {
                HttpListener.Start();
                while (KeepListening)
                    ProcessRequest(HttpListener.GetContext());
                HttpListener.Stop();
            }
            catch (System.Exception E) {
                System.Windows.Forms.MessageBox.Show(E.ToString());
            }
        }
        public System.Threading.Thread ListenerThread;
        public void ProcessRequest(System.Net.HttpListenerContext context) {
            if (context.Request.HttpMethod == "GET")
                ProcessRequestGet(context);
            else if (context.Request.HttpMethod == "PUT")
                ProcessRequestPut(context);
        }
        public int RecordsPerPage = 20;
        public string GetFieldsFromQuery(string Query) {
            using (System.Data.SqlClient.SqlCommand Command = GetCommand(Query)) {
                using (System.Data.SqlClient.SqlDataReader reader = Command.ExecuteReader()) {
                    string result = "";
                    for (int I = 0; I < reader.VisibleFieldCount; I++)
                        result += (I == 0 ? "" : ", ") + "[" + reader.GetName(I) + "]";
                    return result;
                }
            }
        }
        public string ProcessSqlXml(string Query) {
            using (System.Data.SqlClient.SqlCommand Command = GetCommand(Query)) {
                using (System.Data.SqlClient.SqlDataReader reader = Command.ExecuteReader()) {
                    string result = "";
                    foreach (System.Data.Common.DbDataRecord record in reader)
                        result += record.GetString(0);
                    return result;
                }
            }
        }
        public bool GetTableName(string URL, ref string database, ref string schema, ref string table, ref string KeyValue, ref string condition) {
            int n = 0;
            int start = 0;
            while ((start = ListenerPrefix.IndexOf("/", start) + 1) > 0)
                n++;
            n -= 2;
            string[] Path = URL.Split(new string[] { "?" }, System.StringSplitOptions.None)[0]
                               .Split(new string[] { "/" }, System.StringSplitOptions.None);
            if (Path.Length == n)
                return false;
            if (Path.Length > n)
                database = System.Net.WebUtility.UrlDecode(Path[n]);
            if (Path.Length > n + 1)
                schema =   System.Net.WebUtility.UrlDecode(Path[n + 1]);
            if (Path.Length > n + 2)
                table =    System.Net.WebUtility.UrlDecode(Path[n + 2]);
            if (Path.Length == n + 4)
                KeyValue = System.Net.WebUtility.UrlDecode(Path[n + 3]);
            for (; Path.Length > n + 4; n += 2)
                condition = (condition != "" ? condition + " AND " : "") + "(" + System.Net.WebUtility.UrlDecode(Path[n + 3]) + " = '" + System.Net.WebUtility.UrlDecode(Path[n + 4]) + "')";
            return true;
        }
        public string GetFirstField(string Fields) {
            int I = Fields.IndexOf(",");
            if (I < 0) return Fields;
            return Fields.Substring(0, I);
        }
        public System.Xml.XmlDocument ProcessQueryToXmlDoc() {
            using (System.Data.SqlClient.SqlCommand Command = GetCommand(Query)) {
                using (System.Xml.XmlReader reader = Command.ExecuteXmlReader()) {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(reader);
                    return doc;
                }
            }
        }
        public void ProcessRequestGet(System.Net.HttpListenerContext context) {
            string result = ListenerPrefix + "[database]/[schema]/[table]/[KeyValue][KeyField1/KeyValue1[/KeyField2/KeyValue2...]]?option1&option2...\n";
            result += "options:\n\t";
            result += "includecount\t=fieldname\n\t";
            result += "page\t=n\n\t";
            result += "perpage\t=recordsperpage\n\t";
            result += "limit\t=perpage\n\t";
            result += "start\t=startrecord\n\t";
            result += "finish\t=finalrecord\n\n\t";
            result += "showquery\t=true\n\n\t";
            result += "database\t=DBName\n\t";
            result += "schema\t=Schema\n\t";
            result += "table\t=TableName\n\t";
            result += "q\t=SQLQuery\n\t";
            result += "keyvalue\t=Id\n\t";
            result += "keyfield\t=12\n\t";
            result += "condition\t=sqlcondition\n\t";
            result += "fields\t=fieldlist\n\t";
            result += "orderby\t=fields\n\t";
            result += "xmlsuffix\t=FOR XML PATH('Record'), ELEMENTS XSINIL\n\t";
            result += "xmlroot\t=Data\n\t";
            result += "xmlrecord\t=Record\n\n\t";
            result += "format\t=xml|json\n\t";
            result += "mime\t=text/xml\n\n\t";
            result += "prepend\t=sometext\n\t";
            result += "append\t=sometext\n\t";
            result += "callback\t=jsoncallback\n\t";
            try {
                string Mime = "";
                string condition = "";
                string database = "";
                string schema = "";
                string table = "";
                string keyvalue = "";
                string keyfield = "";
                if (GetTableName(context.Request.RawUrl, ref database, ref schema, ref table, ref keyvalue, ref condition)) {
                    string basequery = "";
                    bool ShowQuery = false;
                    int PerPage = RecordsPerPage;
                    int Page = 1;
                    int Start = 0;
                    int Finish = 0;
                    string format = "json";
                    string XmlSuffix = "";
                    string XmlRoot = "Data";
                    string XmlRecord = "";
                    string Prepend = "";
                    string Append = "";
                    string callback = "";
                    string OrderBy = "";
                    string InclCount = "";
                    string Fields = "*";
                    foreach (string key in context.Request.QueryString.AllKeys) {
                        string[] values = context.Request.QueryString.GetValues(key);
                        if (values.Length > 0)
                            switch (key) {
                                case "page": Page = System.Convert.ToInt32(values[0]); break;
                                case "perpage": PerPage = System.Convert.ToInt32(values[0]); break;
                                case "limit": PerPage = System.Convert.ToInt32(values[0]); break;
                                case "start": Start = System.Convert.ToInt32(values[0]); break;
                                case "finish": Finish = System.Convert.ToInt32(values[0]); break;
                                case "showquery": ShowQuery = values[0] == "true"; break;
                                case "database": database = values[0]; break;
                                case "schema": schema = values[0]; break;
                                case "table": table = values[0]; break;
                                case "keyvalue": keyvalue = values[0]; break;
                                case "keyfield": keyfield = values[0]; break;
                                case "condition": condition = values[0]; break;
                                case "fields": Fields = values[0]; break;
                                case "orderby": OrderBy = values[0]; break;
                                case "xmlsuffix": XmlSuffix = values[0]; break;
                                case "xmlroot": XmlRoot = values[0]; break;
                                case "xmlrecord": XmlRecord = values[0]; break;
                                case "format": format = values[0]; break;
                                case "mime": Mime = values[0]; break;
                                case "prepend": Prepend = values[0]; break;
                                case "append": Append = values[0]; break;
                                case "callback": callback = values[0]; break;
                                case "includecount": InclCount = values[0]; break;
                                case "q": basequery = values[0]; break;
                            }
                    }
                    if (Start == 0) Start = (Page - 1) * PerPage + 1;
                    if (Finish == 0) Finish = Start + PerPage - 1;
                    if (XmlRecord == "") XmlRecord = (table == "") ? "Record" : table;
                    if (XmlRoot == "") XmlRoot = XmlRecord + "s";
                    if (XmlSuffix == "") XmlSuffix = "FOR XML PATH('" + XmlRecord + "'), ELEMENTS XSINIL";
                    if (database != "") database = "[" + database + "].";
                    if (table == "") {
                        table = "(SELECT S.name + '.' + T.name TableName FROM  " + database + "Sys.Tables T, " + database + "sys.schemas S WHERE T.schema_id = S.schema_id " + (schema == "" ? "" : " AND S.name = '" + schema + "'") + ") S";
                        database = "";
                        schema = "";
                    }
                    else table = "[" + table + "]";
                    if (schema != "") schema = "[" + schema + "]";
                    if (schema != "" || database != "") schema = schema + ".";
                    if (basequery == "")
                        basequery = "SELECT " + Fields + " FROM " + database + schema + table;
                    else
                        basequery = "SELECT " + Fields + " FROM (" + basequery + ") S";
                    if (OrderBy == "")
                        if (keyfield != "")
                            OrderBy = keyfield;
                        else {
                            Fields = GetFieldsFromQuery(basequery);
                            OrderBy = GetFirstField(Fields);
                        }
                    if (keyvalue != "") {
                        if (keyfield == "") keyfield = GetFirstField(OrderBy);
                        condition = (condition != "" ? condition + " AND " : "") + "(" + keyfield + " = '" + keyvalue + "')";
                    }
                    if (condition != "")
                        basequery += " WHERE " + condition;
                    string query = "WITH  _BaseQuery_ as (\n\t" + basequery + "\n)\n";
                    query += ", _QueryRowNum_ as (\n\tSELECT *, _Row_Num = ROW_NUMBER() OVER (ORDER BY " + OrderBy + ")\n\tFROM _BaseQuery_\n)\n";
                    query += ", _QueryPaged_ as (\n\tSELECT " + Fields + "\n\tFROM _QueryRowNum_\n\t";
                    if (Finish < 0)
                        query += "WHERE _Row_Num >= " + Start.ToString();
                    else
                        query += "WHERE _Row_Num BETWEEN " + Start.ToString() + " AND " + Finish.ToString();
                    query += "\n)\nSELECT ";
                    if (InclCount != "")
                        query += InclCount + " = (SELECT Count(*) FROM _BaseQuery_ S), \n\t";
                    query += "cast((SELECT * FROM _QueryPaged_ " + XmlSuffix + ") as XML)\n";
                    query += "  FOR XML PATH ('" + XmlRoot + "')";
                    if (callback != "") callback = ";" + callback;
                    if (ShowQuery)
                        result = query;
                    else {
                        if (format == "xml") 
                            result = ProcessSqlXml(query);
                        else
                            result = ProcessSqlXmlToJson(query);
                        result = Prepend + result + callback + Append;
                    }
                    if (Mime == "")
                        Mime = ShowQuery ? "text/plain" : (format == "xml" ? "text/xml" : "text/json");
                }
                context.Response.ContentType = Mime;
            }
            catch (System.Exception E) {
                context.Response.StatusCode = 500;
                result = E.ToString();
            }
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(context.Response.OutputStream)) {
                writer.Write(result);
                writer.Close();
            }
        }
        public void ProcessRequestPut(System.Net.HttpListenerContext context) {
            string result = "usage: \n GET : \\json?query=SELECT * FROM dbo.Customers FOR XML PATH ('Customer'), ROOT('Customers')";
            try {
                if (context.Request.HttpMethod == "GET") {
                }
            }
            catch (System.Exception E) {
                context.Response.StatusCode = 500;
                result = E.ToString();
            }
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(context.Response.OutputStream)) {
                writer.Write(result);
                writer.Close();
            }
        }
    }
}
