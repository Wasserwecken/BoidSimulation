using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <param name="action"></param>
    public static void Times (this int count, Action action)
    {
        for (int i = 0; i < count; i++)
            action();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="notFoundValue"></param>
    /// <returns></returns>
    public static TValue GetValueOr<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue notFoundValue)
    {
        return dictionary.ContainsKey(key) ? dictionary[key] : notFoundValue;
    }
}
