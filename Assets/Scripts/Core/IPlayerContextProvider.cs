namespace GuneyKapisi.Core
{
    public interface IPlayerContextProvider
    {
        string GetContextForNpc();
        bool HasItem(string id);
    }
}