using FMOD.Extensions;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Items
{
    public class Item:ItemBase
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
            if (itemBase == null)
            {
                return null;
            }
            Firearm firearm = itemBase as Firearm;
            Item result2;
            if (firearm == null)
            {
                result2 = null;
            }
            else
            {
                result2 = new API.Items.FirearmItem(firearm);
            }
            return result2;
        }
        public static ItemBase GetItemBase(ItemType type)
        {
            return type.GetItemBase();
        }
        public ItemBase Base { get; set; }
        public ItemType Type => Base.ItemTypeId;
        public ushort Serial => Base.ItemSerial;
        public GameObject GameObject => Base.gameObject;
        public Vector3 Scale => GameObject.transform.localScale;
        public Player CurrentOwner => Player.Get(Base.Owner);
        public Vector3 Position => GameObject.transform.localPosition;

        public override float Weight => Base.Weight;

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
