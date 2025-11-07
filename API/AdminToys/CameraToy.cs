using AdminToys;
using FMOD.API.Interface;
using FMOD.Enums;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.AdminToys
{
    public class CameraToy : AdminToy
    {
        public CameraToy(Scp079CameraToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public static CameraToy Get(AdminToy toy)
        {
            CameraToy cameraToy = toy as CameraToy;
            if (cameraToy != null)
            {
                return cameraToy;
            }
            return null;
        }
        public static CameraToy Get(Scp079CameraToy scp079Camera)
        {
            AdminToy toy = AdminToy.Get(scp079Camera);
            if (toy != null)
            {
                CameraToy cameraToy = toy as CameraToy;
                return cameraToy;
            }
            return null;
        }
        public static AdminToy Create(Vector3 Position)
        {
            Scp079CameraToy scp079CameraToy = new Scp079CameraToy();
            NetworkServer.Spawn(scp079CameraToy.gameObject);
            scp079CameraToy.NetworkPosition = Position;
            AdminToy toy = AdminToy.Get(scp079CameraToy);
            return toy;
        }
        public new Scp079CameraToy Base { get; set; }
        public Room CurrentRoom
        {
            get
            {
                Room room = Room.GetRoom(Base.Room);
                return room;
            }
        }
        public string CameraName
        {
            get
            {
                return Base.Camera.Label;
            }
            set
            {
                Base.Camera.Label = value;
            }
        }
        public Vector2 VerticalConstraint
        {
            get
            {
                return this.Base.NetworkVerticalConstraint;
            }
            set
            {
                this.Base.NetworkVerticalConstraint = value;
            }
        }

        public Vector2 HorizontalConstraint
        {
            get
            {
                return this.Base.NetworkHorizontalConstraint;
            }
            set
            {
                this.Base.NetworkHorizontalConstraint = value;
            }
        }

        public Vector2 ZoomConstraint
        {
            get
            {
                return this.Base.NetworkZoomConstraint;
            }
            set
            {
                this.Base.NetworkZoomConstraint = value;
            }
        }

        public override AdminToyType AdminToyType { get;} = AdminToyType.Camera;
    }
}
