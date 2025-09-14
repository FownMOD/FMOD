using CustomPlayerEffects;
using PlayerRoles;
using Respawning;
using Respawning.Waves;
using Respawning.Waves.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API
{
    public class Respawn
    {
        public static List<SpawnableWaveBase> SpawnableWaveBases = new List<SpawnableWaveBase>();
        public static WaveQueueState State => WaveManager.State;
        public static Faction NextKnowRespawnTeam { get; set; }
        public static int GetTokens(Faction faction)
        {
            SpawnableWaveBase spawnableWaveBase = GetFaction(faction);
            ILimitedWave limitedWave = spawnableWaveBase as ILimitedWave;
            if (limitedWave != null)
            {
                return limitedWave.RespawnTokens;
            }
            return 0;
        }
        public static bool SetTokens(Faction faction, int ammo)
        {
            int Tokens = GetTokens(faction);
            Tokens = ammo;
            return true;
        }
        public static bool RemoveTokens(Faction faction, int amount)
        {
            int Tokens = GetTokens(faction);
            Tokens = Math.Max(0, Tokens - amount);
            return false;
        }
        public static SpawnableWaveBase GetFaction(Faction faction)
        {
            WaveManager.TryGet(faction, out SpawnableWaveBase spawnWave);
            return spawnWave;
        }
        public static float ProtectionTime
        {
            get
            {
                return SpawnProtected.SpawnDuration;
            }
            set
            {
                SpawnProtected.SpawnDuration = value;
            }
        }
        public static bool ProtectedCanShoot
        {
            get
            {
                return SpawnProtected.CanShoot;
            }
            set
            {
                SpawnProtected.CanShoot = value;
            }
        }
        public static bool IsSpawning
        {
            get
            {
                return WaveManager.State == WaveQueueState.WaveSpawning;
            }
        }
        public static void SpawnWave(SpawnableWaveBase spawnableWaveBase)
        {
            if (spawnableWaveBase.TargetFaction == Faction.SCP)
            {
                NextKnowRespawnTeam = Faction.SCP;
            }
            else if (spawnableWaveBase.TargetFaction == Faction.FoundationStaff)
            {
                NextKnowRespawnTeam = Faction.FoundationStaff;
            }
            else if (spawnableWaveBase.TargetFaction == Faction.FoundationEnemy)
            {
                NextKnowRespawnTeam = Faction.FoundationEnemy;
            }
            else if (spawnableWaveBase.TargetFaction == Faction.Unclassified)
            {
                NextKnowRespawnTeam = Faction.Unclassified;
            }
            else
            {
                NextKnowRespawnTeam = Faction.Flamingos;
            }
            WaveManager.Spawn(spawnableWaveBase);
        }
        public void SpawnWave(Faction faction)
        {
            var Spawnbase = GetFaction(faction);
            if (Spawnbase != null)
            {
                SpawnWave(Spawnbase);
            }
        }
        public static void RestartWaves(Faction faction)
        {
            NextKnowRespawnTeam = Faction.Unclassified;
            var Base = GetFaction(faction);
            Base.Destroy();
            WaveManager.Waves.Clear();
        }
    }
}
