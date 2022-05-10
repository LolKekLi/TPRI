using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class UICounter : MonoBehaviour
    {
        [SerializeField]
        private Image _backgroundStrokeIcon = null;

        [SerializeField]
        private TextMeshProUGUI _counterLabel = null;
        
        public void Setup(Color color)
        {
            _backgroundStrokeIcon.color = color;
        }
        
        public void Refresh(int count)
        {
            gameObject.SetActive(count > 0);

            if (gameObject.activeSelf)
            {
                _counterLabel.text = $"{count}";
            }
        }
    }
}