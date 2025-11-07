using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Attributes
{
    public class AttributeHelper
    {
        public static bool ValidatePluginType(Type pluginType)
        {
            // 必须有 PluginInfo 特性
            if (GetPluginInfo(pluginType) == null)
            {
                Console.WriteLine($"[FMOD] 插件类型 {pluginType.Name} 缺少 PluginInfo 特性");
                return false;
            }

            // 检查是否有公共的无参数构造函数
            var constructor = pluginType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                Console.WriteLine($"[FMOD] 插件类型 {pluginType.Name} 缺少公共的无参数构造函数");
                return false;
            }

            return true;
        }
        public static PluginInfo GetPluginInfo(Type pluginType)
        {
            return pluginType.GetCustomAttribute<PluginInfo>();
        }
        public static (PropertyInfo property, PluginConfig attribute) GetPluginConfigProperty(Type pluginType)
        {
            var properties = pluginType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<PluginConfig>();
                if (attribute != null)
                {
                    return (property, attribute);
                }
            }
            return (null, null);
        }
        public static List<(MethodInfo method, PluginEnabled attribute)> GetEnabledMethods(Type pluginType)
        {
            return pluginType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(m => (method: m, attribute: m.GetCustomAttribute<PluginEnabled>()))
                .Where(x => x.attribute != null)
                .OrderBy(x => x.attribute.Order)
                .ToList();
        }

        public static List<(MethodInfo method, PluginDisabled attribute)> GetDisabledMethods(Type pluginType)
        {
            return pluginType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(m => (method: m, attribute: m.GetCustomAttribute<PluginDisabled>()))
                .Where(x => x.attribute != null)
                .OrderBy(x => x.attribute.Order)
                .ToList();
        }
    }
}
