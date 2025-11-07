using AdminToys;
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
    public class Capybara : AdminToy
    {
        public Capybara(CapybaraToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public static Capybara Create(Vector3 Position)
        {
            CapybaraToy capybaraToy = new CapybaraToy();
            NetworkServer.Spawn(capybaraToy.gameObject);
            capybaraToy.NetworkPosition = Position;
            return (Capybara)AdminToy.Get(capybaraToy);
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
