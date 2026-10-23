namespace GuneyKapisi.Companion
{
    public enum CompanionCommand
    {
        ActivateTarget,
        Follow
    }

    public enum CompanionStatus
    {
        Idle,
        Running,
        Success,
        Failed
    }

    public interface ICommandable
    {
        void ReceiveCommand(CompanionCommand command);
        CompanionStatus Status { get; }
    }
}