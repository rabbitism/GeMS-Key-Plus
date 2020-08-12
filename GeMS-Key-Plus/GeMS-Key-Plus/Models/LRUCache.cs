using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace GeMS_Key_Plus.Models
{
    public class LRUCache<TKey, TValue> where TKey: notnull
    {
        private LinkedList<TKey> _list;
        private Dictionary<TKey, LinkedListNode<TKey>> _nodeDictionary;
        private Dictionary<TKey, TValue> _dictionary;
        public int Capacity { get; set; }

        public LRUCache(int capacity)
        {
            Capacity = capacity;
            _list = new LinkedList<TKey>();
            _nodeDictionary = new Dictionary<TKey, LinkedListNode<TKey>>();
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public void Put(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key))
            {
                LinkedListNode<TKey> node = _nodeDictionary[key];
                _dictionary[key] = value;
                _list.Remove(node);
                _list.AddFirst(node);
            }
            else
            {
                _list.AddFirst(key);
                _nodeDictionary[key] = _list.First!;
                _dictionary[key] = value;
                if (_list.Count > Capacity)
                {
                    TKey keyToRemove = _list.Last!.Value;
                    _nodeDictionary.Remove(keyToRemove);
                    _list.RemoveLast();
                    _dictionary.Remove(keyToRemove);
                }
            }
        }

        /// <summary>
        /// Get All Elements in order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<TKey, TValue>> GetAll()
        {
            foreach(TKey key in _list)
            {
                yield return new Tuple<TKey, TValue>(key, _dictionary[key]);
            }
        }


    }
}
