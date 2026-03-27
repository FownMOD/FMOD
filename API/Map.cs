using FMOD.API.Items;
using LightContainmentZoneDecontamination;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API
{
    public class Map
    {
        public static List<Item> Items = Item.List;
        public static List<Room> Rooms = Room.List;
        public static List<Pickup> Pickups = Pickup.List;
        public static DecontaminationController DecontaminationController {  get; set; }
        public static void ChangRoomColor(RoomName roomName, UnityEngine.Color color)
        {
            Room room = Room.GetRoom(roomName);
            ChangRoomColor(room, color);
        }
        public static void ChangRoomColor(Room room, UnityEngine.Color color)
        {
            room.RoomLightController.NetworkOverrideColor = color;
        }
        public static void SendBroadcast(string msg, ushort time)
        {
            Broadcast.Singleton.RpcAddElement(msg, time, Broadcast.BroadcastFlags.Normal);
        }
    }
}
