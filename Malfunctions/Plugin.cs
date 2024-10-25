using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace Malfunctions
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency("LethalNetworkAPI")]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "com.zealsprince.malfunctions";
        public const string ModName = "Malfunctions";
        public const string ModVersion = "1.9.3";

        // These need to be lowercase because we're passing through the protected properties.
        public static ManualLogSource logger;
        public static ConfigFile config;

        private readonly Harmony harmony = new Harmony(ModGUID);

        private void Awake()
        {
            logger = Logger;
            config = Config;

            Malfunctions.Config.Load();

            // Make sure asset loading is completed successfully and abort otherwise.
            if (Assets.Load() != Assets.LoadStatusCode.Success)
                return;

            // Load the tracking of the malfunction states and objects.
            State.Load();

            harmony.PatchAll();
        }
    }
}
