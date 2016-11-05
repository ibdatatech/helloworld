using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using RestSharp;
using System.IO;
using System.Globalization;
using System.Diagnostics;

public class Code2040
{
    public Code2040()
    {
    }

    string token = "49fef78e415228786a594d0390a5f41e";

    //Step 1
    public string Registration()
    {
        var url = "http://challenge.code2040.org/api/register";
        Param product = new Param();
        product.token = token;
        product.github = "https://github.com/ibdatatech/helloworld";
        var json_param = JsonConvert.SerializeObject(product);
        var getString = HttpRestClient("POST", true, json_param, url, "JSON");

        return getString;
    }

    //Step 2
    public string Reverse()
    {
        var url = "http://challenge.code2040.org/api/reverse";
        Param1 product = new Param1();
        product.token = token;
        var json_param = JsonConvert.SerializeObject(product);
        var getString = HttpRestClient("POST", true, json_param, url, "JSON");

        var index = getString.Length;
        string[] result = new string[index];
        char[] gtS = getString.ToCharArray();

        foreach (var item in gtS)
        {
            index--;
            result[index] = item.ToString(); 
        }

        string final = string.Concat(result);

        var url_Val = "http://challenge.code2040.org/api/reverse/validate";
        var validateJson = "{\"token\": \"" + token + "\", \"string\": \"" + final + "\"}";
        var validateString = HttpRestClient("POST", true, validateJson, url_Val, "JSON");

        return validateString;
    }

    //Step 3
    public string NeedleInHaystake()
    {
        var url = "http://challenge.code2040.org/api/haystack";
        Param1 product = new Param1();
        product.token = token;
        var json_param = JsonConvert.SerializeObject(product);
        var getString = HttpRestClient("POST", true, json_param, url, "JSON");

        JToken jToken = JObject.Parse(getString);
        var hStack = jToken.SelectToken("haystack").ToArray();
        var needle = jToken.SelectToken("needle").ToString();
        var counter = 0;

        foreach (var item in hStack)
        {
            if (needle == item.ToString())
            {
                break;
            }
            counter++;
        }

        var url_Val = "http://challenge.code2040.org/api/haystack/validate";
        Param2 product1 = new Param2();
        product1.token = token;
        product1.needle = counter;
        var jValPar = JsonConvert.SerializeObject(product1);
        var validateString = HttpRestClient("POST", true, jValPar, url_Val, "JSON");

        return validateString;
    }

    //Step 4
    public string PrefixArray()
    {
        var url = "http://challenge.code2040.org/api/prefix";
        Param1 product = new Param1();
        product.token = token;
        var json_param = JsonConvert.SerializeObject(product);
        var getString = HttpRestClient("POST", true, json_param, url, "JSON");

        JToken jToken = JObject.Parse(getString);
        var array = jToken.SelectToken("array").ToArray();
        var prefix = jToken.SelectToken("prefix").ToString();
        StringBuilder strB = new StringBuilder();

        foreach (var item in array)
        {
            var itemConv = item.ToString();
            if (!itemConv.StartsWith(prefix))
            {
                strB = strB.Append(itemConv);
                strB = strB.Append("~");
            }
        }

        var url_Val = "http://challenge.code2040.org/api/prefix/validate";
        Param3 product1 = new Param3();
        product1.token = token;
        char[] split = new char[1];
        split[0] = '~';
        product1.array = strB.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);
        var jValPar = JsonConvert.SerializeObject(product1);
        var validateString = HttpRestClient("POST", true, jValPar, url_Val, "JSON");

        return validateString;
    }

    //Step 5 -  ISO 8601 datestamp
    public string TimeGame()
    {
        var url = "http://challenge.code2040.org/api/dating";
        Param1 product = new Param1();
        product.token = token;
        var json_param = JsonConvert.SerializeObject(product);
        var getString = HttpRestClient("POST", true, json_param, url, "JSON");

        JToken jToken = JObject.Parse(getString);
        var datestamp = jToken.SelectToken("datestamp").ToString();
        var interval = Convert.ToDouble(jToken.SelectToken("interval").ToString());
        DateTime dt = DateTime.Parse(datestamp, null, System.Globalization.DateTimeStyles.RoundtripKind);
        var dt1 = dt.AddSeconds(interval).ToString("yyyy-MM-ddTHH:mm:ssZ");

        var url_Val = "http://challenge.code2040.org/api/dating/validate";
        Param4 product1 = new Param4();
        product1.token = token;
        product1.datestamp = dt1;
        var jValPar = JsonConvert.SerializeObject(product1);
        var validateString = HttpRestClient("POST", true, jValPar, url_Val, "JSON");

        return validateString;
    }

    public string HttpRestClient(string method, bool useParam, string parameter, string url, string requestFormat)
    {
        try
        {
            var client = new RestClient(url);
            var request = new RestRequest();

            switch (method.ToUpper())
            {
                case "POST":
                    request = new RestRequest(Method.POST);
                    break;
                case "GET":
                    request = new RestRequest(Method.GET);
                    break;
                case "DELETE":
                    request = new RestRequest(Method.DELETE);
                    break;
                default:
                    break;
            }

            if (useParam)
            {
                switch (requestFormat.ToUpper())
                {
                    case "XML":
                        request.AddParameter("application/xml", parameter, ParameterType.RequestBody);
                        break;
                    case "JSON":
                        request.AddParameter("application/json", parameter, ParameterType.RequestBody);
                        break;
                    default:
                        break;
                }
            }

            var restResponse = client.Execute(request);

            return restResponse.Content;
        }
        catch (Exception ex)
        {
            Log("HttpRestClient", url, ex.Message);
            return "Failed. Check log for more details.";
        }
    }

    public static void Log(string method, string uniqueRef, string message)
    {
        try
        {
            string path = "~/Error/Activity Log ~ " + DateTime.Today.ToString("dd-MM-yy") + ".txt";
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
            }
            using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                w.WriteLine("\r\nLog Entry:");
                w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                w.WriteLine("{0}", method);
                w.WriteLine("{0}", uniqueRef);
                w.WriteLine("{0}", System.Web.HttpContext.Current.Request.Url.ToString());
                w.WriteLine("{0}", "MESSAGE:");
                w.WriteLine(message);
                w.WriteLine("_____________________________________________________________________________________________");
                w.Flush();
                w.Close();
            }
        }
        catch (Exception ex)
        {
            EventLog eventLog1 = new EventLog();
            eventLog1.Source = "Code2040";
            eventLog1.WriteEntry("Error in: " + System.Web.HttpContext.Current.Request.Url.ToString());
            eventLog1.WriteEntry(ex.Message, EventLogEntryType.Information);
        }
    }
}

public class Param
{
    public string token;
    public string github;
}

public class Param1
{
    public string token;
}

public class Param2
{
    public string token;
    public long needle;
}

public class Param3
{
    public string token;
    public string [] array;
}

public class Param4
{
    public string token;
    public string datestamp;
}

