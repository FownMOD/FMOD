using AdminToys;
using FMOD.Enums;
using Mirror;
using UnityEngine;

namespace FMOD.API.AdminToys
{
    public class Text : AdminToy
    {
        public Text(TextToy adminToyBase) : base(adminToyBase)
        {
            this.Base = adminToyBase;
        }

        public new TextToy Base { get; set; }

        public static Text Create(Vector3 pos, string content = "")
        {
            var t = FPrefabsManger.Spawn(pos, PrefabType.TextToy);
            var textToy = t.gameObject.AddComponent<TextToy>();
            textToy.TextFormat = content;
            return new Text(textToy);
        }

        public string TextContent
        {
            get => Base.Network_textFormat;
            set => Base.Network_textFormat = value;
        }

        public Vector2 Size
        {
            get => Base.Network_displaySize;
            set => Base.Network_displaySize = value;
        }

        public override AdminToyType AdminToyType => AdminToyType.Text;
    }
}