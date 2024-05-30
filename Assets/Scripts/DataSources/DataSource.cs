using UnityEngine;

namespace DataSources
{
    public abstract class DataSource<T> : ScriptableObject
    {
        public T Value { get; set; }
    }
}
