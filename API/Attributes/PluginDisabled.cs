using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PluginDisabled : Attribute
    {
        /// <summary>
        /// 初始化插件禁用特性
        /// </summary>
        public PluginDisabled()
        {
        }

        /// <summary>
        /// 初始化插件禁用特性并指定执行顺序
        /// </summary>
        /// <param name="order">执行顺序（数值越小越先执行）</param>
        public PluginDisabled(int order)
        {
            Order = order;
        }

        /// <summary>
        /// 执行顺序
        /// </summary>
        public int Order { get; } = 0;

        /// <summary>
        /// 是否异步执行
        /// </summary>
        public bool IsAsync { get; set; } = false;

        /// <summary>
        /// 执行超时时间（毫秒），0表示无超时
        /// </summary>
        public int Timeout { get; set; } = 0;
    }
}
