using UnityEngine;
using VP.Nest.UI.Currency;
using VP.Nest.UI.InGame;

namespace VP.Nest.UI
{
	public class UIManager : Singleton<UIManager>
	{
		private InGameUI inGameUI;

		public InGameUI InGameUI {
			get {
				if (inGameUI == null)
					inGameUI = GetComponentInChildren<InGameUI>(true);
				if (inGameUI == null) {
					Debug.Log("InGameUI missing !");
				}

				return inGameUI;
			}
		}

		private CurrencyUI currencyUI;

		public CurrencyUI CurrencyUI {

			get {
				if (currencyUI == null)
					currencyUI = GetComponentInChildren<CurrencyUI>(true);
				if (currencyUI == null) {
					Debug.Log("CurrencyUI missing !");
				}

				return currencyUI;
			}
		}
	}
}