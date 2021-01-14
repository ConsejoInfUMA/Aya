using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Aya.Storage
{

    public class PersistedStorage<T> : IStorage<T>
    {
        private static readonly string root = "storage";
        private readonly string collection = typeof(T).Name;

        private string path { get => root.AppendPathItem(collection) + ".json"; }

        private IDictionary<string, T> db;

        public PersistedStorage()
        {
            db = LoadDb();
        }

        public bool Exists(string key)
            => db.ContainsKey(key);

        public T Get(string key)
        {
            if (db.TryGetValue(key, out T value))
            {
                return value;
            }
            return default(T);
        }

        public void Save(string key, T elem)
        {
            if (db.TryAdd(key, elem))
            {
                SaveDb();
            }
        }

        private void SaveDb()
        {
            var json = JsonConvert.SerializeObject(db);
            File.WriteAllText(path, json);
        }

        private IDictionary<string, T> LoadDb()
        {
            Directory.CreateDirectory(root);

            if (!File.Exists(path))
            {
                return new ConcurrentDictionary<string, T>();
            }

            var json = System.IO.File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ConcurrentDictionary<string, T>>(json) ?? new ConcurrentDictionary<string, T>();
        }

    }
}
