using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;

namespace Bikroy
{
    public class Browser 
    {
        #region [Data Members]
        public String Url { get; set; }
        private WebHeaderCollection WebHeader { get; set; }
        private NameValueCollection FormData { get; set; }
       // private static CacheDb cache;
        //WebClient webClient;
        //HttpClient httpClient;
        #endregion

        #region [Constructor]
        public Browser()
        {
            //cache = new CacheDb();
            ////httpClient = new HttpClient();
        }

        public Browser(String Url)
        {
            //cache = new Cache();
            ////httpClient = new HttpClient();
            //this.Url = Url;
        }
        #endregion

        public HtmlDocument GetWebRequest()
        {
            HtmlDocument document = new HtmlDocument();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = 180000;
           
                byte[] responseBytes;
                //if (!cache.IsCachedUrl(Url))
                //{
                responseBytes = httpClient.DownloadData(Url);
                //    CacheDb.SaveCache(Url, responseBytes);
                //}
                //else
                //    responseBytes = cache.GetCachedUrl(Url);
                MemoryStream mStream = new MemoryStream(responseBytes);
                document.Load(mStream, Encoding.UTF8);
            }
            catch { }
            return document;
        }

        public HtmlDocument PostRequest(WebHeaderCollection Header, NameValueCollection formData)
        {
            HttpClient httpClient = new HttpClient();

            Crawler crawler = new Crawler();
            byte[] responseBytes;
            crawler.Url = Url;
            //WebClient httpClient = new WebClient();
            if (!ReferenceEquals(Header, null))
            {
                httpClient.Headers = Header;
            }
            if (!ReferenceEquals(formData, null))
            {
                httpClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                responseBytes = httpClient.UploadValues(Url, "POST", formData);
            }
            else
            {
                responseBytes = httpClient.DownloadData(Url);
            }
            string resultAuthTicket = Encoding.UTF8.GetString(responseBytes);
            httpClient.Dispose();
            MemoryStream mStream = new MemoryStream(responseBytes);
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.Load(mStream);
            return document;
        }

        public String AjaxPost(NameValueCollection parameters)
        {
            HttpClient httpClient = new HttpClient();

            Uri uristring = new Uri(Url);
            httpClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            httpClient.Headers["ContentType"] = "application/json";
            List<String> Parameters = new List<String>();
            foreach (String key in parameters.AllKeys)
            {
                Parameters.Add(String.Format("\'{0}\':\'{1}\'", key, parameters[key]));
            }

            string JsonStringParams = "{" + String.Join(",", Parameters) + "}";
            return httpClient.UploadString(Url, JsonStringParams);
        }

        public String AjaxPost()
        {
            HttpClient httpClient = new HttpClient();

            Uri uristring = new Uri(Url);
            httpClient.Headers["Referer"] = "http://bikroy.com/en/computers-accessories-in-bangladesh?page=2";
            httpClient.Headers.Add("Accept", "application/json, text/javascript");
           
           // httpClient.Headers["ContentType"] = "application/json";
            return httpClient.DownloadString(Url);
        }

        public HtmlDocument ParseDocument(String htmlString)
        {
            try
            {
                byte[] htmlByte = Encoding.UTF8.GetBytes(htmlString);
                MemoryStream mStream = new MemoryStream(htmlByte);
                HtmlDocument document = new HtmlDocument();
                document.Load(mStream);
                return document;
            }
            catch
            {
                return null;
            }
        }

        public Dictionary<string, object> parseJson(String JsonText)
        {
            Dictionary<string, object> temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonText);
            return temp;
        }
        //public Dictionary<string, string> parseJson(String JsonText)
        //{
        //    Dictionary<string, string> temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonText);
        //    return temp;
        //}
        public void DownloadFile(String FileName)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DownloadFile(Url, FileName);
        }

        public NameValueCollection GetFormData(HtmlDocument document)
        {
            NameValueCollection formData = new NameValueCollection();
            var inputItems = document.DocumentNode.SelectNodes("//body").Descendants()
                .Where(x => x.Attributes.Contains("name") && x.Attributes.Contains("value"))
                .Select(x => new
                {
                    Name = x.Attributes["name"].Value.ToString(),
                    Value = x.Attributes["value"].Value.ToString()
                }
                );


            foreach (var items in inputItems)
            {
                formData.Add(items.Name, items.Value);
            }
            return formData;
        }
    }
}
