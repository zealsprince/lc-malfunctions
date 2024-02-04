using BepInEx.Configuration;

namespace Malfunctions
{
    public class Config
    {
        public static ConfigEntry<double> MalfunctionNavigationChance;

        public static void Load()
        {
            MalfunctionNavigationChance = Plugin.config.Bind(
                "Malfunction Navigation Chance",
                "MalfunctionNavigationChance",
                12.5,
                new ConfigDescription(
                    "Set the chance of the navigation malfunction happening.",
                    new AcceptableValueRange<double>(0, 100)
                )
            );
        }
    }
}
