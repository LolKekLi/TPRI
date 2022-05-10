using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class MainWindow : Window
    {
        [SerializeField]
        private Button _startButton = null;

        [SerializeField]
        private TextMeshProUGUI _counsCounter = null;

        [SerializeField]
        private UpButton[] _upButtons = null;

        public override bool IsPopup => false;

        protected override void Start()
        {
            base.Start();

            _startButton.onClick.AddListener(StartGame);

            for (int i = 0; i < _upButtons.Length; i++)
            {
                _upButtons[i].Setup(Refresh);
            }

            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();

            _counsCounter.text = User.Current.Coins.ToString();

            for (int i = 0; i < _upButtons.Length; i++)
            {
                _upButtons[i].Refresh();
            }
        }


        private void StartGame()
        {
            UISystem.ShowWindow<GameWindow>();
        }
    }
}