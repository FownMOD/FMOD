using CustomPlayerEffects;
using NorthwoodLib.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API
{
    public class Server
    {
        /// <summary>
        /// 本地玩家
        /// </summary>
        public static Player Host { get; internal set; }
        /// <summary>
        /// 玩家数量
        /// </summary>
        public static int PlayerCount => Player.List.Count;
        /// <summary>
        /// 最大玩家数量
        /// </summary>
        public static int MaxPlayerCount => CustomNetworkManager.slots;
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public static int Port => ServerStatic.ServerPort;
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string IP => ServerConsole.Ip;
        /// <summary>
        /// 服务器在线的远程管理员
        /// </summary>
        public static List<Player> RemoteAdmins => Player.List.Where(x => x.RemoteAdminAccess).ToList();
        /// <summary>
        /// 假人数量
        /// </summary>
        public static List<Player> DummyCount => Player.List.Where(x => x.IsDummy).ToList();
        /// <summary>
        /// 服务器的Password
        /// </summary>
        public static string Passcode => ServerConsole.Password;
        /// <summary>
        /// 服务器的列表key
        /// </summary>
        public static string PublicKey => ServerConsole.PublicKey.ToString();
        /// <summary>
        /// 玩家是否被Ban了
        /// </summary>
        /// <param name="value">Steam64Id</param>
        /// <returns></returns>
        public static bool IsPlayerBanned(string value)
        {
            return !string.IsNullOrEmpty(value) && (value.Contains("@") ? BanHandler.GetBan(value, BanHandler.BanType.UserId) : BanHandler.GetBan(value, BanHandler.BanType.IP)) != null;
        }
        /// <summary>
        /// 增加额外玩家位置
        /// </summary>
        /// <param name="UserId">Steam64ID</param>
        public static void AddReservedSlotPlayer(string UserId)
        {
            ReservedSlot.Users.Add(UserId);
            ReservedSlot.Reload();
        }
        /// <summary>
        /// 确定玩家是否具有额外位置
        /// </summary>
        /// <param name="UserId">Steam64ID</param>
        public static bool IsReservedSlotUser(string UserId)
        {
            if (!ReservedSlot.HasReservedSlot(UserId))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除额外玩家位置
        /// </summary>
        /// <param name="UserId">Steam64ID</param>
        public static void RemoveReservedSlotUser(string UserId)
        {
            ReservedSlot.Users.Remove(UserId);
            ReservedSlot.Reload();
        }
        /// <summary>
        /// 解除IP封禁
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool UnbanIpAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || !Server.IsPlayerBanned(ipAddress))
            {
                return false;
            }
            BanHandler.RemoveBan(ipAddress, BanHandler.BanType.IP, false);
            return true;
        }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public static string ServerName => ServerConsole.ServerName;
        /// <summary>
        /// 解放玩家
        /// </summary>
        /// <param name="userId">Steam64ID</param>
        /// <returns></returns>
        public static bool UnbanUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId) || !Server.IsPlayerBanned(userId))
            {
                return false;
            }
            BanHandler.RemoveBan(userId, BanHandler.BanType.UserId, false);
            return true;
        }
        /// <summary>
        /// 获取被封禁的玩家
        /// </summary>
        /// <param name="banType">封禁程度</param>
        /// <returns></returns>
        public static List<BanDetails> GetAllBannedPlayers(BanHandler.BanType banType)
        {
            return BanHandler.GetBans(banType);
        }
        /// <summary>
        /// 获取被封禁的玩家
        /// </summary>
        public static List<BanDetails> GetAllBannedPlayers()
        {
            List<BanDetails> list = ListPool<BanDetails>.Shared.Rent();
            list.AddRange(BanHandler.GetBans(BanHandler.BanType.UserId));
            list.AddRange(BanHandler.GetBans(BanHandler.BanType.IP));
            return list;
        }
        /// <summary>
        /// 获取服务器最大TPS
        /// </summary>
        public static short MaxTps
        {
            get
            {
                return ServerStatic.ServerTickrate;
            }
            set
            {
                ServerStatic.ServerTickrate = value;
            }
        }
        /// <summary>
        /// 服务器的刷新保护时间
        /// </summary>
        public static float SpawnProtectDuration
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
        /// <summary>
        /// 服务器的Tps
        /// </summary>
        public static double Tps
        {
            get
            {
                return Math.Round((double)(1f / Time.smoothDeltaTime));
            }
        }
        /// <summary>
        /// 服务器友伤
        /// </summary>
        public static bool FriendlyFire
        {
            get
            {
                return ServerConsole.FriendlyFire;
            }
            set
            {
                ServerConsole.FriendlyFire = value;
            }
        }
        /// <summary>
        /// 服务器玩家列表名称
        /// </summary>
        public static string PlayerListName
        {
            get
            {
                return PlayerList.Title.Value;
            }
            set
            {
                PlayerList.Title.Value = value;
            }
        }

        /// <summary>
        /// 运行命令
        /// </summary>
        /// <param name="Command">命令</param>
        public static string RunCommand(string command, CommandSender sender = null) =>
        ServerConsole.EnterCommand(command, sender);
        /// <summary>
        /// 重启服务器
        /// </summary>
        public static void Restart()
        {
            RunCommand("restart");
        }
        /// <summary>
        /// 服务器下回合重启
        /// </summary>
        public static void RestartNextround()
        {
            RunCommand("restartnextround");
        }
    }
}
