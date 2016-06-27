using UnityEngine;
using System.Collections.Generic;

namespace Summertime {
	public class ObjectPool : MonoBehaviour {
		private const int POOL_SIZE = 10;
		public static ObjectPool Instance;
		public Dictionary<System.Type, List<GameObject>> objectPool = new Dictionary<System.Type, List<GameObject>>();

		public static bool AddNew<T>(GameObject pooledObject) {
			ensureInstanceInScene();

			if (!Instance.objectPool.ContainsKey(typeof(T))) {
				Instance.objectPool[typeof(T)] = new List<GameObject>();
			}

			if (Instance.objectPool[typeof(T)].Count > POOL_SIZE) return false;
			Instance.objectPool[typeof(T)].Add(pooledObject);
			return true;
		}

		public static GameObject GetFromPool<T>() {
			ensureInstanceInScene();

			if (Instance.objectPool.ContainsKey(typeof(T)) && Instance.objectPool[typeof(T)].Count > 0) {
				GameObject pooledObject = Instance.objectPool[typeof(T)][0];
				Instance.objectPool[typeof(T)].Remove(pooledObject);
				return pooledObject;
			}

			return null;
		}

		private static void ensureInstanceInScene() {
			if (Instance == null) {
				GameObject go = new GameObject();
				go.name = "ObjectPool";
				Instance = go.AddComponent<ObjectPool>();
			}
		}
	}
}
