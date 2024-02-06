using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(ItemCharger))]
    internal class ItemChargerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("ChargeItem")]
        private static bool InterruptChargeItem()
        {
            // Interrupt the charger if a power malfunction has triggered.
            if (State.MalfunctionPower.Active && State.MalfunctionPower.Triggered)
            {
                return false;
            }

            return true;
        }
    }
}
