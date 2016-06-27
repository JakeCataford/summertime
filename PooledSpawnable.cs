using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summertime {
	public abstract class PooledSpawnable : MonoBehaviour {
		public void Recycle<T>() {
			if(ObjectPool.AddNew<T>(gameObject)) {
				gameObject.SetActive(false);
				return;
			}

			Destroy(gameObject);
		}

		public static GameObject Spawn<V>(Vector3 position) {
			GameObject instance = ObjectPool.GetFromPool<V>();

			if (instance == null) {
				instance = Instantiate(Resources.Load<GameObject>(typeof(V).Name));
			}

			instance.SetActive(true);
			instance.transform.position = position;
			instance.GetComponent<PooledSpawnable>().StartCoroutine(instance.GetComponent<PooledSpawnable>().OnSpawn());

			return instance;
		}

		public virtual IEnumerator OnSpawn() { yield return null; }

		protected IEnumerator Start() {
			yield return OnSpawn();
		}
	}
}
