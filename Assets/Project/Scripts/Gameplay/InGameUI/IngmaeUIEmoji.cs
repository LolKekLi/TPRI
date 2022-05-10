using System.Collections;
using Project.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class IngmaeUIEmoji : MonoBehaviour
    {
        [SerializeField]
        private Image _emojiIcon = null;
        
        [SerializeField]
        private Sprite[] _sprites = null;

        [SerializeField]
        private SelfSingleTweenController _tweenController = null;

        private Coroutine _showCor = null;

        public void Setup(Vector2 position)
        {
            ((RectTransform) transform).SetViewportPosition(position);
            Hide();
        }
        
        public void Show()
        {
            if (_showCor == null)
            {
                gameObject.SetActive(true);
                _emojiIcon.sprite = _sprites.RandomElement();
                
                _tweenController.Play();

                _showCor = StartCoroutine(ShowCor());
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _showCor = null;
        }
        
        private IEnumerator ShowCor()
        {
            yield return new WaitForSeconds(1.65f);

            Hide();
        }
    }
}