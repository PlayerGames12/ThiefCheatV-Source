using System.Runtime.InteropServices;
using UnityEngine;


namespace ThiefCheatV
{
    public class Loader : MonoBehaviour
    {

        public static GameObject _jasdcObject;


        public static void Load()
        {

            _jasdcObject = new GameObject();

            _jasdcObject.AddComponent<Main>();

            Object.DontDestroyOnLoad(_jasdcObject);
        }

        public static void Unload()
        {
            _Unload();
        }

        public static void _Unload()
        {

            UnityEngine.Object.Destroy(Loader._jasdcObject);
            Loader._jasdcObject = null;

        }
    }
}