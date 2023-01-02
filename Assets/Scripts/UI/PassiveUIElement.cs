using System;
using Survivors.Managers;
using Survivors.Passives.Enums;
using Survivors.ScriptableObjets.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Elements
{
    public class PassiveUIElement : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text changeText;

        private PASSIVE_TYPE _passiveType;

        public void Init(in PassiveItemProfileScriptableObject passiveProfile)
        {
            image.sprite = passiveProfile.sprite;
            nameText.text = passiveProfile.name;
            _passiveType = passiveProfile.type;
            changeText.text = "-";

            gameObject.name = $"{nameof(PassiveUIElement)}_{_passiveType}";
        }

        public void UpdateDisplayedChange()
        {
            bool usePercent = true;
            bool showSign = true;

            float value = 0f;
            switch (_passiveType)
            {
                case PASSIVE_TYPE.DAMAGE:
                    value = PassiveManager.Damage - 1f;
                    break;
                case PASSIVE_TYPE.DAMAGE_REDUCE:
                    value = PassiveManager.DamageReduction;
                    usePercent = false;
                    break;
                case PASSIVE_TYPE.MAX_HEALTH:
                    value = PassiveManager.MaxHealth - 1f;
                    break;
                case PASSIVE_TYPE.RECOVER:
                    break;
                case PASSIVE_TYPE.COOLDOWN:
                    value = PassiveManager.Cooldown - 1f;
                    showSign = false;
                    break;
                case PASSIVE_TYPE.ATTACK_AREA:
                    value = PassiveManager.AttackArea - 1f;
                    break;
                case PASSIVE_TYPE.PROJECTILE_SPEED:
                    value = PassiveManager.ProjectileSpeed - 1f;
                    break;
                case PASSIVE_TYPE.DURATION:
                    break;
                case PASSIVE_TYPE.PROJECTILE_COUNT:
                    value = PassiveManager.ProjectileAdd;
                    usePercent = false;
                    break;
                case PASSIVE_TYPE.MOVE_SPEED:
                    value = PassiveManager.MoveSpeed - 1f;
                    break;
                case PASSIVE_TYPE.PICKUP_RANGE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (value == 0f)
            {
                changeText.text = "-";
                return;
            }

            changeText.text = usePercent == false
                ? $"{(showSign ? "+" : string.Empty)}{value:0}"
                : $"{(showSign ? "+" : string.Empty)}{value:P0}";
        }

    }
}
