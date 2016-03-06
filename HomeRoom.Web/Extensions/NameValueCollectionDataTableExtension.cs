using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace HomeRoom.Web.Extensions
{
    public static class NameValueCollectionDataTableExtension
    {
        public static T G<T>(this NameValueCollection collection, string key)
        {
            return G<T>(collection, key, default(T));
        }

        public static T G<T>(this NameValueCollection collection, string key, object defaultValue)
        {
            if (collection == null) throw new ArgumentNullException("collection", "The provided collection cannot be null.");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("The provided key cannot be null or empty.", "key");

            var collectionItem = collection[key];
            if (collectionItem == null) return (T)defaultValue;
            return (T)Convert.ChangeType(collectionItem, typeof(T));
        }

        public static void S(this NameValueCollection collection, string key, object value)
        {
            if (collection == null) throw new ArgumentNullException("collection", "The provided collection cannot be null.");
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("The provided key cannot be null or empty.", "key");
            if (value == null) throw new ArgumentNullException("value", "The provided value cannot be null.");

            if (collection.Keys.Cast<string>().Any(_k => _k.Equals(key)))
                collection[key] = value.ToString();
            else
                collection.Add(key, value.ToString());
        }
    }
}