using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    public class SlowMove : MonoBehaviour
    {
        public GameObject prefab;

        public List<Transform> screens;
        public int screenCount = 5;
        public float speed;
        public float smoothing = 1f;

        public Bounds bounds;

        void Awake ()
        {
           
        }

        // Start is called before the first frame update
        void Start()
        {
            int offsetX = -screenCount / 2;
            for (int i = 0; i < screenCount; i++)
            {
                var screen = Instantiate(prefab, transform);
                bounds = screen.GetComponent<SpriteRenderer>().bounds;
                var width = bounds.size.x;
                screen.transform.position = new Vector3((i + offsetX) * width, screen.transform.position.y, screen.transform.position.z);
                screens.Add(screen.transform);
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var screen in screens)
            {
                screen.transform.localPosition = Vector3.Lerp(
                    screen.transform.localPosition,
                    screen.transform.localPosition + Vector3.right * speed,
                    smoothing * Time.deltaTime
                );
                if (screen.transform.position.x > bounds.size.x * (screenCount - screenCount / 2))
                {
                    screen.transform.position += Vector3.left * bounds.size.x * screenCount;
                }
            }
        }
    }
}
