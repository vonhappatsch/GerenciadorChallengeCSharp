using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Exceptions;
using Source;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        private Dictionary<long, Team> teams = new Dictionary<long, Team>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();

        public SoccerTeamsManager()
        {
            teams = new Dictionary<long, Team>();
            players = new Dictionary<long, Player>();
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            Team team = new Team();
            team.Id = id;
            team.Name = name;
            team.CreateDate = createDate;
            team.MainShirtColor = mainShirtColor;
            team.SecondaryShirtColor = secondaryShirtColor;

            if (!teams.ContainsKey(id))
            {
                teams.Add(id, team);
            }
            else
            {
                throw new UniqueIdentifierException();
            }
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            Player player = new Player();
            player.Id = id;
            player.TeamId = teamId;
            player.Name = name;
            player.BirthDate = birthDate;
            player.SkillLevel = skillLevel;
            player.Salary = salary;

            if (!teams.ContainsKey(teamId)) throw new TeamNotFoundException();

            if (!players.ContainsKey(id))
            {
                players.Add(id, player);
            }
            else
            {
                throw new UniqueIdentifierException();
            }
        }

        private Player GetPlayer(long playerId)
        {
            Player player = new Player();

            if (!players.TryGetValue(playerId, out player)) throw new PlayerNotFoundException();

            return player;
        }


        public void SetCaptain(long playerId)
        {
            Player player = GetPlayer(playerId);
            var captain = (teams[player.TeamId].CaptainId = playerId);


            if (captain.HasValue)
            {
                captain = 0;
                captain = playerId;
            }
            else if (!captain.HasValue)
            {
                captain = playerId;
            }

            if (!players.TryGetValue(playerId, out player)) throw new PlayerNotFoundException();
        }

        private Team GetTeam(long teamId)
        {
            Team team = new Team();

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();

            return team;
        }

        public long GetTeamCaptain(long teamId)
        {
            Team team = GetTeam(teamId);
            long result = 0;

            if (team.CaptainId.HasValue)
            {
                result = team.CaptainId.Value;
            }
            else
            {
                throw new CaptainNotFoundException();
            }

            return result;
        }

        public string GetPlayerName(long playerId)
        {
            Player player = GetPlayer(playerId);

            if (!players.TryGetValue(playerId, out player)) throw new PlayerNotFoundException();

            return player.Name;
        }

        public string GetTeamName(long teamId)
        {
            Team team = new Team();

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();

            return team.Name;
        }

        private IEnumerable<Player> GetPlayersOnTeam(long teamId)
        {
            return players.Values.Where(p => p.TeamId == teamId);
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            Team team = GetTeam(teamId);

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();

            return GetPlayersOnTeam(teamId).
                Select(p => p.Id).
                OrderBy(p => p).
                ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            Team team = new Team();

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();

            return GetPlayersOnTeam(teamId).
                OrderByDescending(p => p.SkillLevel).
                ThenBy(p => p.Id).
                Select(p => p.Id).
                First();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            Team team = new Team();

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();

            return GetPlayersOnTeam(teamId).
                OrderBy(p => p.BirthDate).
                ThenBy(p => p.Id).
                Select(p => p.Id).First();
        }

        public List<long> GetTeams()
        {
            return teams.Keys.OrderBy(t => t).ToList();
        }

            public long GetHigherSalaryPlayer(long teamId)
        {
            Team team = new Team();

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();

            return GetPlayersOnTeam(teamId).
                OrderByDescending(p => p.Salary).
                ThenBy(p => p.Id).
                Select(p => p.Id).
                First();
        }

        public decimal GetPlayerSalary(long playerId)
        {
            Player player = new Player();

            if (!players.TryGetValue(playerId, out player)) throw new PlayerNotFoundException();

            return player.Salary;
        }

        public List<long> GetTopPlayers(int top)
        {
            Player playerReg = new Player();

            if (!players.TryGetValue(top, out playerReg)) return new List<long>();

            List<long> result = new List<long>();
            result = players.Values.
                OrderByDescending(p => p.SkillLevel).
                ThenBy(p => p.Id).Take(top).Select(p => p.Id).ToList();

            return result;
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            Team team = new Team();

            if (!teams.TryGetValue(teamId, out team)) throw new TeamNotFoundException();
            if (!teams.TryGetValue(visitorTeamId, out team)) throw new TeamNotFoundException();

            if (GetTeam(teamId).MainShirtColor == GetTeam(visitorTeamId).MainShirtColor)
            {
                return GetTeam(visitorTeamId).SecondaryShirtColor;
            }
            else
            {
                return GetTeam(visitorTeamId).MainShirtColor;
            }
        }

    }
}
