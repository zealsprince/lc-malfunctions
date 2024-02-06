namespace Malfunctions
{
    internal class MalfunctionPower : Malfunction
    {
        public bool Triggered;

        public MalfunctionPower()
            : base()
        {
            Triggered = false;
        }

        public override void Reset()
        {
            base.Reset();

            Triggered = false;
        }
    }
}
