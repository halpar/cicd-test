using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace VP.Nest.UI.Currency
{
	public class MoneyIconAnimations : MonoBehaviour
	{
		public Transform target;
		public float duration => UIManager.Instance.CurrencyUI.moneyAnimationDuration;

		public UnityEvent onComplete;

		private Vector3 startPos;

		private void OnEnable()
		{
			startPos = transform.position;

			transform.DOLocalRotate(Vector3.forward * Random.Range(180, 720), duration,RotateMode.FastBeyond360).SetEase(Ease.InExpo);
			transform.DOMove(target.position, duration).SetEase(Ease.InBack).OnComplete(() => onComplete?.Invoke());
		}

		private void OnDisable()
		{
			transform.position = startPos;
		}
	}
}