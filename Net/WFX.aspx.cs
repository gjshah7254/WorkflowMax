using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class WFX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected XmlDocument Invoke(string xml, string url, string method)
    {
        XmlDocument result = null;
        // convert the xml into a byte array
        byte[] data = null;

        if (!String.IsNullOrEmpty(xml))
            data = System.Text.Encoding.UTF8.GetBytes(xml);

        // set up the request
        StringBuilder responseText = new StringBuilder();
        try
        {
            // create and initialize the web request 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = "Name of Your Application";
            request.KeepAlive = false;
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            // set the timeout to 120 seconds
            request.Timeout = 120 * 1000;
            // set the content length in the request headers 
            if (data != null)
            {
                request.ContentLength = data.Length;

                // write the data to be imported into the request stream
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
            }

            // get the response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (request.HaveResponse == true && response != null)
                {
                    // response should be 200 (OK) however certain errors such as validation errors
                    // will come through in the response so it must be read and parsed accordingly

                    // get the response stream and read it
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseText = new StringBuilder(reader.ReadToEnd());
                    }
                }
            }
        }
        catch (WebException wex)
        {
            // this exception will be raised if the server didn't return 200 (OK)
            // some requests cause the import API to return status codes such as 401 and 500
            // it is important to retrieve more information about any particular errors
            if (wex.Response != null)
            {
                // get the error response
                using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                {

                    // get the error response stream and read it
                    using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        responseText = new StringBuilder(reader.ReadToEnd());
                    }
                }
            }
            else
            {
                throw; // HTTP exception
            }
        }

        try
        {
            // load the response text into an XML document
            // all responses from the import API will return an xml document
            result = new XmlDocument();
            result.LoadXml(responseText.ToString());
        }
        catch (XmlException)
        {
            // response text was not a valid XML document which should not happen
            throw;
        }

        return result;
    }
}