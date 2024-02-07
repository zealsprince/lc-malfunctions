using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkieTalkiePatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ItemActivate")]
        private static bool InterruptItemActivate(WalkieTalkie __instance)
        {
            // Interrupt the walkie if a distortion malfunction has triggered.
            if (State.MalfunctionDistortion.Active && State.MalfunctionDistortion.Triggered)
            {
                // Play a sound and dicharge the batteries.
                __instance.thisAudio.PlayOneShot(__instance.playerDieOnWalkieTalkieSFX);
                __instance.UseUpBatteries();

                return false;
            }

            return true;
        }
    }
}
