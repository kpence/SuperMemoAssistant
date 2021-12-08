using Newtonsoft.Json;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Builders;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Models;
using SuperMemoAssistant.Interop.SuperMemo.Learning;
using SuperMemoAssistant.Plugins.ApiServer.Helpers;
using SuperMemoAssistant.Services;
using SuperMemoAssistant.Sys.IO.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SuperMemoAssistant.Plugins.ApiServer
{
  class ApiServerController
  {
    public static string ElementInfoAction()
    {
      return ApiServerState.Instance.ElementInfo.ToJson();
    }

    public static string LearningModeAction()
    {
      return JsonHelper.JsonFromValue(Svc.SM.UI.ElementWdw.CurrentLearningMode);
    }

    public static string ReadyToGradeAction()
    {
      return JsonHelper.JsonFromValue(ApiServerState.Instance.IsReadyToGrade);
    }

    public static string WasGradedAction()
    {
      return JsonHelper.JsonFromValue(ApiServerState.Instance.WasGraded);
    }

    public static string NextRepetitionAction()
    {
      var success = false;
      if (Svc.SM.UI.ElementWdw.CurrentLearningMode != LearningMode.None)
      {
        success = Svc.SM.UI.ElementWdw.NextRepetition();
        ApiServerState.Instance.IsReadyToGrade = true;
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string NextElementAction()
    {
      var success = false;
      if (Svc.SM.UI.ElementWdw.CurrentLearningMode != LearningMode.None)
      {
        success = Svc.SM.UI.ElementWdw.NextElementInLearningQueue();
        ApiServerState.Instance.IsReadyToGrade = true;

        // This function doesn't call the onelementchanged callback, so I need to reset apiserverstate's elmenet info content
        ApiServerState.Instance.UpdateElementInfo(null);
        ApiServerState.Instance.ElementInfo.Content = System.IO.File.ReadAllText(ApiServerState.Instance.ElementInfo.HTMLFile);
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string SetGradeAction(string gradeJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(gradeJson);
      var grade = jsonDict["grade"];

      var success = false;
      if (ApiServerState.Instance.IsReadyToGrade)
      {
        var currentLearningMode = Svc.SM.UI.ElementWdw.CurrentLearningMode;
        if (currentLearningMode == LearningMode.Standard)
        {
          success = Svc.SM.UI.ElementWdw.SetGrade(grade);
          ApiServerState.Instance.WasGraded = true;
        }
      }
      return JsonHelper.JsonFromValue(success);
    }

    // TODO delete this
    public static string DoneAction()
    {
      return JsonHelper.JsonFromValue(Svc.SM.UI.ElementWdw.Done());
    }

    public static string BeginLearningAction()
    {
      var success = false;
      var currentLearningMode = Svc.SM.UI.ElementWdw.CurrentLearningMode;
      if (currentLearningMode == LearningMode.None)
      {
        success = Svc.SM.UI.ElementWdw.BeginLearning(currentLearningMode);
        ApiServerState.Instance.IsReadyToGrade = true;
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string SetElementContentAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textJson);
      var text = jsonDict["text"];
      var ctrlHtml = Svc.SM.UI.ElementWdw.ControlGroup?.GetFirstHtmlControl();
      var success = false;
      if (ctrlHtml != null)
      {
        success = true;
        ctrlHtml.Text = text;
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string GoToFirstElementWithTitleAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textJson);
      var text = jsonDict["title"];
      var success = false;

      if (!ApiServerState.Instance.WasGraded)
      {
        int sizeOfCollection = Svc.SM.Registry.Element.Count;
        var elemId = -1;
        for (var i = 1; i < sizeOfCollection+1; i += 1)
        {
          var elem = Svc.SM.Registry.Element[i];
          if (elem.Title == text)
          {
            elemId = elem.Id;
            break;
          }
        }
        if (elemId != -1)
        {
          Svc.SM.UI.ElementWdw.GoToElement(elemId);
          success = true;
          ApiServerState.Instance.IsReadyToGrade = false;
        }
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string GoToFirstElementWithCommentAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textJson);
      var comment = jsonDict["comment"];
      var success = false;

      if (!ApiServerState.Instance.WasGraded)
      {
        var elemId = -1;
        var elem = Svc.SM.Registry.Element.FirstOrDefaultByName(new System.Text.RegularExpressions.Regex(comment));
        if (elem != null)
        {
          var elemComment = elem.Comment;
          if (elemComment != null && elemComment.Contains(comment))
          {
            elemId = elem.Id;
          }
        }
        if (elemId != -1)
        {
          Svc.SM.UI.ElementWdw.GoToElement(elemId);
          ApiServerState.Instance.IsReadyToGrade = false;
          success = true;
        }
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string NewTopicAction()
    {
      var success = false;

      if (!ApiServerState.Instance.WasGraded)
      {
        success = Svc.SM.UI.ElementWdw.AppendElement(Interop.SuperMemo.Elements.Models.ElementType.Topic) != -1;
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string NewItemAction()
    {
      var success = false;

      if (!ApiServerState.Instance.WasGraded)
      {
        success = Svc.SM.UI.ElementWdw.AppendElement(Interop.SuperMemo.Elements.Models.ElementType.Item) != -1;
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string PostponeAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(textJson);
      var days = jsonDict["days"];
      var success = false;

      success = Svc.SM.UI.ElementWdw.PostponeRepetition(days);
      if (success)
      {
        ApiServerState.Instance.WasGraded = true;
        ApiServerState.Instance.IsReadyToGrade = false;
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string DismissAction()
    {
      var success = false;
      success = Svc.SM.UI.ElementWdw.DismissElement(Svc.SM.UI.ElementWdw.CurrentElementId);
      return JsonHelper.JsonFromValue(success);
    }

    public static string ForceRepetitionAction(string textJson)
    {
      var success = false;
      if (ApiServerState.Instance.IsReadyToGrade)
      {
        var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(textJson);
        var days = jsonDict["days"];

        success = Svc.SM.UI.ElementWdw.ForceRepetitionAndResume(days, true);
        if (success)
        {
          ApiServerState.Instance.WasGraded = true;
          ApiServerState.Instance.IsReadyToGrade = false;
        }
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string SetPriorityAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, double>>(textJson);
      var priority = jsonDict["priority"];
      var success = false;
      success = Svc.SM.UI.ElementWdw.SetPriority(Svc.SM.UI.ElementWdw.CurrentElementId, priority);

      return JsonHelper.JsonFromValue(success);
    }

    public static string SetElementTitleAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textJson);
      var text = jsonDict["text"];
      Svc.SM.UI.ElementWdw.SetTitle(Svc.SM.UI.ElementWdw.CurrentElementId, text);
      return JsonHelper.JsonFromValue(true);
    }

    public static string AppendCommentAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textJson);
      var comment = jsonDict["comment"];
      var success = Svc.SM.UI.ElementWdw.AppendComment(Svc.SM.UI.ElementWdw.CurrentElementId, comment);
      return JsonHelper.JsonFromValue(success);
    }

    public static string CommentAction()
    {
      return JsonHelper.JsonFromValue(Svc.SM.UI.ElementWdw.CurrentElement.Comment);
    }

  }
}
