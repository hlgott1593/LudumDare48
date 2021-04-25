using UnityEngine;

namespace LD48
{
    public class Checkpoint : MonoBehaviour
    {
        public Transform SpawnPoint => spawnPoint;

        [SerializeField]protected Transform spawnPoint;
    }
}