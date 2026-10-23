using UnityEngine;

namespace GuneyKapisi.Companion
{
    public readonly struct SensorReading
    {
        public readonly bool HasHit;
        public readonly float Distance;
        public readonly float Angle;
        public readonly Vector3 Point;
        public readonly Collider Collider;

        public SensorReading(bool hasHit, float distance, float angle,
                             Vector3 point, Collider collider)
        {
            HasHit = hasHit;
            Distance = distance;
            Angle = angle;
            Point = point;
            Collider = collider;
        }

        public static SensorReading None =>
            new SensorReading(false, float.MaxValue, 0f, Vector3.zero, null);
    }
}
