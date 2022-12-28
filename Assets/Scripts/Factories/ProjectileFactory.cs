using UnityEngine;

namespace Survivors.Factories
{
    public class ProjectileFactory : FactoryBase<SpriteRenderer>
    {
        public ProjectileFactory(SpriteRenderer prefab) : base(prefab)
        {
            
        }

        public SpriteRenderer CreateProjectile(in Vector2 worldPosition, in Sprite sprite, in Color32 color, in float scale = 1f, in Transform parent = null)
        {
            var temp = Create(worldPosition, parent);

            temp.sprite = sprite;
            temp.color = color;
            
            temp.transform.localScale = Vector3.one *scale;

            return temp;
        }
    }
}