using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Survivors.Managers
{
    public class DamageTextManager : ManagerBase, ILateUpdate
    {
        //============================================================================================================//
        
        private class TextAnimationData
        {
            public static AnimationCurve ScaleCurve;
            public static AnimationCurve ColorCurve;
            public static Color32 StartColor;
            public static Color32 EndColor;
            

            //------------------------------------------------//
            public Transform Transform;
            public TMP_Text Text;
            
            public float FadeTime;
            public float ScaleMult;
            
            private float _currentT;

            public bool TryUpdateAnimation(in float deltaTime)
            {
                _currentT += deltaTime;
                var td = _currentT / FadeTime;

                if (td >= 1f)
                {
                    return false;
                }
                
                Text.color = Color32.Lerp(StartColor, EndColor, ColorCurve.Evaluate(td));
                
                Transform.localScale = Vector3.one * (ScaleCurve.Evaluate(td) * ScaleMult);
                return true;
            }
            
            //------------------------------------------------//
        }

        //Properties
        //============================================================================================================//
        private static DamageTextManager _instance;

        private Transform _parentTransform;
        
        private readonly TMP_Text textPrefab;
        private readonly float yOffset;
        private readonly float fadeTime;
        private readonly float scaleMultiplier;

        private List<TextAnimationData> _textAnimationDatas;

        public DamageTextManager(TMP_Text textPrefab,
            float yOffset,
            float fadeTime,
            AnimationCurve scaleCurve,
            float scaleMultiplier,
            AnimationCurve colorCurve,
            Color startColor,
            Color endColor)
        {
            _instance = this;

            _textAnimationDatas = new List<TextAnimationData>();
            //------------------------------------------------//

            this.textPrefab = textPrefab;
            this.yOffset = yOffset;
            this.fadeTime = fadeTime;
            this.scaleMultiplier = scaleMultiplier;

            //Setup Static Values for TextAnimationData
            //------------------------------------------------//
            TextAnimationData.ScaleCurve = scaleCurve;
            TextAnimationData.ColorCurve = colorCurve;
            TextAnimationData.StartColor = startColor;
            TextAnimationData.EndColor = endColor;
        }

        //Unity Functions
        //============================================================================================================//
        // Update is called once per frame
        public void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            for (int i = _textAnimationDatas.Count - 1; i >= 0 ; i--)
            {
                if (_textAnimationDatas[i].TryUpdateAnimation(deltaTime))
                    continue;
                
                //TODO Setup recycling for this
                Object.Destroy(_textAnimationDatas[i].Transform.gameObject);
                _textAnimationDatas.RemoveAt(i);
            }
        }
        
        //Create Text Functions
        //============================================================================================================//

        public static void CreateText(in int value, in Vector2 worldPosition)
        {
            _instance.TryCreateText(value, worldPosition);
        }

        private void TryCreateText(in int value, in Vector2 worldPosition)
        {
            var temp = Object.Instantiate(textPrefab, _parentTransform, false);
            var tempTransform = temp.transform;
            //------------------------------------------------//
            
            var targetPosition = worldPosition;
            targetPosition.y += yOffset;
            tempTransform.position = targetPosition;
            //------------------------------------------------//
            
            _textAnimationDatas.Add(new TextAnimationData
            {
                Transform = tempTransform,
                Text = temp,
                FadeTime = fadeTime,
                ScaleMult = scaleMultiplier
            });
            
            //------------------------------------------------//
            
            temp.text = value.ToString();
            temp.color = TextAnimationData.StartColor;
        }
        //============================================================================================================//

    }
}
