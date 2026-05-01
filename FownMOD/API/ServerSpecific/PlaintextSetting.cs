using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettings.ServerSpecific;
using static TMPro.TMP_InputField;

namespace FMOD.API.ServerSpecific
{
    public class PlaintextSetting:SSServerSpecifiSetting
    {
        public PlaintextSetting(SSPlaintextSetting ss):base(ss)
        {

            this.Base = ss;
        }

        public new SSPlaintextSetting Base { get; }
        public string InputText
        {
            get => Base.SyncInputText;
            set => Base.SendValueUpdate(value);
        }
        public string Placeholder
        {
            get => Base.Placeholder;
            set => Base.SendPlaintextUpdate(value, CollectionId, ContentType);
        }
        public ushort CharacterLimit
        {
            get => (ushort)Base.CharacterLimit;
            set => Base.SendPlaintextUpdate(Placeholder, value, ContentType);
        }
        public ContentType ContentType
        {
            get => Base.ContentType;
            set => Base.SendPlaintextUpdate(InputText, CharacterLimit, value);
        }
    }
}
