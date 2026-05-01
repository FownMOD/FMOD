using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UserSettings.ServerSpecific;
using static UserSettings.ServerSpecific.SSTextArea;

namespace FMOD.API.ServerSpecific
{
    public class TextArea: SSServerSpecifiSetting
    {
        public TextArea(SSTextArea st):base(st)
        {
            this.Base = st;
        }
        public new SSTextArea Base { get; }
        public string Content
        {
            set=>Base.SendTextUpdate(value);
        }
        public TextAlignmentOptions TextAlignment
        {
            get => Base.AlignmentOptions;
        }
        public FoldoutMode Foldout
        {
            get => Base.Foldout;
        }
    }
}
