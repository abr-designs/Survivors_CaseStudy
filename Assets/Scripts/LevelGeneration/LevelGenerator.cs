using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Survivors.LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private Color color;
        [SerializeField]
        private Material material;


        [SerializeField, Min(0f), Header("Tiles")]
        private float tileSize;
        [SerializeField, Min(1)]
        private Vector2Int tileDimensions;
        
        [SerializeField] 
        private SpriteRenderer[] _renderers;


        [ContextMenu("Generate Level")]
        private void GenerateLevel()
        {
            if(_renderers.Length > 0)
                ClearRenderers();

            var renderers = new List<SpriteRenderer>();

            var tempPrefab = new GameObject();
            var prefabSpriteRenderer = tempPrefab.AddComponent<SpriteRenderer>();

            prefabSpriteRenderer.sprite = sprite;
            prefabSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
            prefabSpriteRenderer.size = Vector2.one * tileSize;
            prefabSpriteRenderer.tileMode = SpriteTileMode.Continuous;
            prefabSpriteRenderer.material = material;

            var xOffset = (tileSize * (tileDimensions.x - 1)) / 2f;
            var yOffset = (tileSize * (tileDimensions.y - 1)) / 2f;

            for (var x = 0; x < tileDimensions.x; x++)
            {
                for (var y = 0; y < tileDimensions.y; y++)
                {
                    var position = new Vector2((x * tileSize) - xOffset, (y * tileSize) - yOffset);
                    var temp = Instantiate(prefabSpriteRenderer, position, Quaternion.identity, transform);
                    var gObj = temp.gameObject;
                    gObj.name = $"Tile_[{x}, {y}]";
                    gObj.isStatic = true;

                    temp.color = color;
                    renderers.Add(temp);
                }
            }

            DestroyImmediate(tempPrefab);
            _renderers = renderers.ToArray();
        }

        private void ClearRenderers()
        {
            for (var i = _renderers.Length - 1; i >= 0; i--)
            {
                DestroyImmediate(_renderers[i].gameObject);
            }

            _renderers = Array.Empty<SpriteRenderer>();
        }
    }
}
