using UnityEngine;

namespace GuneyKapisi.Companion
{
    public class WaitState : CompanionStateBase
    {
        private float _enterTime;
        private float _clearSince;

        public WaitState(CompanionAgent pip) : base(pip) { }
        public override string Name => "Wait";

        public override void Enter()
        {
            Pip.Agent.isStopped = true;
            _enterTime = Time.time;
            _clearSince = -1f;
        }

        public override void Tick()
        {
            if (Pip.IsPathBlockedAhead()) _clearSince = -1f;
            else if (_clearSince < 0f) _clearSince = Time.time;

            if (_clearSince >= 0f && Time.time - _clearSince > 0.3f)
                Pip.ChangeState(Pip.MoveToTargetState);
            else if (Time.time - _enterTime > Pip.MaxWaitSeconds)
                Pip.ChangeState(Pip.FailedState);
        }

        public override void Exit() => Pip.Agent.isStopped = false;
    }
}