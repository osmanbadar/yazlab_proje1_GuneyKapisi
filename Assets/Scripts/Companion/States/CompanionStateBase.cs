namespace GuneyKapisi.Companion
{
    // Durumların ortak tabanı: Pip referansını tutar, boş Enter/Tick/Exit sağlar.
    public abstract class CompanionStateBase : ICompanionState
    {
        protected readonly CompanionAgent Pip;

        protected CompanionStateBase(CompanionAgent pip) => Pip = pip;

        public abstract string Name { get; }
        public virtual void Enter() { }
        public virtual void Tick() { }
        public virtual void Exit() { }
    }
}