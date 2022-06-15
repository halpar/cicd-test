using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VP.Nest.Economy;

namespace VP.Nest.UI.Currency
{
	public class CurrencyUI : MonoBehaviour
	{

		private MoneyIconGroup moneyIconGroup;
		private TextMeshProUGUI moneyText;

		private bool isMoneyCounting;

		private void OnEnable()
		{
			GameEconomy.OnPlayerMoneyUpdate += OnPlayerMoneyUpdate;
		}

		private void OnPlayerMoneyUpdate(int newMoney, bool isSpend)
		{
			Debug.Log("Money Updated " + newMoney);
		}

		private void Awake()
		{
			moneyIconGroup = GetComponentInChildren<MoneyIconGroup>(true);
			moneyText = transform.Find("PlayerMoney").GetComponentInChildren<TextMeshProUGUI>(true);
			moneyText.SetText(GameEconomy.PlayerMoney.ToString());
		}

		[ContextMenu("Add 100 Money")]
		public void Add()
		{
			AddMoney(100);
		}

		[ContextMenu("Spend 50 Money")]
		public void Spend()
		{
			SpendMoney(50);
		}

		public void AddMoney(int amount)
		{
			if (isMoneyCounting) {
				Debug.Log("Money Already Counting , Cannot add money !!");
				return;
			}

			StartCoroutine(AddMoneyCoroutine(amount));
		}

		public IEnumerator AddMoneyCoroutine(int amount)
		{
			float currentMoney = GameEconomy.PlayerMoney;
			float nextMoney = currentMoney + amount;
			isMoneyCounting = true;

			moneyIconGroup.Init();

			yield return new WaitForSeconds(1.25f);
			yield return DOTween.To(() => currentMoney, x => currentMoney = x, nextMoney, 1).OnUpdate(() =>
					moneyText.SetText(Mathf.CeilToInt(currentMoney).ToString())).SetEase(Ease.OutCubic)
				.WaitForCompletion();

			isMoneyCounting = false;
			GameEconomy.AdjustPlayerMoney(amount);
		}

		public void SpendMoney(int amount)
		{
			if (isMoneyCounting) {
				Debug.Log("Money Already Counting , Cannot spend money !!");
				return;
			}

			float currentMoney = GameEconomy.PlayerMoney;
			float nextMoney = currentMoney - amount;
			isMoneyCounting = true;

			DOTween.To(() => currentMoney, x => currentMoney = x, nextMoney, 1).OnUpdate(() =>
					moneyText.SetText(Mathf.CeilToInt(currentMoney).ToString())).SetEase(Ease.OutCubic)
				.OnComplete(() => isMoneyCounting = false);

			GameEconomy.AdjustPlayerMoney(-amount);
		}

		private void OnDisable()
		{
			GameEconomy.OnPlayerMoneyUpdate -= OnPlayerMoneyUpdate;
		}
	}
}