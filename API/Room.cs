using FMOD.Extensions;
using MapGeneration;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API
{
    public class Room
    {
        public Room(RoomIdentifier roomIdentifier)
        {
            this.Base= roomIdentifier;
        }
        public static List<Room> List = new List<Room>();
        public static Room RandomRoom()
        {
           return List.GetRandomItem();
        }
        public RoomIdentifier Base { get; }
        public RoomName Name => Base.Name;
        public string RoomName => Base.name;
        public Vector3 Position => Base.transform.position;
        public RoomShape RoomShape => Base.Shape;
        public RoomLight Light { get; }
        public GameObject GameObject => Base.gameObject;
        public Transform Transform => Base.transform;
        public FacilityZone Zone => Base.Zone;
        public Quaternion Quaternion => GameObject.transform.rotation;
        public Vector3 Scale => GameObject.transform.localScale;
        public RoomLightController RoomLightController => Base.LightControllers.FirstOrDefault();
        public IEnumerable<Pickup> Pickups
        {
            get
            {
                return from pickup in Pickup.List
                       where Room.FindParentRoom(pickup.GameObject) == this
                       select pickup;
            }
        }
        public static Room GetRoom(RoomName roomName)
        {
            RoomIdentifier identifier = RoomIdentifier.AllRoomIdentifiers.First(X => X.Name == roomName);
            return new Room(identifier);
        }
        public static Vector3 GetPositionToWord(RoomName roomName, Vector3 position)
        {
            var roomInfo = GetRoom(roomName);
            Vector3 rotatedPosition = roomInfo.GameObject.transform.rotation * position;
            Vector3 worldPosition = roomInfo.GameObject.transform.position + rotatedPosition;
            return worldPosition;
        }
        public static void TryGetWordPosition(RoomName roomName, Vector3 pos, out Vector3 Wordpos)
        {
            Wordpos = GetPositionToWord(roomName, pos);
        }
        public static Room GetRoom(Vector3 Position)
        {
            RoomIdentifier identifier = RoomIdentifier.AllRoomIdentifiers.First(X => X.transform.position == Position);
            return new Room(identifier);
        }
        public static Room GetRoom(RoomIdentifier identifier)
        {
            return new Room(identifier);
        }
        public void ChangColor(UnityEngine.Color color)
        {
            RoomLightController.NetworkOverrideColor = color;
        }
        public bool LightIsOffOrOn
        {
            get
            {
               return RoomLightController.LightsEnabled;
            }
            set
            {
                RoomLightController.LightsEnabled = value;
            }
        }
        public static Room FindParentRoom(GameObject objectInRoom)
        {
            if (objectInRoom == null)
            {
                return null;
            }
            Room room = null;
            if (!objectInRoom.CompareTag("Player"))
            {
                room = objectInRoom.GetComponentInParent<Room>();
            }
            Room result;
            if ((result = room) == null)
            {
                result = (Room.GetRoom(objectInRoom.transform.position) ?? null);
            }
            return result;
        }
        public NetworkIdentity RoomLightControllerNetIdentity
        {
            get
            {
                RoomLightController roomLightController = this.RoomLightController;
                if (roomLightController == null)
                {
                    return null;
                }
                return roomLightController.netIdentity;
            }
        }

    }
}
