using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalInteractions
{
    public interface IWideWorldReader
    {
        string GetSeasons();
        string GetScheduleTypes(string season);
        string GetDivisions(string season, string scheduleType);
        string GetTeams(string season, string scheduleType, string division);
        List<SoccerSchedule> GetSchedule(string season, string scheduleType, string division, string team);
    }
}
