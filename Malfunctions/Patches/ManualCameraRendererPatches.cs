using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(ManualCameraRenderer))]
    internal class ManualCameraRendererPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("SwitchScreenButton")]
        private static bool InterruptSwitchScreenButton()
        {
            // Interrupt the screen button if a power malfunction has triggered.
            if (State.MalfunctionPower.Active && State.MalfunctionPower.Triggered)
            {
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("SwitchRadarTargetClientRpc")]
        private static bool InterruptSwitchCameraView()
        {
            // Interrupt the screen button if a power malfunction has triggered.
            if (State.MalfunctionPower.Active && State.MalfunctionPower.Triggered)
            {
                return false;
            }

            return true;
        }
    }
}
