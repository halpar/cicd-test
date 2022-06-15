using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VP.Nest;

namespace VP.Nest.UI.Currency
{
	public class MoneyIconGroup : MonoBehaviour
	{
		public Transform target;
		private List<MoneyIconAnimations> anims;

		private void OnEnable()
		{
			if (anims is null) {
				anims = new List<MoneyIconAnimations>();
				foreach (Transform child in transform) {
					MoneyIconAnimations anim = child.GetComponent<MoneyIconAnimations>();
					anim.target = target;
					anims.Add(anim);
					anim.duration = Random.Range(1.5f, 1.8f);
				}
			}
		}

		[ContextMenu("Init")]
		public void Init()
		{
			foreach (var anim in anims) {

				anim.gameObject.SetActive(true);

				anim.onComplete.RemoveAllListeners();
				anim.onComplete.AddListener(() => {
					target.localScale = Vector3.one * 1.1f;
					StartCoroutine(Helper.InvokeAction(() => { target.localScale = Vector3.one; }, 0.2f));

					anim.gameObject.SetActive(false);
				});
			}

			StartCoroutine(Helper.InvokeAction(() => { target.localScale = Vector3.one; }, 2.5f));
		}
	}
}