
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using HarmonyLib;

namespace Malfunctions
{
	[BepInPlugin(ModGUID, ModName, ModVersion)]
	public class MalfunctionsBase : BaseUnityPlugin
	{
		public const string ModGUID = "zealsprince.Malfunctions";
		public const string ModName = "Malfunctions";
		public const string ModVersion = "1.0.0";

		public static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
		public static MalfunctionsBase Instance;

		private readonly Harmony harmony = new Harmony(ModGUID);

		private void Awake() {
			if (Instance is null) {
				Instance = this;
			}

			Log.LogDebug("Malfunctions has awoken!");

			LoadConfigs();

			harmony.PatchAll();
		}

		#region Config

		public static ConfigEntry<double> ConfigMalfunctionNavigationChance;
		private void LoadConfigs() {
			ConfigMalfunctionNavigationChance = Config.Bind(
				"Navigation Malfunction Chance",
				"MalfunctionNavigationChance",
				12.5,
                new ConfigDescription(
					"Set the chance of the navigation malfunction happening.",
					new AcceptableValueRange<double>(0, 100)
				)
			);
		}

        #endregion
    }
}
