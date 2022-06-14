using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VP.Nest.Utilities
{
	public class ParticlePooler : Singleton<ParticlePooler>
	{
		[Serializable]
		public class Pool
		{
			[Tooltip("Give a tag to the pool for calling it")]
			public string Tag;
			[Tooltip("Prefab of the Particle to be pooled")]
			public GameObject Prefab;
			[Tooltip("The size (count) of the pool")]
			public int Size;
			[Tooltip("Whether the Particle deactivates itself after finished playing")]
			public bool AutoDeactivate;
		}

		[SerializeField] private List<Pool> Pools = new List<Pool>();
		private Dictionary<string, Queue<ParticleSystem>> PoolDictionary = new Dictionary<string, Queue<ParticleSystem>>();

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
		/// Spawns the pooled particle to given position
		/// </summary>
		/// <param name="poolTag">Tag of the particle to be spawned</param>
		/// <param name="position">Set the world position of the particle</param>
		/// <returns>The particle found matching the tag specified</returns>
		public ParticleSystem Spawn(string poolTag, Vector3 position)
		{
			ParticleSystem particle = SpawnFromPool(poolTag);

			particle.transform.position = position;
			return particle;
		}

		/// <summary>
		/// Spawns the pooled particle to given position and rotation
		/// </summary>
		/// <param name="poolTag">Tag of the particle to be spawned</param>
		/// <param name="position">Set the world position of the particle</param>
		/// <param name="rotation">Set the rotation of the particle</param>
		/// <returns>The particle found matching the tag specified</returns>
		public ParticleSystem Spawn(string poolTag, Vector3 position, Quaternion rotation)
		{
			ParticleSystem particle = SpawnFromPool(poolTag);

			particle.transform.position = position;
			particle.transform.rotation = rotation;
			return particle;
		}

		/// <summary>
		/// Spawns the pooled particle and parents the particle to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the particle to be spawned</param>
		/// <param name="parent">Parent that will be assigned to the particle</param>
		/// <returns>The particle found matching the tag specified</returns>
		public ParticleSystem Spawn(string poolTag, Transform parent)
		{
			ParticleSystem particle = SpawnFromPool(poolTag);

			particle.transform.SetParent(parent);
			particle.transform.localPosition = Vector3.zero;
			particle.transform.forward = parent.forward;
			return particle;
		}

		/// <summary>
		/// Spawns the pooled particle to given position and parents the particle to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the particle to be spawned</param>
		/// <param name="position">Set the world position of the particle</param>
		/// <param name="parent">Parent that will be assigned to the particle</param>
		/// <returns>The particle found matching the tag specified</returns>
		public ParticleSystem Spawn(string poolTag, Vector3 position, Transform parent)
		{
			ParticleSystem particle = SpawnFromPool(poolTag);

			particle.transform.position = position;
			particle.transform.forward = parent.forward;
			particle.transform.SetParent(parent);
			return particle;
		}

		/// <summary>
		/// Spawns the pooled particle to given position and rotation and parents the particle to given Transform
		/// </summary>
		/// <param name="poolTag">Tag of the particle to be spawned</param>
		/// <param name="position">Set the world position of the particle</param>
		/// <param name="rotation">Set the rotation of the particle</param>
		/// <param name="parent">Parent that will be assigned to the particle</param>
		/// <returns>The particle found matching the tag specified</returns>
		public ParticleSystem Spawn(string poolTag, Vector3 position, Quaternion rotation, Transform parent)
		{
			ParticleSystem particle = SpawnFromPool(poolTag);

			particle.transform.position = position;
			particle.transform.rotation = rotation;
			particle.transform.SetParent(parent);
			return particle;
		}

		private ParticleSystem SpawnFromPool(string poolTag)
		{
			if (!PoolDictionary.ContainsKey(poolTag))
			{
				Debug.Log("\"" + poolTag + "\" tag doesn't exist!");
				return null;
			}

			ParticleSystem particle = PoolDictionary[poolTag].Dequeue();
			particle.gameObject.SetActive(true);
			particle.Play();

			PoolDictionary[poolTag].Enqueue(particle);

			int index = PoolDictionary.Keys.ToList().IndexOf(poolTag);
			if (Pools[index].AutoDeactivate && !particle.main.loop)
				StartCoroutine(Deactivate(particle));

			return particle;
		}

		private IEnumerator Deactivate(ParticleSystem particle)
		{
			float delay = .5f;
			while (particle.isPlaying)
			{
				yield return BetterWaitForSeconds.WaitRealtime(delay);
				if (particle.isStopped)
					particle.gameObject.SetActive(false);

				delay = Mathf.Pow(2, delay);
			}
		}

		/// <summary>
		/// Creates a new pool with defined tag and object of the particle
		/// </summary>
		/// <param name="poolTag">Tag for spawning particles</param>
		/// <param name="prefab">Particle to be pooled</param>
		/// <param name="count">Count of the pool</param>
		public void AddToPool(string poolTag, GameObject prefab, int count)
		{
			if (PoolDictionary.ContainsKey(poolTag))
			{
				Debug.LogWarning(gameObject.name + ": \"" + poolTag + "\" Tag has already exists! Skipped.");
				return;
			}

			Queue<ParticleSystem> queue = new Queue<ParticleSystem>();
			for (int i = 0; i < count; i++)
			{
				GameObject obj = Instantiate(prefab, transform);
				obj.SetActive(false);
				queue.Enqueue(obj.GetComponent<ParticleSystem>());
			}

			PoolDictionary.Add(poolTag, queue);
		}
	}
}