using HarmonyLib;
using UnityEngine;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(HangarShipDoor))]
    internal class HangarShipDoorPatches
    {
        private static float lockdownTime;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void OverwriteDoorPower(HangarShipDoor __instance)
        {
            // Overwrite the door power and change the display text.
            if (
                !State.MalfunctionPower.Active
                && State.MalfunctionDoor.Active
                && State.MalfunctionDoor.Triggered
            )
            {
                // Make sure the power is constantly maxed out.
                __instance.doorPower = 1f;

                // Cycle between two text messages.
                lockdownTime = (lockdownTime + Time.deltaTime) % 6;
                __instance.doorPowerDisplay.text = $"{(lockdownTime > 3 ? "LOCKED" : "OPEN 10PM")}";
            }
        }
    }
}
