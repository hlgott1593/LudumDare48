using System;
using UnityEngine;

namespace LD48
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        [SerializeField] private Checkpoint initialCheckpoint;
        
        public event Action<Checkpoint> OnCheckpointLoaded = delegate { };
        public event Action<Checkpoint> OnCheckpointChanged = delegate { };
        public Checkpoint LastCheckpoint { get; set; }

        public void Start()
        {
            ChangeCheckpoint(initialCheckpoint);
        }

        public void ChangeCheckpoint(Checkpoint newCheckpoint)
        {
            LastCheckpoint = newCheckpoint;
            OnCheckpointChanged(newCheckpoint);
        }
        
        public void LoadLastCheckpoint()
        {
            OnCheckpointLoaded(LastCheckpoint);
        }
    }
}
