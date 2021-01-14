namespace Aya.Storage
{
    public static class StringExtensions
    {
        public static string AppendPathItem(this string path, string item) => path + "/" + item;
    }
}

