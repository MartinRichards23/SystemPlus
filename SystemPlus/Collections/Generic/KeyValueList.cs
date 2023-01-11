using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus.Collections.Generic
{
    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void AddValues(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> kvp = new KeyValuePair<TKey, TValue>(key, value);
            Add(kvp);
        }
    }
}
