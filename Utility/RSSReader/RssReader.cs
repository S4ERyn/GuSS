using System;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace GuSS.Utility.RSSReader
{
    class RssReader
    {

        public string ReadWeb(string uri)
        {
            string strResult = "";

            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress.AbsoluteUri);
            request.Headers.Add("User-Agent", "MyApplication/v1.0 (http://foo.bar.baz; foo@bar.baz)");
            var httpResponse = httpClient.SendAsync(request).Result;

            WebResponse objResponse;
            WebRequest objRequest = System.Net.HttpWebRequest.Create(uri);



            objResponse = objRequest.GetResponse();
         

            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                strResult = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }

            // Display results to a webpage
            return strResult;
        }

        public string ReadXML(string uri)
        {
            XmlDocument doc = new XmlDocument();

            //string stationName = @"http://w1.weather.gov/xml/current_obs/PASC.xml";
            string stationName = @"https://w1.weather.gov/xml/current_obs/KHFD.xml";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(stationName);

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "MyApplication/v1.0 (http://foo.bar.baz; foo@bar.baz)";
            request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            doc.Load(resStream);
            try
            {

                XmlNodeList list = doc.GetElementsByTagName("temp_f");
                if (list.Count > 0)
                {
                    float fTemperature = float.Parse(list.Item(0).InnerText);

                }
            }
            catch (Exception x)
            {
                //this is the error
                Console.WriteLine(x.Message);
            }

            XmlNodeList list2 = doc.GetElementsByTagName("current_observation");

            string weather = list2.Item(0).InnerText;
            weather = weather.Substring(weather.IndexOf("xml_logo.gif")+12, weather.Length - weather.IndexOf("xml_logo.gif") - 12);
            weather = weather.Substring(0, weather.IndexOf("http://forecast.weather.gov"));
            weather = weather.Remove(weather.IndexOf("http://weather.gov"), 18);

            return weather;
        }

        public string ReadRSS(string uri)
        {
            string retVal = string.Empty;
            try
            {

                XmlReader r = XmlReader.Create(uri);
                SyndicationFeed areas = SyndicationFeed.Load(r);
                r.Close();
                if (areas.Title.Text != null)
                {
                    foreach (SyndicationItem area in areas.Items)
                    {
                        //return Regex.Match(r, @"<content:encoded>").Groups[1].Value;

                        retVal= area.Summary.Text;
                    }
                }
                else
                {
                    retVal="nothing found";
                }
            }
            catch (Exception ex)
            {
                //show ex.message
            }
            return retVal;
            
           
        }

    }
}


