using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malfunctions
{
    internal class State
    {
        // Whether the navigation malfunction is active.
        public static bool malfunctionNavigation;

        // Store the state of sparks in the level.
        public static UnityEngine.GameObject sparksNavigation;

        public static void Reset()
        {
            // Destroy any instantiated effect objects on reset.
            if (sparksNavigation != null)
            {
                UnityEngine.Object.Destroy(sparksNavigation);
            }

            // Reset all malfunctions to their default state.
            malfunctionNavigation = false;
        }
    }
}
