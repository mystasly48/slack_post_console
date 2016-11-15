using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace slack_post_console {
  public class Program {
    public static void Main() {
      var url = ReadLine("URL");
      var username = ReadLine("Username");
      var channel = ReadLine("Channel");
      var text = ReadLine("Text");
      SlackClient client = new SlackClient(url);
      client.PostMessage(channel: channel, username: username, text: text);
    }

    public static string ReadLine(string text) {
      Console.Write(text + ": ");
      return Console.ReadLine();
    }
  }

  public class SlackClient {
    private readonly Uri _uri;
    private readonly Encoding _encoding = new UTF8Encoding();

    public SlackClient(string urlWithAccessToken) {
      _uri = new Uri(urlWithAccessToken);
    }

    public void PostMessage(string text, string username = null, string channel = null) {
      Payload payload = new Payload() { Channel = channel, Username = username, Text = text };
      PostMessage(payload);
    }

    public void PostMessage(Payload payload) {
      string payloadJson = JsonConvert.SerializeObject(payload);
      payloadJson.Log(); // {"channel":"random","username":"dick","text":"suck my dick"}
      using (WebClient client = new WebClient()) {
        NameValueCollection data = new NameValueCollection();
        data["payload"] = payloadJson;
        data.ToString().Log(); // System.Collections.Specialized.NameValueCollection
        byte[] response = client.UploadValues(_uri, "POST", data);
        response.ToString().Log(); // System.Byte[]
        string responseText = _encoding.GetString(response);
        responseText.Log(); // ok
      }
    }
  }

  public class Payload {
    [JsonProperty("channel")]
    public string Channel { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
  }

  public static class Extention {
    public static void Log(this string str) {
      Console.WriteLine(str);
    }
  }
}