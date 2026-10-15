namespace GuneyKapisi.Companion
{
    // Durum makinesindeki her durumun sözleşmesi.
    public interface ICompanionState
    {
        string Name { get; }
        void Enter();   // duruma girerken bir kez
        void Tick();    // her karede
        void Exit();    // durumdan çıkarken bir kez
    }
}