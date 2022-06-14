using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VP.Nest.UI;

namespace QA
{
    public class QAMoneyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Button moneyButton;
        private Ray ray;
        private RaycastHit hit;
        private Coroutine giveMoneyCoroutine;

        public void OnPointerDown(PointerEventData eventData)
        {
            UIManager.Instance.CurrencyUI.AddMoney(10000, false);
            StartCoroutine(WaitForContionusMoneyGive());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (giveMoneyCoroutine != null) StopCoroutine(giveMoneyCoroutine);
        }

        private IEnumerator WaitForContionusMoneyGive()
        {
            float waitDuration = .8f;
            float elapsedTime = 0;

            while (Input.GetMouseButton(0) && elapsedTime < waitDuration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            if (Input.GetMouseButton(0))
            {
                giveMoneyCoroutine = StartCoroutine(GiveMoneyContionusly());
            }
        }

        private IEnumerator GiveMoneyContionusly()
        {
            float power = 1;
            while (Input.GetMouseButton(0))
            {
                UIManager.Instance.CurrencyUI.AddMoney((int) MathF.Pow(10000, power), false);
                if (power < 1.5f) power += .05f;
                yield return BetterWaitForSeconds.Wait(.3f);
            }
        }
    }
}