using System;
using UnityEngine;

namespace Utils
{
    public class LackPropertyException : Exception
    {
        public LackPropertyException(string message = "")
        : base(message) {}

        public static LackPropertyException NoComponent
        (string component, string gameobject) {
            return new LackPropertyException(
                component + " is not found on " + gameobject + " !"
            );
        }
    }

    public static class MonoBehaviourExtension {

        public static void GetAndCheckObject<T>
        (this MonoBehaviour mono, out T result)
        where T : UnityEngine.Object {
            if (!mono.TryGetComponent(out result)) {
                throw LackPropertyException.NoComponent(
                    result.ToString(),
                    mono.name
                );
            }
        }

        public static void CheckObject<T>
        (this MonoBehaviour mono, T component)
        where T : UnityEngine.Object {
            if ( component==null ) {
                throw LackPropertyException.NoComponent(
                    component.ToString(),
                    mono.name
                );
            }
        }

    }

    public static class GameObjectExtension {

        public static void GetAndCheckObject<T>
        (this GameObject gameObject, out T result)
        where T : UnityEngine.Object {
            if (!gameObject.TryGetComponent(out result)) {
                throw LackPropertyException.NoComponent(
                    result.ToString(), 
                    gameObject.name
                );
            }
        }

    }
}

