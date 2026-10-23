namespace GuneyKapisi.NPC
{
    
    public interface IResponseParser<T>
    {
        bool TryParse(string raw, out T result);
    }
}