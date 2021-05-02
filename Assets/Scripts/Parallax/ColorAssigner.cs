using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace LD48 {
    [RequireComponent(typeof(Light2D))]
    public class ColorAssigner : MonoBehaviour {
        [SerializeField]
        public bool _randomColor;

        private Light2D _light;
        [SerializeField] private List<Color> _colors; 
        private void Start() {
            _light = GetComponent<Light2D>();
            _light.color = _randomColor ? _colors[Random.Range(0, _colors.Count)] : _colors[0];
        }
    }
}