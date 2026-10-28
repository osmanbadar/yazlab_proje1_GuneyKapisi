using UnityEngine;

namespace GuneyKapisi.Companion
{
    public class ReturnState : CompanionStateBase
    {
        private float _nextRepathTime;

        public ReturnState(CompanionAgent pip) : base(pip) { }
        public override string Name => "Return";

        public override void Enter()
        {
            Pip.Agent.isStopped = false;
            _nextRepathTime = 0f;
        }

        public override void Tick()
        {
            if (Pip.Player == null)
            {
                Pip.ChangeState(Pip.IdleState);
                return;
            }
            if (Pip.DistanceToPlayer <= Pip.FollowDistance)
            {
                Pip.ChangeState(Pip.FollowState);
                return;
            }
            if (Time.time >= _nextRepathTime)
            {
                _nextRepathTime = Time.time + 0.5f;
                Pip.Agent.SetDestination(Pip.Player.position);
            }
        }
    }
}