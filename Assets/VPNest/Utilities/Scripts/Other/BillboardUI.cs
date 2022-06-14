using UnityEngine;

namespace VP.Nest.Utilities
{
	public class BillboardUI : MonoBehaviour
	{
		private Transform camTransform;

		private void Awake()
		{
			if (Camera.main)
			{
				Camera mainCam = Camera.main;
				camTransform = mainCam.transform;
				if (!GetComponent<Canvas>().worldCamera)
					GetComponent<Canvas>().worldCamera = mainCam;
			}
			else
				Debug.LogError("Main Camera is empty!");
		}

		private void Update()
		{
			transform.LookAt(transform.position + camTransform.rotation * Vector3.forward, camTransform.rotation * Vector3.up);
		}
	}
}