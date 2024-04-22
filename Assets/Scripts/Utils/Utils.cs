using System;

namespace Utils
{
    using System.Diagnostics;
    using UnityEngine;
    public class LackingPropertyException : Exception
    {
        public LackingPropertyException(string message = "") : base(message) {}

        public static LackingPropertyException NoComponent(string component,
                                                    string gameobject) {
            return new LackingPropertyException(component + " is not found on " + gameobject + " !");
        }

    }

    public static class MonoBehaviourExtension {
        public static void GetAndCheckComponent<T>(this MonoBehaviour mono, out T result) where T : Component {
            if (!mono.TryGetComponent(out result))
                throw LackingPropertyException.NoComponent(result.ToString(), mono.gameObject.name);
        }
    }

    public static class GameObjectExtension {
        public static void GetAndCheckComponent<T>(this GameObject gameObject, out T result) where T : Component {
            if (!gameObject.TryGetComponent(out result))
                throw LackingPropertyException.NoComponent(result.ToString(), gameObject.name);
        }
    }
}