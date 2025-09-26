using PlayerRoles.Spectating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace FMOD.API.ServerSpecific
{
    public class SimpleKeyBind : SSKeybindSetting
    {
        public static List<SimpleKeyBind> simpleKeys = new List<SimpleKeyBind>();
        public static Dictionary<ReferenceHub, SimpleKeyBind> SimpleKeyList = new Dictionary<ReferenceHub, SimpleKeyBind>();
        public SimpleKeyBind(int? id, string label, KeyCode suggestedKey = UnityEngine.KeyCode.None, bool preventInteractionOnGui = true, bool allowSpectatorTrigger = true, string hint = null, byte collectionId = 255) : base(id, label, suggestedKey, preventInteractionOnGui, allowSpectatorTrigger, hint, collectionId)
        {
            Id = id ?? 0;
            base.Label = label;
            KeyCode = suggestedKey;
            PreventInteractionOnGui = preventInteractionOnGui;
            AllowSpectatorTrigger = allowSpectatorTrigger;
            Hint = hint;
            CollectionId = collectionId;
        }
        public static void SendToPlayer(ReferenceHub referenceHub, SimpleKeyBind simpleKeyBind)
        {
            if (!SimpleKeyList.ContainsValue(simpleKeyBind))
            {
                SimpleKeyList.Add(referenceHub, simpleKeyBind);
            }
            SimpleKeyList[referenceHub] = simpleKeyBind;
            UserSettings.ServerSpecific.ServerSpecificSettingsSync.SendToPlayer(referenceHub, new ServerSpecificSettingBase[] { simpleKeyBind });
        }
        protected void RegisterEvents()
        {
            
        }
        protected void UnRegister()
        {
            
        }
        public static SimpleKeyBind Get(ReferenceHub referenceHub)
        {
            return SimpleKeyList[referenceHub];
        }
        public static SimpleKeyBind Get(int? ID)
        {
            return simpleKeys.Where(x => x.Id == ID).FirstOrDefault();
        }
        public static SimpleKeyBind Get(KeyCode keyCode)
        {
            return simpleKeys.Where(x => x.KeyCode == keyCode).FirstOrDefault();
        }
        public int? Id { get; set; }
        public new string Label { get; set; }
        public KeyCode KeyCode {  get; set; }
        public bool PreventInteractionOnGui { get; set; }
        public new bool AllowSpectatorTrigger { get; set; }
        public string Hint { get; set; }
        public new byte CollectionId { get; set; }
    }
}
