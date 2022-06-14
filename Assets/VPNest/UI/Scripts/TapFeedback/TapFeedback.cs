using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VP.Nest.UI.TapFeedback
{
    public class TapFeedback : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Material _material;

        private float _animationTime = 0.4f;
        private float _currentTime = 0;
        private float _length = 300;
        private static Color _pokemonOrange = new Color(0.94f, 0.58f, 0.34f);
        private Color _color;

        private AnimationCurve _timeCurve;

        private void Awake()
        {
            RawImage image = GetComponent<RawImage>();
            _rectTransform = GetComponent<RectTransform>();
            _timeCurve = AnimationCurve.Linear(0, 0, 1, 1);
            // create a material instance in runtime to change properties without affecting the original material
            _material = Instantiate(image.material);
            image.material = _material;
            _color = _pokemonOrange;

            ResetMaterialVariables();
        }

        public void InitParticle(float animationTime = 0.4f, float length = 300)
        {
            _animationTime = animationTime;
            _rectTransform.sizeDelta = new Vector2(length, length);
            ResetMaterialVariables();
        }

        public void InitParticle(Color color, AnimationCurve curve, float animationTime = 0.4f, float length = 300)
        {
            _animationTime = animationTime;
            _length = length;
            _rectTransform.sizeDelta = Vector2.one * length;
            _color = color;
            _timeCurve = curve;
            ResetMaterialVariables();
        }

        public void PlayParticle(Vector3 particlePosition)
        {
            _rectTransform.position = particlePosition;
            gameObject.SetActive(true);
            StartCoroutine(StartClickAnimEnum());
        }

        private void ResetMaterialVariables()
        {
            _material.SetFloat("_R", 0);
            _material.SetColor("_Color", _color);
        }

        private IEnumerator StartClickAnimEnum()
        {
            while (_currentTime < _animationTime)
            {
                _currentTime += Time.deltaTime;
                _material.SetFloat("_R", _timeCurve.Evaluate(_currentTime / _animationTime));
                yield return null;
            }

            _currentTime = 0;
            _material.SetFloat("_R", 0);
            gameObject.SetActive(false);
        }
    }
}