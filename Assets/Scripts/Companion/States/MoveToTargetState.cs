using UnityEngine;
using UnityEngine.AI;

namespace GuneyKapisi.Companion
{
    // Şaltere gider. Yolu NavMesh (A*) hesaplar; bu sınıf yalnızca karar verir.
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
            if (agent.pathPending) return;   // yol hâlâ hesaplanıyor

            // 1) Yol hiç yoksa ya da 2 sn'den uzun süre yarımsa: başarısız
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

            // 2) Sensör önde yakın bir engel görüyorsa: bekle
            if (Pip.IsPathBlockedAhead())
            {
                Pip.ChangeState(Pip.WaitState);
                return;
            }

            // 3) Hedefe vardıysa: şalteri indir
            if (Pip.HasArrived())
            {
                Pip.ChangeState(Pip.InteractState);
                return;
            }

            // 4) Kalan mesafe uzun süredir azalmıyorsa: takıldı
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