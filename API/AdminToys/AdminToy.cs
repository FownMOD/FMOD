using AdminToys;
using FMOD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.AdminToys
{
    public abstract class AdminToy
    {
        public abstract AdminToyType AdminToyType { get;}
        public AdminToy(AdminToyBase adminToyBase) 
        {
            this.Base = adminToyBase;
        }
        public static Dictionary<AdminToy, AdminToyBase> Dictionary = new Dictionary<AdminToy, AdminToyBase>();
        public AdminToyBase Base { get; set; }
        public static AdminToy Get(AdminToyBase adminToyBase)
        {
            return Dictionary.Keys.FirstOrDefault(x=>x.Base == adminToyBase);
        }
        public static AdminToy Create(AdminToyType adminToyType, Vector3 pos)
        {
            switch(adminToyType)
            {
                case AdminToyType.Camera:
                    return CameraToy.Create(pos);
                case AdminToyType.Capybara:
                    return Capybara.Create(pos);
                case AdminToyType.Interactable:
                    return InteractableToy.Create(pos);
                case AdminToyType.Light:
                   return LightToy.Create(pos, 100);
                case AdminToyType.Primitive:
                    return Primitive.Create(pos);
                case AdminToyType.ShootingTarget:
                    return ShootingTargetToy.Create(pos);
                case AdminToyType.Speaker:
                    return Speaker.Create(pos);
                case AdminToyType.Text:
                    return Text.Create(pos);
                case AdminToyType.Waypoint:
                    return Waypoint.Create(pos);
                default:
                    throw new NotImplementedException();
            }
        }
        public static bool TryGet(AdminToyBase adminToyBase, out AdminToy adminToy)
        {
            AdminToy toy = Get(adminToyBase);
            if (toy == null)
            {
                adminToy = null;
                return false;
            }
            adminToy = toy;
            return true;
        }
        public Vector3 Scale
        {
            get { return this.Base.Scale; }
            set { Base.Scale = value; }
        }
        public Vector3 Position
        {
            get
            {
                return Base.Position;
            }
            set
            {
                Base.Position = value;
            }
        }
        public Quaternion Rotation
        {
            get { return Base.Rotation; }
            set { Base.Rotation = value; }
        }
    }
}
