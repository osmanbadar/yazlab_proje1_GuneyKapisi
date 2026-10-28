using System;
using UnityEngine;
using UnityEngine.AI;
using GuneyKapisi.Core;
using GuneyKapisi.Interactables;

namespace GuneyKapisi.Companion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CompanionAgent : MonoBehaviour, ICommandable, IResettable
    {
        [Header("Bağlantılar")]
        [SerializeField] private Transform player;
        [SerializeField] private MonoBehaviour targetSource;
        [SerializeField] private MonoBehaviour sensorSource;

        [Header("Davranış ayarları")]
        [SerializeField] private float followDistance = 2.5f;
        [SerializeField] private float waitDistance = 1.5f;
        [SerializeField] private float waitHalfAngle = 35f;
        [SerializeField] private float stuckSeconds = 4f;
        [SerializeField] private float maxWaitSeconds = 10f;
        [SerializeField] private int maxRetries = 3;

        public event Action<string> Notified;

        public CompanionStatus Status { get; internal set; } = CompanionStatus.Idle;
        public int RetryCount { get; internal set; }
        public NavMeshAgent Agent { get; private set; }
        public IInteractable Target { get; private set; }
        public ISensor Sensor { get; private set; }
        public Transform Player => player;
        public string CurrentStateName => _current?.Name ?? "-";

        public float FollowDistance => followDistance;
        public float StuckSeconds => stuckSeconds;
        public float MaxWaitSeconds => maxWaitSeconds;
        public int MaxRetries => maxRetries;

        public ICompanionState IdleState { get; private set; }
        public ICompanionState FollowState { get; private set; }
        public ICompanionState MoveToTargetState { get; private set; }
        public ICompanionState WaitState { get; private set; }
        public ICompanionState InteractState { get; private set; }
        public ICompanionState ReturnState { get; private set; }
        public ICompanionState FailedState { get; private set; }

        private ICompanionState _current;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Target = targetSource as IInteractable;
            Sensor = sensorSource as ISensor;

            IdleState = new IdleState(this);
            FollowState = new FollowState(this);
            MoveToTargetState = new MoveToTargetState(this);
            WaitState = new WaitState(this);
            InteractState = new InteractState(this);
            ReturnState = new ReturnState(this);
            FailedState = new FailedState(this);
        }

        private void Start() => ChangeState(player != null ? FollowState : IdleState);

        private void Update()
        {
            Sensor?.Sense();
            _current?.Tick();
        }

        public void ChangeState(ICompanionState next)
        {
            _current?.Exit();
            _current = next;
            Debug.Log($"[Pip] Durum: {next.Name}");
            _current.Enter();
        }

         public void ReceiveCommand(CompanionCommand command)
        {
            switch (command)
            {
                case CompanionCommand.ActivateTarget:
                    if (Target == null || Target.IsActivated)
                    {
                        Notify("Pip: yapılacak bir şey yok.");
                        return;
                    }
                    RetryCount = 0;
                    Status = CompanionStatus.Running;
                    ChangeState(MoveToTargetState);
                    break;
                case CompanionCommand.Follow:
                    ChangeState(FollowState);
                    break;
            }
        }

        public void ResetState()
        {
            Agent.ResetPath();
            Agent.isStopped = false;
            RetryCount = 0;
            Status = CompanionStatus.Idle;
            ChangeState(IdleState);
        }

        public void Notify(string message)
        {
            Debug.Log("[Pip] " + message);
            Notified?.Invoke(message);
        }

        public bool IsPathBlockedAhead()
        {
            if (Sensor == null) return false;
            foreach (SensorReading r in Sensor.Readings)
                if (r.Distance <= waitDistance && Mathf.Abs(r.Angle) <= waitHalfAngle)
                    return true;
            return false;
        }

        public bool HasArrived() =>
            !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance + 0.05f;

        public float DistanceToPlayer =>
            player == null ? 0f : Vector3.Distance(transform.position, player.position);
    }
}
