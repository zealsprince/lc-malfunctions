using BepInEx.Configuration;

namespace Malfunctions
{
    public class Config
    {
        public static ConfigEntry<double> MalfunctionChanceNavigation;
        public static ConfigEntry<double> MalfunctionChanceTeleporter;
        public static ConfigEntry<double> MalfunctionChanceDistortion;
        public static ConfigEntry<double> MalfunctionChanceDoor;
        public static ConfigEntry<double> MalfunctionChancePower;

        public static ConfigEntry<int> MalfunctionPassedDaysNavigation;
        public static ConfigEntry<int> MalfunctionPassedDaysTeleporter;
        public static ConfigEntry<int> MalfunctionPassedDaysDistortion;
        public static ConfigEntry<int> MalfunctionPassedDaysDoor;
        public static ConfigEntry<int> MalfunctionPassedDaysPower;

        public static ConfigEntry<bool> MalfunctionPenaltyEnabled;
        public static ConfigEntry<bool> MalfunctionPenaltyOnly;
        public static ConfigEntry<double> MalfunctionPenaltyMultiplier;

        public static ConfigEntry<bool> MalfunctionPowerBlockLever;
        public static ConfigEntry<double> MalfunctionPowerBlockLeverChance;

        public static ConfigEntry<bool> MalfunctionMiscAllowConsecutive;

        public static void Load()
        {
            MalfunctionChanceNavigation = Plugin.config.Bind(
                "Chances",
                "MalfunctionChanceNavigation",
                7.5,
                new ConfigDescription(
                    "Set the chance of the navigation malfunction happening - this will force the ship to route to a random moon with no regard to cost",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionChanceTeleporter = Plugin.config.Bind(
                "Chances",
                "MalfunctionChanceTeleporter",
                7.5,
                new ConfigDescription(
                    "Set the chance of the teleporter malfunction happening - this will cause teleporters to disable themselves either at landing or after a random interval into the match",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionChanceDistortion = Plugin.config.Bind(
                "Chances",
                "MalfunctionChanceDistortion",
                5.0,
                new ConfigDescription(
                    "Set the chance of the distortion malfunction happening - this will cause the map and terminal displays as well as walkies to become unusable either at landing or after a random interval into the match",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionChanceDoor = Plugin.config.Bind(
                "Chances",
                "MalfunctionChanceDoor",
                3.0,
                new ConfigDescription(
                    "Set the chance of the door malfunction happening - this will disable ship door controls either at landing or after a random interval into the match",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionChancePower = Plugin.config.Bind(
                "Chances",
                "MalfunctionChancePower",
                1.5,
                new ConfigDescription(
                    "Set the chance of the power malfunction happening - this will make the entire ship to go dark after landing, disabling battery charging, door controls, terminal and map displays",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionPassedDaysNavigation = Plugin.config.Bind(
                "Passed Days",
                "MalfunctionPassedDaysNavigation",
                3,
                new ConfigDescription(
                    "Set how many days must have passed for the navigation malfunction to enable"
                )
            );

            MalfunctionPassedDaysTeleporter = Plugin.config.Bind(
                "Passed Days",
                "MalfunctionPassedDaysTeleporter",
                11,
                new ConfigDescription(
                    "Set how many days must have passed for the teleporter malfunction to enable"
                )
            );

            MalfunctionPassedDaysDistortion = Plugin.config.Bind(
                "Passed Days",
                "MalfunctionPassedDaysDistortion",
                3,
                new ConfigDescription(
                    "Set how many days must have passed for the distortion malfunction to enable"
                )
            );

            MalfunctionPassedDaysDoor = Plugin.config.Bind(
                "Passed Days",
                "MalfunctionPassedDaysDoor",
                7,
                new ConfigDescription(
                    "Set how many days must have passed for the door malfunction to enable"
                )
            );

            MalfunctionPassedDaysPower = Plugin.config.Bind(
                "Passed Days",
                "MalfunctionPassedDaysPower",
                11,
                new ConfigDescription(
                    "Set how many days must have passed for the power malfunction to enable"
                )
            );

            MalfunctionPenaltyEnabled = Plugin.config.Bind(
                "Penalty",
                "MalfunctionPenaltyEnabled",
                true,
                new ConfigDescription(
                    "Enable the penalty system that increases malfunction chances after not recovering a player"
                )
            );

            MalfunctionPenaltyOnly = Plugin.config.Bind(
                "Penalty",
                "MalfunctionPenaltyOnly",
                false,
                new ConfigDescription(
                    "Only enable malfunctions when a player has not been recovered"
                )
            );

            MalfunctionPenaltyMultiplier = Plugin.config.Bind(
                "Penalty",
                "MalfunctionPenaltyMultiplier",
                2.0,
                new ConfigDescription(
                    "Set the multiplier on triggering a malfunction after not recovering a player"
                )
            );

            /* Temporarily disabled until I can figure out how to efficiently get the moon value.
            MalfunctionNavigationBlockAboveQuota = Plugin.config.Bind(
                "Miscellaneous",
                "MalfunctionNavigationBlockAboveQuota",
                false,
                new ConfigDescription(
                    "Block the navigation malfunction from chosing destinations that cost more than the current quota"
                )
            );
            */

            MalfunctionPowerBlockLever = Plugin.config.Bind(
                "Power Malfunction",
                "MalfunctionPowerBlockLever",
                true,
                new ConfigDescription(
                    "Enable a chance of pulling the lever and taking off blocking when the power malfunction is active"
                )
            );

            MalfunctionPowerBlockLeverChance = Plugin.config.Bind(
                "Power Malfunction",
                "MalfunctionPowerBlockLeverChance",
                50.0,
                new ConfigDescription(
                    "Chance that pulling the lever will not cause take-off",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionMiscAllowConsecutive = Plugin.config.Bind(
                "Mmiscellaneous",
                "MalfunctionMiscAllowConsecutive",
                false,
                new ConfigDescription(
                    "Allow malfunctions to trigger consecutively - by default if a malfunction is triggered it can not repeat the next day"
                )
            );
        }
    }
}
