using FMOD.API.SpawnHelper;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Roles
{
    public abstract class CustomRole
    {
        public static Dictionary<uint, CustomRole> List = new Dictionary<uint, CustomRole>();
        public static Dictionary<Player, CustomRole> RoleByPlayer = new Dictionary<Player, CustomRole>();
        public abstract uint Id { get; set; }
        public abstract string Name { get; set; }
        public abstract float MaxHealth { get; set; }
        public abstract RoleTypeId RoleType { get; set; }
        public Player SpawnedPlayer { get; set; }
        public abstract StaticSpawnPosition SpawnPosition { get; set; }
        public abstract string SpawnHint { get; set; }
        public abstract List<ItemType> ItemTypes { get; set; }
        public abstract void Regiter();
        public abstract void UnRegister();
        public void Spawn(Player player)
        {
            if (RoleByPlayer.ContainsKey(player))
            {
                RoleByPlayer[player] = this;
            }
            RoleByPlayer.Add(player, this);
            this.SpawnedPlayer = player;
            player.SetRole(RoleType);
            if(SpawnPosition is SpawnDoorStaticPosition doorpos)
            {
                player.Position = doorpos.Position;
            }
            if(SpawnPosition is SpawnRoleStaticPosition rp)
            {
                player.Position = rp.Position;
            }
            player.MaxHealth = MaxHealth;
            player.SendHint(SpawnHint, 30);
            player.AddItem(ItemTypes);
        }
        public bool Is(Player player)
        {
            if(!RoleByPlayer.ContainsKey(player))
                return false;
            if (RoleByPlayer[player] != this)
                return false;
            return true;
        }
        public void Destroy()
        {
            RoleByPlayer.Remove(SpawnedPlayer);
        }
    }
}
