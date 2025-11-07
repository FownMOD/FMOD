using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PluginInfo : Attribute
    {
        /// <summary>
        /// 初始化插件信息特性
        /// </summary>
        /// <param name="name">插件名称</param>
        /// <param name="author">插件作者</param>
        /// <param name="version">插件版本</param>
        /// <param name="description">插件描述</param>
        public PluginInfo(string name, string author, Version version, string description = "")
        {
            Name = name;
            Author = author;
            Version = version;
            Description = description;
        }

        /// <summary>
        /// 初始化插件信息特性（使用字符串版本号）
        /// </summary>
        /// <param name="name">插件名称</param>
        /// <param name="author">插件作者</param>
        /// <param name="version">插件版本字符串（格式：主版本.次版本.修订号）</param>
        /// <param name="description">插件描述</param>
        public PluginInfo(string name, string author, string version, string description = "")
        {
            Name = name;
            Author = author;
            Version = new Version(version);
            Description = description;
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// 插件版本
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 获取格式化版本字符串
        /// </summary>
        public string VersionString => Version.ToString();

        /// <summary>
        /// 获取插件信息摘要
        /// </summary>
        public string Summary => $"{Name} v{Version} by {Author} - {Description}";
    }
}
