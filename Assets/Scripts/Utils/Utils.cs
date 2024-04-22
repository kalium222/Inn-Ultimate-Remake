using System;

namespace Utils
{
    public class LackingPropertyException : Exception
    {
        public LackingPropertyException(string message = "") : base(message) {}

        public static LackingPropertyException NoComponent(string component,
                                                    string gameobject) {
            return new LackingPropertyException(component + " is not found on " + gameobject + " !");
        }
    }
}