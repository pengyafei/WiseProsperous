using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RZXK.Common
{
    public class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        private readonly Dictionary<string, object> dictionary;

        public DynamicDictionary()
        {
            this.dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.dictionary[binder.Name] = value;
            return true;
        }

        public void GenerateProperty(string key, object value)
        {
            this.dictionary[key] = value;
        }

        public void ObjectInDynamic<T>(T t) where T : new()
        {
            if (t == null)
            {
                return;
            }

            List<PropertyInfo> properties = t.GetType().GetProperties().ToList();
            properties.ForEach(p => { this.GenerateProperty(p.Name, p.GetValue(t, null)); });
        }

        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.dictionary.TryGetValue(binder.Name, out result);
        }

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }

        public ICollection<object> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object this[string key]
        {
            get
            {
                return this.dictionary[key];
            }

            set
            {
                this.dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }
    }
}
