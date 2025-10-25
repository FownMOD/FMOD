using InventorySystem.Items;
using InventorySystem.Items.Usables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Items
{
    public class UseItem : Item
    {
        public UseItem(UsableItem usable)
        {
            Base = usable;
        }
        public new UsableItem Base { get; }
        public bool IsUsing => Base.IsUsing;
        public float UseTime
        {
            get => Base.UseTime;
            set => Base.UseTime = value;
        }
        public float MaxCancellableTime
        {
            get => Base.MaxCancellableTime; set => Base.MaxCancellableTime = value;
        }
        public float RemainingCooldown
        {
            get => Base.RemainingCooldown; set => Base.RemainingCooldown = value;
        }
        public float Cooldown
        {
            get
            {
                return UsableItemsController.GetCooldown(base.Serial, this.Base, UsableItemsController.GetHandler(this.Base.Owner));
            }
        }
    }
}
