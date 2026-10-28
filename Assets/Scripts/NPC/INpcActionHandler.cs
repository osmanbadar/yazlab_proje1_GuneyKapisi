namespace GuneyKapisi.NPC
{
    // NPC'nin verdiği kararı oyun dünyasına uygular (kapı aç, saldır...).
    public interface INpcActionHandler
    {
        void Handle(NpcAction action);
    }
}
