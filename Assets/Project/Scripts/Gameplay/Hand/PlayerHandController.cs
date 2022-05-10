using System;
using Project.UI;
using UnityEngine;

namespace Project
{
    public class PlayerHandController : HandController
    {
        [SerializeField]
        private GameObject[] _hands = null;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            RequestButton.RequestClicked += AddMoreButton_RequestClicked;
            ApproveButton.ApproveClicked += ApproveButtonApproveClicked;
            RejectButton.RejectClicked += RejectButton_RejectClicked;
            
            MainWindow.Start += MainWindow_Start;
            ResultPopup.ClaimClicked += ResultPopup_ClaimClicked;
        }

        

        protected override void OnDisable()
        {
            base.OnDisable();
            
            RequestButton.RequestClicked -= AddMoreButton_RequestClicked;
            ApproveButton.ApproveClicked -= ApproveButtonApproveClicked;
            RejectButton.RejectClicked -= RejectButton_RejectClicked;

            MainWindow.Start -= MainWindow_Start;
            ResultPopup.ClaimClicked -= ResultPopup_ClaimClicked;
        }
        
        private void MainWindow_Start()
        {
            for (int i = 0; i < _hands.Length; i++)
            {
                _hands[i].SetActive(true);
            }
        }

        private void ResultPopup_ClaimClicked()
        {
            this.InvokeWithDelay(.2f, () =>
            {
                for (int i = 0; i < _hands.Length; i++)
                {
                    _hands[i].SetActive(false);
                }
            });
        }

        private void Start()
        {
            for (int i = 0; i < _hands.Length; i++)
            {
                _hands[i].SetActive(false);
            }
        }

        private void AddMoreButton_RequestClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                Play(HandActionType.More);
            }
        }

        private void ApproveButtonApproveClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                Play(HandActionType.Trade);
            }
        }

        private void RejectButton_RejectClicked(bool isOpponent)
        {
            if (!isOpponent)
            {
                Play(HandActionType.Reject);
            }
        }
    }
}