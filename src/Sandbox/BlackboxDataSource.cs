using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace Sandbox
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

        public string Where { get; set; }

        public object WhereParameters { get; set; }

        public IQueryable<T> Select() {

            if (!string.IsNullOrWhiteSpace(Where))
            {
                return (WhereParameters != null) ? DataSource.Where(Where, GetParameters()) : DataSource.Where(Where);
            }

            return DataSource;

        }

        public IQueryable<T> Select(ParsingConfig config)
        {
            if (!string.IsNullOrWhiteSpace(Where))
            {
                return (WhereParameters != null) ? DataSource.Where(config, Where, GetParameters()) : DataSource.Where(config, Where);
            }

            return DataSource;
        }
    }

}
