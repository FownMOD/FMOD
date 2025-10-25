using AdminToys;
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
