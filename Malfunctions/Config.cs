using BepInEx.Configuration;

namespace Malfunctions
{
    public class Config
    {
        public static ConfigEntry<double> MalfunctionNavigationChance;
        public static ConfigEntry<double> MalfunctionTeleporterChance;
        public static ConfigEntry<double> MalfunctionDistortionChance;
        public static ConfigEntry<double> MalfunctionPowerChance;

        public static void Load()
        {
            MalfunctionNavigationChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionNavigationChance",
                12.5,
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
                10.0,
                new ConfigDescription(
                    "Set the chance of the distortion malfunction happening - this will cause the map and terminal displays to become unusable after landing",
                    new AcceptableValueRange<double>(0, 100)
                )
            );

            MalfunctionPowerChance = Plugin.config.Bind(
                "Chances",
                "MalfunctionPowerChance",
                1.0,
                new ConfigDescription(
                    "Set the chance of the power malfunction happening - this will make the entire ship to go dark after landing, disabling battery charging, door controls, terminal and map displays",
                    new AcceptableValueRange<double>(0, 100)
                )
            );
        }
    }
}
