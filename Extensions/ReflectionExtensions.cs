using HarmonyLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Extensions
{
    public static class ReflectionExtensions
    {
        public static void InvokeStaticMethod(this Type type, string methodName, object[] param)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            if (method == null)
            {
                return;
            }
            method.Invoke(null, param);
        }

        public static void InvokeStaticEvent(this Type type, string eventName, object[] param)
        {
            MulticastDelegate multicastDelegate = (MulticastDelegate)type.GetField(eventName, AccessTools.all).GetValue(null);
            if (multicastDelegate != null)
            {
                foreach (Delegate @delegate in multicastDelegate.GetInvocationList())
                {
                    @delegate.Method.Invoke(@delegate.Target, param);
                }
            }
        }

        public static void CopyProperties(this object target, object source)
        {
            Type type = target.GetType();
            if (type != source.GetType())
            {
                throw new InvalidTypeException("Target and source type mismatch!");
            }
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                PropertyInfo property = type.GetProperty(propertyInfo.Name);
                if (property != null)
                {
                    property.SetValue(target, propertyInfo.GetValue(source, null), null);
                }
            }
        }
    }
}
