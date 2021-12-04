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

    public bool IsMatch(string Path)
    {
      return Regex.IsMatch(Path);
    }

    public string Run(string req)
    {
      return "{\"test\":\"value\"}";
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

      // While a user hasn't visited the `shutdown` url, keep on handling requests
      while (runServer)
      {
        // Will wait here until we hear from a connection
        HttpListenerContext ctx = await listener.GetContextAsync();

        // Peel out the requests and response objects
        HttpListenerRequest req = ctx.Request;
        HttpListenerResponse resp = ctx.Response;

        string postBody;
        using (var reader = new StreamReader(req.InputStream,
                                             req.ContentEncoding))
        {
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
    }

    public void Route(string routeRegexString) =>
      endPoints.Add(new EndPoint
    {
      Regex = new Regex(routeRegexString)
    });

    public static void Create()
    {
      // Create a Http server and start listening for incoming connections
      Instance.listener = new HttpListener();
      Instance.listener.Prefixes.Add(Instance.url);
      Instance.listener.Start();
      Instance.Route("/test");
      //Console.WriteLine("Listening for connections on {0}", Instance.url);

      // Handle requests
      Task.Run(() => Instance.HandleIncomingConnections());
    }

    public static void Dispose()
    {
      // Close the listener
      Instance.listener.Close();
    }
  }
}
