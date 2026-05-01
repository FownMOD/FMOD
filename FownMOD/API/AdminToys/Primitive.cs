using AdminToys;
using FMOD.Enums;
using Mirror;
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
            var p = FPrefabsManger.Spawn(pos, PrefabType.PrimitiveObjectToy);
            var primitiveToy = p.gameObject.AddComponent<PrimitiveObjectToy>();
            return new Primitive(primitiveToy);
        }

        public PrimitiveType PrimitiveType
        {
            get => Base.NetworkPrimitiveType;
            set => Base.NetworkPrimitiveType = value;
        }

        public Color Color
        {
            get => Base.NetworkMaterialColor;
            set => Base.NetworkMaterialColor = value;
        }

        public override AdminToyType AdminToyType => AdminToyType.Primitive;
    }
}