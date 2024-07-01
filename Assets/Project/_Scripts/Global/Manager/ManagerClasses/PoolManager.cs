using System.Collections.Generic;
using UnityEngine;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
   [DefaultExecutionOrder(560)]
   public class PoolManager : MonoBehaviour
   {
      #region Fields
      [System.Serializable]
      public class Pool
      {
         public string ObjectName;
         public GameObject Object;
         public List<GameObject> Objects { get; set; }
         public Transform ObjectHolder;
         public int PoolSize;
      }

      public List<Pool> Pools;
      public static Dictionary<string, Queue<GameObject>> SPoolDictionary;

      #endregion

      #region Unity Functions
      public void Start()
      {
         //Initializer
         SPoolDictionary = new Dictionary<string, Queue<GameObject>>();

         foreach (Pool pool in Pools)
         {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.PoolSize; i++)
            {
               GameObject poolObject = Instantiate(pool.Object, pool.ObjectHolder);
               poolObject.name = pool.ObjectName;

               pool.Objects = new List<GameObject>
               {
                  poolObject
               };

               poolObject.SetActive(false);

               //Mark the object in the pool
               objectPool.Enqueue(poolObject);
            }

            SPoolDictionary.Add(pool.ObjectName, objectPool);
         }
      }
      #endregion
      
      #region Pooling Behaviours
      /// <summary>
      /// Spawning the object as type of GameObject with given Transform data
      /// </summary>
      /// <param name="type"></param>
      /// <param name="spawnPosition"></param>
      /// <param name="spawnRotation"></param>
      /// <returns></returns>
      public GameObject SpawnFromPool(string type, Vector3 spawnPosition, Quaternion spawnRotation)
      {
         if (SPoolDictionary.ContainsKey(type) == false) { return null; }

         //Mark the object in the pool
         GameObject objectToSpawn = SPoolDictionary[type].Dequeue();

         objectToSpawn.transform.position = spawnPosition;
         objectToSpawn.transform.rotation = spawnRotation;

         objectToSpawn.SetActive(true);

         //Mark the object in the pool
         SPoolDictionary[type].Enqueue(objectToSpawn);
         return objectToSpawn;
      }

      /// <summary>
      /// Spawning the object as type of GameObject with given Transform data
      /// </summary>
      /// <param name="type"></param>
      /// <param name="spawnPosition"></param>
      /// <param name="spawnRotation"></param>
      /// <returns></returns>
      public T SpawnFromPool<T>(string type, Vector3 spawnPosition, Quaternion spawnRotation) where T : MonoBehaviour
      {
         if (SPoolDictionary.ContainsKey(type) == false) { return null; }

         //Mark the object in the pool
         GameObject objectToSpawn = SPoolDictionary[type].Dequeue();

         objectToSpawn.transform.position = spawnPosition;
         objectToSpawn.transform.rotation = spawnRotation;

         objectToSpawn.gameObject.SetActive(true);

         //Mark the object in the pool
         SPoolDictionary[type].Enqueue(objectToSpawn.gameObject);

         return objectToSpawn.GetComponent<T>();
      }

      /// <summary>
      /// Disables the object and returns it to the pool
      /// </summary>
      /// <param name="poolObject"></param>
      public void DestroyPoolObject<T>(T poolObject) where T : MonoBehaviour
      {
         poolObject.gameObject.SetActive(false);

         Pool pool = Pools.Find(x => x.Objects.Find(y => poolObject.gameObject));

         poolObject.transform.parent = pool.ObjectHolder;
         poolObject.transform.localScale = pool.Object.transform.localScale;
         poolObject.transform.localPosition = Vector3.zero;

         SPoolDictionary[pool.ObjectName].Enqueue(poolObject.gameObject);
      }
      #endregion
   }
}