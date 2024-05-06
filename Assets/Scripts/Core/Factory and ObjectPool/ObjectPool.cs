using System.Collections.Generic;
using DG.Tweening;
using Factory_and_ObjectPool;
using UnityEngine;

namespace Core.Factory_and_ObjectPool
{
    public class ObjectPool
    {
        public int PoolCount => _objectPool.Count;
        private PoolableObjectInitializeData InitializeData { get; }

        private readonly Transform _poolParent;
        private readonly PoolableObject _prefab;
        private readonly int _refillCount;
        private readonly Queue<PoolableObject> _objectPool = new Queue<PoolableObject>();
        private int _index;

        public ObjectPool(PoolableObject prefab, int count, PoolableObjectInitializeData poolableObjectInitializeData,
            int refillCount = 10, Transform parent = null)
        {
            _refillCount = refillCount;
            _prefab = prefab;
            _poolParent = parent ? parent : new GameObject($"{prefab.name} Pool").transform;
            InitializeData = poolableObjectInitializeData;
            Fill(count);
        }

        private void Fill(int count)
        {
            if (CheckIfPrefabNullOrEmpty()) return;

            for (var i = 0; i < count; i++)
            {
                var entity = Object.Instantiate(_prefab);
                entity.gameObject.name += _index;
                _index++;
                entity.Initialize(InitializeData);
                entity.SetObjectPool(this);
                ReturnEntity(entity);
            }
        }

        private bool CheckIfPrefabNullOrEmpty()
        {
            if (!_prefab)
            {
                return true;
            }

            return false;
        }

        public PoolableObject GetEntity()
        {
            PoolableObject entity;

            if (_objectPool.Count > 0)
            {
                entity = _objectPool.Dequeue();
            }
            else
            {
                Fill(_refillCount);
                entity = _objectPool.Dequeue();
            }

            if (entity)
            {
                entity.gameObject.SetActive(true);
            }
            else
            {
                entity = GetEntity();
            }

            return entity;
        }

        public void ReturnEntity(PoolableObject entity)
        {
            if (!entity) return;
            if (_objectPool.Contains(entity)) return; 
                entity.gameObject.SetActive(false);
            entity.transform.SetParent(_poolParent);

            var itemTransform = entity.gameObject.transform;
            itemTransform.position = Vector3.zero;
            itemTransform.eulerAngles = Vector3.zero;
            entity.gameObject.transform.DOKill();
            _objectPool.Enqueue(entity);
        }
        
        public List<PoolableObject> GetAllPooledObjects()
        {
            return new List<PoolableObject>(_objectPool);
        }
    }
}