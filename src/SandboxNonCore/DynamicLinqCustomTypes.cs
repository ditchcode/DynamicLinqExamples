using System;
using System.Collections.Generic;
using System.Linq.Dynamic;

namespace SandboxNonCore
{

    public static class DynamicLinqCustomTypes<T>
    {
        public static readonly Type IsMatch = DynamicExpression.CreateClass(new DynamicProperty("IsMatch", typeof(Func<T, bool>)));

        public static readonly Type Compare = DynamicExpression.CreateClass(new DynamicProperty("Compare", typeof(Func<T, T, int>)));

        public static readonly Type IEnumerableIsMatch = DynamicExpression.CreateClass(new DynamicProperty("IsMatch", typeof(Func<IEnumerable<T>, T, bool>)));

    }

    public static class DynamicLinqCustomTypes
    {
        public static object SetIsMatch<T>(this object instance, Func<T, bool> func)
        {
            DynamicLinqCustomTypes<T>.IsMatch.GetProperty("IsMatch").SetValue(instance, func);
            return instance;
        }

        public static object SetCompare<T>(this object instance, Func<T, T, int> func)
        {
            DynamicLinqCustomTypes<T>.Compare.GetProperty("Compare").SetValue(instance, func);
            return instance;
        }

        public static object SetIEnumerableIsMatch<T>(this object instance, Func<IEnumerable<T>, T, bool> func)
        {
            DynamicLinqCustomTypes<T>.IEnumerableIsMatch.GetProperty("IsMatch").SetValue(instance, func);
            return instance;
        }

    }
}
