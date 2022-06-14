using System;
using System.Collections.Generic;
using UnityEngine;

namespace VP.Nest.Utilities
{
	public class ObjectPooler : Singleton<ObjectPooler>
	{
		[Serializable]
		public class Pool
		{
			[Tooltip("Give a tag to the pool for calling it")]
			public string Tag;
			[Tooltip("The prefab to be pooled")]
			public GameObject Prefab;
			[Tooltip("The size (count) of the pool")]
			public int Size;
		}

		[SerializeField] private List<Pool> Pools = new List<Pool>();
		private Dictionary<string, Queue<GameObject>> PoolDictionary = new Dictionary<string, Queue<GameObject>>();

		private void Awake()
		{
			InitPool();
		}

		private void InitPool()
		{
			foreach (Pool pool in Pools)
				AddToPool(pool.Tag, pool.Prefab, pool.Size);
		}

		/// <summary>
		/// Spawns the pooled object to given position
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="position">Set the world position of the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Vector3 position)
		{
			GameObject obj = SpawnFromPool(poolTag);

			obj.transform.position = position;
			return obj;
		}

		/// <summary>
		/// Spawns the pooled object to given position and rotation
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="position">Set the world position of the object</param>
		/// <param name="rotation">Set the rotation of the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Vector3 position, Quaternion rotation)
		{
			GameObject obj = SpawnFromPool(poolTag);

			obj.transform.position = position;
			obj.transform.rotation = rotation;
			return obj;
		}

		/// <summary>
		/// Spawns the pooled object and parents the object to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="parent">Parent that will be assigned to the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Transform parent)
		{
			GameObject obj = SpawnFromPool(poolTag);

			obj.transform.SetParent(parent);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.forward = parent.forward;
			return obj;
		}

		/// <summary>
		/// Spawns the pooled object to given position and parents the object to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="position">Set the world position of the object</param>
		/// <param name="parent">Parent that will be assigned to the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Vector3 position, Transform parent)
		{
			GameObject obj = SpawnFromPool(poolTag);

			obj.transform.position = position;
			obj.transform.forward = parent.forward;
			obj.transform.SetParent(parent);
			return obj;
		}

		/// <summary>
		/// Spawns the pooled object to given position and rotation and parents the object to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the object to be spawned</param>
		/// <param name="position">Set the world position of the object</param>
		/// <param name="rotation">Set the rotation of the object</param>
		/// <param name="parent">Parent that will be assigned to the object</param>
		/// <returns>The object found matching the tag specified</returns>
		public GameObject Spawn(string poolTag, Vector3 position, Quaternion rotation, Transform parent)
		{
			GameObject obj = SpawnFromPool(poolTag);

			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.transform.SetParent(parent);
			return obj;
		}

		private GameObject SpawnFromPool(string poolTag)
		{
			if (!PoolDictionary.ContainsKey(poolTag)) return null;

			GameObject obj = PoolDictionary[poolTag].Dequeue();
			obj.SetActive(true);
			PoolDictionary[poolTag].Enqueue(obj);
			return obj;
		}


		/// <summary>
		/// Creates a new pool with defined tag and object
		/// </summary>
		/// <param name="poolTag">Tag for spawning objects</param>
		/// <param name="prefab">Object to be pooled</param>
		/// <param name="count">Count of the pool</param>
		public void AddToPool(string poolTag, GameObject prefab, int count)
		{
			if (PoolDictionary.ContainsKey(poolTag))
			{
				Debug.LogWarning(gameObject.name + ": \"" + poolTag + "\" Tag has already exists! Skipped.");
				return;
			}

			Queue<GameObject> queue = new Queue<GameObject>();
			for (int i = 0; i < count; i++)
			{
				GameObject obj = Instantiate(prefab, transform);
				obj.SetActive(false);
				queue.Enqueue(obj);
			}

			PoolDictionary.Add(poolTag, queue);
		}
	}
}