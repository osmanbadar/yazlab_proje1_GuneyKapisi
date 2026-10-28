using UnityEngine;
namespace GuneyKapisi.Testing
{
    // Sürekli döner. Model düşünürken dönmeye devam ediyorsa oyun donmuyor demektir.
    public class RotateForever : MonoBehaviour
    {
        [SerializeField] private Vector3 degreesPerSecond = new Vector3(0f, 90f, 0f);
        private void Update() => transform.Rotate(degreesPerSecond * Time.deltaTime);
    }
}