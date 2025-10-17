using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utf8Json;

namespace FMOD.API.Items
{
    public abstract class CustomItem
    {
        public static Dictionary<int, Info> List = new Dictionary<int, Info>();
        public static Dictionary<int, Pickup> Pickups = new Dictionary<int, Pickup>();
        public abstract int ID { get; set; }
        public abstract string Name { get; set; }
        public abstract string Description { get;set; }
        public abstract ItemType ItemType { get; set; }
        public abstract RoomName Room { get; set; }
        public virtual Vector3 Position { get; set; }
        public virtual bool Is(Pickup pickup)
        {
            if (!Pickups.ContainsKey(ID)) return false;
            if (!Pickups.Values.Where(x => x.Serial == pickup.Serial).Any()) return false;
            return true;
        }
        public virtual bool Is(Item item)
        {
            if (!Pickups.ContainsKey(ID)) return false;
            if (!Pickups.Values.Where(x => x.Serial == item.Serial).Any()) return false;
            return true;
        }
        public virtual Pickup Spawn()
        {
            if (Position == null)
            {
                if (List.Values.Any(x => x.ID == ID))
                {
                    Position = List.Values.FirstOrDefault(x => x.ID == ID).Position;
                }
                Position = API.Room.GetRoom(Room).Position + Vector3.up;
            }
            Info info = new Info()
            {
                Position = Position,
                ItemType = ItemType,
                ID = ID,
                Name = Name,
                Description = Description,
            };
            if (List.ContainsKey(ID))
            {
                List[ID] = info;
            }
            List.Add(ID, info);
            Pickup pickup = Pickup.CreatAndSpawn(Position, ItemType);
            Pickups.Add(ID, pickup);
            return pickup;
        }
        public virtual Pickup Spawn(Vector3 position)
        {
            Info info = new Info()
            {
                Position = position,
                ItemType = ItemType,
                ID = ID,
                Name = Name,
                Description = Description,
            };
            if (List.ContainsKey(ID))
            {
                List[ID] = info;
            }
            List.Add(ID, info);
            Pickup pickup = Pickup.CreatAndSpawn(Position, ItemType);
            return pickup;
        }
        public static void Register(CustomItem customItem)
        {
            string Paths = Path.Combine(API.Paths.CustomItem, $"{customItem.Name}.yaml");
            if (Paths!=null)
            {
                Log.Debug($"{customItem.Name}自定义物品已存在");
                File.WriteAllText(Path.Combine(API.Paths.CustomItem, $"{customItem.Name}.yaml"), List.Values.FirstOrDefault(x => x.Name == customItem.Name).ToString());
            }
            var Info = List.Values.FirstOrDefault(x => x.Name == customItem.Name);
            File.WriteAllText(Path.Combine(API.Paths.CustomItem, $"{customItem.Name}.yaml"), Info.ToString());
            customItem.RegisterEvents();
            customItem.Spawn();
        }
        public static void UnRegister(CustomItem customItem)
        {
            customItem.UnregisterEvents();
        }
        public virtual void RegisterEvents()
        {

        }
        public virtual void UnregisterEvents()
        {

        }
    }
    public class Info
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType ItemType { get; set; }
        public Vector3 Position { get; set; }
    }
}
