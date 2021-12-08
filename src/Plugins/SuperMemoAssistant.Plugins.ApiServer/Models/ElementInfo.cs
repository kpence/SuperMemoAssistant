using Newtonsoft.Json;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SuperMemoAssistant.Plugins.ApiServer.Models
{
  public class ElementInfo
  {
    [JsonProperty(PropertyName = "Content")]
    public string Content;
    [JsonProperty(PropertyName = "Source")]
    public string Source;
    [JsonProperty(PropertyName = "ParentElementId")]
    public string ParentElementId;
    [JsonProperty(PropertyName = "ElementId")]
    public string ElementId;
    [JsonProperty(PropertyName = "ParentTitle")]
    public string ParentTitle;  
    [JsonProperty(PropertyName = "Priority")]
    public string Priority;
    [JsonProperty(PropertyName = "Title")]
    public string Title;
    [JsonProperty(PropertyName = "ElementType")]
    public string ElementType;
    [JsonProperty(PropertyName = "ElementStatus")]
    public string ElementStatus;
    [JsonProperty(PropertyName = "FirstGrade")]
    public string FirstGrade;
    [JsonProperty(PropertyName = "Ordinal")]
    public string Ordinal;
    [JsonProperty(PropertyName = "Repetitions")]
    public string Repetitions;
    [JsonProperty(PropertyName = "Lapses")]
    public string Lapses;
    [JsonProperty(PropertyName = "Interval")]
    public string Interval;
    [JsonProperty(PropertyName = "LastRepetition")]
    public string LastRepetition;
    [JsonProperty(PropertyName = "AFactor")]
    public string AFactor;
    [JsonProperty(PropertyName = "UFactor")]
    public string UFactor;
    [JsonProperty(PropertyName = "ForgettingIndex")]
    public string ForgettingIndex;
    [JsonProperty(PropertyName = "Reference")]
    public string Reference;
    [JsonProperty(PropertyName = "SourceArticle")]
    public string SourceArticleId;
    [JsonProperty(PropertyName = "HTMLFile")]
    public string HTMLFile;
    [JsonProperty(PropertyName = "_FullInfo")]
    public string _FullInfo;

    public ElementInfo(string elementInfo, string content)
    {
      _FullInfo = elementInfo;
      Content = content;
      Source = TryParse(elementInfo, @"^Source=(.*)\r$");
      ParentElementId = TryParse(elementInfo, @"^Parent=(.*)\r$");
      ElementId = TryParse(elementInfo, @"^Begin Element #(.*)\r$");
      ParentTitle = TryParse(elementInfo, @"^ParentTitle=(.*)\r$");
      Priority = TryParse(elementInfo, @"^Priority=(.*)\r$");
      Title = TryParse(elementInfo, @"^Title=(.*)\r$");
      ElementType = TryParse(elementInfo, @"^Type=(.*)\r$");
      ElementStatus = TryParse(elementInfo, @"^Status=(.*)\r$");
      FirstGrade = TryParse(elementInfo, @"^FirstGrade=(.*)\r$");
      Ordinal = TryParse(elementInfo, @"^Ordinal=(.*)\r$");
      Repetitions = TryParse(elementInfo, @"^Repetitions=(.*)\r$");
      Lapses = TryParse(elementInfo, @"^Lapses=(.*)\r$");
      Interval = TryParse(elementInfo, @"^Interval=(.*)\r$");
      LastRepetition = TryParse(elementInfo, @"^LastRepetition=(.*)\r$"); // TODO Parse this for datetime
      AFactor = TryParse(elementInfo, @"^AFactor=(.*)\r$");
      UFactor = TryParse(elementInfo, @"^UFactor=(.*)\r$");
      ForgettingIndex = TryParse(elementInfo, @"^ForgettingIndex=(.*)\r$");
      Reference = TryParse(elementInfo, @"^Reference=(.*)\r$");
      SourceArticleId = TryParse(elementInfo, @"^SourceArticle=(.*)\r$");
      HTMLFile = TryParse(elementInfo, @"^HTMLFile=(.*)\r$");
    }

    public string ToJson() => JsonConvert.SerializeObject(this);
    
    private string TryParse(string source, string pattern)
    {
      var result = Regex.Match(source, pattern, RegexOptions.Multiline);
      return result.Success
        ? result.Groups[1].Captures[0].Value
        : "";
    }
  }
}
