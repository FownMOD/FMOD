using FMOD.API;
using FMOD.API.Items;
using FMOD.Events.Interfaces;
using InventorySystem.Items.Firearms.Modules.Misc;
using LabApi.Events.Arguments.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.Events.EventArgs.Player
{
    public class ShootingEventArgs : IFMODPlayerEvent
    {
        public ShootingEventArgs(InventorySystem.Items.Firearms.Firearm firearm, ref ShotBacktrackData shotBacktrackData)
        {
            this.Firearm = (Firearm)Item.Get(firearm);
            this.Player = this.Firearm.CurrentOwner;
            this.ShotBacktrackData = shotBacktrackData;
        }

        public API.Player Player { get; set; }

        public API.Player ClaimedTarget
        {
            get
            {
                if (!this.ShotBacktrackData.HasPrimaryTarget)
                {
                    return null;
                }
                return API.Player.Get(this.ShotBacktrackData.PrimaryTargetHub);
            }
        }

        public ShotBacktrackData ShotBacktrackData { get; }

        public Vector3 Direction
        {
            get
            {
                return this.Player.Camera.forward;
            }
            set
            {
                this.Player.Camera.forward = value;
            }
        }


        public Item Item
        {
            get
            {
                return this.Firearm;
            }
        }
        public Firearm Firearm;
        public bool IsAllowed { get; set; } = true;
    }
}
