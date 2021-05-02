using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD48
{
    public class Parallax : MonoBehaviour
    {
        public Transform screen;
        public float parallaxScale;
        public float smoothing = 1f;

        public Transform cam;
        private Vector3 previousCamPos;

        void Awake ()
        {
            cam = Camera.main.transform;
        }

        // Start is called before the first frame update
        void Start()
        {
            previousCamPos = cam.position;
        }

        // Update is called once per frame
        void Update()
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;
            float backgroundTargetPosX = transform.localPosition.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, backgroundTargetPos, smoothing * Time.deltaTime);
            previousCamPos = cam.position;
        }
    }
}
