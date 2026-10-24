using TMPro;
using UnityEngine;
using GuneyKapisi.Companion;

namespace GuneyKapisi.UI
{
    public class CompanionMessageUI : MonoBehaviour
    {
        [SerializeField] private CompanionAgent companion;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private float showSeconds = 3f;

        private float _hideTime;

        private void Awake() => messageText.gameObject.SetActive(false);
        private void OnEnable() => companion.Notified += Show;
        private void OnDisable() => companion.Notified -= Show;

        private void Show(string message)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            _hideTime = Time.time + showSeconds;
        }

        private void Update()
        {
            if (messageText.gameObject.activeSelf && Time.time > _hideTime)
                messageText.gameObject.SetActive(false);
        }
    }
}