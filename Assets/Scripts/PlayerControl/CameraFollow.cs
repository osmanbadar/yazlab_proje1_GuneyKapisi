using UnityEngine;

namespace GuneyKapisi.PlayerControl
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 14f, -10f);
        [SerializeField] private float smoothTime = 0.2f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + offset;
            transform.position = Vector3.SmoothDamp(
                transform.position, desired, ref _velocity, smoothTime);
            transform.LookAt(target.position);
        }
    }
}
