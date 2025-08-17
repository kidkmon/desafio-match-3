using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => _instance ??= FindFirstObjectByType<T>();
    }
}