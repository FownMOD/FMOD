using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Items
{
    public class FirearmItem:Item
    {
        public FirearmItem(InventorySystem.Items.Firearms.Firearm firearm)
        {
            this.Base = firearm;
        }
        public static List<FirearmItem> Firearms = new List<FirearmItem>();
        public HitscanHitregModuleBase HitscanHitregModule { get; }
        public AnimatorReloaderModuleBase AnimatorReloaderModule { get; }
        public new GameObject GameObject => Base.gameObject;
        public new Vector3 Position => Base.gameObject.transform.position;
        public Quaternion Rotation => Base.gameObject.transform.rotation;
        public new Vector3 Scale => Base.gameObject.transform.localScale;
        public new API.Player Owner
        {
            get
            {
                return Player.Get(Base.Footprint);
            }
        }
        public float Damage
        {
            get
            {
                return this.HitscanHitregModule.BaseDamage;
            }
        }
        public int TotalAmmo
        {
            get
            {
                return this.Base.GetTotalStoredAmmo();
            }
        }
        public bool IsReloading
        {
            get
            {
                IReloaderModule reloaderModule;
                return this.Base.TryGetModule(out reloaderModule, true) && reloaderModule.IsReloading;
            }
        }

        public int TotalMaxAmmo
        {
            get
            {
                return Base.GetTotalMaxAmmo();
            }
        }
        public new InventorySystem.Items.Firearms.Firearm Base { get; }
    }
}
