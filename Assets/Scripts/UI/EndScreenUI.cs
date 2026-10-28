using System;
using UnityEngine;
using UnityEngine.UI;
namespace GuneyKapisi.UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private Button retryButton; // kaybetme panelinde
        [SerializeField] private Button[] quitButtons; // iki panelde de olabilir
        public event Action RetryClicked;
        private void Awake()
        {
            HideAll();
            retryButton.onClick.AddListener(() => RetryClicked?.Invoke());  
            foreach (Button button in quitButtons)
                button.onClick.AddListener(Quit);
        }
        public void ShowWin() { HideAll(); winPanel.SetActive(true); }
        public void ShowLose() { HideAll(); losePanel.SetActive(true); }
        public void HideAll()
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }
        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // editörde Play'i durdur
#else
 Application.Quit();
#endif
        }
    }
}