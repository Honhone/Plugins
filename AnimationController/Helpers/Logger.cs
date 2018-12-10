using System;

namespace AnimationController
{
    public static class Logger
    {
        public static void Destroy(Type T)
        {
            UnityEngine.Debug.Log(T + " destroyed");
        }

        public static void Create(Type T)
        {
            UnityEngine.Debug.Log(T + " awakened");
        }

        public static void Call(Type T, string message)
        {
            UnityEngine.Debug.Log(T + " called " + message);
        }

        public static void Exception(Type T, string message)
        {
            UnityEngine.Debug.Log(T + " caught in layer: " + message);
        }

    }

}
