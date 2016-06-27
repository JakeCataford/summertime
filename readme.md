Summertime
----

## A (Object) Pool party for unity3D.

Summertime is a simple object pooling utility for Unity3D. It lets you easily implement object pooling into your game.

Summertime is built as a "magic" pooling layer that requires as little to know about the pooling logic as possible.

It also provides a resource spawning workflow that I frequently use for effects/spawnable things.

#### How it works

Summertime is particularily useful for explosions, one off particle effects or other ephermeal objects that you might spawn to do one thing and then want to remove.

Let's consider an explosion particle effect for an example.

You're going to have a GameObject, with a particle system component on it, saved in a prefab somewhere in a `resources` folder. Attached you might have a component you wrote like this:

```c#
public class Explosion : MonoBehaviour {
  public ParticleSystem ExplosionSystem;

  public IEnumerator Start() {
    ExplosionSystem.Emit(10);
    yield return new WaitForSeconds(ExplostionSystem.Particle.lifetime);
    Destroy(gameObject);
  }

  public static void At(Vector3 position) {
    Instantiate(Resources.Load<GameObject>("Explosion"), position, Quaternion.identity);
  }
}
```

And you would do something like this in another script:
```c#
Explosion.At(Vector3.zero);
```

This is cool and all, but it's not efficient because we are calling `Instantiate` and `Destroy` over and over. Summertime makes this a simple change to get a pooling functionality

```c#
using Summertime;
public class Explosion : PooledSpawnable {
  public ParticleSystem ExplosionSystem;

  // override onspawn instead of Start
  public override IEnumerator OnSpawn() {
    ExplosionSystem.Emit(10);
    yield return new WaitForSeconds(ExplostionSystem.Particle.lifetime);

    //Recycle automatically places the object into the pool and deactivates it.
    Recycle<Explosion>();
  }

  public static void At(Vector3 position) {
    // Spawn automatically loads and instantiates the Resource with the name "Explosion", or fetches it from the pool
    Spawn<Explosion>(position);
  }
}
```

The pool works by maintaining a per-scene, lazy instantiated dictionary of objects categorized by type. So objects of the same type can coexist up to the pool size, If the pool capacity is exceded, then we start to destroy objects and instantiate them as usual.

#### Wish list

This project is in a very prototypical stage, so here's a few things I would like to implement:

- Max Pool Size by type (probably via overrides)
- Allow "do not instantiate if pool is empty" for non-important objects
- Investigate ways to make this faster
- Flesh out the api to make it more robust
- Decouple the spawnable logic from the pooling logic
