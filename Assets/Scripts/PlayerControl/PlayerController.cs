using UnityEngine;
using UnityEngine.InputSystem;
using GuneyKapisi.Core;

namespace GuneyKapisi.PlayerControl
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 720f;
        [SerializeField] private float gravity = -20f;

        private CharacterController _controller;
        private float _verticalVelocity;
        private bool _inputLocked;

        private void Awake() => _controller = GetComponent<CharacterController>();

        private void OnEnable() => GameEvents.OnDialogueToggled += SetInputLocked;
        private void OnDisable() => GameEvents.OnDialogueToggled -= SetInputLocked;
        private void SetInputLocked(bool locked) => _inputLocked = locked;

        private void Update()
        {
            Vector2 input = _inputLocked ? Vector2.zero : ReadMoveInput();
            float yaw = cameraTransform != null ? cameraTransform.eulerAngles.y : 0f;
            Vector3 move = Quaternion.Euler(0f, yaw, 0f) * new Vector3(input.x, 0f, input.y);
            if (move.sqrMagnitude > 1f) move.Normalize();

            if (move.sqrMagnitude > 0.001f)
            {
                Quaternion target = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, target, turnSpeed * Time.deltaTime);
            }

            _verticalVelocity = _controller.isGrounded
                ? -1f
                : _verticalVelocity + gravity * Time.deltaTime;

            Vector3 velocity = move * moveSpeed + Vector3.up * _verticalVelocity;
            _controller.Move(velocity * Time.deltaTime);
        }

        private static Vector2 ReadMoveInput()
        {
            Keyboard k = Keyboard.current;
            if (k == null) return Vector2.zero;
            float x = (k.dKey.isPressed || k.rightArrowKey.isPressed ? 1f : 0f)
                    - (k.aKey.isPressed || k.leftArrowKey.isPressed ? 1f : 0f);
            float y = (k.wKey.isPressed || k.upArrowKey.isPressed ? 1f : 0f)
                    - (k.sKey.isPressed || k.downArrowKey.isPressed ? 1f : 0f);
            return new Vector2(x, y);
        }
    }
}
