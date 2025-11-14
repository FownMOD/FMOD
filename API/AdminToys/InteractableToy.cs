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
    public class InteractableToy : AdminToy
    {
        public InteractableToy(InvisibleInteractableToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public new InvisibleInteractableToy Base { get; set; }
        public static InteractableToy Create(Vector3 Pos)
        {
            var prefab = FindPrefab<InvisibleInteractableToy>();
            if (prefab == null) return null;

            var primitiveObject = Object.Instantiate(prefab);
            InvisibleInteractableToy invisible = primitiveObject.GetComponent<InvisibleInteractableToy>();

            NetworkServer.Spawn(primitiveObject);
            invisible.NetworkPosition = Pos;
            return Get(invisible);
        }
        public static InteractableToy Get(AdminToy adminToy)
        {
            InteractableToy interactable = adminToy as InteractableToy;
            return interactable;
        }
        public static InteractableToy Get(InvisibleInteractableToy invisibleInteractableToy)
        {
            AdminToy.TryGet(invisibleInteractableToy, out var adminToy);
            return Get(adminToy);
        }
        public InvisibleInteractableToy.ColliderShape Shape
        {
            get
            {
                return this.Base.NetworkShape;
            }
            set
            {
                this.Base.NetworkShape = value;
            }
        }
        public float InteractionDuration
        {
            get
            {
                return this.Base.NetworkInteractionDuration;
            }
            set
            {
                this.Base.NetworkInteractionDuration = value;
            }
        }

        public bool IsLocked
        {
            get
            {
                return this.Base.NetworkIsLocked;
            }
            set
            {
                this.Base.NetworkIsLocked = value;
            }
        }
        public bool CanSearch
        {
            get
            {
                return Base.CanSearch;

            }
        }

        public override AdminToyType AdminToyType => AdminToyType.Interactable;
    }
}
