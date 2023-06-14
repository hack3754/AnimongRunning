using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MRPG
{
    [System.Serializable]
    public class MPool<T> where T : Component, new()
    {
        Transform m_parent;
        GameObject m_baseResource;
        public readonly int m_max;
        readonly Stack<T> m_freeObjects;

        public MPool(GameObject baseObj, Transform parent, int initialCapacity = 1, int max = int.MaxValue)
        {
            m_parent = parent;
            m_baseResource = baseObj;
            m_freeObjects = new Stack<T>();
            for (int i = 0; i < initialCapacity; i++)
            {
                var obj = CreateObj();
                obj.gameObject.SetActive(false);
                m_freeObjects.Push(obj);
            }
            this.m_max = max;
        }

        public T Obtain()
        {
            T v = m_freeObjects.Count == 0 ? CreateObj() : m_freeObjects.Pop();
            return v;
        }

        public void Free(T obj)
        {
            if (obj == null)
            {
                // throw new ArgumentNullException("obj", "obj cannot be null");
                Debug.LogError("obj cannot be null");
                return;
            }
            if (m_freeObjects.Count < m_max)
            {
                m_freeObjects.Push(obj);
            }

            obj.transform.SetParent(m_parent);
            obj.transform.localScale = m_baseResource.transform.localScale;

            Reset(obj);
        }

        public void Clear()
        {
            GameObject.Destroy(m_baseResource.gameObject);
            
            while (m_freeObjects.Count > 0)
            {
                var obj = m_freeObjects.Pop();
                GameObject.Destroy(obj.gameObject);
            }

            m_freeObjects.Clear();
        }

        protected void Reset(T obj)
        {
            var poolable = obj as IPoolable;
            if (poolable != null) poolable.Reset();
        }

        protected T CreateObj()
        {
            GameObject instantObj = MonoBehaviour.Instantiate(m_baseResource);
            instantObj.transform.SetParent(m_parent);
            instantObj.transform.localScale = m_baseResource.transform.localScale;
            var t = instantObj.GetComponent<T>();
            if (t == null)
            {
                t = instantObj.AddComponent<T>();
            }

            return t;
        }

        public interface IPoolable
        {
            void Reset();
        }

        public int GetCount()
        {
            return m_freeObjects.Count;
        }
    }
}
