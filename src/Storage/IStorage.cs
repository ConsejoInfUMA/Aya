namespace Aya.Storage
{
    public interface IStorage<T>
    {
        void Save(string key, T elem);
        T Get(string key);
        bool Exists(string key);
    }
}

