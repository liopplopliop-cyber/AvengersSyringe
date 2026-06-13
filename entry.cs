// entry.cs
// Small stable entry wrapper that delegates to the existing mod implementation.
// Saves the loader from needing to know the original namespace/class details.

using System;

namespace AvengerSyringe
{
    public class Entry
    {
        // The loader may call Main or OnLoad. Provide both and delegate to the mod.
        public static void Main()
        {
            // Delegate to the mod's startup. Adjust if the real startup lives elsewhere.
            try
            {
                Mod.Mod.Main();
            }
            catch (Exception)
            {
                // if Main() isn't available, try OnLoad or ignore the exception
                try { Mod.Mod.OnLoad(); } catch { /* swallow to avoid loader crash */ }
            }
        }

        public static void OnLoad()
        {
            Main();
        }
    }
}
