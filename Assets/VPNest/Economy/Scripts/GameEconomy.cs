
using UnityEngine;
using UnityEngine.Events;

namespace VP.Nest.Economy
{
	public static class GameEconomy
	{
		public static UnityAction<int, bool> OnPlayerMoneyUpdate;
		public static int PlayerMoney => PlayerPrefs.GetInt("PlayerMoney", 0);

		public static void AdjustPlayerMoney(int amount)
		{
			var currentMoney = PlayerMoney;
			var nextMoney = PlayerMoney + amount;
			if (nextMoney < 0)
				nextMoney = 0;
			PlayerPrefs.SetInt("PlayerMoney", nextMoney);
			OnPlayerMoneyUpdate?.Invoke(nextMoney, nextMoney - currentMoney > 0);
		}

		public static bool HasPlayerEnoughMoney(int amount)
		{
			return amount <= PlayerMoney;
		}
	}
}