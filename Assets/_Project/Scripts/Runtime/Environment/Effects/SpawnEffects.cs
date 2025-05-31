using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Runtime.Environment.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class SpawnEffects : MonoBehaviour
    {
        [SerializeField] private GameObject spawnVFX;
        [SerializeField] private float animationDuration = 1f;

        private void Start()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, animationDuration)
                .SetEase(Ease.OutBack);

            if (spawnVFX)
            {
                Instantiate(spawnVFX, transform.position, Quaternion.identity);
            }
            
            GetComponent<AudioSource>().Play();
        }
    }
}