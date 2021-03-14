public void GetCustNumber()
    {
        try
        {
            DateTime now = DateTime.Now;
            string s = now.ToString();
            string newStr = s.Remove(2, 1);
            string newStr1 = newStr.Remove(4, 1);
            string newStr2 = newStr1.Remove(8, 1);
            string newStr3 = newStr2.Remove(10, 1);
            string newStr4 = newStr3.Remove(12, 1);
            string url = "https://appsuat.tataaia.com/TraversalWS/Service.asmx/getCustomerNumber?date=" + newStr4;
            string stringResult = "";
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "GET";
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            stringResult = sr.ReadToEnd().Trim();
            StringReader theReader = new StringReader(stringResult);
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);
            string status = theDataSet.Tables[0].Rows[0][0].ToString();
            string CLI = theDataSet.Tables[0].Rows[0][1].ToString();
            string STRCON = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            SqlConnection con = new SqlConnection(STRCON);
            con.Open();
            SqlCommand cmd = new SqlCommand("USP_TATA_INBOUND_DATA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@CLI", CLI);
            cmd.Parameters.AddWithValue("@input_datetime", newStr4);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            sr.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
