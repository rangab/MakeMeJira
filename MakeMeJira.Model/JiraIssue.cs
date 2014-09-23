using Newtonsoft.Json;

namespace MakeMeJira.Model
{
    public enum JiraIssueType
    {
        Bug=1,
        Story,
        Task=3,
        Improvement=4,
        Epic=5
    }

    public class JiraIssue
    {
        [JsonProperty(PropertyName = "fields")]
        public Fields Fields { get; set; }

        public JiraIssue()
        {
            Fields = new Fields();
        }
    }
 
    public class Fields
    {
        [JsonProperty(PropertyName = "project")]
        public Project Project { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        
        [JsonProperty(PropertyName = "issuetype")]
        public IssueType IssueType { get; set; }

        public Fields()
        {
            Project = new Project();
            IssueType = new IssueType();
        }
    }
 
    public class Project
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
 
    public class IssueType
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
