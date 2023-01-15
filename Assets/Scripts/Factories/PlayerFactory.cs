using System.Collections.Generic;
using Survivors.Player;
using Survivors.ScriptableObjets;
using UnityEngine;

namespace Survivors.Factories
{
    public class PlayerFactory : FactoryBase<SpriteRenderer>
    {
        private readonly Dictionary<string, PlayerProfileScriptableObject> _playerProfiles;
        
        public PlayerFactory(SpriteRenderer prefab, IEnumerable<PlayerProfileScriptableObject> playerProfiles) : base(prefab)
        {
            _playerProfiles = new Dictionary<string, PlayerProfileScriptableObject>();
            foreach (var playerProfile in playerProfiles)
            {
                _playerProfiles.Add(playerProfile.name, playerProfile);
            }
        }
        
        //FIXME I should be using a GUID or something
        public (SpriteRenderer, PlayerProfileScriptableObject) CreatePlayer(in string name, in Vector2 worldPosition, in Transform parent = null)
        {
            if (_playerProfiles.TryGetValue(name, out var playerProfile) == false)
                throw new KeyNotFoundException($"No enemy with name {name}");

            var playerSpriteRenderer = Create(worldPosition, parent);

            playerSpriteRenderer.name = $"{name}_Instance";

            return (playerSpriteRenderer, playerProfile);
        }
    }
}