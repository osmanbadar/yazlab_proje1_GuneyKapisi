using UnityEngine;

namespace GuneyKapisi.Companion
{
    public class FollowState : CompanionStateBase
    {
        private float _nextRepathTime;

        public FollowState(CompanionAgent pip) : base(pip) { }
        public override string Name => "Follow";

        public override void Enter()
        {
            Pip.Agent.isStopped = false;
            _nextRepathTime = 0f;
        }

        public override void Tick()
        {
            if (Pip.Player == null || Time.time < _nextRepathTime) return;
            _nextRepathTime = Time.time + 0.25f;

            if (Pip.DistanceToPlayer > Pip.FollowDistance)
                Pip.Agent.SetDestination(Pip.Player.position);
            else
                Pip.Agent.ResetPath();
        }
    }
}