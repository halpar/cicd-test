using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VP.Nest.Economy;
using VP.Nest.Utilities;

namespace VP.Nest.UI.Currency
{
	public class CurrencyUI : MonoBehaviour
	{
		private MoneyIconGroup moneyIconGroup;
		private TextMeshProUGUI moneyText;

		private bool isMoneyCounting;

		private Camera cam => UsefulFunctions.MainCamera;

		[SerializeField] private Transform target;
		[SerializeField] private GameObject moneyIconPrefab;

		[Space]
		public float moneyAnimationDuration = 1.5f;

		private void OnEnable()
		{
			GameEconomy.OnPlayerMoneyUpdate += OnPlayerMoneyUpdate;
		}

		private void OnDisable()
		{
			GameEconomy.OnPlayerMoneyUpdate -= OnPlayerMoneyUpdate;
		}

		private void OnPlayerMoneyUpdate(int newMoney, bool isSpend)
		{
#if UNITY_EDITOR
			Debug.Log("Money Updated " + newMoney);
#endif
		}

		private void Awake()
		{
			moneyIconGroup = GetComponentInChildren<MoneyIconGroup>(true);
			moneyText = transform.Find("PlayerMoney").GetComponentInChildren<TextMeshProUGUI>(true);
			moneyText.SetText(MoneyFormatter.FormatMoney(GameEconomy.PlayerMoney));

			if (gameObject.activeSelf)
				ObjectPooler.Instance.AddToPool("MoneyIcon", moneyIconPrefab, 100);

			transform.SetSiblingIndex(100);
		}

		[ContextMenu("Add 100 Money")]
		private void Add()
		{
			AddMoney(100);
		}

		[ContextMenu("Spend 50 Money")]
		private void Spend()
		{
			SpendMoney(50);
		}

		/// <summary>
		/// Adds money and plays a scattered animation
		/// </summary>
		/// <param name="amount">Amount of money you want to add</param>
		/// <param name="isAnimated">Whether the money will be animated or not</param>
		public void AddMoney(int amount, bool isAnimated = true)
		{
			if (isMoneyCounting)
			{
#if UNITY_EDITOR
				Debug.Log("Money Already Counting , Cannot add money !!");
#endif
				return;
			}

			StartCoroutine(AddMoneyCoroutine(amount, isAnimated));
		}

		/// <summary>
		/// Adds money and plays an harmonious animation that starts at given position
		/// </summary>
		/// <param name="amount">Amount of money you want to add</param>
		/// <param name="from">The position that animation will be played from</param>	
		/// <param name="multiple">Will the animated icons be more than one? (Default is false)</param>
		public void AddMoney(int amount, Vector3 from, bool multiple = false)
		{
			if (amount.Equals(0)) return;
			StartCoroutine(AddMoneyCoroutine(amount, from, multiple));
		}

		private float currentMoney, nextMoney;
		private IEnumerator AddMoneyCoroutine(int amount, bool isAnimated)
		{
			currentMoney = GameEconomy.PlayerMoney;
			nextMoney = currentMoney + amount;
			isMoneyCounting = true;

			GameEconomy.AdjustPlayerMoney(amount);

			if (isAnimated)
			{
				moneyIconGroup.Init();
				yield return BetterWaitForSeconds.WaitRealtime(moneyAnimationDuration - moneyAnimationDuration / 6f);
			}
			
			moneyText.transform.DOComplete();
			moneyText.transform.DOPunchScale(Vector3.one * .4f, .2f, 2, .5f);
			moneyText.SetText(MoneyFormatter.FormatMoney(Mathf.CeilToInt(nextMoney)));
			
			target.DOComplete();
			target.DOPunchScale(Vector3.one * .8f, .2f, 2, .5f);

			// yield return DOTween.To(() => currentMoney, x => currentMoney = x, nextMoney, moneyAnimationDuration/2f).OnUpdate(() =>
			// 	moneyText.SetText(Mathf.CeilToInt(currentMoney).ToString())).SetEase(Ease.OutCubic).WaitForCompletion();

			isMoneyCounting = false;
		}

		private IEnumerator AddMoneyCoroutine(int amount, Vector3 fromPosition, bool multiple = false)
		{
			float currentMoney = GameEconomy.PlayerMoney;
			float nextMoney = currentMoney + amount;
			GameEconomy.AdjustPlayerMoney(amount);

			Vector3 pos = cam.WorldToScreenPoint(fromPosition);

			int count = multiple ? Mathf.Clamp(amount, 1, 10) : 1;

			for (int i = 0; i < count; i++)
			{
				GameObject icon = ObjectPooler.Instance.Spawn("MoneyIcon", pos, transform);
				icon.transform.localScale = Vector3.one;
				icon.transform.DOMove(target.position, moneyAnimationDuration).SetEase(Ease.InBack).SetId("icon").OnComplete(() =>
				{
					target.DOComplete();
					target.DOPunchScale(Vector3.one * .9f, .2f, 2, .5f);
					icon.gameObject.SetActive(false);

					moneyText.SetText(Mathf.CeilToInt(Mathf.Lerp(int.Parse(moneyText.text), nextMoney, .5f)).ToString());
				});

				yield return BetterWaitForSeconds.WaitRealtime(.04f);
			}

			yield return BetterWaitForSeconds.WaitRealtime(moneyAnimationDuration);
			moneyText.SetText(MoneyFormatter.FormatMoney(Mathf.CeilToInt(nextMoney)));
		}

		/// <summary>
		/// Spends the money by given amount
		/// </summary>
		/// <param name="amount">The amount of money will be spent</param>
		/// <param name="isAnimated">Will there be an animation? (Default is true)</param>
		public void SpendMoney(int amount, bool isAnimated = true)
		{
			float currentMoney = GameEconomy.PlayerMoney;
			float nextMoney = currentMoney - amount;

			if (nextMoney < 0)
			{
				Debug.LogWarning("Not enough money");
				return;
			}

			isMoneyCounting = true;

			GameEconomy.AdjustPlayerMoney(-amount);

			DOTween.Complete("SpendMoney");
			DOTween.To(() => currentMoney, x => currentMoney = x, nextMoney, moneyAnimationDuration).SetId("SpendMoney").SetEase(Ease.OutCubic)
				.OnUpdate(() => moneyText.SetText(MoneyFormatter.FormatMoney(Mathf.CeilToInt(nextMoney)))).OnComplete(() => isMoneyCounting = false);

			if (isAnimated)
				StartCoroutine(SpendMoneyCoroutine(amount));
		}

		private IEnumerator SpendMoneyCoroutine(int amount)
		{
			int count = Mathf.Clamp(amount, 1, 6);
			for (int i = 0; i < count; i++)
			{
				string tweenId = "moneyRotate" + i;
				Image icon = ObjectPooler.Instance.Spawn("MoneyIcon", target).GetComponentInChildren<Image>();
				icon.transform.DOLocalMove(Vector3.right * Random.Range(-100f, 100f) + Vector3.down * Random.Range(200f, 300f), moneyAnimationDuration).SetEase(Ease.OutCubic)
					.OnComplete(() => DOTween.Kill(tweenId));
				icon.transform.DORotate(Vector3.forward * 360, Random.Range(.5f, 1f), RotateMode.FastBeyond360).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Incremental)
					.SetId(tweenId);
				icon.DOAlpha(0, moneyAnimationDuration).OnComplete(() =>
				{
					icon.gameObject.SetActive(false);
					Color tempColor = icon.color;
					tempColor.a = 1;
					icon.color = tempColor;
				});

				yield return BetterWaitForSeconds.WaitRealtime(.1f);
			}
		}
	}
}