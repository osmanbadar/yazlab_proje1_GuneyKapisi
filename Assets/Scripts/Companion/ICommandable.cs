namespace GuneyKapisi.Companion
{
    public enum CompanionCommand
    {
        ActivateTarget,   // hedefe (şalter) git ve etkileşime gir
        Follow            // oyuncuyu takip et
    }

    public enum CompanionStatus
    {
        Idle,      // görev yok
        Running,   // görev sürüyor
        Success,   // şalter indirildi
        Failed     // tüm denemeler başarısız
    }

    // Emir alabilen her karakterin sözleşmesi. Test aracı da bunu kullanır.
    public interface ICommandable
    {
        void ReceiveCommand(CompanionCommand command);
        CompanionStatus Status { get; }
    }
}