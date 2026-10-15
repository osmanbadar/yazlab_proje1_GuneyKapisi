using System.Collections.Generic;

namespace GuneyKapisi.Companion
{
    // Çevreyi algılayan sensör sözleşmesi.
    public interface ISensor
    {
        void Sense();                                   // yeni ölçüm yap
        SensorReading Closest { get; }                  // en yakın isabet
        IReadOnlyList<SensorReading> Readings { get; }  // tüm isabetler
    }
}
