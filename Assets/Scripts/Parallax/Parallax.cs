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

        [SerializeField] private float _manualBounds;

        void Awake ()
        {
            player = Camera.main.transform;
        }

        // Start is called before the first frame update
        void Start()
        {
            int offsetX = -screenCount / 2;
            for (int i = 0; i < screenCount; i++)
            {
                var screen = Instantiate(prefab, transform);
                var width = _manualBounds > 0 ? _manualBounds : screen.GetComponent<SpriteRenderer>().bounds.size.x;
                screen.transform.localPosition = new Vector3((i + offsetX) * width, screen.transform.localPosition.y, screen.transform.localPosition.z);
                screens.Add(screen.transform);
            }

            previousPlayerPos = player.position;
        }

        // Update is called once per frame
        void Update()
        {
            float parallax = (previousPlayerPos.x - player.position.x) * parallaxScale;
            float backgroundTargetPosX = transform.localPosition.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, backgroundTargetPos, smoothing * Time.deltaTime);
            previousPlayerPos = player.position;
        }
    }
}
