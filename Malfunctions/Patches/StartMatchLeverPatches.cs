using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(StartMatchLever))]
    internal class StartMatchLeverPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("PullLever")]
        private static bool InterruptPullLever(StartMatchLever __instance)
        {
            // Interrupt pulling the lever if a power malfunction has triggered.
            if (State.MalfunctionPower.Active && State.MalfunctionPower.Triggered)
            {
                if (Config.MalfunctionPowerBlockLever.Value && State.MalfunctionPower.Delay != 0)
                {
                    // TODO: Swap this out with a custom HUD animation prefab similar to the radation one.
                    HUDManager.Instance.globalNotificationText.text =
                        "SHIP CORE DEPLETION:\nAWAIT 12AM EMERGENCY AUTOPILOT";

                    HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                    HUDManager.Instance.UIAudio.PlayOneShot(
                        HUDManager.Instance.radiationWarningAudio,
                        1f
                    );

                    __instance.leverHasBeenPulled = false;
                    __instance.triggerScript.interactable = true;

                    __instance.leverAnimatorObject.SetBool(
                        "pullLever",
                        !__instance.leverHasBeenPulled
                    );

                    return false;
                }
            }

            return true;
        }
    }
}
