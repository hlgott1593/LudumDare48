using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    public class Parallax : MonoBehaviour
    {
        public GameObject prefab;

        public List<Transform> screens;
        public int screenCount = 5;
        public float parallaxScale;
        public float smoothing = 1f;

        public Transform player;
        private Vector3 previousPlayerPos;


        void Awake ()
        {
            player = GameObject.Find("Shaman").transform;
        }

        // Start is called before the first frame update
        void Start()
        {
            int offsetX = -screenCount / 2;
            for (int i = 0; i < screenCount; i++)
            {
                var screen = Instantiate(prefab, transform);
                var width = screen.GetComponent<SpriteRenderer>().bounds.size.x;
                screen.transform.position = new Vector3((i + offsetX) * width, screen.transform.position.y, screen.transform.position.z);
                screens.Add(screen.transform);
            }

            previousPlayerPos = player.position;
        }

        // Update is called once per frame
        void Update()
        {
            float parallax = (previousPlayerPos.x - player.position.x) * parallaxScale;
            float backgroundTargetPosX = transform.position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, smoothing * Time.deltaTime);
            previousPlayerPos = player.position;
        }
    }
}
