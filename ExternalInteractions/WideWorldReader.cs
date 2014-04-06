using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExternalInteractions
{
    public class WideWorldReader : IWideWorldReader
    {
        private string SeasonsUrl = "http://secure.hammerweb.net/wws_membership/SchedulesScoresDisplay.asp";
        private string DivisionsPageHTML;
        private string SeasonDesignation = "Season:";
        private string ScheduleTypeStyle = "style128";

        public List<string> GetSeasons()
        {
            var seasonsList = new List<string>();
            DivisionsPageHTML = new StreamReader(WebRequest.Create(SeasonsUrl).GetResponse().GetResponseStream()).ReadToEnd();
            var responseStreamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(DivisionsPageHTML)));
            while (!responseStreamReader.EndOfStream)
            {
                var responseLine = responseStreamReader.ReadLine();
                if (responseLine.Contains(SeasonDesignation))
                {
                    seasonsList.Add(responseLine.Substring(responseLine.LastIndexOf('>')+1));
                }
            }

            return seasonsList;
        }

        public List<string> GetScheduleTypes(string season)
        {
            var trimBeginningLineText = ScheduleTypeStyle + "\">";
            var scheduleTypesList = new List<string>();
            if (string.IsNullOrEmpty(DivisionsPageHTML))
            {
                GetSeasons();
            }

            var responseStreamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(DivisionsPageHTML)));

            var addScheduleTypeForSeason = false;
            while (!responseStreamReader.EndOfStream)
            {
                var responseLine = responseStreamReader.ReadLine();
                bool currentLineIsRequestedSeason = responseLine.Contains(season);
                if (responseLine.Contains(SeasonDesignation))
                {
                    addScheduleTypeForSeason = responseLine.Contains(season);
                }
                if (addScheduleTypeForSeason && responseLine.Contains(ScheduleTypeStyle))
                {
                    var beginningTrimmedLine = responseLine.Substring(responseLine.IndexOf(trimBeginningLineText) + trimBeginningLineText.Length);
                    var scheduleType = beginningTrimmedLine.Substring(0, beginningTrimmedLine.IndexOf('<'));
                    scheduleTypesList.Add(scheduleType);
                }
            }
            return scheduleTypesList;
        }

        public List<Link> GetDivisions(string season, string scheduleType)
        {
            throw new NotImplementedException();
        }

        public List<Link> GetTeams(string season, string scheduleType, string division)
        {
            throw new NotImplementedException();
        }

        public List<SoccerSchedule> GetSchedule(string season, string scheduleType, string division, string team)
        {
            throw new NotImplementedException();
        }
    }
}
