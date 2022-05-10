using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class UICategoryTab : MonoBehaviour
    {
        public static event Action<CategoryType> TabClicked = delegate { };

        [SerializeField]
        private Button _tabButton = null;

        [SerializeField]
        private Image _categoryIcon = null;

        [SerializeField]
        private GameObject _activeGroup = null;

        [SerializeField]
        private GameObject _disabledGroup = null;
        
        private CategoryType _category = default;

        private bool IsActive
        {
            get => _activeGroup.activeSelf;
        }
        
        private void Start()
        {
            _tabButton.onClick.AddListener(OnTabButtonClick);
        }

        private void OnEnable()
        {
            TabClicked += UICategoryTab_TabClicked;
        }

        private void OnDisable()
        {
            TabClicked -= UICategoryTab_TabClicked;
        }

        public void Setup(CategoryType category)
        {
            _category = category;
            
            var categoryPreset = AssetsManager.Instance.CategorySettings.GetCategoryPreset(category);
            _categoryIcon.sprite = categoryPreset.CategoryIcon;
            _categoryIcon.SetNativeSize();
        }

        public void Select(bool isForce = false)
        {
            if (IsActive && !isForce)
            {
                return;
            }

            TabClicked(_category);

            if (isForce)
            {
                UpdateState(_category);
            }
        }

        private void UpdateState(CategoryType category)
        {
            _activeGroup.SetActive(category == _category);
            _disabledGroup.SetActive(category != _category);
        }

        private void OnTabButtonClick()
        {
            Select();
        }
        
        private void UICategoryTab_TabClicked(CategoryType category)
        {
            UpdateState(category);
        }
    }
}