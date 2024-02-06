using HarmonyLib;
using UnityEngine;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatches
    {
        // Plugin.logger.LogDebug($"Current hour: {__instance.hour}");
        // 1: Opening doors
        // 2: Clock shows up
        // 2+n: 8am + hours * n

        // Periodically check if the teleporter malfunction should be triggered.
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void CheckMalfunctionTeleporterTrigger(TimeOfDay __instance)
        {
            // 1: Opening doors
            // 2: Clock shows up
            // 2+n: 8am + hours * n
            // Plugin.logger.LogDebug($"Current hour: {__instance.hour}");

            // Check if the teleporter malfunction is active and not triggered yet.
            // Already disabled if the power malfunction is active.
            if (
                !State.MalfunctionPower.Active
                && State.MalfunctionTeleporter.Active
                && !State.MalfunctionTeleporter.Triggered
            )
            {
                // If the total  time is past our delay, trigger the teleporter timeout.
                if (__instance.hour >= 1 + State.MalfunctionTeleporter.Delay)
                {
                    Plugin.logger.LogDebug($"Triggered teleporter malfunction!");

                    State.MalfunctionTeleporter.Triggered = true;

                    if (!State.MalfunctionTeleporter.Notified)
                    {
                        // TODO: Swap this out with a custom HUD animation prefab similar to the radation one.
                        HUDManager.Instance.globalNotificationText.text =
                            "SHIP TELEPORTER MALFUNCTION:\nTIMEOUT FROM ATOMIC MISALIGNMENT";

                        HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                        HUDManager.Instance.UIAudio.PlayOneShot(
                            HUDManager.Instance.radiationWarningAudio,
                            1f
                        );

                        // We just notified the player this malfunction is active. Make sure we flag ourselves that way.
                        State.MalfunctionTeleporter.Notified = true;
                    }

                    // Create sparks on the teleporter for the teleporter malfunction.
                    ShipTeleporter teleporterDevice =
                        UnityEngine.Object.FindObjectOfType<ShipTeleporter>();

                    if (teleporterDevice == null)
                    {
                        Plugin.logger.LogError("Failed to find a teleporter object.");

                        return;
                    }

                    // Instantiate the new sparks object on top of the lever.
                    GameObject sparks = Assets.SpawnPrefab(
                        "sparks",
                        teleporterDevice.transform.position
                    );

                    // Assign the sparks to the teleporter so that they move together.
                    sparks.transform.SetParent(teleporterDevice.transform, true);

                    // Make sure it's tracked on the malfunction.
                    State.MalfunctionTeleporter.AssignChild(sparks);
                }
            }
        }
    }
}
