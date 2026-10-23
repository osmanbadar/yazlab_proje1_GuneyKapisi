using System.Collections.Generic;
using UnityEngine;

namespace GuneyKapisi.Companion
{
    public class RaycastSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private float range = 4f;
        [SerializeField] private int rayCount = 7;
        [SerializeField] private float fieldOfView = 90f;
        [SerializeField] private float rayHeight = 0.3f;
        [SerializeField] private LayerMask obstacleMask;

        private readonly List<SensorReading> _readings = new List<SensorReading>();

        public SensorReading Closest { get; private set; } = SensorReading.None;
        public IReadOnlyList<SensorReading> Readings => _readings;

        public void Sense()
        {
            _readings.Clear();
            Closest = SensorReading.None;
            Vector3 origin = transform.position + Vector3.up * rayHeight;

            for (int i = 0; i < rayCount; i++)
            {
                float t = rayCount == 1 ? 0.5f : i / (float)(rayCount - 1);
                float angle = Mathf.Lerp(-fieldOfView / 2f, fieldOfView / 2f, t);
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

                if (Physics.Raycast(origin, direction, out RaycastHit hit, range,
                                    obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    var reading = new SensorReading(true, hit.distance, angle,
                                                    hit.point, hit.collider);
                    _readings.Add(reading);
                    if (reading.Distance < Closest.Distance) Closest = reading;
                    Debug.DrawLine(origin, hit.point, Color.red);
                }
                else
                {
                    Debug.DrawRay(origin, direction * range, Color.green);
                }
            }
        }
    }
}