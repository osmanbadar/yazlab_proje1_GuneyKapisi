namespace GuneyKapisi.Companion
{
    // Hiçbir şey yapmaz, yerinde durur (test başlangıcı vb.).
    public class IdleState : CompanionStateBase
    {
        public IdleState(CompanionAgent pip) : base(pip) { }
        public override string Name => "Idle";
        public override void Enter() => Pip.Agent.ResetPath();
    }
}