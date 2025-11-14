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
            var prefab = FindPrefab<TextToy>();
            if (prefab == null) return null;

            var textObject = Object.Instantiate(prefab);
            TextToy textToy = textObject.GetComponent<TextToy>();

            NetworkServer.Spawn(textObject);
            textToy.NetworkPosition = pos;
            textToy.Network_textFormat = content;

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