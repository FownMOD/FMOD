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
    public class Text : AdminToy
    {
        public Text(TextToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }
        public static Text Create(Vector3 pos,string Content ="")
        {
            TextToy textToy = new TextToy();
            NetworkServer.Spawn(textToy.gameObject);
            Text text = (Text)AdminToy.Get(textToy);
            text.TextContent = Content;
            text.Position = pos;
            return text;
        }
        public static Text Get(AdminToy adminToy)
        {
            Text text = adminToy as Text;
            return text;
        }
        public static Text Get(TextToy text)
        {
            AdminToy.TryGet(text, out var adminToy);
            return Get(adminToy);
        }
        public new TextToy Base { get; set; }
        public string TextContent
        {
            get
            {
                return this.Base.Network_textFormat;
            }
            set
            {
                this.Base.Network_textFormat = value;
            }
        }
        public Vector2 Size
        {
            get
            {
                return this.Base.Network_displaySize;
            }
            set
            {
                this.Base.Network_displaySize = value;
            }
        }
    }
}
