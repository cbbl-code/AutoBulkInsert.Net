using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Repository
{
    public  class TypePropertyLoader<T>
    {
        IImmutableDictionary<string, PropertyInfo> _typePropertiesByName;
        IImmutableDictionary<int, PropertyInfo> _typePropertiesById;

        public static TypePropertyLoader<T> Create<T>()
        {
            return new TypePropertyLoader<T>();
        }

        public TypePropertyLoader()
        {
            var propertyInfos = typeof(T)
           .GetProperties(BindingFlags.Instance | BindingFlags.Public)
           .Where(p => p.CanRead);

           _typePropertiesByName = propertyInfos
           .ToImmutableDictionary(p => p.Name, p => p);

            _typePropertiesById = propertyInfos
              .Select((p, i) => new { Item = p, Index = i })
            .ToImmutableDictionary(x => x.Index, x => x.Item);

        }

        public object GetPropertyValue(string name, object source)
        {
            var propertyInfo = _typePropertiesByName[name];
            var propertyValue = propertyInfo.GetValue(source,null);
            return propertyValue;
        }

        public object GetPropertyValue(int index, object source)
        {
            var propertyInfo = _typePropertiesById[index];
            var propertyValue = propertyInfo.GetValue(source, null);
            return propertyValue;
        }

        public int GetFieldCount
        {
            get { return _typePropertiesByName.Count; }
        }

        public int GetFieldIndex(string fieldName)
        {
            return _typePropertiesById
                .Single(x => x.Value.Name.Equals(fieldName))
                .Key;
        }

        public PropertyInfo[]  GetPropertyInfos => _typePropertiesByName
            .Select(x => x.Value)
            .ToArray();
        

    }
}
