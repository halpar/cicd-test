
using System;
using UnityEngine;
using VP.Nest;
using DG.Tweening;
using UnityEngine.Events;

namespace VP.Nest.UI.Currency
{
	public class MoneyIconAnimations : MonoBehaviour
	{
		public Transform target;
		public float duration = 2;

		public UnityEvent onComplete;

		private Vector3 startPos;

		private void OnEnable()
		{
			startPos = transform.position;

			transform.DOMove(target.position, duration).SetEase(Ease.InBack).OnComplete(() => {

				onComplete?.Invoke();
			});

		}

		private void OnDisable()
		{
			transform.position = startPos;
		}

	}


}


