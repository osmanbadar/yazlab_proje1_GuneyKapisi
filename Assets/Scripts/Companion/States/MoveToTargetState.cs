using UnityEngine;
using UnityEngine.AI;

namespace GuneyKapisi.Companion
{
    public class MoveToTargetState : CompanionStateBase
    {
        private float _bestDistance;
        private float _lastProgressTime;
        private float _partialSince;

        public MoveToTargetState(CompanionAgent pip) : base(pip) { }
        public override string Name => "MoveToTarget";

        public override void Enter()
        {
            Pip.Agent.isStopped = false;
            Pip.Agent.SetDestination(Pip.Target.InteractionPoint.position);
            _bestDistance = float.MaxValue;
            _lastProgressTime = Time.time;
            _partialSince = -1f;
        }

        public override void Tick()
        {
            NavMeshAgent agent = Pip.Agent;
            if (agent.pathPending) return;

            if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Pip.ChangeState(Pip.FailedState);
                return;
            }
            if (agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                if (_partialSince < 0f) _partialSince = Time.time;
                if (Time.time - _partialSince > 2f)
                {
                    Pip.ChangeState(Pip.FailedState);
                    return;
                }
            }
            else
            {
                _partialSince = -1f;
            }

            if (Pip.IsPathBlockedAhead())
            {
                Pip.ChangeState(Pip.WaitState);
                return;
            }

            if (Pip.HasArrived())
            {
                Pip.ChangeState(Pip.InteractState);
                return;
            }

            if (agent.remainingDistance < _bestDistance - 0.1f)
            {
                _bestDistance = agent.remainingDistance;
                _lastProgressTime = Time.time;
            }
            else if (Time.time - _lastProgressTime > Pip.StuckSeconds)
            {
                Pip.ChangeState(Pip.FailedState);
            }
        }
    }
}