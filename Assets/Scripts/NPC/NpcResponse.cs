namespace GuneyKapisi.NPC
{
    
    public class NpcResponse
    {
        public string Reply { get; }
        public NpcAction Action { get; }
        public string Mood { get; }
        public NpcResponse(string reply, NpcAction action, string mood)
        {
            Reply = reply;
            Action = action;
            Mood = mood;
        }
    }
}