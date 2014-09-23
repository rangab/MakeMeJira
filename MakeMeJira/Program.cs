using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using System.Xml;
using System.Xml;
using System.Xml.Serialization;
using MakeMeJira.Model;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Contrib;
using RestSharp.Deserializers;
using XmlSerializer = RestSharp.Serializers.XmlSerializer;

namespace MakeMeJira
{
    class Program
    {
        private const string SDP_BASE_URL = "http://servicedesk.peakadventuretravel.com/WorkOrder.do?woMode=viewWO&woID={0}";
        private const string JIRA_BASE_URL = "https://peakadventuretravel.atlassian.net/rest/api/2/";


        static void Main(string[] args)
        {

            Console.WriteLine("Please enter the incident Id");
            Console.WriteLine("");
            var incidentId = Convert.ToInt32(Console.ReadLine());
            var projectKey = ConfigurationManager.AppSettings["ProjectKey"];

            Console.WriteLine("");
            Console.WriteLine("Please enter the Issue Type");
            Console.WriteLine("");
            Console.WriteLine("1. Bug");
            Console.WriteLine("2. Story");
            Console.WriteLine("3. Task");
            Console.WriteLine("4. Improvement");
            Console.WriteLine("5. Epic");
            Console.WriteLine("");

            var issueTypeId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Reading the incident from SDP...");
            Console.WriteLine("");

            while (issueTypeId > 0 && issueTypeId > 5)
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid Issue Type. Please select one of the below issue types");
                Console.WriteLine("");
                Console.WriteLine("1. Bug");
                Console.WriteLine("2. Story");
                Console.WriteLine("3. Task");
                Console.WriteLine("4. Improvement");
                Console.WriteLine("5. Epic");
                issueTypeId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("");
            }


            //var issueTypeId = 1;
            //var incidentId = 101359;
            //var projectKey = "SI";

            var sdpIncident = ReadFromSDP(incidentId);
            var jiraIssue = sdpIncident.ToJiraIssue(projectKey, SDP_BASE_URL,(JiraIssueType)issueTypeId);

            var isCreated = CreateInJira(jiraIssue);

            while (!isCreated)
            {
                Console.WriteLine("Oooops!!! somethign went wrong. Please try again...");
                Console.WriteLine("");
                Main(args);
            }


            Console.Read();

        }

        public static SdpApi DeserializeObject(string toDeserialize)
        {
            if (string.IsNullOrWhiteSpace(toDeserialize)) return new SdpApi();

            using (var reader = new StringReader(toDeserialize))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SdpApi));
                return (SdpApi)serializer.Deserialize(new NamespaceIgnorantXmlTextReader(reader));
            }
        }

        public static string SerializeObject(SdpApi toSerialize)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SdpApi));
            var textWriter = new StringWriter();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xmlSerializer.Serialize(textWriter, toSerialize, ns);
            return textWriter.ToString();
        }
        
        private static SdpApi ReadFromSDP(int incidentId)
        {
            var sdpApiKey = ConfigurationManager.AppSettings["SDPApiKey"];
            string getUrl = string.Format("http://servicedesk.peakadventuretravel.com//sdpapi/request/{0}", incidentId);

            IRestRequest request = new RestRequest(getUrl);
            request.AddHeader("Content-type", string.Format("{0}; charset=utf-16", "text/xml"));


            request.AddParameter("OPERATION_NAME", "GET_REQUEST");
            request.AddParameter("TECHNICIAN_KEY", sdpApiKey);

            request.AddHeader("Accept", "text/xml");
            request.Method = Method.GET;


            var result = ExecuteRequest(request);
            var deserializer = new XmlDeserializer();

           
            var sdpIncident = DeserializeObject(result.Content);


            return sdpIncident;

        }

        public static IRestResponse ExecuteRequest(IRestRequest request)
        {
          
            IRestClient client = new RestClient();
            var result = client.Execute(request);
            return result;
        }

        private static bool CreateInJira(JiraIssue issue)
        {
 
            issue.Fields.Description = HttpUtility.HtmlDecode(issue.Fields.Description);
            issue.Fields.Summary = HttpUtility.HtmlDecode(issue.Fields.Summary);

            if (issue.Fields.Summary.Length > 255)
                issue.Fields.Summary = issue.Fields.Summary.Substring(0, 255);
            
            var client = new RestClient();
            var jiraUserName = ConfigurationManager.AppSettings["JiraUserName"];
            var jiraPassword = ConfigurationManager.AppSettings["JiraPassword"];
            var cred = UTF8Encoding.UTF8.GetBytes(jiraUserName + ":" + jiraPassword);
            var restRequest = new RestRequest(JIRA_BASE_URL + "issue",Method.POST);
            var serializedIssue = JsonConvert.SerializeObject(issue);

            restRequest.AddHeader("Authorization", "Basic " + Convert.ToBase64String(cred));
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json",serializedIssue,ParameterType.RequestBody);

            var result          = client.Execute<JiraResponse>(restRequest);
            var jiraResponse    = JsonConvert.DeserializeObject<JiraResponse>(result.Content);


            if (result.StatusCode == HttpStatusCode.Created)
            {
                Console.WriteLine("Issue has been successfully created in JIRA. Please search for " + jiraResponse.Key);
                Console.WriteLine("Url to access issue is : " + jiraResponse.Self);
                return true;
            }
             
            return false;
        }
    }

    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(System.IO.TextReader reader) : base(reader) { }

        public override string NamespaceURI
        {
            get { return ""; }
        }
    }
}
