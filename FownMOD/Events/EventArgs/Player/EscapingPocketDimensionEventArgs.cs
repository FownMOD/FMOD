using FMOD.API;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.Events.EventArgs.Player
{
    public class EscapingPocketDimensionEventArgs : System.EventArgs, IFMODPlayerEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EscapingPocketDimensionEventArgs" /> class.
        /// </summary>
        /// <param name="pocketDimensionTeleport">
        /// <inheritdoc cref="Teleporter" />
        /// </param>
        /// <param name="hub">
        /// <inheritdoc cref="Player" />
        /// </param>
        public EscapingPocketDimensionEventArgs(PocketDimensionTeleport pocketDimensionTeleport, ReferenceHub hub)
        {
            Teleporter = pocketDimensionTeleport;
            Player = API.Player.Get(hub);
            TeleportPosition = pocketDimensionTeleport.gameObject.transform.position;
        }

        /// <summary>
        /// Gets the PocketDimensionTeleport the player walked into.
        /// </summary>
        public PocketDimensionTeleport Teleporter { get; }


        /// <summary>
        /// Gets or sets the position in which the player is going to be teleported to.
        /// </summary>
        public Vector3 TeleportPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can successfully escape the pocket dimension.
        /// </summary>
        public bool IsAllowed { get; set; } = true;
        public API.Player Player {  get; set; }
    }
}
