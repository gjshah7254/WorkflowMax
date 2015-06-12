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
using System.Xml.Serialization;

public partial class WFX : System.Web.UI.Page
{
    public const string apiKey = "14C10292983D48CE86E1AA1FE0F8DDFE";
    public const string accountKey = "D0118AA751C34D418E6710F692D0ED43";
    public const string staffListUrl = "https://api.workflowmax.com/staff.api/list";
    public const string SinglestaffUrl = "https://api.workflowmax.com/staff.api/get";
    public const string categoryUrl = "https://api.workflowmax.com/category.api/list";
    public const string timeListUrl = "https://api.workflowmax.com/time.api/list";
    public const string timeGetStaffUrl = "https://api.workflowmax.com/time.api/staff";

    protected void Page_Load(object sender, EventArgs e)
    {
        //GetStaffList();

        //string ID = "233831";

    }

    protected void btnGetTimeForAllStaff_Click(object sender, EventArgs e)
    {
        if ((!string.IsNullOrEmpty(txtDateFrom.Text)) && (!string.IsNullOrEmpty(txtDateTo.Text)))
        {
            GetStaffList();
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "<span class='has-error'>Please enter dates.<br /></span>";
        }
    }

    private void GetStaffList()
    {
        string xmlItemsRequest = CreateRequest(staffListUrl, "");
        XmlDocument staffListXml = MakeRequest(xmlItemsRequest);
        StaffListProcessResponse(staffListXml);
    }

    //Create the request URL
    public static string CreateRequest(string url, string queryString)
    {
        string UrlRequest = string.Format("{3}?apiKey={0}&accountKey={1}&{2}", apiKey, accountKey, queryString, url);
        return (UrlRequest);
    }

    //Submit the HTTP Request and return the XML response
    public static XmlDocument MakeRequest(string requestUrl)
    {
        try
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());
            return (xmlDoc);

        }
        catch
        {
            return null;
        }
    }

    public void CatgeoriesProcessResponse(XmlDocument xml)
    {
        #region Categories
        XmlNodeList xmlItems = xml.SelectNodes("/Response/Categories/Category");
        foreach (XmlNode xmlItem in xmlItems)
        {
            string test = xmlItem["Name"].InnerText;
        }
        #endregion
    }

    public void StaffListProcessResponse(XmlDocument xml)
    {
        #region staffList
        XmlNodeList xmlItems = xml.SelectNodes("/Response/StaffList/Staff");
        Response.Write("<table class='table table-hover table-bordered table-striped'><tr><th width='300px'>Name</th><th width='300px'>Billable (HRS)</th><th width='300px'>Non Billable (HRS)</th><th width='300px'>Total (HRS)</th></tr>");
        foreach (XmlNode xmlItem in xmlItems)
        {
            string Name = xmlItem["Name"].InnerText;
            string ID = xmlItem["ID"].InnerText;

            string xmlItemsRequest = CreateRequest(SinglestaffUrl + "/" + ID, "");
            XmlDocument SingleStaffXml = MakeRequest(xmlItemsRequest);
            SingleStaffProcessResponse(SingleStaffXml);
        }
        Response.Write("</table>");
        #endregion
    }

    public void SingleStaffProcessResponse(XmlDocument xml)
    {
        #region SingleStaff
        XmlNodeList xmlItems = xml.SelectNodes("/Response/Staff");

        foreach (XmlNode xmlItem in xmlItems)
        {
            string Name = xmlItem["Name"].InnerText;
            string ID = xmlItem["ID"].InnerText;

            string querystring = string.Format("from={0}&to={1}", txtDateFrom.Text, txtDateTo.Text);
            Response.Write("<tr>");
            Response.Write("<td>" + Name + "</td>");

            string xmlItemsRequest = CreateRequest(timeGetStaffUrl + "/" + ID, querystring);
            XmlDocument staffXml = MakeRequest(xmlItemsRequest);
            timeGetStaffProcessResponse(staffXml);
            Response.Write("</tr>");
        }

        #endregion
    }

    public void timeGetStaffProcessResponse(XmlDocument xml)
    {
        #region SingleStaff
        XmlNodeList xmlItems = xml.SelectNodes("/Response/Times/Time");

        int billableTime = 0;
        int nonBillableTime = 0;
        int totalTime = 0;

        foreach (XmlNode xmlItem in xmlItems)
        {
            int Minutes = 0;
            Minutes = Convert.ToInt32(xmlItem["Minutes"].InnerText);
            bool Billable = Convert.ToBoolean(xmlItem["Billable"].InnerText);

            if (Billable)
            {
                billableTime = billableTime + Minutes;
            }
            else
            {
                nonBillableTime = nonBillableTime + Minutes;
            }

            totalTime = totalTime + Minutes;
        }

        Response.Write(string.Format("<td>{0}</td><td>{1}</td><td>{2}</td>", ConvertHours(billableTime), ConvertHours(nonBillableTime), ConvertHours(totalTime)));
        #endregion
    }

    private string ConvertHours(int minutes)
    {
        var timeSpan = TimeSpan.FromMinutes(minutes);
        int hh = timeSpan.Hours;
        int mm = timeSpan.Minutes;

        return string.Format("{0}:{1}", hh.ToString(), mm.ToString());
    }

    public void timeListProcessResponse(XmlDocument xml)//pass in from and to date paramters
    {
        #region SingleStaff
        XmlNodeList xmlItems = xml.SelectNodes("/Response/Times/Time");
        foreach (XmlNode xmlItem in xmlItems)
        {
            string Minutes = xmlItem["Minutes"].InnerText;
            string Staff = xmlItem["Staff"].InnerText;
        }
        #endregion
    }
}