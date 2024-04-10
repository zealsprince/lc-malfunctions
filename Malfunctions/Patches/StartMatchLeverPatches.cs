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
                    HUDManager.Instance.globalNotificationText.text =
                        "SHIP CORE DEPLETION:\nAWAIT 12AM EMERGENCY AUTOPILOT";

                    HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                    HUDManager.Instance.UIAudio.PlayOneShot(
                        HUDManager.Instance.radiationWarningAudio,
                        1f
                    );

                    // Make sure that the tooltip specifies this.
                    __instance.triggerScript.disabledHoverTip = "[No power to hydraulics]";

                    return false;
                }
            }
            else if (State.MalfunctionLever.Active && State.MalfunctionLever.Triggered)
            {
                HUDManager.Instance.globalNotificationText.text =
                    "SHIP LEVER HYDRAULICS JAM:\nAWAIT 12AM EMERGENCY AUTOPILOT";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );

                return false;
            }

            return true;
        }
    }
}
