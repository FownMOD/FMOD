using AdminToys;
using FMOD.API.DamageHandles;
using FMOD.Enums;
using Interactables.Verification;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.AdminToys
{
    public class ShootingTargetToy : AdminToy
    {
        public override AdminToyType AdminToyType => AdminToyType.ShootingTarget;
        public ShootingTargetToy(ShootingTarget adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public static ShootingTargetToy Create(Vector3 pos)
        {
            var prefab = FindPrefab<ShootingTarget>();
            if (prefab == null) return null;

            var primitiveObject = UnityEngine.Object.Instantiate(prefab);
            ShootingTarget shooting = primitiveObject.GetComponent<ShootingTarget>();

            NetworkServer.Spawn(primitiveObject);
            shooting.NetworkPosition = pos;

            return new ShootingTargetToy(shooting);
        }
        public static ShootingTargetToy Get(AdminToy adminToy)
        {
            ShootingTargetToy shootingTargetToy = adminToy as ShootingTargetToy;
            return shootingTargetToy;
        }
        public static ShootingTargetToy Get(ShootingTarget shootingTarget)
        {
            AdminToy.TryGet(shootingTarget, out var adminToy);
            return Get(adminToy);
        }
        public new ShootingTarget Base { get; set; }
        public uint NetworkId => Base.NetworkId;
        public IVerificationRule VerificationRule => Base.VerificationRule;
        public bool Network_syncMode => Base.Network_syncMode;
        public void Damage(float d)
        {
            DamageBase damageBase = new DamageBase();
            Base.Damage(d, damageBase,new Vector3(1,1,1));
        }
    }
}
