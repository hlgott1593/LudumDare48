using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    public class SceneLoadingTest : MonoBehaviour
    {
        // Start is called before the first frame update
        public void StartLoad(string sceneName)
        {
            SceneLoadingManager.LoadScene(sceneName);
        }

    }
}
