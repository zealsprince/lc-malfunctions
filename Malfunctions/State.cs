namespace Malfunctions
{
    internal class State
    {
        public static int PreviousMoon;

        // Track our malfunctions.
        public static Malfunction MalfunctionNavigation;
        public static MalfunctionWithDelay MalfunctionTeleporter;
        public static MalfunctionWithDelay MalfunctionDistortion;
        public static MalfunctionWithDelay MalfunctionDoor;
        public static MalfunctionWithDelay MalfunctionLever;
        public static MalfunctionWithDelay MalfunctionPower;

        public static void Load()
        {
            PreviousMoon = -1;
            MalfunctionNavigation = new Malfunction();
            MalfunctionTeleporter = new MalfunctionWithDelay();
            MalfunctionDistortion = new MalfunctionWithDelay();
            MalfunctionDoor = new MalfunctionWithDelay();
            MalfunctionLever = new MalfunctionWithDelay();
            MalfunctionPower = new MalfunctionWithDelay();
        }

        public static void Reset()
        {
            // Reset all malfunctions to their default state.
            MalfunctionNavigation.Reset();
            MalfunctionTeleporter.Reset();
            MalfunctionDistortion.Reset();
            MalfunctionDoor.Reset();
            MalfunctionLever.Reset();
            MalfunctionPower.Reset();
        }
    }
}
