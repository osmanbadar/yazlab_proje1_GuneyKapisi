using UnityEngine;
using GuneyKapisi.Core;

namespace GuneyKapisi.Interactables
{
    public class MovingBlock : MonoBehaviour, IRandomizable, IResettable
    {
        [SerializeField] private Vector3 moveOffset = new Vector3(0f, 0f, 4f);
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float minSpeed = 0.3f;
        [SerializeField] private float maxSpeed = 0.9f;

        private Vector3 _startPosition;
        private float _phase;
        private float _currentSpeed;

        private void Awake()
        {
            _startPosition = transform.position;
            _currentSpeed = speed;
        }

        private void Update()
        {
            _phase += Time.deltaTime * _currentSpeed;
            float t = Mathf.PingPong(_phase, 1f);
            transform.position = _startPosition + moveOffset * t;
        }

        public void Randomize(System.Random rng)
        {
            _currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, (float)rng.NextDouble());
            _phase = (float)rng.NextDouble() * 2f;
        }

        public void ResetState()
        {
            _currentSpeed = speed;
            _phase = 0f;
            transform.position = _startPosition;
        }
    }
}