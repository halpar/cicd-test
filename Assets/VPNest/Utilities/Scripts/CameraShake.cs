using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

namespace VP.Nest.Utilities
{
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CameraShake : MonoBehaviour
	{
		private CinemachineBasicMultiChannelPerlin perlin;

		private float startingIntensity;
		private float shakeTimer;
		private float shakeTimerTotal;

		public UnityAction OnComplete;

		private void Awake()
		{
			perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			perlin.m_AmplitudeGain = 0;
		}

		/// <summary>
		/// Shakes the camera
		/// </summary>
		/// <param name="intensity">How much shake will be applied to camera</param>
		/// <param name="duration">How long the camera shake will take</param>
		/// <param name="isSmooth">Will the shake slow down gradually?</param>
		public void Shake(float intensity, float duration, bool isSmooth = true)
		{
			perlin.m_AmplitudeGain = intensity;

			startingIntensity = intensity;
			shakeTimer = duration;
			shakeTimerTotal = duration;

			StopCoroutine(ShakeCoroutine(isSmooth));
			StartCoroutine(ShakeCoroutine(isSmooth));
		}

		/// <summary>
		/// Shakes the camera
		/// </summary>
		/// <param name="intensity">How much shake will be applied to camera</param>
		/// <param name="frequency">How rapidly will the shake applied</param>
		/// <param name="duration">How long the camera shake will take</param>
		/// <param name="isSmooth">Will the shake slow down gradually?</param>
		public void Shake(float intensity, float frequency, float duration, bool isSmooth = true)
		{
			perlin.m_FrequencyGain = frequency;

			Shake(intensity, duration, isSmooth);
		}

		private IEnumerator ShakeCoroutine(bool isSmooth)
		{
			while (shakeTimer > 0)
			{
				shakeTimer -= Time.deltaTime;

				if (isSmooth)
					perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTimerTotal));
				else
				{
					if (shakeTimer <= 0)
						perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0, 1 - (shakeTimer / shakeTimerTotal));
				}

				yield return UsefulFunctions.BetterWaitForSeconds.Wait(Time.deltaTime);
			}

			perlin.m_AmplitudeGain = 0;
			perlin.m_FrequencyGain = 1;
			
			OnComplete?.Invoke();
		}
	}
}