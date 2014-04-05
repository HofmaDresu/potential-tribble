using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalInteractions
{
    public class WideWorldReader : IWideWorldReader
    {
        private string SeasonsUrl = "http://secure.hammerweb.net/wws_membership/SchedulesScoresDisplay.asp";

        public string GetSeasons()
        {
            throw new NotImplementedException();
        }

        public string GetScheduleTypes(string season)
        {
            throw new NotImplementedException();
        }

        public string GetDivisions(string season, string scheduleType)
        {
            throw new NotImplementedException();
        }

        public string GetTeams(string season, string scheduleType, string division)
        {
            throw new NotImplementedException();
        }

        public List<SoccerSchedule> GetSchedule(string season, string scheduleType, string division, string team)
        {
            throw new NotImplementedException();
        }
    }
}
