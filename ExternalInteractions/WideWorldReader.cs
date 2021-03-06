﻿using System;
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
        private const string baseUrl = "http://secure.hammerweb.net/wws_membership/";
        private const string SeasonsPage = "SchedulesScoresDisplay.asp";
        private string DivisionsPageHTML;
        private const string SeasonDesignation = "Season:";
        private const string ScheduleTypeStyle = "style128";
        private const string trimHrefLineText = "href=\'";

        public List<string> GetSeasons()
        {
            var seasonsList = new List<string>();
            DivisionsPageHTML = new StreamReader(WebRequest.Create(baseUrl + SeasonsPage).GetResponse().GetResponseStream()).ReadToEnd();
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
            var divisions = new List<Link>();
            if (string.IsNullOrEmpty(DivisionsPageHTML))
            {
                GetSeasons();
            }

            var responseStreamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(DivisionsPageHTML)));

            bool correctSeasonFound = false;
            bool correctScheduleTypeFound = false;
            while (!responseStreamReader.EndOfStream)
            {
                var responseLine = responseStreamReader.ReadLine();
                
                correctSeasonFound |= responseLine.Contains(season);
                correctScheduleTypeFound |= (correctSeasonFound && responseLine.Contains(scheduleType));

                
                if (correctSeasonFound && correctScheduleTypeFound)
                {
                    if (responseLine.Contains("</table>"))
                    {
                        break;
                    }

                    foreach (var linkString in responseLine.Split(new string[] { "</a>" }, StringSplitOptions.RemoveEmptyEntries).Where(s => s.Contains(trimHrefLineText)))
                    {
                        var trimmedBeginningPage = linkString.Substring(linkString.IndexOf(trimHrefLineText) + trimHrefLineText.Length);
                        var trimmedBeginningPageName = linkString.Substring(linkString.LastIndexOf('>')+1);


                        divisions.Add(new Link
                        {
                            Url = trimmedBeginningPage.Substring(0, trimmedBeginningPage.IndexOf("'>")),
                            Name = trimmedBeginningPageName
                        });
                    }
                }
            }

            return divisions;
        }

        public List<Link> GetTeams(Link division)
        {
            var teams = new List<Link>();
            var teamTableStyle = "style132";
            var TeamsPageReader = new StreamReader(WebRequest.Create(baseUrl + division.Url).GetResponse().GetResponseStream());

            while (!TeamsPageReader.EndOfStream)
            {
                var responseLine = TeamsPageReader.ReadLine();
                if (responseLine.Contains(teamTableStyle))
                {
                    foreach (var row in responseLine.Split(new string[] {"</tr>"}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var linkString = row.Split(new string[] {"</a>"}, StringSplitOptions.RemoveEmptyEntries)[1];

                        var trimmedBeginningPage = linkString.Substring(linkString.IndexOf(trimHrefLineText) + trimHrefLineText.Length);
                        var trimmedBeginningPageName = linkString.Substring(linkString.LastIndexOf('>')+1);


                        teams.Add(new Link
                        {
                            Url = trimmedBeginningPage.Substring(0, trimmedBeginningPage.IndexOf("'>")),
                            Name = trimmedBeginningPageName
                        });
                    }
                }
            }


            return teams;
        }

        public List<SoccerGame> GetSchedule(Link team)
        {
            var schedule = new List<SoccerGame>();
            var schedulePageReader = new StreamReader(WebRequest.Create(baseUrl + team.Url).GetResponse().GetResponseStream());
            var teamId = string.Empty;
            var teamSplitter = "Team: ";

            var scheduleLineSeen = false;
            while (!schedulePageReader.EndOfStream)
            {
                var responseLine = schedulePageReader.ReadLine();

                if (responseLine.Contains(teamSplitter))
                {
                    var trimmedBeginningTeamId = responseLine.Substring(responseLine.IndexOf(teamSplitter) + teamSplitter.Length);
                    teamId = trimmedBeginningTeamId.Substring(0, trimmedBeginningTeamId.IndexOf("-"));
                }

                scheduleLineSeen |= responseLine.Contains("Schedules");

                if (scheduleLineSeen && responseLine.Contains("style3"))
                {
                    foreach (var row in responseLine.Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var cells = row.Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                        //Date: cell 0
                        //Time: cell 2
                        //Field: cell 3
                        //Home: cell 4
                        var dateString = cells[0].Split('>').Last();
                        var timeString = cells[2].Split(new string[] { "<span class='style3'>" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('<').First();
                        var field = cells[3].Split(new string[] { "<span class='style3'>" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('<').First();
                        var isHome = (cells[4].Split(new string[] { "<span class='style3'>" }, StringSplitOptions.RemoveEmptyEntries).Last().Split('<').First() == teamId);
                        schedule.Add(new SoccerGame
                        {
                            Field = field,
                            HomeTeam = isHome,
                            Time = Convert.ToDateTime(String.Join(" ", dateString, timeString))
                        });
                    }
                }
            }
            return schedule;
        }
    }
}
