using Mirror;
using NetworkManagerUtils.Dummies;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API
{
    public class DummyBase:MonoBehaviour
    {
        public DummyBase(string name ="Dummy")
        {
            Spawn(RoleTypeId.Tutorial, name);
        }
        public static List<DummyBase> Dummies = new List<DummyBase>();
        public Player Player => API.Player.Get(ReferenceHub);
        public ReferenceHub ReferenceHub { get; set; }
        public static DummyBase Get(string name)
        {
            return Dummies.FirstOrDefault(x => x.ReferenceHub.nicknameSync.MyNick == name);
        }
        public static DummyBase Get(ReferenceHub referenceHub)
        {
            return Dummies.FirstOrDefault(x => x.ReferenceHub == referenceHub);
        }
        public static DummyBase Get(Player player)
        {
            return Dummies.FirstOrDefault(x => x.Player == player);
        }
        public static bool Is(ReferenceHub hub, DummyBase dummy)
        {
            if (hub == null) return false;
            if (dummy.ReferenceHub != hub) return false;
            return true;
        }
        public static DummyBase Spawn(RoleTypeId roleTypeId, string name ="Dummy")
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            gameObject.AddComponent<DummyBase>();
            gameObject.AddComponent<ReferenceHub>();
            gameObject.GetComponent<ReferenceHub>().nicknameSync.MyNick = name;
            gameObject.GetComponent<ReferenceHub>().roleManager.ServerSetRole(roleTypeId, RoleChangeReason.None);
            if (!gameObject.TryGetComponent<DummyBase>(out var component))
            {
                return null;
            }
            NetworkServer.AddPlayerForConnection(new DummyNetworkConnection(), gameObject);
            Dummies.Add(component);
            Events.EventArgs.Dummy.Create create = new Events.EventArgs.Dummy.Create(component.ReferenceHub);
            Events.Handlers.Dummy.OnCreate(create);
            return component;
        }
        public void Destoy()
        {
            OnDestroy();
        }
        void OnDestroy()
        {
            Events.EventArgs.Dummy.Destroy destroy = new Events.EventArgs.Dummy.Destroy(this.ReferenceHub);
            Events.Handlers.Dummy.OnDestroy(destroy);
            Dummies.Remove(this);
            NetworkServer.Destroy(gameObject);
            GameObject.Destroy(gameObject);
        }
    }
}
