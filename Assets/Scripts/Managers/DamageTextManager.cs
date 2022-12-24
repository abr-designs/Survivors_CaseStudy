using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Managers
{
    public class DamageTextManager : MonoBehaviour
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
        
        [SerializeField]
        private TMP_Text textPrefab;

        [SerializeField] private float yOffset;
        [SerializeField, Min(0f)]
        private float fadeTime;
        [SerializeField, Header("Scale")]
        private AnimationCurve scaleCurve;
        [SerializeField, Min(0f)]
        private float scaleMultiplier;
        [SerializeField, Header("Color")]
        private AnimationCurve colorCurve;
        [SerializeField]
        private Color startColor = Color.white;
        [SerializeField]
        private Color endColor = Color.white;

        private List<TextAnimationData> _textAnimationDatas;

        //Unity Functions
        //============================================================================================================//
        private void Awake()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _textAnimationDatas = new List<TextAnimationData>();

            //Setup Static Values for TextAnimationData
            //------------------------------------------------//
            TextAnimationData.ScaleCurve = scaleCurve;
            TextAnimationData.ColorCurve = colorCurve;
            TextAnimationData.StartColor = startColor;
            TextAnimationData.EndColor = endColor;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            for (int i = _textAnimationDatas.Count - 1; i >= 0 ; i--)
            {
                if (_textAnimationDatas[i].TryUpdateAnimation(deltaTime))
                    continue;
                
                //TODO Setup recycling for this
                Destroy(_textAnimationDatas[i].Transform.gameObject);
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
            var temp = Instantiate(textPrefab, transform, false);
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
            temp.color = startColor;
        }
        //============================================================================================================//

#if UNITY_EDITOR
        [ContextMenu("Test")]
        private void Test()
        {
            CreateText(Random.Range(1,999), Vector2.zero);
        }
#endif
    }
}
