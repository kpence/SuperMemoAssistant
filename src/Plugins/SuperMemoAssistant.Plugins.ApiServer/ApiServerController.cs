using Newtonsoft.Json;
using SuperMemoAssistant.Interop.SuperMemo.Learning;
using SuperMemoAssistant.Plugins.ApiServer.Helpers;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static string WasGradedAction()
    {
      return JsonHelper.JsonFromValue(ApiServerState.Instance.WasGraded);
    }

    public static string NextElementAction()
    {
      var success = false;
      if (Svc.SM.UI.ElementWdw.CurrentLearningMode == LearningMode.Standard && ApiServerState.Instance.WasGraded)
      {
        success = Svc.SM.UI.ElementWdw.NextElementInLearningQueue();
      }
      return JsonHelper.JsonFromValue(success);
    }

    public static string SetGradeAction(string gradeJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(gradeJson);
      var grade = jsonDict["grade"];

      var success = false;
      var currentLearningMode = Svc.SM.UI.ElementWdw.CurrentLearningMode;
      if (currentLearningMode == LearningMode.Standard)
      {
        success = Svc.SM.UI.ElementWdw.SetGrade(grade);
      }
      if (success)
      {
        ApiServerState.Instance.WasGraded = true;
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
      var text = jsonDict["text"];
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
      return JsonHelper.JsonFromValue(success);
    }

    public static string SetPriorityAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, double>>(textJson);
      var priority = jsonDict["priority"];
      var success = Svc.SM.UI.ElementWdw.SetPriority(Svc.SM.UI.ElementWdw.CurrentElementId, priority);

      return JsonHelper.JsonFromValue(success);
    }

    public static string SetElementTitleAction(string textJson)
    {
      var jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textJson);
      var text = jsonDict["text"];
      Svc.SM.UI.ElementWdw.SetTitle(Svc.SM.UI.ElementWdw.CurrentElementId, text);
      return JsonHelper.JsonFromValue(false);
    }

  }
}
