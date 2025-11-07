using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class PluginConfig : Attribute
    {
        /// <summary>
        /// 初始化插件配置特性
        /// </summary>
        public PluginConfig()
        {
        }

        /// <summary>
        /// 初始化插件配置特性并指定配置类型
        /// </summary>
        /// <param name="configType">配置类型</param>
        public PluginConfig(Type configType)
        {
            ConfigType = configType;
        }

        /// <summary>
        /// 配置类型
        /// </summary>
        public Type ConfigType { get; }

        /// <summary>
        /// 配置键名（可选，如果为空则使用属性名）
        /// </summary>
        public string ConfigKey { get; set; }

        /// <summary>
        /// 是否必需配置
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// 默认值（字符串形式，将在运行时转换）
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
