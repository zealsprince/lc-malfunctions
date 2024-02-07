namespace Malfunctions
{
    internal class State
    {
        // Track our malfunctions.
        public static Malfunction MalfunctionNavigation;
        public static MalfunctionWithDelay MalfunctionTeleporter;
        public static MalfunctionWithDelay MalfunctionDistortion;
        public static MalfunctionWithDelay MalfunctionDoor;
        public static MalfunctionWithDelay MalfunctionPower;

        public static void Load()
        {
            MalfunctionNavigation = new Malfunction();
            MalfunctionTeleporter = new MalfunctionWithDelay();
            MalfunctionDistortion = new MalfunctionWithDelay();
            MalfunctionDoor = new MalfunctionWithDelay();
            MalfunctionPower = new MalfunctionWithDelay();
        }

        public static void Reset()
        {
            // Reset all malfunctions to their default state.
            MalfunctionNavigation.Reset();
            MalfunctionTeleporter.Reset();
            MalfunctionDistortion.Reset();
            MalfunctionDoor.Reset();
            MalfunctionPower.Reset();
        }
    }
}
