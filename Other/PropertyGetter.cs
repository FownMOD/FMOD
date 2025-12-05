using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Other
{
    public class PropertyGetter
    {
        public static bool TryGetPropertyType(Type classType, string propertyName, out Type propertyType)
        {
            propertyType = null;

            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("属性名不能为空", nameof(propertyName));
            var propertyInfo = classType.GetProperty(propertyName,
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.Static);

            if (propertyInfo != null)
            {
                propertyType = propertyInfo.PropertyType;
                return true;
            }

            return false;
        }
    }
}
