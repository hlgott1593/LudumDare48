using UnityEngine;

namespace LD48 {
    [CreateAssetMenu(fileName = "AnimationMap", menuName = "Spirit/AnimationMap", order = 0)]
    public class AnimationMap : ScriptableObject {
        public string WalkName;
        public string IdleName;
    }
}