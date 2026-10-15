using UnityEngine;

namespace GuneyKapisi.Companion
{
    // Hedefe vardı: şalteri indirir, kısa bir an bekleyip geri döner.
    public class InteractState : CompanionStateBase
    {
        private float _doneTime;

        public InteractState(CompanionAgent pip) : base(pip) { }
        public override string Name => "Interact";

        public override void Enter()
        {
            Pip.Agent.ResetPath();
            Pip.Target.Interact();
            _doneTime = Time.time + 0.5f;
        }

        public override void Tick()
        {
            if (Time.time < _doneTime) return;
            Pip.Status = CompanionStatus.Success;
            Pip.Notify("Pip şalteri indirdi!");
            Pip.ChangeState(Pip.ReturnState);
        }
    }
}