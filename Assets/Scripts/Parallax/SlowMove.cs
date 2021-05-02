using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    public class SlowMove : MonoBehaviour
    {
        public GameObject screen;

        public List<Transform> screens;
        public float speed;
        public float smoothing = 1f;
        private Vector3 startPos;

        public Bounds bounds;

        // Start is called before the first frame update
        void Start()
        {
            startPos = screen.transform.position;
            screens.Add(screen.transform);
            bounds = screen.GetComponent<SpriteRenderer>().bounds;
            var width = bounds.size.x;
            // Create second screen
            var screen2 = Instantiate(screen, transform);
            screen2.transform.position = new Vector3( screen.transform.position.x - width, screen.transform.position.y, screen.transform.position.z);
            screens.Add(screen2.transform);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var scrn in screens)
            {
                scrn.transform.localPosition = Vector3.Lerp(
                    scrn.transform.localPosition,
                    scrn.transform.localPosition + Vector3.right * speed,
                    smoothing * Time.deltaTime
                );
                if (scrn.transform.position.x > startPos.x + bounds.size.x)
                {
                    scrn.transform.position += Vector3.left * bounds.size.x * 2;
                }
            }
        }
    }
}
