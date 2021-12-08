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
    public bool WasGraded { get; set; } = false;
    //public IntPtr SuperMemoHWnd { get; set; }

    public void OnElementChanged(IElement newElement, IControlHtml ctrlHtml)
    {
      UpdateElementInfo(ctrlHtml.Text);
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
          var htmFile = ApiServerState.Instance.ElementInfo.HTMLFile;
          ApiServerState.Instance.ElementInfo.Content = (String.IsNullOrEmpty(htmFile)) ? text : System.IO.File.ReadAllText(htmFile);
        }
      }
      catch (RemotingException) { }
    }

  }
}
