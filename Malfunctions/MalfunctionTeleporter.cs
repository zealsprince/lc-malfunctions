namespace Malfunctions
{
    internal class MalfunctionTeleporter : Malfunction
    {
        public bool Triggered;
        public int Delay;

        public MalfunctionTeleporter()
            : base()
        {
            Triggered = false;
            Delay = 0;
        }

        public override void Reset()
        {
            base.Reset();

            Triggered = false;
            Delay = 0;
        }
    }
}
