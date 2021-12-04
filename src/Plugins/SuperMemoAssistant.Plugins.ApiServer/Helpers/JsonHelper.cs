using SuperMemoAssistant.Interop.SuperMemo.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.ApiServer.Helpers
{
  class JsonHelper
  {
    public static string JsonFromValue(LearningMode value) =>
      "{\"Result\": \"" + value.ToString() + "\"}";

    public static string JsonFromValue(int value) =>
      "{\"Result\": \"" + value.ToString() + "\"}";

    public static string JsonFromValue(bool value) =>
      "{\"Result\": \"" + (value ? "true" : "false") + "\"}";
  }
}
