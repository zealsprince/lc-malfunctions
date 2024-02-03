
using System;
using System.Linq;

using HarmonyLib;

using Malfunctions.Helpers;

namespace Malfunctions.Patches
{
	[HarmonyPatch(typeof(StartOfRound))]
	public class StartOfRoundPatches
    {
        private static readonly System.Random rand = new System.Random();

        public static bool malfunctionNavigation = false; // Whether the navigation malfunction is active.

        [HarmonyPostfix]
        [HarmonyPatch("EndOfGame")]
        public static void TriggerMapMalfunction(StartOfRound __instance) {
            MalfunctionsBase.Log.LogDebug(
                String.Format(
                    "Days left: {0}",
                    TimeOfDay.Instance.daysUntilDeadline
                )
            );

            // If we previously had a malfunction make sure to unset it.
            if(malfunctionNavigation || TimeOfDay.Instance.daysUntilDeadline < 2) {
                malfunctionNavigation = false;

			} else {
                // Make sure we can't trigger the navigation malfunction on the last day.
                if (__instance.currentLevel.name != "CompanyBuildingLevel" && TimeOfDay.Instance.daysUntilDeadline >= 2) {

                    // Chance of navigation malfunctioning.
                    double chance = MalfunctionsBase.ConfigMalfunctionNavigationChance.Value;
                    double roll = rand.NextDouble() * 100;

                    MalfunctionsBase.Log.LogDebug(String.Format("Roll: {0} / {1} ({2})", roll, chance, roll < chance ? "SUCCESS" : "FAIL"));
  
                    if (chance != 0 && roll < chance) {
                        // Create an array of levels that we can use to filter out ones like the company.
                        SelectableLevel[] filteredLevels = __instance.levels.Where(level => level.name != "CompanyBuildingLevel").ToArray();

                        // Select the next random level from the list of all levels.
                        SelectableLevel selected = rand.NextFromCollection(__instance.levels);

                        // Set the current level to the one we have now selected.
                        __instance.ChangeLevel(selected.levelID);

                        foreach (SelectableLevel level in filteredLevels) {
                            MalfunctionsBase.Log.LogDebug(
                                String.Format(
                                    "Level ID: {0} / Level Name: {1}",
                                    level.levelID,
                                    level.name
                                )
                            );
                        }

                        MalfunctionsBase.Log.LogDebug(String.Format("Selected Level ID: {0}", selected.levelID));

                        malfunctionNavigation = true;
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("SetMapScreenInfoToCurrentLevel")]
        public static void HideMapScreenInfo(StartOfRound __instance) {
            if (__instance.currentLevel.name != "CompanyBuildingLevel" && malfunctionNavigation == true) {
                // TODO: Swap this out with a custom HUD animation prefab similar to the radation one.
                HUDManager.Instance.globalNotificationText.text = "SHIP MALFUNCTION: NAVIGATION COMPROMISED";
                HUDManager.Instance.globalNotificationAnimator.SetTrigger("TriggerNotif");
                HUDManager.Instance.UIAudio.PlayOneShot(HUDManager.Instance.radiationWarningAudio, 1f);

                // Update the screen to have scrambled text.
                __instance.screenLevelDescription.text = "SHIP NAVIGATION MALFUNCTION\nOrbiting: Unknown\nPopulation: Unknown\nConditions: Unknown\nFauna: Unknown\nWeather: Unknown";
                __instance.screenLevelVideoReel.enabled = false;

                // For some reason just setting .enabled to false here didn't work, so we also undo the other stuff it sets
                __instance.screenLevelVideoReel.clip = null;
                __instance.screenLevelVideoReel.gameObject.SetActive(false);
                __instance.screenLevelVideoReel.Stop();
            }
		}
	}
}
