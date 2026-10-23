namespace GuneyKapisi.Core
{
    // Test sırasında rastgele ayar alabilen objeler (hareketli bloklar).
    // Aynı seed ile aynı sonuçları üretmek için System.Random dışarıdan verilir.
    public interface IRandomizable
    {
        void Randomize(System.Random rng);
    }
}
