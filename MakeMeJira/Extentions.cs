using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeMeJira.Model;

namespace MakeMeJira
{
    public static class Extentions
    {
        public static JiraIssue ToJiraIssue(this SdpApi sdpIncident, string projectKey, string sdpBaseUrl, JiraIssueType issueType)
        {
            var jiraIssue = new JiraIssue();

            jiraIssue.Fields = new Fields()
            {
                Project = new Project() {Key = projectKey},
                Description =
                    sdpIncident.Response.Operation.Details.Parameters.First(p => p.Name == SdpParaNames.ShortDescription)
                        .Value,
                IssueType = new IssueType()
                {
                    Name = issueType.ToString()
                },
                Summary =
                    sdpIncident.Response.Operation.Details.Parameters.First(p => p.Name == SdpParaNames.Subject)
                        .Value
            };

            var incidentId = 
                sdpIncident.Response.Operation.Details.Parameters.First(p => p.Name == SdpParaNames.WorkOrderId)
                    .Value;

            jiraIssue.Fields.Description = jiraIssue.Fields.Description + Environment.NewLine + Environment.NewLine;
            jiraIssue.Fields.Description = jiraIssue.Fields.Description + "Click below link to view in SDP" + Environment.NewLine;
            jiraIssue.Fields.Description = jiraIssue.Fields.Description + string.Format(sdpBaseUrl, incidentId);

            return jiraIssue;
        }
    }
}
