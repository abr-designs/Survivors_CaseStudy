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

        private void OnDisable()
        {
            _onButtonPressed = null;
            itemButton.onClick.RemoveAllListeners();
        }

        public void Init(in ItemBaseScriptableObject item, in Action onSelectedCallback)
        {
            itemImage.sprite = item.sprite;
            //FIXME Need to include Item level/new
            itemNameText.text = $"{item.name}\t\t{GetItemAltText(item)}";
            itemDescriptionText.text = item.description;

            _onButtonPressed = onSelectedCallback;
            itemButton.onClick.AddListener(() =>
            {
                _onButtonPressed?.Invoke();
            });
        }

        private static WeaponManager _weaponManager;
        private static PassiveManager _passiveManager;

        private static string GetItemAltText(in ItemBaseScriptableObject item)
        {
            var level = 0;
            switch (item)
            {
                case WeaponProfileScriptableObject weapon:
                    if (_weaponManager == null) _weaponManager = FindObjectOfType<WeaponManager>();
                    level = _weaponManager.GetWeaponLevel(weapon.type);
                    break;
                case PassiveItemProfileScriptableObject passive:
                    if (_passiveManager == null) _passiveManager = FindObjectOfType<PassiveManager>();
                    level = _passiveManager.GetPassiveLevel(passive.type);
                    break;
            }

            return level == 0 ? "<color=yellow>new!</color>" : $"level: {level + 1}";
        }
    }
}
