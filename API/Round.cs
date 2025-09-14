using GameCore;
using PlayerRoles;
using RoundRestarting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API
{
    public class Round
    {
        public static TimeSpan ElapsedTime
        {
            get
            {
                return RoundStart.RoundLength;
            }
        }

        public static DateTime StartedTime
        {
            get
            {
                return DateTime.Now - Round.ElapsedTime;
            }
        }

        public static bool IsStarted
        {
            get
            {
                ReferenceHub referenceHub;
                return ReferenceHub.TryGetHostHub(out referenceHub) && referenceHub.characterClassManager.RoundStarted;
            }
        }

        public static bool InProgress
        {
            get
            {
                return !Round.IsEnded && RoundSummary.RoundInProgress();
            }
        }

        public static bool IsEnded
        {
            get
            {
                return RoundSummary.singleton.IsRoundEnded;
            }
        }

        public static bool IsLobby
        {
            get
            {
                return !Round.IsEnded && !Round.IsStarted;
            }
        }

        public static RoundSummary.SumInfo_ClassList LastClassList { get; internal set; }

        public static int ExtraTargetCount
        {
            get
            {
                return RoundSummary.singleton.Network_extraTargets;
            }
            set
            {
                RoundSummary.singleton.Network_extraTargets = value;
            }
        }

        public static bool IsLocked
        {
            get
            {
                return RoundSummary.RoundLock;
            }
            set
            {
                RoundSummary.RoundLock = value;
            }
        }

        public static bool IsLobbyLocked
        {
            get
            {
                return RoundStart.LobbyLock;
            }
            set
            {
                RoundStart.LobbyLock = value;
            }
        }

        public static int EscapedDClasses
        {
            get
            {
                return RoundSummary.EscapedClassD;
            }
        }

        public static int EscapedScientists
        {
            get
            {
                return RoundSummary.EscapedScientists;
            }
        }

        public static int Kills
        {
            get
            {
                return RoundSummary.Kills;
            }
        }

        public static int SurvivingSCPs
        {
            get
            {
                return RoundSummary.SurvivingSCPs;
            }
        }

        public static int KillsByScp
        {
            get
            {
                return RoundSummary.KilledBySCPs;
            }
        }

        public static int ChangedIntoZombies
        {
            get
            {
                return RoundSummary.ChangedIntoZombies;
            }
        }

        public static short LobbyWaitingTime
        {
            get
            {
                return RoundStart.singleton.NetworkTimer;
            }
            set
            {
                RoundStart.singleton.NetworkTimer = value;
            }
        }

        public static ServerStatic.NextRoundAction NextRoundAction
        {
            get
            {
                return ServerStatic.StopNextRound;
            }
            set
            {
                ServerStatic.StopNextRound = value;
            }
        }

        public static int UptimeRounds
        {
            get
            {
                return RoundRestart.UptimeRounds;
            }
        }

        public static IEnumerable<Team> AliveSides
        {
            get
            {
                List<Team> list = new List<Team>(4);
                foreach (Player player in Player.List.Where(x => x.IsAlive))
                {
                    if (!list.Contains(player.Role.Team))
                    {
                        list.Add(player.Role.Team);
                    }
                }
                return list;
            }
        }

        public static void Restart(bool fastRestart = true, bool overrideRestartAction = false, ServerStatic.NextRoundAction restartAction = ServerStatic.NextRoundAction.DoNothing)
        {
            if (overrideRestartAction)
            {
                ServerStatic.StopNextRound = restartAction;
            }
            bool enableFastRestart = CustomNetworkManager.EnableFastRestart;
            CustomNetworkManager.EnableFastRestart = fastRestart;
            RoundRestart.InitiateRoundRestart();
            CustomNetworkManager.EnableFastRestart = enableFastRestart;
        }

        public static void RestartSilently()
        {
            Round.Restart(true, true, ServerStatic.NextRoundAction.DoNothing);
        }

        public static bool EndRound(bool forceEnd = false)
        {
            if (RoundSummary.singleton.KeepRoundOnOne && Player.List.Count < 2 && !forceEnd)
            {
                return false;
            }
            if ((Round.IsStarted && !Round.IsLocked) || forceEnd)
            {
                RoundSummary.singleton.ForceEnd();
                return true;
            }
            return false;
        }

        public static void Start()
        {
            CharacterClassManager.ForceRoundStart();
        }
    }
}
