using System.Collections.Generic;
using UnityEngine;

namespace Malfunctions
{
    internal class Malfunction
    {
        public bool Active;
        public bool Notified;

        protected List<GameObject> effects;

        public Malfunction()
        {
            Active = false;
            Notified = false;

            effects = new List<GameObject>();
        }

        // Add a GameObject child to this malfunction.
        public void AssignChild(GameObject child)
        {
            effects.Add(child);
        }

        // Destroy all GameObject children of this malfunction.
        public void DestroyChildren()
        {
            foreach (GameObject child in effects)
            {
                UnityEngine.Object.Destroy(child);
            }

            effects.Clear();
        }

        // Reset the malfunction to its default state making sure any remaining effects are removed.
        public virtual void Reset()
        {
            Active = false;
            Notified = false;

            DestroyChildren();
        }
    }
}
