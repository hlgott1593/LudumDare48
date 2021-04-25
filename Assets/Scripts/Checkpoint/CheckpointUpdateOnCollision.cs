using UnityEngine;

namespace LD48
{
    public class CheckpointUpdateOnCollision : MonoBehaviour
    {
        [SerializeField] private Checkpoint newCheckpoint;
        
        private void OnTriggerEnter2D(Collider2D other) {
            var checkpointUpdater = other.gameObject.transform.root.GetComponentInChildren<ICheckpointUpdater>();
            if (checkpointUpdater as Object == null) return;
            CheckpointManager.Instance.ChangeCheckpoint(newCheckpoint);
        }
    }
}