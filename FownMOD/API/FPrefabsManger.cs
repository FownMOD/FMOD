using FMOD.Enums;
using LabApi.Features.Wrappers;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FMOD.API
{
    public class FPrefabsManger
    {
        public static Dictionary<PrefabType,GameObject> List = new Dictionary<PrefabType,GameObject>();
        public static void Register()
        {
            List.Clear();
            List.Add(PrefabType.Player, GetPrefabType("Player"));
            List.Add(PrefabType.EZBreakableDoor, GetPrefabType("EZ BreakableDoor"));
            List.Add(PrefabType.HCZBreakableDoor, GetPrefabType("Hcz BreakableDoor"));
            List.Add(PrefabType.LCZBreakableDoor, GetPrefabType("LCZ BreakableDoor"));
            List.Add(PrefabType.HCZOneSided, GetPrefabType("HCZ OneSided"));
            List.Add(PrefabType.HCZTwoSided, GetPrefabType("HCZ TwoSided"));
            List.Add(PrefabType.HCZOpenHallway, GetPrefabType("OpenHallway"));
            List.Add(PrefabType.HCZOpenHallway_Construct_A, GetPrefabType("OpenHallway Construct A"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_A, GetPrefabType("HCZOpenHallway_Clutter_A"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_B, GetPrefabType("HCZOpenHallway_Clutter_B"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_C, GetPrefabType("HCZOpenHallway_Clutter_C"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_D, GetPrefabType("HCZOpenHallway_Clutter_D"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_E, GetPrefabType("HCZOpenHallway_Clutter_E"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_F, GetPrefabType("HCZOpenHallway_Clutter_F"));
            List.Add(PrefabType.HCZOpenHallway_Clutter_G, GetPrefabType("HCZOpenHallway_Clutter_G"));
            List.Add(PrefabType.HCZBulkDoor, GetPrefabType("HCZ BulkDoor"));
            List.Add(PrefabType.SportTarget, GetPrefabType("sportTargetPrefab"));
            List.Add(PrefabType.DBoyTarget, GetPrefabType("dboyTargetPrefab"));
            List.Add(PrefabType.BinaryTarget, GetPrefabType("binaryTargetPrefab"));
            List.Add(PrefabType.TantrumObj, GetPrefabType("TantrumObj"));
            List.Add(PrefabType.PrimitiveObjectToy, GetPrefabType("PrimitiveObjectToy"));
            List.Add(PrefabType.LightSourceToy, GetPrefabType("LightSourceToy"));
            List.Add(PrefabType.SpeakerToy, GetPrefabType("SpeakerToy"));
            List.Add(PrefabType.RegularKeycardPickup, GetPrefabType("RegularKeycardPickup"));
            List.Add(PrefabType.ChaosKeycardPickup, GetPrefabType("ChaosKeycardPickup"));
            List.Add(PrefabType.RadioPickup, GetPrefabType("RadioPickup"));
            List.Add(PrefabType.FirearmPickup, GetPrefabType("FirearmPickup"));
            List.Add(PrefabType.Com15Pickup, GetPrefabType("Com15Pickup"));
            List.Add(PrefabType.MedkitPickup, GetPrefabType("MedkitPickup"));
            List.Add(PrefabType.FlashlightPickup, GetPrefabType("FlashlightPickup"));
            List.Add(PrefabType.MicroHidPickup, GetPrefabType("MicroHidPickup"));
            List.Add(PrefabType.SCP500Pickup, GetPrefabType("SCP500Pickup"));
            List.Add(PrefabType.SCP207Pickup, GetPrefabType("SCP207Pickup"));
            List.Add(PrefabType.Ammo12gaPickup, GetPrefabType("Ammo12gaPickup"));
            List.Add(PrefabType.E11SRPickup, GetPrefabType("E11SRPickup"));
            List.Add(PrefabType.CrossvecPickup, GetPrefabType("CrossvecPickup"));
            List.Add(PrefabType.Ammo556mmPickup, GetPrefabType("Ammo556mmPickup"));
            List.Add(PrefabType.Fsp9Pickup, GetPrefabType("Fsp9Pickup"));
            List.Add(PrefabType.LogicerPickup, GetPrefabType("LogicerPickup"));
            List.Add(PrefabType.HegPickup, GetPrefabType("HegPickup"));
            List.Add(PrefabType.FlashbangPickup, GetPrefabType("FlashbangPickup"));
            List.Add(PrefabType.Ammo44calPickup, GetPrefabType("Ammo44calPickup"));
            List.Add(PrefabType.Ammo762mmPickup,GetPrefabType("Ammo762mmPickup"));
            List.Add(PrefabType.Ammo9mmPickup,GetPrefabType("Ammo9mmPickup"));
            List.Add(PrefabType.Com18Pickup, GetPrefabType("Com18Pickup"));
            List.Add(PrefabType.Scp018Projectile, GetPrefabType("Scp018Projectile"));
            List.Add(PrefabType.SCP268Pickup, GetPrefabType("SCP268Pickup"));
            List.Add(PrefabType.AdrenalinePrefab, GetPrefabType("AdrenalinePrefab"));
            List.Add(PrefabType.PainkillersPickup, GetPrefabType("PainkillersPickup"));
            List.Add(PrefabType.CoinPickup, GetPrefabType("CoinPickup"));
            List.Add(PrefabType.LightArmorPickup, GetPrefabType("Light Armor Pickup"));
            List.Add(PrefabType.CombatArmorPickup, GetPrefabType("Combat Armor Pickup"));
            List.Add(PrefabType.HeavyArmorPickup, GetPrefabType("Heavy Armor Pickup"));
            List.Add(PrefabType.RevolverPickup, GetPrefabType("RevolverPickup"));
            List.Add(PrefabType.AkPickup, GetPrefabType("AkPickup"));
            List.Add(PrefabType.ShotgunPickup, GetPrefabType("ShotgunPickup"));
            List.Add(PrefabType.Scp330Pickup, GetPrefabType("Scp330Pickup"));
            List.Add(PrefabType.Scp2176Projectile, GetPrefabType("Scp2176Projectile"));
            List.Add(PrefabType.SCP244APickup, GetPrefabType("SCP244APickup Variant"));
            List.Add(PrefabType.SCP244BPickup, GetPrefabType("SCP244BPickup Variant"));
            List.Add(PrefabType.SCP1853Pickup, GetPrefabType("SCP1853Pickup"));
            List.Add(PrefabType.DisruptorPickup, GetPrefabType("DisruptorPickup"));
            List.Add(PrefabType.Com45Pickup, GetPrefabType("Com45Pickup"));
            List.Add(PrefabType.SCP1576Pickup, GetPrefabType("SCP1576Pickup"));
            List.Add(PrefabType.JailbirdPickup, GetPrefabType("JailbirdPickup"));
            List.Add(PrefabType.AntiSCP207Pickup, GetPrefabType("AntiSCP207Pickup"));
            List.Add(PrefabType.FRMG0Pickup, GetPrefabType("FRMG0Pickup"));
            List.Add(PrefabType.A7Pickup, GetPrefabType("A7Pickup"));
            List.Add(PrefabType.LanternPickup, GetPrefabType("LanternPickup"));
            List.Add(PrefabType.Scp1344Pickup, GetPrefabType("SCP1344Pickup"));
            List.Add(PrefabType.AmnesticCloudHazard, GetPrefabType("Amnestic Cloud Hazard"));
            List.Add(PrefabType.Scp018PedestalStructure, GetPrefabType("Scp018PedestalStructure Variant"));
            List.Add(PrefabType.Scp207PedestalStructure, GetPrefabType("Scp207PedestalStructure Variant"));
            List.Add(PrefabType.Scp244PedestalStructure, GetPrefabType("Scp244PedestalStructure Variant"));
            List.Add(PrefabType.Scp268PedestalStructure, GetPrefabType("Scp268PedestalStructure Variant"));
            List.Add(PrefabType.Scp500PedestalStructure, GetPrefabType("Scp500PedestalStructure Variant"));
            List.Add(PrefabType.Scp1853PedestalStructure, GetPrefabType("Scp1853PedestalStructure Variant"));
            List.Add(PrefabType.Scp2176PedestalStructure, GetPrefabType("Scp2176PedestalStructure Variant"));
            List.Add(PrefabType.Scp1576PedestalStructure, GetPrefabType("Scp1576PedestalStructure Variant"));
            List.Add(PrefabType.AntiScp207PedestalStructure, GetPrefabType("AntiScp207PedestalStructure Variant"));
            List.Add(PrefabType.Scp1344PedestalStructure, GetPrefabType("Scp1344PedestalStructure Variant"));
            List.Add(PrefabType.LargeGunLockerStructure, GetPrefabType("LargeGunLockerStructure"));
            List.Add(PrefabType.ExperimentalLockerStructure, GetPrefabType("Experimental Weapon Locker"));
            List.Add(PrefabType.RifleRackStructure, GetPrefabType("RifleRackStructure"));
            List.Add(PrefabType.MiscLocker, GetPrefabType("MiscLocker"));
            List.Add(PrefabType.GeneratorStructure, GetPrefabType("GeneratorStructure"));
            List.Add(PrefabType.WorkstationStructure, GetPrefabType("Spawnable Work Station Structure"));
            List.Add(PrefabType.RegularMedkitStructure, GetPrefabType("RegularMedkitStructure"));
            List.Add(PrefabType.AdrenalineMedkitStructure, GetPrefabType("AdrenalineMedkitStructure"));
            List.Add(PrefabType.HegProjectile, GetPrefabType("HegProjectile"));
            List.Add(PrefabType.FlashbangProjectile, GetPrefabType("FlashbangProjectile"));
            List.Add(PrefabType.Scp173Ragdoll, GetPrefabType("SCP-173_Ragdoll"));
            List.Add(PrefabType.Ragdoll1, GetPrefabType("Ragdoll_1"));
            List.Add(PrefabType.Scp106Ragdoll, GetPrefabType("SCP-106_Ragdoll"));
            List.Add(PrefabType.Ragdoll4, GetPrefabType("Ragdoll_4"));
            List.Add(PrefabType.Ragdoll7, GetPrefabType("Ragdoll_7"));
            List.Add(PrefabType.Ragdoll6, GetPrefabType("Ragdoll_6"));
            List.Add(PrefabType.Ragdoll12, GetPrefabType("Ragdoll_12"));
            List.Add(PrefabType.Ragdoll8, GetPrefabType("Ragdoll_8"));
            List.Add(PrefabType.Ragdoll10, GetPrefabType("Ragdoll_10"));
            List.Add(PrefabType.RagdollTutorial, GetPrefabType("Ragdoll_Tut"));
            List.Add(PrefabType.Scp939Ragdoll, GetPrefabType("SCP-939_Ragdoll"));
            List.Add(PrefabType.Scp3114Ragdoll, GetPrefabType("Scp3114_Ragdoll"));
            List.Add(PrefabType.ElevatorChamber, GetPrefabType("ElevatorChamber"));
            List.Add(PrefabType.ElevatorChamber_Gates, GetPrefabType("ElevatorChamber_Gates"));
            List.Add(PrefabType.ElevatorChamberNuke, GetPrefabType("ElevatorChamberNuke"));
            List.Add(PrefabType.CapybaraToy, GetPrefabType("CapybaraToy"));
            List.Add(PrefabType.Sinkhole, GetPrefabType("Sinkhole"));
            List.Add(PrefabType.AutoRagdoll, GetPrefabType("AutoRagdoll"));
            List.Add(PrefabType.ElevatorChamberCargo, GetPrefabType("ElevatorChamberCargo"));
            List.Add(PrefabType.InvisibleInteractableToy, GetPrefabType("InvisibleInteractableToy"));
            List.Add(PrefabType.EzArmCameraToy, GetPrefabType("EzArmCameraToy"));
            List.Add(PrefabType.EzCameraToy, GetPrefabType("EzCameraToy"));
            List.Add(PrefabType.HczCameraToy, GetPrefabType("HczCameraToy"));
            List.Add(PrefabType.LczCameraToy, GetPrefabType("LczCameraToy"));
            List.Add(PrefabType.SzCameraToy, GetPrefabType("SzCameraToy"));
            List.Add(PrefabType.KeycardPickupChaos, GetPrefabType("KeycardPickup_Chaos"));
            List.Add(PrefabType.TextToy, GetPrefabType("TextToy"));
            List.Add(PrefabType.SpawnableUnsecuredPryableGateDoor, GetPrefabType("Spawnable Unsecured Pryable GateDoor"));
            List.Add(PrefabType.CullableParentToy, GetPrefabType("CullableParentToy"));
            List.Add(PrefabType.WaypointToy, GetPrefabType("WaypointToy"));
            List.Add(PrefabType.PrismaticCloud, GetPrefabType("PrismaticCloud"));
            List.Add(PrefabType.TantrumObjBrownCandy, GetPrefabType("TantrumObj (Brown Candy)"));
            List.Add(PrefabType.Scp1509Pickup, GetPrefabType("Scp1509Pickup"));
            List.Add(PrefabType.HubertMoon, GetPrefabType("Hubert Moon"));
            List.Add(PrefabType.Scp018ProjectileHalloween, GetPrefabType("Scp018Projectile Halloween"));
            List.Add(PrefabType.JailbirdPickupHalloween, GetPrefabType("JailbirdPickup Halloween"));
            List.Add(PrefabType.Scp1509PedestalStructureVariant, GetPrefabType("Scp1509PedestalStructure Variant"));
            List.Add(PrefabType.Scp173RagdollMrNuttySombreroVariant, GetPrefabType("SCP-173 Ragdoll - MrNutty Sombrero Variant"));
            List.Add(PrefabType.Scp049RagdollHalloween, GetPrefabType("SCP-049 Ragdoll Halloween"));
            List.Add(PrefabType.Scp096RagdollHalloween, GetPrefabType("SCP-096 Ragdoll Halloween"));
            List.Add(PrefabType.ZombieRagdollHalloween, GetPrefabType("Zombie Ragdoll Halloween"));
            List.Add(PrefabType.Scp939RagdollHalloween, GetPrefabType("SCP-939 Ragdoll Halloween"));
        }
        public static GameObject Spawn(Vector3 pos, PrefabType prefabType,Quaternion quaternion)
        {
            GameObject gameObject = List[prefabType];
            GameObject AGB = Object.Instantiate(gameObject,pos, quaternion);
            NetworkServer.Spawn(AGB);
            return AGB;
        }
        public static GameObject Spawn(Vector3 pos, PrefabType prefabType)
        {
            GameObject gameObject = List[prefabType];
            GameObject AGB = Object.Instantiate(gameObject, pos, Quaternion.identity);
            NetworkServer.Spawn(AGB);
            return AGB;
        }
        public static void Spawn(PrefabType prefabType, Vector3 pos, out GameObject gameObject)
        {
            gameObject = Spawn(pos, prefabType);
        }
        public static GameObject GetPrefabType(string name)
        {
            return NetworkManager.singleton.spawnPrefabs.First(f => f.name == name);
        }
    }
}
