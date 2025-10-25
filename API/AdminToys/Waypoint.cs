using AdminToys;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.AdminToys
{
    public class Waypoint : AdminToy
    {
        public Waypoint(WaypointToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public static Waypoint Create(Vector3 pos)
        {
            WaypointToy waypointToy = new WaypointToy();
            NetworkServer.Spawn(waypointToy.gameObject);
            waypointToy.NetworkPosition = pos;
            return (Waypoint)AdminToy.Get(waypointToy);
        }
        public static Waypoint Get(AdminToy adminToy)
        {
            Waypoint waypoint = adminToy as Waypoint;
            return waypoint;
        }
        public static Waypoint Get(WaypointToy waypoint)
        {
            AdminToy.TryGet(waypoint, out var a );
            return Get(a);
        }
        public new WaypointToy Base { get; set; }
        public float Priority
        {
            get
            {
                return this.Base.Priority;
            }
            set
            {
                this.Base.Priority = value;
            }
        }

        public bool VisualizeBounds
        {
            get
            {
                return this.Base.NetworkVisualizeBounds;
            }
            set
            {
                this.Base.NetworkVisualizeBounds = value;
            }
        }
    }
}
