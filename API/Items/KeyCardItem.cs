using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Keycards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Items
{
    public class KeyCardItem:Item
    {
        public static List<KeyCardItem> KeyCards = new List<KeyCardItem>();
        public KeyCardItem(InventorySystem.Items.Keycards.KeycardItem keycard)
        {
            this.Base = keycard;
        }
        public new InventorySystem.Items.Keycards.KeycardItem Base {  get; set; }
        public DoorPermissionFlags DoorPermissionFlags => this.Base.GetPermissions(null);
        public KeycardLevels keycardLevels
        {
            get
            {
                return new KeycardLevels(DoorPermissionFlags);
            }
        }
        public static KeyCardItem CreateCustomCard(ItemType itemType, Player targetPlayer, params object[] args)
        {
            if (targetPlayer == null)
            {
                return null;
            }
            KeycardItem keycardItem;
            if (!itemType.TryGetTemplate(out keycardItem))
            {
                throw new ArgumentException("Template for itemType not found");
            }
            if (!keycardItem.Customizable)
            {
                return null;
            }
            int num = 0;
            DetailBase[] details = keycardItem.Details;
            for (int i = 0; i < details.Length; i++)
            {
                ICustomizableDetail customizableDetail = details[i] as ICustomizableDetail;
                if (customizableDetail != null)
                {
                    customizableDetail.SetArguments(new ArraySegment<object>(args, num, customizableDetail.CustomizablePropertiesAmount));
                    num += customizableDetail.CustomizablePropertiesAmount;
                }
            }
            Item item = targetPlayer.AddItem(itemType, ItemAddReason.AdminCommand);
            return (KeyCardItem)item;
        }
    }
}
