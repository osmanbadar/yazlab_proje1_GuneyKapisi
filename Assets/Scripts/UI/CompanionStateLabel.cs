using TMPro;
using UnityEngine;
using GuneyKapisi.Companion;

namespace GuneyKapisi.UI
{
    public class CompanionStateLabel : MonoBehaviour
    {
        [SerializeField] private CompanionAgent companion;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Vector3 offset = new Vector3(0f, 1f, 0f);

        private void LateUpdate()
        {
            label.text = companion.CurrentStateName;
            transform.position = companion.transform.position + offset;
            if (Camera.main != null)
                transform.rotation = Camera.main.transform.rotation;
        }
    }
}
