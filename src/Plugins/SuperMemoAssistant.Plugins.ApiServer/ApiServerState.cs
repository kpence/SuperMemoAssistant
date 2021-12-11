using Newtonsoft.Json;
using SuperMemoAssistant.Interop.SuperMemo.Content.Controls;
using SuperMemoAssistant.Interop.SuperMemo.Core;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Types;
using SuperMemoAssistant.Interop.SuperMemo.Learning;
using SuperMemoAssistant.Plugins.ApiServer.Helpers;
using SuperMemoAssistant.Plugins.ApiServer.Models;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.ApiServer
{
  class ApiServerState
  {
    public static ApiServerState Instance { get; } = new ApiServerState();
    public ElementInfo ElementInfo { get; set; }
    public bool IsReadyToGrade { get; set; } = false;
    public bool AwaitElementChange { get; set; } = false;
    public bool WasGraded { get; set; } = false;
    //public IntPtr SuperMemoHWnd { get; set; }

    public void OnElementChanged(IElement newElement)
    {
      Console.WriteLine("ApiServerState1");
      //if (!AwaitElementChange  && !(Svc.SM.UI.ElementWdw?.ControlGroup?.IsDisposed ?? true))
        // UpdateElementInfo(Svc.SM.UI.ElementWdw?.ControlGroup?.GetFirstHtmlControl()?.Text);
      ElementInfo = null;
      WasGraded = false;
    }

    public void UpdateElementInfo(string content)
    {
      try
      {
        ElementInfo = new ElementInfo(Svc.SM.UI.ElementWdw.GetElementAsText(), content);
        if (content == null)
        {
          var text = ApiServerState.Instance.ElementInfo.Text;
          Console.WriteLine("ApiServerState UpdateElementInfo");
          var htmFile = ApiServerState.Instance.ElementInfo.HTMLFile;
          ApiServerState.Instance.ElementInfo.Content = (String.IsNullOrEmpty(htmFile)) ? text : System.IO.File.ReadAllText(htmFile);
          if (ApiServerState.Instance.ElementInfo.Content.Length > 5000)
          {
            ApiServerState.Instance.ElementInfo.Content = "The element was too big, you have to manually copy it over.";
          }

        }
      }
      catch (RemotingException) { }
    }

  }
}
