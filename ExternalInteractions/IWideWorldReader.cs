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
        List<Link> GetTeams(Link division);
        List<SoccerGame> GetSchedule(Link team);
    }
}
