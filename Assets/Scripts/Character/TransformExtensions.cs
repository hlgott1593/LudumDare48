using UnityEngine;

namespace LD48 {
    public static class TransformExtensions {
        
        // Assing the layers on all children to our layer
        public static void AssignChildLayers(this GameObject goParent)
        {
            foreach (Transform child in goParent.transform)
            {
                child.ChangeLayersRecursively(goParent.layer);
            }
        }

        private static void ChangeLayersRecursively(this Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            foreach (Transform child in trans)
            {
                child.ChangeLayersRecursively(layer);
            }
        }
    }
}