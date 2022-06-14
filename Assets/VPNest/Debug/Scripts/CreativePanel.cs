using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP.Nest.CreativeBuild
{
	public class CreativePanel : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}

}
