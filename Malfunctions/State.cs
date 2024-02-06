namespace Malfunctions
{
    internal class State
    {
        // Track our malfunctions.
        public static Malfunction MalfunctionNavigation;
        public static MalfunctionTeleporter MalfunctionTeleporter;
        public static Malfunction MalfunctionDistortion;
        public static MalfunctionPower MalfunctionPower;

        public static void Load()
        {
            MalfunctionNavigation = new Malfunction();
            MalfunctionTeleporter = new MalfunctionTeleporter();
            MalfunctionDistortion = new Malfunction();
            MalfunctionPower = new MalfunctionPower();
        }

        public static void Reset()
        {
            // Reset all malfunctions to their default state.
            MalfunctionNavigation.Reset();
            MalfunctionTeleporter.Reset();
            MalfunctionDistortion.Reset();
            MalfunctionPower.Reset();
        }
    }
}
