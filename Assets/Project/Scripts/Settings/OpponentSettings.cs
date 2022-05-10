using System;
using System.Linq;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class OpponentBehaviourPreset
    {
        [field: SerializeField]
        public float MaxAcceptRate
        {
            get;
            private set;
        }

        [field: SerializeField]
        public float AcceptRateProbability
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float UniqueItemProbability
        {
            get;
            private set;
        }
    }
    
    [Serializable]
    public class AcceptPreset
    {
        [field: SerializeField]
        public IntRange TradeCountRange
        {
            get;
            private set;
        }

        [field: SerializeField]
        public OpponentBehaviourPreset[] BehaviourPresets
        {
            get;
            private set;
        }
    }
    
    [CreateAssetMenu(fileName = "OpponentSettings", menuName = "Scriptable/OpponentSettings", order = 0)]
    public class OpponentSettings : ScriptableObject
    {
        [SerializeField]
        private AcceptPreset[] _acceptPresets = null;

        [field: SerializeField]
        public float DelayBeforeReaction
        {
            get;
            private set;
        }

        [field: SerializeField]
        public ItemType[] TutorialItems
        {
            get;
            private set;
        }

        public OpponentBehaviourPreset GetAcceptRatePreset(float ratePercent)
        {
            var acceptPreset = _acceptPresets.FirstOrDefault(pr => pr.TradeCountRange.InRange(LocalConfig.LevelIndex));
            if (acceptPreset == null)
            {
                acceptPreset = _acceptPresets[_acceptPresets.Length - 1];
            }

            OpponentBehaviourPreset opponentBehaviorPreset =
                acceptPreset.BehaviourPresets.FirstOrDefault(p => p.MaxAcceptRate > ratePercent);
            if (opponentBehaviorPreset == null)
            {
                opponentBehaviorPreset = acceptPreset.BehaviourPresets[acceptPreset.BehaviourPresets.Length - 1];
            }
            
            return opponentBehaviorPreset;
        }

        public float GetUniqueItemProbability(float ratePercent)
        {
            var preset = GetAcceptRatePreset(ratePercent);

            return preset.UniqueItemProbability;
        }
    }
}