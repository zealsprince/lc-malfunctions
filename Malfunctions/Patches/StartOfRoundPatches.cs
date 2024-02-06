using System;
using System.Linq;
using HarmonyLib;
using Malfunctions.Helpers;
using UnityEngine;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatches
    {
        // When a round ends roll malfunction chances and set resulting states.
        // For navigation malfunction immediately set next moon.
        [HarmonyPostfix]
        [HarmonyPatch("EndOfGame")]
        private static void RollMalfunctions(StartOfRound __instance)
        {
            // Add the current epoch timestamp to the random map seed to still
            // sync up but have a varied experience not bound to seeds alone.
            int dailyEpochUTC = (int)
                (
                    DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd"))
                    - new DateTime(1970, 1, 1)
                ).TotalSeconds;

            int syncedSeed = __instance.randomMapSeed + dailyEpochUTC;

            // Use our map seed combined with the epoch seed.
            System.Random rand = new System.Random(syncedSeed);

            Plugin.logger.LogDebug($"Got random synced seed: {syncedSeed}");

            // Perform the rolls.
            double malfunctionNavigationRoll = rand.NextDouble() * 100;
            double malfunctionTeleporterRoll = rand.NextDouble() * 100;
            double malfunctionDistortionRoll = rand.NextDouble() * 100;
            double malfunctionPowerRoll = rand.NextDouble() * 100;

            // Check our rolls.
            bool malfunctionNavigationRollSucceeded =
                Config.MalfunctionNavigationChance.Value != 0
                && malfunctionNavigationRoll < Config.MalfunctionNavigationChance.Value;

            bool malfunctionTeleporterRollSucceeded =
                Config.MalfunctionTeleporterChance.Value != 0
                && malfunctionTeleporterRoll < Config.MalfunctionTeleporterChance.Value;

            bool malfunctionDistortionRollSucceeded =
                Config.MalfunctionDistortionChance.Value != 0
                && malfunctionDistortionRoll < Config.MalfunctionDistortionChance.Value;

            bool malfunctionPowerRollSucceeded =
                Config.MalfunctionPowerChance.Value != 0
                && malfunctionPowerRoll < Config.MalfunctionPowerChance.Value;

            Plugin.logger.LogDebug(
                $"Malfunction Navigation Roll: {malfunctionNavigationRoll} < {Config.MalfunctionNavigationChance.Value} ({(malfunctionNavigationRollSucceeded ? "SUCCESS" : "FAIL")})"
            );

            Plugin.logger.LogDebug(
                $"Malfunction Teleporter Roll: {malfunctionTeleporterRoll} < {Config.MalfunctionTeleporterChance.Value} ({(malfunctionTeleporterRollSucceeded ? "SUCCESS" : "FAIL")})"
            );

            Plugin.logger.LogDebug(
                $"Malfunction Distortion Roll: {malfunctionDistortionRoll} < {Config.MalfunctionDistortionChance.Value} ({(malfunctionDistortionRollSucceeded ? "SUCCESS" : "FAIL")})"
            );

            Plugin.logger.LogDebug(
                $"Malfunction Power Roll: {malfunctionPowerRoll} < {Config.MalfunctionPowerChance.Value} ({(malfunctionPowerRollSucceeded ? "SUCCESS" : "FAIL")})"
            );

            // Plugin.logger.LogDebug($"Days left: {TimeOfDay.Instance.daysUntilDeadline}");

            #region Navigation

            // If we previously had a navigation malfunction make sure to reset it and any children.
            if (State.MalfunctionNavigation.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionNavigation.Reset();
            }
            else
            {
                // Make sure we can't trigger the navigation malfunction on the last day.
                if (
                    __instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (malfunctionNavigationRollSucceeded)
                    {
                        // Create an array of levels that we can use to filter out ones like the company.
                        SelectableLevel[] filteredLevels = __instance
                            .levels.Where(level => level.name != "CompanyBuildingLevel")
                            .ToArray();

                        // Select the next random level from the list of all levels.
                        SelectableLevel selected = rand.NextFromCollection(filteredLevels);

                        // Set the current level to the one we have now selected.
                        __instance.ChangeLevel(selected.levelID);

                        foreach (SelectableLevel level in filteredLevels)
                        {
                            Plugin.logger.LogDebug(
                                $"Level ID: {level.levelID} / Level Name: {level.name}"
                            );
                        }

                        Plugin.logger.LogDebug($"Selected Level ID: {selected.levelID}");

                        State.MalfunctionNavigation.Active = true;
                    }
                }
            }

            #endregion

            #region Teleporter

            // If we previously had a teleporter malfunction make sure to reset it and any children.
            if (State.MalfunctionTeleporter.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionTeleporter.Reset();
            }
            // Make sure we don't trigger this malfunction if the power one is in place.
            else if (!State.MalfunctionPower.Active)
            {
                // Make sure we can't trigger the teleporter malfunction on the last day.
                if (
                    __instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (malfunctionTeleporterRollSucceeded)
                    {
                        // Only enable the teleporter malfunction if players have a teleporter.
                        if (UnityEngine.Object.FindObjectOfType<ShipTeleporter>() != null)
                        {
                            // Set the teleporter hour delay.
                            State.MalfunctionTeleporter.Delay = rand.Next(12);

                            State.MalfunctionTeleporter.Active = true;

                            Plugin.logger.LogDebug(
                                $"Teleporter malfunction will trigger at {7 + State.MalfunctionTeleporter.Delay}:00"
                            );
                        }
                        else
                        {
                            Plugin.logger.LogDebug(
                                "Didn't find a teleporter! Skipping teleporter malfunction."
                            );
                        }
                    }
                }
            }

            #endregion

            #region Distortion

            // If we previously had a teleporter malfunction make sure to reset it and any children.
            if (State.MalfunctionDistortion.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionDistortion.Reset();
            }
            // Make sure we don't trigger this malfunction if the power one is in place.
            else if (!State.MalfunctionPower.Active)
            {
                // Make sure we can't trigger the distortion malfunction on the last day.
                if (
                    __instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (malfunctionDistortionRollSucceeded)
                    {
                        State.MalfunctionDistortion.Active = true;
                    }
                }
            }

            #endregion

            #region Power

            // If we previously had a power malfunction make sure to reset it and any children.
            if (State.MalfunctionPower.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionPower.Reset();

                // Restore terminal functionality.
                Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
                if (terminal != null)
                {
                    terminal.terminalTrigger.interactable = true;
                }

                // Restore the lights from the door controls.
                GameObject elevatorPanelScreen = UnityEngine.GameObject.Find("ElevatorPanelScreen");
                elevatorPanelScreen?.SetActive(true);

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
            else
            {
                // Make sure we can't trigger the power malfunction on the last day.
                if (
                    __instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (malfunctionPowerRollSucceeded)
                    {
                        State.MalfunctionPower.Active = true;
                    }
                }
            }

            #endregion
        }

        // Update the navigation moon info screen with relevant information.
        [HarmonyPostfix]
        [HarmonyPatch("SetMapScreenInfoToCurrentLevel")]
        [HarmonyAfter(new string[] { "jamil.corporate_restructure" })]
        private static void OverwriteMapScreenInfo(StartOfRound __instance)
        {
            // Make sure to only show the Malfunction notification if it's active, we're not on the company
            // and if it wasn't shown previously such as on a reload of the save.
            if (
                __instance.currentLevel.name != "CompanyBuildingLevel"
                && State.MalfunctionNavigation.Active == true
                && State.MalfunctionNavigation.Notified == false
            )
            {
                // TODO: Swap this out with a custom HUD animation prefab similar to the radation one.
                HUDManager.Instance.globalNotificationText.text =
                    "SHIP NAVIGATION MALFUNCTION:\nROUTE TABLE OFFSET ERROR";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );

                // We just notified the player this malfunction is active. Make sure we flag ourselves that way.
                State.MalfunctionNavigation.Notified = true;

                // Update the screen to have scrambled text.
                __instance.screenLevelDescription.text =
                    "SHIP NAVIGATION MALFUNCTION\nOrbiting: Unknown\nWeather: Unknown\nBE ADVISED THE ROUTE TABLE WILL RECALIBRATE AFTER LANDING";

                // Adopted this code from stromytuna's RouteRandom mod.
                __instance.screenLevelVideoReel.enabled = false;
                __instance.screenLevelVideoReel.clip = null;
                __instance.screenLevelVideoReel.gameObject.SetActive(false);
                __instance.screenLevelVideoReel.Stop();

                // Create sparks on the lever for the navigation malfunction.
                StartMatchLever leverDevice =
                    UnityEngine.Object.FindObjectOfType<StartMatchLever>();

                if (leverDevice == null)
                {
                    Plugin.logger.LogError("Failed to find lever device object.");

                    return;
                }

                // Instantiate the new sparks object on top of the lever.
                GameObject sparks = Assets.SpawnPrefab("sparks", leverDevice.transform.position);

                // Assign the sparks to the lever device so that they move together.
                sparks.transform.SetParent(leverDevice.transform, true);

                // Make sure it's tracked on the malfunction.
                State.MalfunctionNavigation.AssignChild(sparks);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("openingDoorsSequence")]
        private static void TriggerPowerMalfunction(StartOfRound __instance)
        {
            // Check if the power malfunction is active and not triggered yet.
            if (State.MalfunctionPower.Active)
            {
                Plugin.logger.LogDebug($"Triggered power malfunction!");

                State.MalfunctionPower.Triggered = true;

                if (!State.MalfunctionPower.Notified)
                {
                    Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
                    if (terminal != null)
                    {
                        terminal.terminalTrigger.interactable = false;
                    }

                    // Remove the lights from the door controls.
                    GameObject elevatorPanelScreen = UnityEngine.GameObject.Find(
                        "ElevatorPanelScreen"
                    );
                    elevatorPanelScreen?.SetActive(false);

                    // Disable the button control triggers.
                    GameObject hangarDoorButtonPanel = UnityEngine.GameObject.Find(
                        "HangarDoorButtonPanel"
                    );
                    if (hangarDoorButtonPanel != null)
                    {
                        InteractTrigger[] triggers =
                            hangarDoorButtonPanel.GetComponentsInChildren<InteractTrigger>(true);

                        foreach (InteractTrigger trigger in triggers)
                        {
                            trigger.interactable = false;
                        }
                    }

                    // Cause a power outage.
                    __instance.PowerSurgeShip();

                    // Play the stormy weather sound effect from the terminal.
                    StormyWeather stormyWeather =
                        UnityEngine.Object.FindObjectOfType<StormyWeather>(true);
                    stormyWeather.PlayThunderEffects(
                        terminal.transform.position,
                        terminal.terminalAudio
                    );

                    // TODO: Swap this out with a custom HUD animation prefab similar to the radation one.
                    HUDManager.Instance.globalNotificationText.text =
                        "SHIP CORE SURGE:\nRUNNING ON EMERGENCY POWER";

                    HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                    HUDManager.Instance.UIAudio.PlayOneShot(
                        HUDManager.Instance.radiationWarningAudio,
                        1f
                    );

                    // We just notified the player this malfunction is active. Make sure we flag ourselves that way.
                    State.MalfunctionPower.Notified = true;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void Reset()
        {
            // Reset/setup the tracking of the malfunction states and objects.
            State.Reset();
        }
    }
}
