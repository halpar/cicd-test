using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace VP.Nest
{
    public class HandInputController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) Tap();
            if (Input.GetMouseButtonUp(0)) UnTap();
        }

        private void Tap()
        {
            transform.DOComplete();
            transform.DOScale(Vector3.one * .75f, .25f);
        }

        private void UnTap()
        {
            transform.DOComplete();
            transform.DOScale(Vector3.one, .25f);
        }
    }
}
