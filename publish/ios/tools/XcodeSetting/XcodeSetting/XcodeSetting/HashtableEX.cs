using System.Collections;
using System;

namespace UnityEditor.iOS.Xcode.Custom
{

    public static class HashtableEX
    {
        public static object SGet(this Hashtable inst, object key)
        {
            if (inst == null)
            {
                Console.WriteLine("hashtable is null");
                return null;
            }
            if (key == null || (string)key == "")
            {
                return null;
            }
            else if (inst.ContainsKey(key))
                return inst[key];
            else
                return null;
        }
        /// <summary>
        /// ??????????????????????
        /// </summary>
        public static void SSet(this Hashtable inst, object key, object value)
        {
            if (inst == null)
            {
                //Debug.Log("hashtable is null");
                return;
            }
            if (key == null || (string)key == "")
            {
                return;
            }
            else if (inst.ContainsKey(key))
                inst[key] = value;
            else
                inst.Add(key, value);
        }
        /// <summary>
        /// ??????????????????????
        /// </summary>
        public static T SGet<T>(this Hashtable inst, object key)
        {
            if (inst == null)
            {
                //Debug.Log("hashtable is null");
                return default(T);
            }
            if (key == null || (string)key == "")
            {
                return default(T);
            }
            else if (inst.ContainsKey(key) && inst[key] != null)
            {
                if (inst[key] is T)
                {
                    return (T)inst[key];
                }
                else
                {
                    return default(T);
                }
            }
            else
                return default(T);
        }
        /// <summary>
        /// ??????????hashtable
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Hashtable Construct(params object[] p)
        {
            var inst = new Hashtable();
            if (p != null)
            {
                for (int i = 0; i < p.Length;)
                {
                    inst.Add(p[i], p[i + 1]);
                    i = i + 2;
                }
            }
            return inst;
        }
        public static bool UpdateByKey<T>(this Hashtable inst, object key, ref T v)
        {
            if (inst == null)
            {
                return false;
            }
            if (key == null || (string)key == "")
            {
                return false;
            }
            else if (inst.ContainsKey(key) && inst[key] != null)
            {
                if (inst[key].GetType() == typeof(T))
                {
                    v = (T)inst[key];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
    }
}
