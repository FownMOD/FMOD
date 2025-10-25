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
    public class Primitive : AdminToy
    {
        public Primitive(PrimitiveObjectToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public new PrimitiveObjectToy Base { get; set; }
        public static Primitive Create(Vector3 pos)
        {
            PrimitiveObjectToy primitiveObjectToy = new PrimitiveObjectToy();
            NetworkServer.Spawn(primitiveObjectToy.gameObject);
            primitiveObjectToy.NetworkPosition = pos;
            return (Primitive)AdminToy.Get(primitiveObjectToy);
        }
        public static Primitive Create(PrimitiveType primitiveType)
        {
            PrimitiveObjectToy primitiveObject = new PrimitiveObjectToy();
            NetworkServer.Spawn(primitiveObject.gameObject);
            primitiveObject.NetworkPrimitiveType = primitiveType;
            return (Primitive)AdminToy.Get(primitiveObject);
        }
        public static Primitive Create(Vector3 pos, Color color)
        {
            Primitive primitive = Primitive.Create(pos);
            primitive.ChangeColor(color);
            return primitive;
        }
        public static Primitive Create(Vector3 pos, Color color, bool IsHide = false)
        {
            Primitive primitive = Primitive.Create(pos, color);
            primitive.ChangeHide(IsHide);
            return primitive;
        }
        public static Primitive Get(AdminToy adminToy)
        {
            Primitive primitive = adminToy as Primitive;
            return primitive;
        }
        public static Primitive Get(PrimitiveObjectToy primitiveObject)
        {
            AdminToy.TryGet(primitiveObject, out var adminToy);
            return Get(adminToy);
        }
        public PrimitiveType PrimitiveType
        {
            get
            {
                return Base.NetworkPrimitiveType;
            }
            set
            {
                Base.NetworkPrimitiveType = value;
            }
        }
        public Color Color
        {
            get
            {
                return Base.NetworkMaterialColor;
            }
            set
            {
                Base.NetworkMaterialColor = value;
            }
        }
        public PrimitiveFlags PrimitiveFlags
        {
            get
            {
                return Base.NetworkPrimitiveFlags;
            }
            set
            {
                Base.NetworkPrimitiveFlags = value;
            }
        }
        public void ChangeHide(bool Hide = false)
        {
            if (Hide == true)
            {
                Base.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                Base.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        public void ChangeColor(Color Color)
        {
            this.Color = Color;
        }

    }
}
