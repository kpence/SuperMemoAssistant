using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.ApiServer
{
  class ApiServerState
  {
    public static ApiServerState Instance { get; } = new ApiServerState();
    public string ElementInfo { get; set; }
  }
}
