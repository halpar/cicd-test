using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace VP.Nest.UI.Currency
{
	public class MoneyIconGroup : MonoBehaviour
	{
		public Transform target;
		private List<MoneyIconAnimations> anims;

		private void OnEnable()
		{
			if (anims is null)
			{
				anims = new List<MoneyIconAnimations>();
				foreach (Transform child in transform)
				{
					MoneyIconAnimations anim = child.GetComponent<MoneyIconAnimations>();
					anim.target = target;
					anims.Add(anim);
				}
			}
		}

		[ContextMenu("Init")]
		public void Init()
		{
			foreach (MoneyIconAnimations anim in anims)
			{
				anim.gameObject.SetActive(true);

				anim.onComplete.RemoveAllListeners();
				anim.onComplete.AddListener(() =>
				{
					target.DOComplete();
					target.DOPunchScale(Vector3.one * .9f, .2f, 2, .5f);

					anim.gameObject.SetActive(false);
				});
			}

			StartCoroutine(Helper.InvokeAction(() => { target.localScale = Vector3.one; }, 2.5f));
		}
	}
}