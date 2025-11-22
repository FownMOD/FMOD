using FMOD.Extensions;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Keycards;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ToggleableLights.Flashlight;
using InventorySystem.Items.Usables;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Items
{
    public class Item
    {
        public static List<Item> List = new List<Item>();
        public static Item Get(ItemType type)
        {
            return List.First(x => x.Type == type);
        }
        public static Item Get(ushort Serial)
        {
            return List.First(x => x.Serial == Serial);
        }
        public static Item Get(ItemBase itemBase)
        {
            if (itemBase is Firearm)
                return new FirearmItem((Firearm)itemBase);
            if (itemBase is FlashlightItem)
                return new Flashlight((FlashlightItem)itemBase);
            if (itemBase is KeycardItem)
                return new KeyCardItem((KeycardItem)itemBase);
            if (itemBase is UsableItem)
                return new UseItem((UsableItem)itemBase);
            return null;
        }
        public void SetWeight(float value)
        {
            Base.gameObject.GetComponent<Rigidbody>().mass = value;
        }

        public static ItemBase GetItemBase(ItemType type)
        {
            return type.GetItemBase();
        }
        public ItemBase Base { get; set; }
        public ItemCategory ItemCategory => Base.Category;
        public ItemType Type => Base.ItemTypeId;
        public ushort Serial => Base.ItemSerial;
        public GameObject GameObject => Base.gameObject;
        public Vector3 Scale => GameObject.transform.localScale;
        public Player CurrentOwner => Player.Get(Base.Owner);
        public Vector3 Position => GameObject.transform.position;
        public ItemPickupBase ItemPickupBase => Base.PickupDropModel;

        public  float Weight => Base.Weight;
        public float GetWeight()
        { return Base.Weight; }
        public void Spawn()
        {
            NetworkServer.Spawn(this.GameObject);
        }
        public void UnSpawn()
        {
            NetworkServer.UnSpawn(this.GameObject);
        }
    }
}
