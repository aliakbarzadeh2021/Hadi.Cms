using System;
using System.Linq;
using System.Xml.Linq;

namespace Hadi.Cms.ApplicationService.Services
{
    public class StatisticService
    {
        public int GetAlexaRank(string domain)
        {
            var alexaRank = 0;
            try
            {
                var url = $"http://data.alexa.com/data?cli=10&dat=snbamz&url={domain}";
                var doc = XDocument.Load(url);
                var rank = doc.Descendants("POPULARITY")
                    .Select(node => node.Attribute("TEXT")?.Value)
                    .FirstOrDefault();
                if (!int.TryParse(rank, out alexaRank))
                    alexaRank = -1;
            }
            catch (Exception e)
            {
                return -1;
            }

            return alexaRank;
        }

        public int GetAlexaRankInCountry(string domain)
        {
            var alexaRank = 0;
            try
            {
                var url = $"http://data.alexa.com/data?cli=10&dat=snbamz&url={domain}";
                var doc = XDocument.Load(url);
                var rank = doc.Descendants("COUNTRY")
                    .Select(node => node.Attribute("RANK")?.Value)
                    .FirstOrDefault();
                if (!int.TryParse(rank, out alexaRank))
                    alexaRank = -1;
            }
            catch (Exception e)
            {
                return -1;
            }

            return alexaRank;
        }
    }
}
