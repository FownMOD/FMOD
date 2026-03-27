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
    public class Capybara : AdminToy
    {
        public Capybara(CapybaraToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public static Capybara Create(Vector3 Position)
        {
            var cb = FPrefabsManger.Spawn(Position, PrefabType.CapybaraToy);
            var capybara = cb.gameObject.AddComponent<CapybaraToy>();
            return Get(capybara);
        }
        public static Capybara Get(AdminToy toy)
        {
            Capybara capybara = toy as Capybara;
            if (capybara ==null)
            {
                return null;
            }
            return capybara;
        }
        public static Capybara Get(CapybaraToy capybaraT)
        {
            AdminToy toy = AdminToy.Get(capybaraT);
            Capybara capybara = toy as Capybara;
            if (capybara == null)
            {
                return null;
            }
            return capybara;
        }
        public bool CollisionsEnabled
        {
            get
            {
                return Base.CollisionsEnabled;
            }
            set
            {
                Base.CollisionsEnabled = value;
            }
        }
        public bool NetworkCollisionsEnabled
        {
            get
            {
                return Base.NetworkCollisionsEnabled;
            }
            set
            {
                Base.NetworkCollisionsEnabled = value;
            }
        }
        public new CapybaraToy Base { get; }

        public override AdminToyType AdminToyType => AdminToyType.Capybara;
    }
}
