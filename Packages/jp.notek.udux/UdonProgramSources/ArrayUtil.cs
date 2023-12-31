
using System;

namespace JP.Notek.Udux
{
    public static class ArrayUtil {
        public static T[] Add<T>(this T[] target, T source) {
            var ret = new T[target.Length + 1];
            Array.Copy(target, ret, target.Length);
            ret[ret.Length - 1] = source;
            return ret;
        }
        public static T Pop<T>(this T[] target, out T[] newArray) {
            var ret = target[0];
            newArray = new T[target.Length - 1];
            Array.Copy(target, 1, newArray, 0, target.Length - 1);
            return ret;
        }
    }
}