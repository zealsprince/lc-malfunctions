using BepInEx.Configuration;

namespace Malfunctions
{
    public class Config
    {
        public static ConfigEntry<double> MalfunctionNavigationChance;
        public static ConfigEntry<double> MalfunctionTeleporterChance;
        public static ConfigEntry<double> MalfunctionDistortionChance;
        public static ConfigEntry<double> MalfunctionDoorChance;
        public static ConfigEntry<double> MalfunctionPowerChance;

        public static ConfigEntry<bool> MalfunctionPenaltyEnabled;
        public static ConfigEntry<double> MalfunctionPenaltyMultiplier;

        public static ConfigEntry<bool> MalfunctionNavigationBlockAboveQuota;

        public static void Load()
        {
            MalfunctionNavigationChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionNavigationChance",
                7.5,
                new ConfigDescription(
                    "Set the chance of the navigation malfunction happening - this will force the ship to route to a random moon with no regard to cost",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionTeleporterChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionTeleporterChance",
                5.0,
                new ConfigDescription(
                    "Set the chance of the teleporter malfunction happening - this will cause teleporters to disable themselves either at landing or after a random interval into the match",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionDistortionChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionDistortionChance",
                5.0,
                new ConfigDescription(
                    "(Currently not implemented) Set the chance of the distortion malfunction happening - this will cause the map and terminal displays as well as walkies to become unusable either at landing or after a random interval into the match",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionDoorChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionDoorChance",
                5.0,
                new ConfigDescription(
                    "Set the chance of the door malfunction happening - this will disable ship door controls either at landing or after a random interval into the match",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionPowerChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionPowerChance",
                2.5,
                new ConfigDescription(
                    "Set the chance of the power malfunction happening - this will make the entire ship to go dark after landing, disabling battery charging, door controls, terminal and map displays",
                    new AcceptableValueRange<double>(0, 100)
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
        }
    }
}
