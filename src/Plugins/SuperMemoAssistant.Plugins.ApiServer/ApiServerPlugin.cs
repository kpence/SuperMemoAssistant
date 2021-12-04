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




namespace SuperMemoAssistant.Plugins.DevSandbox
{
  using System;
  using System.Runtime.Remoting;
  using System.Windows;
  using System.Windows.Input;
  using Services;
  using Services.IO.Keyboard;
  using SuperMemoAssistant.Interop.Plugins;
  using SuperMemoAssistant.Interop.SuperMemo.Core;
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
           "TestAnotherThing",
           "TestAnotherThing",
           HotKeyScopes.SM,
           new HotKey(Key.D2, KeyModifiers.CtrlAltShift),
           TestAnotherThing
         )
         .RegisterGlobal(
           "TestSomething",
           "TestSomething",
           HotKeyScopes.SM,
           new HotKey(Key.Y, KeyModifiers.CtrlAlt),
           TestSomething
         );

      Kernel32.CreateConsole();

      Svc.SM.UI.ElementWdw.OnElementChanged += new ActionProxy<SMDisplayedElementChangedEventArgs>(OnElementChanged);

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
        ApiServerState.Instance.ElementInfo = Svc.SM.UI.ElementWdw.GetElementAsText();
      }
      catch (RemotingException) { }
    }

    private static void TestSomething()
    {
      MessageBox.Show(ApiServerState.Instance.ElementInfo);
    }

    private static void TestAnotherThing()
    {
      foreach (var template in Svc.SM.Registry.Template)
        Console.WriteLine($"{template.Id}: {template.Name} ({template.UseCount})");

      int templateId;

      do
      {
        Console.Write("Input template id: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out templateId) == false)
          Console.WriteLine($"Invalid input: '{input}'.");

        var template = Svc.SM.Registry.Template[templateId];

        if (template == null || template.Empty)
        {
          Console.WriteLine("No such template.");
          templateId = 0;
        }
      } while (templateId == 0);

      Svc.SM.UI.ElementWdw.ApplyTemplate(templateId);
    }

    #endregion
  }
}
