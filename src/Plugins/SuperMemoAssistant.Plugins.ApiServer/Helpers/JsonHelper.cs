using SuperMemoAssistant.Interop.SuperMemo.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SuperMemoAssistant.Plugins.ApiServer.Helpers
{
  class JsonHelper
  {
    public static string JsonFromValue(LearningMode value) =>
      "{\"result\": \"" + value.ToString() + "\"}";

    public static string JsonFromValue(int value) =>
      "{\"result\": \"" + value.ToString() + "\"}";

    public static string JsonFromValue(bool value) =>
      "{\"result\": \"" + (value ? "true" : "false") + "\"}";

    public static string JsonFromValue(string value) =>
      "{\"result\": \"" + HttpUtility.JavaScriptStringEncode(value) + "\"}";
  }
}
