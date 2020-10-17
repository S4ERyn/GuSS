using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;

namespace GuSS.Utility.RSSReader
{
    class RssReader2
    {
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

                        retVal = area.Summary.Text;
                    }
                }
                else
                {
                    retVal = "nothing found";
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
