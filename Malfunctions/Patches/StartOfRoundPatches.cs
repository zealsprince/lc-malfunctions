using System;
using System.Linq;
using Dissonance;
using HarmonyLib;
using Malfunctions.Helpers;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatches
    {
        private static readonly System.Random rand = new System.Random();

        [HarmonyPostfix]
        [HarmonyPatch("EndOfGame")]
        public static void TriggerMapMalfunction(StartOfRound __instance)
        {
            Plugin.logger.LogDebug($"Days left: {TimeOfDay.Instance.daysUntilDeadline}");

            // If we previously had a malfunction make sure to unset it.
            if (State.malfunctionNavigation || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.malfunctionNavigation = false;
            }
            else
            {
                // Make sure we can't trigger the navigation malfunction on the last day.
                if (
                    __instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    // Chance of navigation malfunctioning.
                    double chance = Config.MalfunctionNavigationChance.Value;
                    double roll = rand.NextDouble() * 100;

                    Plugin.logger.LogDebug(
                        $"Roll: {roll} / {chance} ({(roll < chance ? "SUCCESS" : "FAIL")})"
                    );

                    if (chance != 0 && roll < chance)
                    {
                        // Create an array of levels that we can use to filter out ones like the company.
                        SelectableLevel[] filteredLevels = __instance
                            .levels.Where(level => level.name != "CompanyBuildingLevel")
                            .ToArray();

                        // Select the next random level from the list of all levels.
                        SelectableLevel selected = rand.NextFromCollection(__instance.levels);

                        // Set the current level to the one we have now selected.
                        __instance.ChangeLevel(selected.levelID);

                        foreach (SelectableLevel level in filteredLevels)
                        {
                            Plugin.logger.LogDebug(
                                $"Level ID: {level.levelID} / Level Name: {level.name}"
                            );
                        }

                        Plugin.logger.LogDebug($"Selected Level ID: {selected.levelID}");

                        State.malfunctionNavigation = true;
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("SetMapScreenInfoToCurrentLevel")]
        public static void HandleMalfunction(StartOfRound __instance)
        {
            if (
                __instance.currentLevel.name != "CompanyBuildingLevel"
                && State.malfunctionNavigation == true
            )
            {
                // TODO: Swap this out with a custom HUD animation prefab similar to the radation one.
                HUDManager.Instance.globalNotificationText.text =
                    "SHIP MALFUNCTION:\nROUTE TABLE OFFSET ERROR";

                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(
                    HUDManager.Instance.radiationWarningAudio,
                    1f
                );

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
                    Plugin.logger.LogError("Failed to find Terminal object.");

                    return;
                }

                // Instantiate the new sparks object on top of the lever.
                State.sparksNavigation = Assets.SpawnPrefab(
                    "sparks",
                    leverDevice.transform.position
                );

                // Assign the sparks to the map device so that they move together.
                State.sparksNavigation.transform.SetParent(leverDevice.transform, true);
            }
            else
            {
                // If there is no malfunction, destroy the sparks objects again.
                if (State.sparksNavigation != null)
                {
                    UnityEngine.Object.Destroy(State.sparksNavigation);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Reset(StartOfRound __instance)
        {
            // Reset/setup the tracking of the malfunction states and objects.
            State.Reset();
        }
    }
}
