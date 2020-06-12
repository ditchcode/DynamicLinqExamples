using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace Sandbox
{
    class DynamicLinqTypeProvider : AbstractDynamicLinqCustomTypeProvider, IDynamicLinkCustomTypeProvider
    {
        private HashSet<Type> _customTypes;

        public virtual HashSet<Type> GetCustomTypes()
        {
            if (_customTypes != null)
            {
                return _customTypes;
            }

            _customTypes = new HashSet<Type>(FindTypesMarkedWithDynamicLinqTypeAttribute(new[] { GetType().Assembly })) { typeof(Regex) };
            return _customTypes;
        }

        public Type ResolveType(string typeName)
        {
            return _customTypes.Where(typ => typ.FullName == typeName).Single();
        }

        public Type ResolveTypeBySimpleName(string simpleTypeName)
        {
            return _customTypes.Where(typ => typ.FullName.EndsWith(simpleTypeName)).Single();
        }
    }

    //[DynamicLinqType]
    //public class RegexFactory
    //{
    //    public Regex Get(string pattern)
    //    {
    //        return new Regex(pattern);
    //    }
    //}


}
