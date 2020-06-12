using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic;

namespace SandboxNonCore
{
    public class BlackBoxDataSource<T>
    {
        IEnumerable<T> _data;

        public IQueryable<T> DataSource
        {
            get
            {

                if (!(_data is IQueryable<T> queryable))
                {
                    queryable = _data.AsQueryable();
                }

                return queryable;
            }
            set
            {
                _data = value;
            }
        }

        public string Where { get; set; }

        public object WhereParameters { get; set; }

        public IQueryable<T> Select()
        {
            var queryable = DataSource;

            if (!string.IsNullOrWhiteSpace(Where))
            {
                queryable = (WhereParameters != null) ? queryable.Where(Where, GetParameters()) : queryable.Where(Where);
            }

            return queryable;
        }

        object[] GetParameters()
        {
            var parameters = new List<object>();

            foreach (var prop in WhereParameters.GetType().GetProperties())
            {
                var value = prop.GetValue(WhereParameters);
                parameters.Add(value);
            }

            return parameters.ToArray();
        }
    }

}
