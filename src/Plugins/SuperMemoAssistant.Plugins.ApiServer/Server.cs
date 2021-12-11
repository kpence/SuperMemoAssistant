using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SuperMemoAssistant.Plugins.ApiServer
{
  public class EndPoint
  {
    public Regex Regex { get; set; }
    public Func<string, string> Action { get; set; }

    public bool IsMatch(string Path)
    {
      return Regex.IsMatch(Path);
    }

    public string Run(string req)
    {
      //MessageBox.Show(Regex.ToString());
      Console.WriteLine("End Point Called: "+Regex.ToString());
      return Action.Invoke(req);
    }
  }

  public class HttpServer
  {
    public static HttpServer Instance { get; } = new HttpServer();

    public List<EndPoint> endPoints = new List<EndPoint>();
    public HttpListener listener;
    private string url = "http://+:31337/";
    private string defaultPageData = "";


    public async Task HandleIncomingConnections()
    {
      bool runServer = true;

      while (runServer)
      {
        try
        {
          HttpListenerContext ctx = await listener.GetContextAsync();
          HttpListenerRequest req = ctx.Request;
          HttpListenerResponse resp = ctx.Response;

          string postBody = null;

          if (req.HttpMethod == "POST")
          {
            using var reader = new StreamReader(req.InputStream,
                                                 req.ContentEncoding);
            postBody = reader.ReadToEnd();
          }

          byte[] data = Encoding.UTF8.GetBytes(defaultPageData);

          foreach (var endPoint in endPoints)
          {
            if (endPoint.IsMatch(req.Url.AbsolutePath))
            {
              // todo, maybe i can get capture groups for query string or sth
              data = Encoding.UTF8.GetBytes(endPoint.Run(postBody));
              break;
            }
          }

          resp.ContentType = "application/json";
          resp.ContentEncoding = Encoding.UTF8;
          resp.ContentLength64 = data.LongLength;
          await resp.OutputStream.WriteAsync(data, 0, data.Length);
          resp.Close();
        }
        catch (Exception e)
        {
          MessageBox.Show("Server died: " + e.Message);
        }
      }
    }

    public void Route(string routeRegexString, Func<string,string> action) =>
      endPoints.Add(new EndPoint
    {
      Regex = new Regex(routeRegexString),
      Action = action,
    });

    public static void Create()
    {
      Instance.listener = new HttpListener();
      Instance.listener.Prefixes.Add(Instance.url);
      Instance.listener.Start();

      Task.Run(() => Instance.HandleIncomingConnections());
    }

    public static void Dispose()
    {
      Instance.listener.Close();
    }
  }
}
