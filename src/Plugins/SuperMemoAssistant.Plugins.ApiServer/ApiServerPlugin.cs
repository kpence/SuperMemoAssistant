#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

#endregion




namespace SuperMemoAssistant.Plugins.ApiServer
{
  using System;
  using System.Diagnostics;
  using System.Runtime.Remoting;
  using System.Windows;
  using System.Windows.Input;
  using Services;
  using Services.IO.Keyboard;
  using SuperMemoAssistant.Interop.Plugins;
  using SuperMemoAssistant.Interop.SuperMemo.Content.Controls;
  using SuperMemoAssistant.Interop.SuperMemo.Core;
  using SuperMemoAssistant.Plugins.ApiServer.Models;
  using SuperMemoAssistant.Sys.Remoting;
  using Sys.IO.Devices;

  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  public class ApiServerPlugin : SMAPluginBase<ApiServerPlugin>
  {
    #region Constructors

    public ApiServerPlugin() : base() { }

    protected override void Dispose(bool disposing)
    {
      Kernel32.FreeConsole();
      HttpServer.Dispose();

      base.Dispose(disposing);
    }

    #endregion




    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "ApiServer";

    public override bool HasSettings => false;

    #endregion




    #region Methods Impl

    /// <inheritdoc />
    protected override void OnSMStarted(bool wasSMAlreadyStarted)
    {
      Svc.HotKeyManager
         .RegisterGlobal(
           "TestSomething",
           "TestSomething",
           HotKeyScopes.SM,
           new HotKey(Key.Y, KeyModifiers.CtrlAlt),
           TestSomething
         );

      Kernel32.CreateConsole();

      HttpServer.Create();
      HttpServer.Instance.Route("/element-info", _ => ApiServerController.ElementInfoAction());
      HttpServer.Instance.Route("/learning-mode", _ => ApiServerController.LearningModeAction());
      HttpServer.Instance.Route("/set-grade", grade => ApiServerController.SetGradeAction(grade));
      HttpServer.Instance.Route("/was-graded", _ => ApiServerController.WasGradedAction());
      HttpServer.Instance.Route("/next-element", _ => ApiServerController.NextElementAction());
      HttpServer.Instance.Route("/begin-learning", _ => ApiServerController.BeginLearningAction());
      HttpServer.Instance.Route("/set-element-content", text => ApiServerController.SetElementContentAction(text));
      HttpServer.Instance.Route("/set-element-title", text => ApiServerController.SetElementTitleAction(text));
      HttpServer.Instance.Route("/goto-first-element-with-title", text => ApiServerController.GoToFirstElementWithTitleAction(text));
      HttpServer.Instance.Route("/goto-first-element-with-comment", text => ApiServerController.GoToFirstElementWithCommentAction(text));
      HttpServer.Instance.Route("/new-topic", _ => ApiServerController.NewTopicAction());
      HttpServer.Instance.Route("/new-item", _ => ApiServerController.NewItemAction());
      HttpServer.Instance.Route("/set-priority", priority => ApiServerController.SetPriorityAction(priority));
      HttpServer.Instance.Route("/append-comment", comment => ApiServerController.AppendCommentAction(comment));
      HttpServer.Instance.Route("/comment", _ => ApiServerController.CommentAction());
      HttpServer.Instance.Route("/postpone", days => ApiServerController.PostponeAction(days));

      // Unneeded and unfinished?
      HttpServer.Instance.Route("/done", _ => ApiServerController.DoneAction());

      Svc.SM.UI.ElementWdw.OnElementChanged += new ActionProxy<SMDisplayedElementChangedEventArgs>(OnElementChanged);
      ApiServerState.Instance.UpdateElementInfo(null);

      //ApiServerState.Instance.SuperMemoHWnd = Process.GetProcessById(Svc.SM.ProcessId).MainWindowHandle;

      base.OnSMStarted(wasSMAlreadyStarted);
    }

    /// <inheritdoc />
    public override void ShowSettings() { }

    #endregion




    #region Methods
    public static void OnElementChanged(SMDisplayedElementChangedEventArgs e)
    {
      try
      {
        IControlHtml ctrlHtml = Svc.SM.UI.ElementWdw.ControlGroup.GetFirstHtmlControl();

        ApiServerState.Instance.OnElementChanged(e.NewElement, ctrlHtml);
      }
      catch (RemotingException) { }
    }

    private static void TestSomething()
    {
      //MessageBox.Show(ApiServerState.Instance.ElementInfo.ToJson());
      //Svc.SM.UI.ElementWdw.FindText();
      int sizeOfCollection = Svc.SM.Registry.Element.Count;
      var elemId = -1;
      for (var i = 1; i < sizeOfCollection+1; i += 1)
      {
        var elem = Svc.SM.Registry.Element[i];
        if (elem.Title == "12345")
        {
          elemId = elem.Id;
          break;
        }
      }
    }
    #endregion
  }
}
