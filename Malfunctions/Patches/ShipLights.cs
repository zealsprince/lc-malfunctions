using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(ShipLights))]
    internal class ShipLightsPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ToggleShipLights")]
        private static bool InterruptToggleShipLights()
        {
            // Interrupt light switching if a power malfunction has triggered.
            if (State.MalfunctionPower.Active && State.MalfunctionPower.Triggered)
            {
                return false;
            }

            return true;
        }
    }
}
