using AdminToys;
using FMOD.Enums;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FMOD.API.AdminToys
{
    public class LightToy : AdminToy
    {
        public LightToy(LightSourceToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public new LightSourceToy Base { get; set; }
        public static LightToy Create(Vector3 pos, float Range)
        {
            var prefab = FindPrefab<LightSourceToy>();
            if (prefab == null) return null;

            var primitiveObject = Object.Instantiate(prefab);
            LightSourceToy light = primitiveObject.GetComponent<LightSourceToy>();

            NetworkServer.Spawn(primitiveObject);
            light.NetworkPosition = pos;
            light.NetworkLightRange = Range;
            return Get(light);
        }
        public static LightToy Get(AdminToy adminToy)
        {
            LightToy lightToy = adminToy as LightToy;
            return lightToy;
        }
        public static LightToy Get(LightSourceToy lightSourceToy)
        {
            AdminToy toy = AdminToy.Get(lightSourceToy);
            return Get(toy);
        }
        public Color Color
        {
            get
            {
                return this.Base.NetworkLightColor;
            }
            set
            {
                this.Base.NetworkLightColor = value;
            }
        }
        public float InnerSpotAngle
        {
            get
            {
                return this.Base.NetworkInnerSpotAngle;
            }
            set
            {
                this.Base.NetworkInnerSpotAngle = value;
            }
        }
        public float Intensity
        {
            get
            {
                return this.Base.NetworkLightIntensity;
            }
            set
            {
                this.Base.NetworkLightIntensity = value;
            }
        }
        public LightType LightType
        {
            get
            {
                return this.Base.NetworkLightType;
            }
            set
            {
                this.Base.NetworkLightType = value;
            }
        }
        public float Range
        {
            get
            {
                return this.Base.NetworkLightRange;
            }
            set
            {
                this.Base.NetworkLightRange = value;
            }
        }
        public float ShadowStrength
        {
            get
            {
                return this.Base.NetworkShadowStrength;
            }
            set
            {
                this.Base.NetworkShadowStrength = value;
            }
        }
        public LightShadows ShadowType
        {
            get
            {
                return this.Base.NetworkShadowType;
            }
            set
            {
                this.Base.NetworkShadowType = value;
            }
        }
        public float SpotAngle
        {
            get
            {
                return this.Base.NetworkSpotAngle;
            }
            set
            {
                this.Base.NetworkSpotAngle = value;
            }
        }

        public override AdminToyType AdminToyType => AdminToyType.Light;
    }
}
