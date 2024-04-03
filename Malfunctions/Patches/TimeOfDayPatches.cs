using HarmonyLib;
using UnityEngine;

namespace Malfunctions.Patches
{
    // 1: Opening doors
    // 2: Clock shows up
    // 2+n: 8am + hours * n
    // Plugin.logger.LogDebug($"Current hour: {__instance.hour}");

    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatches
    {
        // Periodically check if the teleporter malfunction should be triggered.
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void CheckMalfunctionTeleporterTrigger(TimeOfDay __instance)
        {
            // Check if the teleporter malfunction is active and not triggered yet.
            // Already disabled if the power malfunction is active.
            if (
                !State.MalfunctionPower.Active
                && State.MalfunctionTeleporter.Active
                && !State.MalfunctionTeleporter.Triggered
            )
            {
                // If the total  time is past our delay, trigger the teleporter timeout.
                if (
                    __instance.currentDayTimeStarted
                    && __instance.hour >= 1 + State.MalfunctionTeleporter.Delay
                )
                {
                    Plugin.logger.LogDebug($"Triggered teleporter malfunction!");

                    State.MalfunctionTeleporter.Triggered = true;

                    if (!State.MalfunctionTeleporter.Notified)
                    {
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

        // Periodically check if the distortion malfunction should be triggered.
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void CheckMalfunctionDistortionTrigger(TimeOfDay __instance)
        {
            // Check if the distortion malfunction is active and not triggered yet.
            // Already disabled if the power malfunction is active.
            if (
                !State.MalfunctionPower.Active
                && State.MalfunctionDistortion.Active
                && !State.MalfunctionDistortion.Triggered
            )
            {
                // If the total  time is past our delay, trigger the distortion effect.
                if (
                    __instance.currentDayTimeStarted
                    && __instance.hour >= 1 + State.MalfunctionDistortion.Delay
                )
                {
                    Plugin.logger.LogDebug($"Triggered distortion malfunction!");

                    State.MalfunctionDistortion.Triggered = true;

                    if (!State.MalfunctionDistortion.Notified)
                    {
                        HUDManager.Instance.globalNotificationText.text =
                            "SHIP COMS DISTURBANCE:\nELECTROMAGNETIC ANOMALY";

                        HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                        HUDManager.Instance.UIAudio.PlayOneShot(
                            HUDManager.Instance.radiationWarningAudio,
                            1f
                        );

                        // We just notified the player this malfunction is active. Make sure we flag ourselves that way.
                        State.MalfunctionDistortion.Notified = true;
                    }

                    // Disable the map screen.
                    __instance.playersManager.mapScreen.SwitchScreenOn(on: false);

                    // Create sparks on the terminal and disable it for the distortion malfunction.
                    Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();

                    if (terminal == null)
                    {
                        Plugin.logger.LogError("Failed to find the terminal object.");

                        return;
                    }

                    // Disable interaction with the terminal.
                    terminal.gameObject.GetComponent<InteractTrigger>().interactable = false;

                    // Instantiate the new sparks object on top of the lever.
                    GameObject sparks = Assets.SpawnPrefab("sparks", terminal.transform.position);

                    // Assign the sparks to the teleporter so that they move together.
                    sparks.transform.SetParent(terminal.transform, true);

                    // Make sure it's tracked on the malfunction.
                    State.MalfunctionDistortion.AssignChild(sparks);
                }
            }
        }

        // Periodically check if the door malfunction should be triggered.
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void CheckMalfunctionDoorTrigger(TimeOfDay __instance)
        {
            // Check if the door malfunction is active and not triggered yet.
            // Already disabled if the power malfunction is active.
            if (
                !State.MalfunctionPower.Active
                && State.MalfunctionDoor.Active
                && !State.MalfunctionDoor.Triggered
            )
            {
                // If the total  time is past our delay, trigger the door timeout. Don't trigger after 10pm if reset.
                if (
                    __instance.currentDayTimeStarted
                    && __instance.hour >= 1 + State.MalfunctionDoor.Delay
                    && __instance.hour < 16
                )
                {
                    Plugin.logger.LogDebug($"Triggered door malfunction!");

                    State.MalfunctionDoor.Triggered = true;

                    if (!State.MalfunctionDoor.Notified)
                    {
                        HUDManager.Instance.globalNotificationText.text =
                            "SHIP DOOR LOCK FAIL:\nCRACKDOWN PROTOCOL ACTIVE";

                        HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                        HUDManager.Instance.UIAudio.PlayOneShot(
                            HUDManager.Instance.radiationWarningAudio,
                            1f
                        );

                        // We just notified the player this malfunction is active. Make sure we flag ourselves that way.
                        State.MalfunctionDoor.Notified = true;
                    }

                    // Close the doors during the malfunction.
                    HangarShipDoor hangarShipDoor =
                        UnityEngine.GameObject.FindAnyObjectByType<HangarShipDoor>();
                    if (hangarShipDoor != null)
                    {
                        hangarShipDoor.SetDoorClosed();
                        hangarShipDoor.PlayDoorAnimation(true);
                    }

                    // Create sparks on the teleporter for the teleporter malfunction.
                    GameObject hangarDoorButtonPanel = UnityEngine.GameObject.Find(
                        "HangarDoorButtonPanel"
                    );

                    if (hangarDoorButtonPanel == null)
                    {
                        Plugin.logger.LogError("Failed to find door panel object.");

                        return;
                    }
                    else
                    {
                        InteractTrigger[] triggers =
                            hangarDoorButtonPanel.GetComponentsInChildren<InteractTrigger>(true);

                        foreach (InteractTrigger trigger in triggers)
                        {
                            trigger.interactable = false;
                        }
                    }

                    // Instantiate the new sparks object on top of the lever.
                    GameObject sparks = Assets.SpawnPrefab(
                        "sparks",
                        hangarDoorButtonPanel.transform.position
                    );

                    // Assign the sparks to the teleporter so that they move together.
                    sparks.transform.SetParent(hangarDoorButtonPanel.transform, true);

                    // Make sure it's tracked on the malfunction.
                    State.MalfunctionDoor.AssignChild(sparks);
                }
            }
            else if ( // Check if we should restore functionality before the end of the day.
                !State.MalfunctionPower.Active
                && State.MalfunctionDoor.Active
                && State.MalfunctionDoor.Triggered
            )
            {
                // If the total time is past 10pm restore door functionality.
                if (__instance.hour >= 16)
                {
                    // Reset the triggered state.
                    State.MalfunctionDoor.Triggered = false;

                    // Close the doors during the malfunction.
                    HangarShipDoor hangarShipDoor =
                        UnityEngine.GameObject.FindAnyObjectByType<HangarShipDoor>();
                    if (hangarShipDoor != null)
                    {
                        hangarShipDoor.SetDoorOpen();
                        hangarShipDoor.PlayDoorAnimation(false);
                    }

                    // Restore door functionality.
                    GameObject hangarDoorButtonPanel = UnityEngine.GameObject.Find(
                        "HangarDoorButtonPanel"
                    );
                    if (hangarDoorButtonPanel != null)
                    {
                        InteractTrigger[] triggers =
                            hangarDoorButtonPanel.GetComponentsInChildren<InteractTrigger>(true);

                        foreach (InteractTrigger trigger in triggers)
                        {
                            trigger.interactable = true;
                        }
                    }
                }
            }
        }

        // Periodically check if the lever malfunction should be triggered.
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void CheckMalfunctionLeverTrigger(TimeOfDay __instance)
        {
            // Check if the lever malfunction is active and not triggered yet.
            // Already disabled if the power malfunction is active.
            if (
                !State.MalfunctionPower.Active
                && !State.MalfunctionDoor.Active
                && State.MalfunctionLever.Active
            )
            {
                // If the total time is past our delay - 4, trigger the lever notification. Don't trigger after 10pm.
                if (
                    !State.MalfunctionLever.Notified
                    && __instance.currentDayTimeStarted
                    && __instance.hour >= 3 + State.MalfunctionLever.Delay
                    && __instance.hour < 16
                )
                {
                    Plugin.logger.LogDebug("Notified lever malfunction!");

                    // Make sure we don't display 14pm etc.
                    int value = State.MalfunctionLever.Delay;
                    if (value == 0)
                    {
                        value = 12;
                    }

                    HUDManager.Instance.globalNotificationText.text =
                        $"MANUAL HYDRAULICS FAILURE IMMINENT:\nLEAVE BEFORE {value}PM OR AWAIT AUTOPILOT";

                    HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                    HUDManager.Instance.UIAudio.PlayOneShot(
                        HUDManager.Instance.radiationWarningAudio,
                        1f
                    );

                    // We just notified the player this malfunction is active. Make sure we flag ourselves that way.
                    State.MalfunctionLever.Notified = true;
                }
                // If the total time is past our delay, trigger the lever timeout. Don't trigger after 10pm.
                else if (
                    !State.MalfunctionLever.Triggered
                    && __instance.currentDayTimeStarted
                    && __instance.hour >= 6 + State.MalfunctionLever.Delay
                    && __instance.hour < 16
                )
                {
                    Plugin.logger.LogDebug("Triggered lever malfunction!");

                    State.MalfunctionLever.Triggered = true;

                    // Change the lever disabled tooltip for all players.
                    StartMatchLever leverDevice =
                        UnityEngine.Object.FindObjectOfType<StartMatchLever>();

                    if (leverDevice == null)
                    {
                        Plugin.logger.LogError("Failed to find lever device object.");

                        return;
                    }

                    leverDevice.triggerScript.disabledHoverTip = "[The lever is jammed]";

                    // Instantiate the new sparks object on top of the lever.
                    GameObject sparks = Assets.SpawnPrefab(
                        "sparks",
                        leverDevice.transform.position
                    );

                    // Assign the sparks to the lever device so that they move together.
                    sparks.transform.SetParent(leverDevice.transform, true);

                    // Make sure it's tracked on the malfunction.
                    State.MalfunctionLever.AssignChild(sparks);
                }
            }
        }
    }
}
