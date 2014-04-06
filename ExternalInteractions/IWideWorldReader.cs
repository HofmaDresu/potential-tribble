using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalInteractions
{
    public interface IWideWorldReader
    {
        List<string> GetSeasons();
        List<string> GetScheduleTypes(string season);
        List<Link> GetDivisions(string season, string scheduleType);
        List<Link> GetTeams(string season, string scheduleType, string division);
        List<SoccerSchedule> GetSchedule(string season, string scheduleType, string division, string team);
    }
}
