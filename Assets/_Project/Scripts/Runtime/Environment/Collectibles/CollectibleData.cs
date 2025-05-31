using _Project.Scripts.Runtime.Core.SpawnSystem;
using UnityEngine;

namespace _Project.Scripts.Runtime.Environment.Collectibles
{
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "Platformer/Collectible/CollectibleData")]
    public class CollectibleData : EntityData
    {
        public int score;
        
        // Additional properties specific to collectibles.
    }
}