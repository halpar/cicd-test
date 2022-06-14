using System.Collections.Generic;
using UnityEngine;
using VP.Nest.SceneManagement;

namespace VP.Nest.UI.TapFeedback
{
    public class TapFeedbackController : MonoBehaviour
    {
        [Tooltip("Animation length in seconds")] [SerializeField]
        private float animationTime = 1;

        [Tooltip("Width and height of the feedback particle")] [SerializeField]
        private float size = 300;

        [Tooltip("Animation progression over time curve")] [SerializeField]
        private AnimationCurve timeCurve;

        [Tooltip("Feedback particle prefab")] [SerializeField]
        private GameObject particlePrefab;

        [Tooltip("Feedback particle color")] [SerializeField]
        private Color32 particleColor;

        private Queue<TapFeedback> particleQueue;
        private List<TapFeedback> particleList;

        private void Awake()
        {
            particleQueue = new Queue<TapFeedback>();
            particleList = new List<TapFeedback>();

            for (int i = 0; i < 10; i++)
            {
                var particle = Instantiate(particlePrefab).GetComponent<TapFeedback>();
                particle.transform.SetParent(transform, false);
                particle.InitParticle(particleColor, timeCurve, animationTime, size);
                particleQueue.Enqueue(particle);
                particleList.Add(particle);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !LevelManager.IsLevelPlaying)
            {
                PlayParticle(Input.mousePosition);
            }
        }

        /// <summary>
        /// Play a particle at a specified position on canvas.
        /// </summary>
        /// <param name="position">Screen space position on canvas</param>
        public void PlayParticle(Vector3 position)
        {
            TapFeedback playedParticle = particleQueue.Dequeue();
            playedParticle.PlayParticle(position);
            particleQueue.Enqueue(playedParticle);
        }

        public void UpdateParticles()
        {
            foreach (TapFeedback particle in particleList)
            {
                particle.InitParticle(particleColor, timeCurve, animationTime, size);
            }
        }
    }
}