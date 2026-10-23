using System.Collections.Generic;

namespace GuneyKapisi.Companion
{
    public interface ISensor
    {
        void Sense();
        SensorReading Closest { get; }
        IReadOnlyList<SensorReading> Readings { get; }
    }
}
