using UnityEngine;

namespace GuneyKapisi.Companion
{
    public class FailedState : CompanionStateBase
    {
        private float _retryTime;
        private bool _givingUp;

        public FailedState(CompanionAgent pip) : base(pip) { }
        public override string Name => "Failed";

        public override void Enter()
        {
            Pip.Agent.ResetPath();
            Pip.RetryCount++;
            _givingUp = Pip.RetryCount > Pip.MaxRetries;

            if (_givingUp)
            {
                Pip.Status = CompanionStatus.Failed;
                Pip.Notify("Pip şaltere ulaşamadı. Tekrar denemek için E'ye bas.");
            }
            else
            {
                Pip.Notify("Pip yolu yeniden hesaplıyor " +
                           $"({Pip.RetryCount}/{Pip.MaxRetries})...");
                _retryTime = Time.time + 1f;
            }
        }

        public override void Tick()
        {
            if (_givingUp) Pip.ChangeState(Pip.ReturnState);
            else if (Time.time >= _retryTime) Pip.ChangeState(Pip.MoveToTargetState);
        }
    }
}