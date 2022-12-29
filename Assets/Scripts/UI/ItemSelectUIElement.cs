using System;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
using Survivors.ScriptableObjets.Items;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Elements
{
    public class ItemSelectUIElement : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text itemNameText;
        [SerializeField]
        private TMP_Text itemDescriptionText;

        [SerializeField]
        private Button itemButton;
        private Action _onButtonPressed;

        //Unity Functions
        //============================================================================================================//
        
        private void OnDisable()
        {
            _onButtonPressed = null;
            itemButton.onClick.RemoveAllListeners();
        }
        
        //============================================================================================================//

        public void Init(in ItemBaseScriptableObject item, in Action onSelectedCallback)
        {
            itemImage.sprite = item.sprite;
            //FIXME Need to include Item level/new
            itemNameText.text = $"{item.name}\t\t{ItemManager.GetItemAltTitleText(item)}";
            itemDescriptionText.text = ItemManager.GetItemAltDescriptionText(item);

            _onButtonPressed = onSelectedCallback;
            itemButton.onClick.AddListener(() =>
            {
                _onButtonPressed?.Invoke();
            });
        }
        
        //============================================================================================================//
    }
}
