using System;
using System.Linq;
using HarmonyLib;
using LethalNetworkAPI;
using Malfunctions.Helpers;
using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatches
    {
        // Store objects which we search for by name and can't retrieve once inactive.
        private static GameObject elevatorPanelScreen;

        // Store the floodlight objects as their parent gets disabled.
        private static GameObject floodlight1;
        private static GameObject floodlight2;

        // Get the floodlight material since we'll overwrite it and then reset it.
        private static Material floodlightMaterial;

        // Store the state of unrecovered players since it's required in multiple states.
        private static bool hadUnrecoveredDeadPlayers;

        private static string originalLeverDisableTooltip = "";

        public static LethalClientEvent MalfunctionResetNetworkClientMessage =
            new LethalClientEvent("MALFUNCTION_RESET");

        public static void MalfunctionResetNetworkHandler()
        {
            Plugin.logger.LogDebug($"Received networking malfunction reset broadcast!");

            RestoreAfterMalfunctions();
        }

        public static LethalServerMessage<MalfunctionNavigationNetworkData> MalfunctionNavigationNetworkServerMessage =
            new LethalServerMessage<MalfunctionNavigationNetworkData>("MALFUNCTION_NAVIGATION");

        public static LethalClientMessage<MalfunctionNavigationNetworkData> MalfunctionNavigationNetworkClientMessage =
            new LethalClientMessage<MalfunctionNavigationNetworkData>("MALFUNCTION_NAVIGATION");

        public class MalfunctionNavigationNetworkData
        {
            public bool result;
            public int levelId;

            public MalfunctionNavigationNetworkData(bool result, int levelId)
            {
                this.result = result;
                this.levelId = levelId;
            }
        }

        public static void MalfunctionNavigationNetworkHandler(
            MalfunctionNavigationNetworkData message
        )
        {
            Plugin.logger.LogDebug(
                $"Received network message for malfunction navigation roll result: {message.result} (levelId: {message.levelId})"
            );

            HandleRollNavigation(message.result, message.levelId);
        }

        public static LethalServerMessage<MalfunctionTeleporterNetworkData> MalfunctionTeleporterNetworkServerMessage =
            new LethalServerMessage<MalfunctionTeleporterNetworkData>("MALFUNCTION_TELEPORTER");

        public static LethalClientMessage<MalfunctionTeleporterNetworkData> MalfunctionTeleporterNetworkClientMessage =
            new LethalClientMessage<MalfunctionTeleporterNetworkData>("MALFUNCTION_TELEPORTER");

        public class MalfunctionTeleporterNetworkData
        {
            public bool result;
            public int delay;

            public MalfunctionTeleporterNetworkData(bool result, int delay)
            {
                this.result = result;
                this.delay = delay;
            }
        }

        public static void MalfunctionTeleporterNetworkHandler(
            MalfunctionTeleporterNetworkData message
        )
        {
            Plugin.logger.LogDebug(
                $"Received network message for malfunction teleporter roll result: {message.result} (Delay: {message.delay})"
            );

            HandleRollTeleporter(message.result, message.delay);
        }

        public static LethalServerMessage<MalfunctionDistortionNetworkData> MalfunctionDistortionNetworkServerMessage =
            new LethalServerMessage<MalfunctionDistortionNetworkData>("MALFUNCTION_DISTORTION");

        public static LethalClientMessage<MalfunctionDistortionNetworkData> MalfunctionDistortionNetworkClientMessage =
            new LethalClientMessage<MalfunctionDistortionNetworkData>("MALFUNCTION_DISTORTION");

        public class MalfunctionDistortionNetworkData
        {
            public bool result;
            public int delay;

            public MalfunctionDistortionNetworkData(bool result, int delay)
            {
                this.result = result;
                this.delay = delay;
            }
        }

        public static void MalfunctionDistortionNetworkHandler(
            MalfunctionDistortionNetworkData message
        )
        {
            Plugin.logger.LogDebug(
                $"Received network message for malfunction distortion roll result: {message.result} (Delay: {message.delay})"
            );

            HandleRollDistortion(message.result, message.delay);
        }

        public static LethalServerMessage<MalfunctionDoorNetworkData> MalfunctionDoorNetworkServerMessage =
            new LethalServerMessage<MalfunctionDoorNetworkData>("MALFUNCTION_DOOR");

        public static LethalClientMessage<MalfunctionDoorNetworkData> MalfunctionDoorNetworkClientMessage =
            new LethalClientMessage<MalfunctionDoorNetworkData>("MALFUNCTION_DOOR");

        public class MalfunctionDoorNetworkData
        {
            public bool result;
            public int delay;

            public MalfunctionDoorNetworkData(bool result, int delay)
            {
                this.result = result;
                this.delay = delay;
            }
        }

        public static void MalfunctionDoorNetworkHandler(MalfunctionDoorNetworkData message)
        {
            Plugin.logger.LogDebug(
                $"Received network message for malfunction door roll result: {message.result} (Delay: {message.delay})"
            );

            HandleRollDoor(message.result, message.delay);
        }

        public static LethalServerMessage<MalfunctionLeverNetworkData> MalfunctionLeverNetworkServerMessage =
            new LethalServerMessage<MalfunctionLeverNetworkData>("MALFUNCTION_LEVER");

        public static LethalClientMessage<MalfunctionLeverNetworkData> MalfunctionLeverNetworkClientMessage =
            new LethalClientMessage<MalfunctionLeverNetworkData>("MALFUNCTION_LEVER");

        public class MalfunctionLeverNetworkData
        {
            public bool result;
            public int delay;

            public MalfunctionLeverNetworkData(bool result, int delay)
            {
                this.result = result;
                this.delay = delay;
            }
        }

        public static void MalfunctionLeverNetworkHandler(MalfunctionLeverNetworkData message)
        {
            Plugin.logger.LogDebug(
                $"Received network message for malfunction lever roll result: {message.result} (Delay: {message.delay})"
            );

            HandleRollLever(message.result, message.delay);
        }

        public static LethalServerMessage<MalfunctionPowerNetworkData> MalfunctionPowerNetworkServerMessage =
            new LethalServerMessage<MalfunctionPowerNetworkData>("MALFUNCTION_POWER");

        public static LethalClientMessage<MalfunctionPowerNetworkData> MalfunctionPowerNetworkClientMessage =
            new LethalClientMessage<MalfunctionPowerNetworkData>("MALFUNCTION_POWER");

        public class MalfunctionPowerNetworkData
        {
            public bool result;
            public bool blockResult;

            public MalfunctionPowerNetworkData(bool result, bool blockResult)
            {
                this.result = result;
                this.blockResult = blockResult;
            }
        }

        public static void MalfunctionPowerNetworkHandler(MalfunctionPowerNetworkData message)
        {
            Plugin.logger.LogDebug(
                $"Received network message for malfunction power roll result: {message.result} (Block lever: {message.blockResult})"
            );

            HandleRollPower(message.result, message.blockResult);
        }

        public static bool NetworkHandlersRegistered = false;

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void RegisterNetworkHandlers()
        {
            if (!NetworkHandlersRegistered)
            {
                MalfunctionResetNetworkClientMessage.OnReceived += MalfunctionResetNetworkHandler;

                MalfunctionNavigationNetworkClientMessage.OnReceived +=
                    MalfunctionNavigationNetworkHandler;

                MalfunctionTeleporterNetworkClientMessage.OnReceived +=
                    MalfunctionTeleporterNetworkHandler;

                MalfunctionDistortionNetworkClientMessage.OnReceived +=
                    MalfunctionDistortionNetworkHandler;

                MalfunctionDoorNetworkClientMessage.OnReceived += MalfunctionDoorNetworkHandler;

                MalfunctionLeverNetworkClientMessage.OnReceived += MalfunctionLeverNetworkHandler;

                MalfunctionPowerNetworkClientMessage.OnReceived += MalfunctionPowerNetworkHandler;

                Plugin.logger.LogDebug("Registered network handlers!");
            }
        }

        // Check if there were any dead players. Required for the malfunction penalty.
        [HarmonyPrefix]
        [HarmonyPatch("EndOfGame")]
        private static void CheckDeadPlayers(StartOfRound __instance, int bodiesInsured)
        {
            // Previously this was done as a postfix to the EndOfGame function and relying on dead player bodies.
            // I believe the desync would happen because for some players bodies were already cleaned up at that point.

            // Perform the same calculation logic as the EndOfGame function receives and performs.
            int playersDead = __instance.connectedPlayersAmount + 1 - __instance.livingPlayers;
            if (playersDead > bodiesInsured)
            {
                hadUnrecoveredDeadPlayers = true;

                Plugin.logger.LogDebug(
                    $"There were unrecovered players! (Players: {__instance.connectedPlayersAmount + 1} / Dead: {playersDead} / Recovered: {bodiesInsured})"
                );
            }
            else
            {
                hadUnrecoveredDeadPlayers = false;

                Plugin.logger.LogDebug(
                    $"All players are alive or recovered! (Players: {__instance.connectedPlayersAmount + 1} / Dead: {playersDead} / Recovered: {bodiesInsured})"
                );
            }
        }

        // When a round ends and players are revived, roll malfunction chances and set resulting states.
        // For navigation malfunction immediately set next moon.
        [HarmonyPostfix]
        [HarmonyPatch("ReviveDeadPlayers")]
        private static void RollMalfunctions(StartOfRound __instance)
        {
            // Reset everything to a normal state after the malfunctions have passed.
            RestoreAfterMalfunctions();

            // Reset all malfunctions if consecutive mode is enabled to allow triggering even if previously triggerd.
            if (Config.MalfunctionMiscAllowConsecutive.Value)
            {
                State.Reset();
            }

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

            Plugin.logger.LogDebug(
                $"Elapsed days: {__instance.gameStats.daysSpent} / Days to next deadline: {TimeOfDay.Instance.daysUntilDeadline}"
            );

            if (__instance.IsServer)
            {
                #region Chance Rolls

                // Perform the rolls.
                double malfunctionNavigationRoll = rand.NextDouble() * 100;
                double malfunctionTeleporterRoll = rand.NextDouble() * 100;
                double malfunctionDistortionRoll = rand.NextDouble() * 100;
                double malfunctionPowerRoll = rand.NextDouble() * 100;
                double malfunctionLeverRoll = rand.NextDouble() * 100;
                double malfunctionDoorRoll = rand.NextDouble() * 100;

                // Penalty multiplier if a body wasn't recovered.
                double multiplier = 1;
                if (Config.MalfunctionPenaltyEnabled.Value)
                {
                    Plugin.logger.LogDebug(
                        "Penalty multiplier active. Checking for unrecovered players."
                    );

                    if (hadUnrecoveredDeadPlayers == true)
                    {
                        multiplier = Config.MalfunctionPenaltyMultiplier.Value;

                        Plugin.logger.LogDebug(
                            "Had unrecovered players. Increasing malfunction multiplier for this round."
                        );
                    }
                    else if (Config.MalfunctionPenaltyOnly.Value)
                    {
                        multiplier = 0;

                        Plugin.logger.LogDebug(
                            "No unrecovered players. Setting probability to zero as penalty mode only is enabled."
                        );
                    }
                }

                // Check our rolls.
                bool malfunctionNavigationRollSucceeded =
                    Config.MalfunctionChanceNavigation.Value != 0
                    && malfunctionNavigationRoll
                        < Config.MalfunctionChanceNavigation.Value * multiplier;

                bool malfunctionTeleporterRollSucceeded =
                    Config.MalfunctionChanceTeleporter.Value != 0
                    && malfunctionTeleporterRoll
                        < Config.MalfunctionChanceTeleporter.Value * multiplier;

                bool malfunctionDistortionRollSucceeded =
                    Config.MalfunctionChanceDistortion.Value != 0
                    && malfunctionDistortionRoll
                        < Config.MalfunctionChanceDistortion.Value * multiplier;

                bool malfunctionDoorRollSucceeded =
                    Config.MalfunctionChanceDoor.Value != 0
                    && malfunctionDoorRoll < Config.MalfunctionChanceDoor.Value * multiplier;

                bool malfunctionLeverRollSucceeded =
                    Config.MalfunctionChanceLever.Value != 0
                    && malfunctionLeverRoll < Config.MalfunctionChanceLever.Value * multiplier;

                bool malfunctionPowerRollSucceeded =
                    Config.MalfunctionChancePower.Value != 0
                    && malfunctionPowerRoll < Config.MalfunctionChancePower.Value * multiplier;

                Plugin.logger.LogDebug(
                    $"Malfunction Navigation Roll: {malfunctionNavigationRoll} < {Config.MalfunctionChanceNavigation.Value * multiplier} ({(malfunctionNavigationRollSucceeded ? "SUCCESS" : "FAIL")})"
                );

                Plugin.logger.LogDebug(
                    $"Malfunction Teleporter Roll: {malfunctionTeleporterRoll} < {Config.MalfunctionChanceTeleporter.Value * multiplier} ({(malfunctionTeleporterRollSucceeded ? "SUCCESS" : "FAIL")})"
                );

                Plugin.logger.LogDebug(
                    $"Malfunction Distortion Roll: {malfunctionDistortionRoll} < {Config.MalfunctionChanceDistortion.Value * multiplier} ({(malfunctionDistortionRollSucceeded ? "SUCCESS" : "FAIL")})"
                );

                Plugin.logger.LogDebug(
                    $"Malfunction Door Roll: {malfunctionDoorRoll} < {Config.MalfunctionChanceDoor.Value * multiplier} ({(malfunctionDoorRollSucceeded ? "SUCCESS" : "FAIL")})"
                );

                Plugin.logger.LogDebug(
                    $"Malfunction Lever Roll: {malfunctionLeverRoll} < {Config.MalfunctionChanceLever.Value * multiplier} ({(malfunctionLeverRollSucceeded ? "SUCCESS" : "FAIL")})"
                );

                Plugin.logger.LogDebug(
                    $"Malfunction Power Roll: {malfunctionPowerRoll} < {Config.MalfunctionChancePower.Value * multiplier} ({(malfunctionPowerRollSucceeded ? "SUCCESS" : "FAIL")})"
                );

                // Calculate an additional roll for the lever to be broken during the power malfunction.
                double blockLeverRoll = rand.NextDouble() * 100;

                bool blockLeverSucceeded =
                    Config.MalfunctionPowerBlockLeverChance.Value != 0
                    && blockLeverRoll < Config.MalfunctionPowerBlockLeverChance.Value;

                Plugin.logger.LogDebug(
                    $"Malfunction Power - Block Lever Roll : {blockLeverRoll} < {Config.MalfunctionPowerBlockLeverChance.Value} ({(blockLeverSucceeded ? "SUCCESS" : "FAIL")})"
                );

                #endregion

                #region Navigation

                if (
                    StartOfRound.Instance.gameStats.daysSpent
                    <= Config.MalfunctionPassedDaysNavigation.Value
                )
                {
                    Plugin.logger.LogDebug(
                        $"Blocking navigation malfunction because elapsed days hasn't passed required threshold: {StartOfRound.Instance.gameStats.daysSpent} / {Config.MalfunctionPassedDaysNavigation.Value}"
                    );

                    malfunctionNavigationRollSucceeded = false;
                }

                // Create an array of levels that we can use to filter out ones like the company.
                SelectableLevel[] filteredLevels = StartOfRound
                    .Instance.levels.Where(level => level.name != "CompanyBuildingLevel")
                    .ToArray();

                // Select the next random level from the list of all levels.
                SelectableLevel selected = rand.NextFromCollection(filteredLevels);

                if (malfunctionNavigationRollSucceeded)
                {
                    foreach (SelectableLevel level in filteredLevels)
                    {
                        Plugin.logger.LogDebug(
                            $"Level ID: {level.levelID} / Level Name: {level.name}"
                        );
                    }

                    Plugin.logger.LogDebug($"Selected Level ID: {selected.levelID}");
                }

                MalfunctionNavigationNetworkServerMessage.SendAllClients(
                    new MalfunctionNavigationNetworkData(
                        malfunctionNavigationRollSucceeded,
                        selected.levelID
                    )
                );

                #endregion

                #region Teleporter

                if (
                    StartOfRound.Instance.gameStats.daysSpent
                    <= Config.MalfunctionPassedDaysTeleporter.Value
                )
                {
                    Plugin.logger.LogDebug(
                        $"Blocking teleporter malfunction because elapsed days hasn't passed required threshold: {StartOfRound.Instance.gameStats.daysSpent} / {Config.MalfunctionPassedDaysTeleporter.Value}"
                    );

                    malfunctionTeleporterRollSucceeded = false;
                }

                int teleporterDelay = 1 + rand.Next(11);

                MalfunctionTeleporterNetworkServerMessage.SendAllClients(
                    new MalfunctionTeleporterNetworkData(
                        malfunctionTeleporterRollSucceeded,
                        teleporterDelay
                    )
                );

                #endregion

                #region Distortion

                if (
                    StartOfRound.Instance.gameStats.daysSpent
                    <= Config.MalfunctionPassedDaysDistortion.Value
                )
                {
                    Plugin.logger.LogDebug(
                        $"Blocking distortion malfunction because elapsed days hasn't passed required threshold: {StartOfRound.Instance.gameStats.daysSpent} / {Config.MalfunctionPassedDaysDistortion.Value}"
                    );

                    malfunctionDistortionRollSucceeded = false;
                }

                int distortionDelay = rand.Next(12);

                MalfunctionDistortionNetworkServerMessage.SendAllClients(
                    new MalfunctionDistortionNetworkData(
                        malfunctionDistortionRollSucceeded,
                        distortionDelay
                    )
                );

                #endregion

                #region Door

                if (
                    StartOfRound.Instance.gameStats.daysSpent
                    <= Config.MalfunctionPassedDaysDoor.Value
                )
                {
                    Plugin.logger.LogDebug(
                        $"Blocking door malfunction because elapsed days hasn't passed required threshold: {StartOfRound.Instance.gameStats.daysSpent} / {Config.MalfunctionPassedDaysDoor.Value}"
                    );

                    malfunctionDoorRollSucceeded = false;
                }

                int doorDelay = 4 + rand.Next(8);

                MalfunctionDoorNetworkServerMessage.SendAllClients(
                    new MalfunctionDoorNetworkData(malfunctionDoorRollSucceeded, doorDelay)
                );

                #endregion

                #region Lever

                if (
                    StartOfRound.Instance.gameStats.daysSpent
                    <= Config.MalfunctionPassedDaysLever.Value
                )
                {
                    Plugin.logger.LogDebug(
                        $"Blocking lever malfunction because elapsed days hasn't passed required threshold: {StartOfRound.Instance.gameStats.daysSpent} / {Config.MalfunctionPassedDaysLever.Value}"
                    );

                    malfunctionLeverRollSucceeded = false;
                }

                int leverDelay = rand.Next(4);

                MalfunctionLeverNetworkServerMessage.SendAllClients(
                    new MalfunctionLeverNetworkData(malfunctionLeverRollSucceeded, leverDelay)
                );

                #endregion

                #region Power

                if (
                    StartOfRound.Instance.gameStats.daysSpent
                    <= Config.MalfunctionPassedDaysPower.Value
                )
                {
                    Plugin.logger.LogDebug(
                        $"Blocking power malfunction because elapsed days hasn't passed required threshold: {StartOfRound.Instance.gameStats.daysSpent} / {Config.MalfunctionPassedDaysPower.Value}"
                    );

                    malfunctionPowerRollSucceeded = false;
                }

                MalfunctionPowerNetworkServerMessage.SendAllClients(
                    new MalfunctionPowerNetworkData(
                        malfunctionPowerRollSucceeded,
                        blockLeverSucceeded
                    ),
                    false
                );

                HandleRollPower(malfunctionPowerRollSucceeded, blockLeverSucceeded);

                #endregion
            }
        }

        private static void RestoreAfterMalfunctions()
        {
            // Reset the disabled tooltip on the lever device.
            StartMatchLever leverDevice = UnityEngine.Object.FindObjectOfType<StartMatchLever>();

            if (leverDevice == null)
            {
                Plugin.logger.LogError("Failed to find lever device object.");
            }
            else
            {
                leverDevice.triggerScript.disabledHoverTip = originalLeverDisableTooltip;
            }

            // Restore terminal functionality.
            Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
            if (terminal != null)
            {
                terminal.terminalTrigger.interactable = true;
            }

            // Restore the lights from the door controls.
            elevatorPanelScreen?.SetActive(true);

            // Restore door functionality.
            GameObject hangarDoorButtonPanel = GameObject.Find("HangarDoorButtonPanel");
            if (hangarDoorButtonPanel != null)
            {
                InteractTrigger[] triggers =
                    hangarDoorButtonPanel.GetComponentsInChildren<InteractTrigger>(true);

                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = true;
                }
            }

            // If the floodlights were captured, restore them.
            if (floodlight1 != null && floodlight2 != null)
            {
                // Double defintion...
                Light[] floodlight1Lights = floodlight1.GetComponentsInChildren<Light>(true);
                Light[] floodlight2Lights = floodlight2.GetComponentsInChildren<Light>(true);

                // Double for loop...
                foreach (Light light in floodlight1Lights)
                {
                    light.enabled = true;
                }

                foreach (Light light in floodlight2Lights)
                {
                    light.enabled = true;
                }

                // Double mesh...
                MeshRenderer floodlight1Mesh = floodlight1.GetComponent<MeshRenderer>();
                MeshRenderer floodlight2Mesh = floodlight2.GetComponent<MeshRenderer>();

                if (floodlight1Mesh != null && floodlight2Mesh != null)
                {
                    // Capture the materials.
                    Material[] materials = floodlight1Mesh.materials;

                    // Swap back the material
                    materials[2] = floodlightMaterial;

                    // Re-assign the materials.
                    floodlight1Mesh.materials = materials;
                    floodlight2Mesh.materials = materials;
                }
            }
        }

        private static void HandleRollNavigation(bool result, int level)
        {
            // If we previously had a navigation malfunction make sure to reset it and any children.
            if (
                State.MalfunctionNavigation.Active
                || TimeOfDay.Instance.daysUntilDeadline < 2
                    && !Config.MalfunctionMiscAllowConsecutive.Value
            )
            {
                State.MalfunctionNavigation.Reset();
            }
            else
            {
                // Make sure we can't trigger the navigation malfunction on the last day.
                if (
                    StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (result)
                    {
                        // Set the current level to the one we have now selected.
                        StartOfRound.Instance.ChangeLevel(level);

                        State.MalfunctionNavigation.Active = true;
                    }
                }
            }
        }

        private static void HandleRollTeleporter(bool result, int delay)
        {
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
                    StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (result)
                    {
                        // Only enable the teleporter malfunction if players have a teleporter.
                        if (UnityEngine.Object.FindObjectOfType<ShipTeleporter>() != null)
                        {
                            // Set the teleporter hour delay.
                            State.MalfunctionTeleporter.Delay = delay;

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
        }

        public static void HandleRollDistortion(bool result, int delay)
        {
            // If we previously had a distortion malfunction make sure to reset it and any children.
            if (State.MalfunctionDistortion.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionDistortion.Reset();
            }
            // Make sure we don't trigger this malfunction if the power one is in place.
            else if (!State.MalfunctionPower.Active)
            {
                // Make sure we can't trigger the distortion malfunction on the last day.
                if (
                    StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (result)
                    {
                        // Set the distortion hour delay.
                        State.MalfunctionDistortion.Delay = delay;

                        State.MalfunctionDistortion.Active = true;

                        Plugin.logger.LogDebug(
                            $"Distortion malfunction will trigger at {7 + State.MalfunctionDistortion.Delay}:00"
                        );
                    }
                }
            }
        }

        public static void HandleRollDoor(bool result, int delay)
        {
            // If we previously had a door malfunction make sure to reset it and any children.
            if (State.MalfunctionDoor.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionDoor.Reset();
            }
            // Make sure we don't trigger this malfunction if the power one is in place.
            else if (!State.MalfunctionPower.Active)
            {
                // Make sure we can't trigger the distortion malfunction on the last day.
                if (
                    StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (result)
                    {
                        // Make sure we capture the door panel for later.
                        elevatorPanelScreen = GameObject.Find("ElevatorPanelScreen");

                        // Set the door hour delay.
                        State.MalfunctionDoor.Delay = delay;

                        State.MalfunctionDoor.Active = true;

                        Plugin.logger.LogDebug(
                            $"Door malfunction will trigger at {7 + State.MalfunctionDoor.Delay}:00"
                        );
                    }
                }
            }
        }

        public static void HandleRollLever(bool result, int delay)
        {
            // If we previously had a lever malfunction make sure to reset it and any children.
            if (State.MalfunctionLever.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionLever.Reset();
            }
            // Make sure we don't trigger this malfunction if the power or door one is in place.
            // The door one is singled out as this would be unfun gameplay to be stuck in the ship
            // with nothing to do and no take-off scenario.
            else if (!State.MalfunctionPower.Active && !State.MalfunctionDoor.Active)
            {
                // Make sure we can't trigger the lever malfunction on the last day.
                if (
                    StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (result)
                    {
                        // Set the lever hour delay.
                        State.MalfunctionLever.Delay = delay;

                        State.MalfunctionLever.Active = true;

                        Plugin.logger.LogDebug(
                            $"Lever malfunction will trigger at {12 + State.MalfunctionLever.Delay}:00"
                        );
                    }
                }
            }
        }

        public static void HandleRollPower(bool result, bool blockLever)
        {
            // If we previously had a power malfunction make sure to reset it and any children.
            if (State.MalfunctionPower.Active || TimeOfDay.Instance.daysUntilDeadline < 2)
            {
                State.MalfunctionPower.Reset();
            }
            else
            {
                // Make sure we can't trigger the power malfunction on the last day.
                if (
                    StartOfRound.Instance.currentLevel.name != "CompanyBuildingLevel"
                    && TimeOfDay.Instance.daysUntilDeadline >= 2
                )
                {
                    if (result)
                    {
                        // Make sure we capture the door panel for later.
                        elevatorPanelScreen = GameObject.Find("ElevatorPanelScreen");

                        State.MalfunctionPower.Active = true;

                        // The delay value here represents whether or not the lever is blocked.
                        if (Config.MalfunctionPowerBlockLever.Value && blockLever)
                        {
                            State.MalfunctionPower.Delay = 1;

                            // Change the lever disabled tooltip for all players.
                            StartMatchLever leverDevice =
                                UnityEngine.Object.FindObjectOfType<StartMatchLever>();

                            if (leverDevice == null)
                            {
                                Plugin.logger.LogError("Failed to find lever device object.");

                                return;
                            }

                            leverDevice.triggerScript.disabledHoverTip =
                                "[There is no power to the hydraulics system]";
                        }
                        else
                        {
                            State.MalfunctionPower.Delay = 0;
                        }
                    }
                }
            }
        }

        // Update the navigation moon info screen with relevant information.
        [HarmonyPostfix]
        [HarmonyPatch("SetMapScreenInfoToCurrentLevel")]
        [HarmonyAfter(new string[] { "jamil.corporate_restructure", "WeatherTweaks" })]
        private static void OverwriteMapScreenInfo(StartOfRound __instance)
        {
            // Get the lever device so we can capture its original tooltip.
            StartMatchLever leverDevice = UnityEngine.Object.FindObjectOfType<StartMatchLever>();

            if (leverDevice == null)
            {
                Plugin.logger.LogError("Failed to find lever device object.");
            }
            else
            {
                originalLeverDisableTooltip = leverDevice.triggerScript.disabledHoverTip;
            }

            // Make sure to only show the Malfunction notification if it's active, we're not on the company
            // and if it wasn't shown previously such as on a reload of the save.
            if (
                __instance.currentLevel.name != "CompanyBuildingLevel"
                && State.MalfunctionNavigation.Active == true
                && State.MalfunctionNavigation.Notified == false
            )
            {
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
                    if (floodlight1 != null && floodlight2 != null)
                    {
                        // Double defintion...
                        Light[] floodlight1Lights = floodlight1.GetComponentsInChildren<Light>(
                            true
                        );
                        Light[] floodlight2Lights = floodlight2.GetComponentsInChildren<Light>(
                            true
                        );

                        // Double for loop...
                        foreach (Light light in floodlight1Lights)
                        {
                            light.enabled = false;
                        }

                        foreach (Light light in floodlight2Lights)
                        {
                            light.enabled = false;
                        }

                        // Double mesh...
                        MeshRenderer floodlight1Mesh = floodlight1.GetComponent<MeshRenderer>();
                        MeshRenderer floodlight2Mesh = floodlight2.GetComponent<MeshRenderer>();

                        if (floodlight1Mesh != null && floodlight2Mesh != null)
                        {
                            // Capture the materials.
                            Material[] materials = floodlight1Mesh.materials;

                            // Swap the material
                            materials[2] = floodlight1Mesh.materials[0];

                            // Re-assign the materials.
                            floodlight1Mesh.materials = materials;
                            floodlight2Mesh.materials = materials;
                        }
                    }

                    Terminal terminal = UnityEngine.Object.FindObjectOfType<Terminal>();
                    if (terminal != null)
                    {
                        terminal.terminalTrigger.interactable = false;
                    }

                    // Remove the lights from the door controls.
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
        [HarmonyPatch("OnShipLandedMiscEvents")]
        private static void CaptureFloodlights()
        {
            if (floodlightMaterial == null)
            {
                // Capture the floodlights while they are active.
                floodlight1 = GameObject.Find("Floodlight1");
                floodlight2 = GameObject.Find("Floodlight2");

                if (floodlight1 == null || floodlight2 == null)
                {
                    Plugin.logger.LogWarning("Failed to capture floodlight gameobjects!");
                }
                else
                {
                    MeshRenderer floodlight1Mesh = floodlight1.GetComponent<MeshRenderer>();

                    if (floodlight1Mesh != null)
                    {
                        // Set the material.
                        floodlightMaterial = floodlight1Mesh.materials[2];
                    }
                    else { }
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
