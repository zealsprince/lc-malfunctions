using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Malfunctions.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("LoadNewNodeIfAffordable")]
        public static bool BlockNewNode(Terminal __instance, TerminalNode node)
        {
            // Make sure we disallow the result of the action
            if (State.malfunctionNavigation)
            {
                // This checks if the current node has selected a moon. By default the value is < 0 if not.
                if (node.buyRerouteToMoon > -1)
                {
                    // Update the display text and play a sound.
                    node.displayText =
                        "ROUTE TABLE OFFSET: NAVIGATION ERROR\n\nIT IS ADVISED TO LAND THE SHIP TO ALLOW RECALIBRATION OF SYSTEMS AND RESTORATION OF FUNCTIONALITY\n\n";

                    node.terminalEvent = "ERROR";

                    __instance.PlayTerminalAudioServerRpc(3);

                    __instance.LoadNewNode(node);

                    return false;
                }
            }

            return true;
        }
    }
}
