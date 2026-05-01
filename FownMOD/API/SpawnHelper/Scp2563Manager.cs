using Christmas.Scp2536;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace FMOD.API.SpawnHelper
{
    public class Scp2563Manager
    {
        public static void SpawnScp2563(Player player)
        {
            Scp2536Controller.Singleton.GiftController.ServerGrantRandomGift(player.ReferenceHub);

        }
    }
}
