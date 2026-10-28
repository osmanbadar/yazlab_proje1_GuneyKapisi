using System.Collections;
using UnityEngine;
using GuneyKapisi.Core;
namespace GuneyKapisi.NPC
{
    // Güney Kapısı. Open() çağrılınca kapı paneli yukarı kayar.
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform doorPanel;
        [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
        [SerializeField] private float openDuration = 1.5f;
        public bool IsOpen { get; private set; }
        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            StartCoroutine(OpenRoutine());
        }
        private IEnumerator OpenRoutine()
        {
            Vector3 start = doorPanel.localPosition;
            Vector3 end = start + openOffset;
            for (float t = 0f; t < 1f; t += Time.deltaTime / openDuration)
            {
                doorPanel.localPosition = Vector3.Lerp(start, end, t);
                yield return null; // bir sonraki kareye kadar bekle
            }
            doorPanel.localPosition = end;
            GameEvents.RaiseDoorOpened(); // GameManager -> Won, kazanma ekranı
        }
    }
}