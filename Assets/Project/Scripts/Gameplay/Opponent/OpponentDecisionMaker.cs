using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Project
{
    public static class OpponentDecisionMaker
    {
        private static float RandomValue
        {
            get => Random.Range(0f, 100f);
        }

        private static OpponentBehaviourPreset GetOpponentBehaviourPreset()
        {
            var ratePercent = TradeControllerExtensions.GetTradeRate();
            var preset = AssetsManager.Instance.OpponentSettings.GetAcceptRatePreset(ratePercent);
            
            return preset;
        }
        
        public static DecisionType GetDecision(PlayerActionType actionType)
        {
            DecisionType decision = default;
            
            var preset = GetOpponentBehaviourPreset();
            bool hasPlayerItems = TradeController.Instance.PlayerItems.Count > 0;

            switch (actionType)
            {
                case PlayerActionType.Accept:
                    
                    int totalAmount = User.Current.GetTotalAmount();

                    if (totalAmount <= 0)
                    {
                        decision = DecisionType.Accept;
                    }
                    else if (preset.AcceptRateProbability >= RandomValue)
                    {
                        decision = DecisionType.Accept;
                    }
                    else
                    {
                        decision = DecisionType.RequestMore;
                    }
                    break;
                
                case PlayerActionType.PlaceFirstItem:
                    decision = DecisionType.AddMore;
                    break;
                
                case PlayerActionType.RequestMore:
                    if (preset.AcceptRateProbability >= RandomValue && hasPlayerItems)
                    {
                        decision = DecisionType.AddMore;
                    }
                    else
                    {
                        decision = DecisionType.RequestMore;
                    }
                    break;
                
                case PlayerActionType.PlaceItem:
                    if (preset.AcceptRateProbability >= RandomValue)
                    {
                        decision = DecisionType.Accept;
                    }
                    else
                    {
                        decision = DecisionType.RequestMore;
                    }
                    break;
            }
            
            return decision;
        }
        
        public static ItemType GetPlaceableItem()
        {
            ItemType itemToGive = default;

            if (LocalConfig.BasicTutorialNeeded)
            {
                itemToGive = AssetsManager.Instance.OpponentSettings.TutorialItems.RandomElement();
            }
            else
            {
                var preset = GetOpponentBehaviourPreset();
                List<ItemType> items = new List<ItemType>((ItemType[])Enum.GetValues(typeof(ItemType)));

                if (RandomValue > preset.UniqueItemProbability)
                {
                    items = items.Where(i => User.Current.GetTradeItemCount(i) > 0).ToList();
                }
                else
                {
                    items = items.Where(i => User.Current.GetTradeItemCount(i) <= 0).ToList();
                }

                if (items == null || items.Count == 0)
                {
                    items = new List<ItemType>((ItemType[])Enum.GetValues(typeof(ItemType)));
                }

                itemToGive = items.RandomElement();
            }

            return itemToGive;
        }
    }
}