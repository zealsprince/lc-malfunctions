using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal class ShipTeleporterPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("PressTeleportButtonOnLocalClient")]
        private static bool InterruptPressTeleportButton(ShipTeleporter __instance)
        {
            // Interrupt the teleporter if the teleporter or power malfunction has triggered.
            if (
                State.MalfunctionTeleporter.Active && State.MalfunctionTeleporter.Triggered
                || State.MalfunctionPower.Active && State.MalfunctionPower.Triggered
            )
            {
                // Still do the animation and play the sound.
                __instance.buttonAnimator.SetTrigger("press");
                __instance.buttonAudio.PlayOneShot(__instance.buttonPressSFX);

                return false;
            }

            return true;
        }
    }
}
