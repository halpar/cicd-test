using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FtueUI : Singleton<FtueUI>
{
    private GameObject joystickFtue;
    private GameObject pathDragDropFtue;
    private GameObject dragFtue;

    private void Awake()
    {
        joystickFtue = transform.Find("JoystickFtue").gameObject;
        pathDragDropFtue = transform.Find("PathDragDropFtue").gameObject;
        dragFtue = transform.Find("DragFtue").gameObject;
    }

    public void JoystickFtue(bool active)
    {
        joystickFtue.SetActive(active);
    }

    public void DragFtue(bool active)
    {
        dragFtue.SetActive(active);
    }

    public void DragDropFtue(bool active)
    {
        pathDragDropFtue.SetActive(active);
    }
}