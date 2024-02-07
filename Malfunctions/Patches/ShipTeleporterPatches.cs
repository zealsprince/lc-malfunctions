using System.Linq;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal class ShipTeleporterPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("PressTeleportButtonOnLocalClient")]
        private static bool InterruptPressTeleportButton(ShipTeleporter __instance)
        {
            // Interrupt the teleporter if the teleporter or power malfunction has triggered.
            if (
                State.MalfunctionTeleporter.Active && State.MalfunctionTeleporter.Triggered
                || State.MalfunctionPower.Active && State.MalfunctionPower.Triggered
            )
            {
                // Still do the animation and play the sound.
                __instance.buttonAnimator.SetTrigger("press");
                __instance.buttonAudio.PlayOneShot(__instance.buttonPressSFX);

                return false;
            }

            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch("PressTeleportButtonOnLocalClient")]
        private static void RandomizePressTeleportButton()
        {
            // When the distortion malfunction is active, set the teleporting player to a random one.
            if (State.MalfunctionDistortion.Active && State.MalfunctionDistortion.Triggered)
            {
                PlayerControllerB[] players = GameObject
                    .FindObjectsByType<PlayerControllerB>(FindObjectsSortMode.None)
                    .Where(player => player.isPlayerControlled)
                    .ToArray();

                if (players.Length > 0)
                {
                    // Select a random player.
                    StartOfRound.Instance.mapScreen.targetedPlayer = players[
                        Random.Range(0, players.Length)
                    ];
                }
            }
        }
    }
}
