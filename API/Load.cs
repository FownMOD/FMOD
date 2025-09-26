using FMOD.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Log = FMOD.API.Log;

namespace FMOD.API
{
    public class Load
    {
        private static List<Plugin> loadedPlugins = new List<Plugin>();
        private static Dictionary<string, object> pluginConfigs = new Dictionary<string, object>();
        public static void LoadAllPlugins(int serverPort)
        {
            string pluginsDir = Paths.GetPluginsDir(serverPort);
            string dependenceDir = Paths.DependenceDir;

            // 确保目录存在
            Directory.CreateDirectory(pluginsDir);
            Directory.CreateDirectory(dependenceDir);

            // 加载依赖项
            LoadDependencies(dependenceDir);

            // 加载插件
            LoadPlugins(pluginsDir, serverPort);
        }
        /// <summary>
        /// 加载所有依赖项
        /// </summary>
        /// <param name="dependenceDir">依赖目录</param>
        private static void LoadDependencies(string dependenceDir)
        {
            if (!Directory.Exists(dependenceDir))
                return;

            var files = Directory.GetFiles(dependenceDir, "*.dll");
            foreach (var file in files)
            {
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(file);
                    Assembly.Load(assemblyName);
                    Log.Debug($"[FMOD] 已加载依赖: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    Log.Debug($"[FMOD] 加载依赖失败: {Path.GetFileName(file)}, 错误: {ex.Message}");
                }
            }
        }
        private static void LoadPlugins(string pluginsDir, int serverPort)
        {
            if (!Directory.Exists(pluginsDir))
            {
                return;
            }

            var files = Directory.GetFiles(pluginsDir, "*.dll");
            if (files.Length == 0)
            {
                return;
            }

            foreach (var file in files)
            {
                try
                {
                    Assembly assembly = null;
                    try
                    {
                        var assemblyName = AssemblyName.GetAssemblyName(file);
                        assembly = Assembly.Load(assemblyName);
                    }
                    catch
                    {
                        // 如果标准加载失败，尝试使用 LoadFrom
                        assembly = Assembly.LoadFrom(file);
                    }

                    if (assembly == null)
                    {
                        continue;
                    }

                    LoadPluginsFromAssembly(assembly, serverPort);
                }
                catch (Exception ex)
                {
                    Log.Debug($"[FMOD] 加载插件文件失败: {Path.GetFileName(file)}, 错误: {ex.Message}");
                    if (ex is ReflectionTypeLoadException typeLoadEx)
                    {
                        foreach (var loaderEx in typeLoadEx.LoaderExceptions)
                        {
                        }
                    }
                }
            }

            // 启用所有插件
            EnableAllPlugins();
        }
        private static void LoadPluginsFromAssembly(Assembly assembly, int serverPort)
        {
            Type[] types = null;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // 处理类型加载异常
                types = ex.Types;
                foreach (var loaderEx in ex.LoaderExceptions)
                {
                }
            }
            catch (Exception ex)
            {
                return;
            }

            if (types == null)
                return;

            foreach (Type type in types)
            {
                try
                {
                    if (type == null) continue;

                    // 检查是否是 Plugin 的子类且不是抽象类
                    if (type.IsSubclassOf(typeof(Plugin)) && !type.IsAbstract)
                    {
                        Plugin plugin = null;
                        try
                        {
                            plugin = (Plugin)Activator.CreateInstance(type);
                        }
                        catch (MissingMethodException)
                        {
                            continue;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        object config = LoadPluginConfig(plugin, serverPort);
                        bool isEnabled = CheckPluginEnabled(config, plugin);

                        if (!isEnabled)
                        {
                            Log.Debug($"[FMOD] 插件已禁用: {plugin.Name}");
                            continue;
                        }

                        try
                        {
                            plugin.Initialize(serverPort, config);
                            loadedPlugins.Add(plugin);
                            pluginConfigs[plugin.Name] = config;

                            Log.Debug($"[FMOD] 已加载插件: {plugin.Name} v{plugin.Version} by {plugin.Author}");
                        }
                        catch (Exception ex)
                        {
                            Log.Debug($"[FMOD] 初始化插件失败: {plugin.Name}, 错误: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug($"[FMOD] 处理类型失败: {type?.FullName ?? "null"}, 错误: {ex.Message}");
                }
            }
        }

        private static bool CheckPluginEnabled(object config, Plugin plugin)
        {
            try
            {
                if (config == null)
                {
                    return true;
                }
                if (config is Dictionary<object, object> configDict)
                {
                    if (configDict.TryGetValue("config", out var configValue) && configValue is Dictionary<object, object> nestedConfig)
                    {
                        if (nestedConfig.TryGetValue("IsEnabled", out var enabledValue))
                        {
                            bool isEnabled = Convert.ToBoolean(enabledValue);
                            return isEnabled;
                        }
                    }
                    if (configDict.TryGetValue("IsEnabled", out var topLevelEnabled))
                    {
                        bool isEnabled = Convert.ToBoolean(topLevelEnabled);
                        return isEnabled;
                    }
                }

                if (config is EnabledPluginConfigWrapper wrapper)
                {
                    return wrapper.IsEnabled;
                }
                var enabledProperty = config.GetType().GetProperty("IsEnabled");
                if (enabledProperty != null && enabledProperty.PropertyType == typeof(bool))
                {
                    bool isEnabled = (bool)enabledProperty.GetValue(config);
                    return isEnabled;
                }
                return true;
            }
            catch (Exception ex)
            {
                return true; 
            }
        }
        /// <summary>
        /// 加载插件配置
        /// </summary>
        private static object LoadPluginConfig(Plugin plugin, int serverPort)
        {
            try
            {
                string pluginName = plugin.Name;
                string configPath = GetPluginConfigPath(serverPort, pluginName);
                string configDir = Path.GetDirectoryName(configPath);
                Directory.CreateDirectory(configDir);

                if (!File.Exists(configPath))
                {
                    object defaultConfig = CreateDefaultConfigWithEnabled(plugin);
                    SavePluginConfig(configPath, pluginName, defaultConfig);
                    return defaultConfig;
                }
                else
                {
                    return LoadAndEnsureEnabledConfig(configPath, plugin.ConfigType, pluginName);
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"[FMOD] 加载插件配置失败: {ex.Message}");
                return CreateDefaultConfigWithEnabled(null);
            }
        }

        private static object CreateDefaultConfigWithEnabled(Plugin plugin)
        {
            try
            {
                var defaultConfig = new Dictionary<object, object>
                {
                    ["pluginName"] = plugin?.Name ?? "Unknown",
                    ["config"] = new Dictionary<object, object>
                    {
                        ["IsEnabled"] = true,
                        ["description"] = "默认插件配置"
                    }
                };

                return defaultConfig;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] 创建默认配置失败: {ex.Message}");
                return new Dictionary<object, object>
                {
                    ["pluginName"] = "Unknown",
                    ["config"] = new Dictionary<object, object>
                    {
                        ["IsEnabled"] = true
                    }
                };
            }
        }

        /// <summary>
        /// 加载配置并确保包含 IsEnabled 字段
        /// </summary>
        private static object LoadAndEnsureEnabledConfig(string configPath, Type configType, string pluginName)
        {
            try
            {
                if (!File.Exists(configPath))
                    return CreateDefaultConfigWithEnabled(null);

                string yaml = File.ReadAllText(configPath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var configData = deserializer.Deserialize<Dictionary<object, object>>(yaml);
                if (configData == null)
                    return CreateDefaultConfigWithEnabled(null);
                bool hasIsEnabled = false;
                if (configData.TryGetValue("config", out var configValue) && configValue is Dictionary<object, object> nestedConfig)
                {
                    hasIsEnabled = nestedConfig.ContainsKey("IsEnabled");
                }
                if (!hasIsEnabled)
                {
                    hasIsEnabled = configData.ContainsKey("IsEnabled");
                }
                if (!hasIsEnabled)
                {
                    Console.WriteLine($"[FMOD] 为插件 {pluginName} 添加 isEnabled 字段");
                    if (!configData.ContainsKey("config") || !(configData["config"] is Dictionary<object, object> configDict))
                    {
                        configData["config"] = new Dictionary<object, object>();
                    }
                    var nestedConfigDict = configData["config"] as Dictionary<object, object>;
                    nestedConfigDict["IsEnabled"] = true;
                    var serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();

                    string updatedYaml = serializer.Serialize(configData);
                    File.WriteAllText(configPath, updatedYaml);
                }

                return configData;
            }
            catch (Exception ex)
            {
                return CreateDefaultConfigWithEnabled(null);
            }
        }


        /// <summary>
        /// 获取插件配置文件路径
        /// </summary>
        private static string GetPluginConfigPath(int serverPort, string pluginName)
        {
            string configDir = Path.Combine(Paths.ConfigDir, serverPort.ToString(), "Plugins");
            return Path.Combine(configDir, $"{pluginName}.yaml");
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        private static object CreateDefaultConfig(Plugin plugin)
        {
            if (plugin.ConfigType != null)
            {
                return Activator.CreateInstance(plugin.ConfigType);
            }
            return new DefaultPluginConfig { IsEnabled = true };
        }

        /// <summary>
        /// 保存插件配置
        /// </summary>
        private static void SavePluginConfig(string configPath, string pluginName, object config)
        {
            try
            {
                object configToSave = config;

                if (config is Dictionary<object, object> configDict)
                {
                    if (!configDict.ContainsKey("pluginName"))
                    {
                        configDict["pluginName"] = pluginName;
                    }

                    if (!configDict.ContainsKey("config") || !(configDict["config"] is Dictionary<object, object> configDictValue))
                    {
                        configDict["config"] = new Dictionary<object, object>();
                    }
                }

                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                string yaml = serializer.Serialize(configToSave);
                File.WriteAllText(configPath, yaml);

                Console.WriteLine($"[FMOD] 已保存插件配置: {Path.GetFileName(configPath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] 保存插件配置失败: {ex.Message}");
            }
        }
        /// <summary>
        /// 加载现有配置
        /// </summary>
        private static object LoadExistingConfig(string configPath, Type configType)
        {
            try
            {
                string yaml = File.ReadAllText(configPath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var configWrappers = deserializer.Deserialize<List<PluginConfigWrapper>>(yaml);
                if (configWrappers != null && configWrappers.Count > 0)
                {
                    return ConvertDictionaryToConfig(configWrappers[0].Config, configType);
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"加载插件配置失败: {ex.Message}");
            }

            return Activator.CreateInstance(configType);
        }

        /// <summary>
        /// 将字典转换为配置对象
        /// </summary>
        private static object ConvertDictionaryToConfig(object configData, Type configType)
        {
            if (configData is Dictionary<object, object> dict)
            {
                try
                {
                    var serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();

                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();

                    // 先序列化为YAML，再反序列化为目标类型
                    string yaml = serializer.Serialize(dict);
                    return deserializer.Deserialize(yaml, configType);
                }
                catch
                {
                    // 如果转换失败，使用包装器
                    return new EnabledPluginConfigWrapper
                    {
                        IsEnabled = dict.ContainsKey("IsEnabled") ? (bool)dict["IsEnabled"] : true,
                        PluginConfig = dict
                    };
                }
            }

            return CreateDefaultConfigWithEnabled(null);
        }
        /// <summary>
        /// 保存所有插件配置
        /// </summary>
        public static void SaveAllPluginConfigs(int serverPort)
        {
            foreach (var plugin in loadedPlugins)
            {
                SavePluginConfig(serverPort, plugin);
            }
        }

        /// <summary>
        /// 保存单个插件配置
        /// </summary>
        public static void SavePluginConfig(int serverPort, Plugin plugin)
        {
            if (pluginConfigs.TryGetValue(plugin.Name, out var config))
            {
                string configPath = GetPluginConfigPath(serverPort, plugin.Name);
                SavePluginConfig(configPath, plugin.Name, config);
            }
        }

        /// <summary>
        /// 获取插件配置
        /// </summary>
        public static T GetPluginConfig<T>(string pluginName) where T : class, new()
        {
            if (pluginConfigs.TryGetValue(pluginName, out var config))
            {
                return config as T;
            }
            return new T();
        }

        /// <summary>
        /// 更新插件配置
        /// </summary>
        public static void UpdatePluginConfig(string pluginName, object config)
        {
            if (pluginConfigs.ContainsKey(pluginName))
            {
                pluginConfigs[pluginName] = config;
            }
        }

        /// <summary>
        /// 启用所有插件
        /// </summary>
        private static void EnableAllPlugins()
        {
            foreach (var plugin in loadedPlugins)
            {
                try
                {
                    // 检查插件是否启用
                    bool isEnabled = true;

                    if (pluginConfigs.TryGetValue(plugin.Name, out var config))
                    {
                        if (config is DefaultPluginConfig defaultConfig)
                        {
                            isEnabled = defaultConfig.IsEnabled;
                        }
                        else if (config.GetType().GetProperty("IsEnabled") != null)
                        {
                            var prop = config.GetType().GetProperty("IsEnabled");
                            isEnabled = (bool)prop.GetValue(config);
                        }
                    }

                    if (isEnabled)
                    {
                        plugin.OnEnabled();
                        Log.Debug($"[FMOD] 已启用插件: {plugin.Name}");
                    }
                    else
                    {
                        plugin.OnDisabled();
                        Log.Debug($"[FMOD] 插件已禁用: {plugin.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug($"[FMOD] 启用插件失败: {plugin.Name}, 错误: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// 禁用所有已加载的插件
        /// </summary>
        public static void DisableAllPlugins()
        {
            foreach (var plugin in loadedPlugins)
            {
                try
                {
                    plugin.OnDisabled();
                    Log.Debug($"已禁用插件: {plugin.Name}");
                }
                catch (Exception ex)
                {
                    Log.Debug($"禁用插件失败: {plugin.Name}, 错误: {ex.Message}");
                }
            }

            loadedPlugins.Clear();
            pluginConfigs.Clear();
        }

        /// <summary>
        /// 获取所有已加载的插件
        /// </summary>
        public static List<Plugin> GetLoadedPlugins()
        {
            return new List<Plugin>(loadedPlugins);
        }

        /// <summary>
        /// 重新加载插件配置
        /// </summary>
        public static void ReloadPluginConfigs(int serverPort)
        {
            foreach (var plugin in loadedPlugins)
            {
                try
                {
                    object config = LoadPluginConfig(plugin, serverPort);
                    UpdatePluginConfig(plugin.Name, config);
                }
                catch (Exception ex)
                {
                }
            }
        }
        /// <summary>
        /// 插件配置包装器（用于YAML序列化）
        /// </summary>
        public class PluginConfigWrapper
        {
            [YamlMember(Alias = "PluginName")]
            public string PluginName { get; set; } = string.Empty;

            [YamlMember(Alias = "Config")]
            public object Config { get; set; }
        }

        /// <summary>
        /// 默认插件配置
        /// </summary>
        public class DefaultPluginConfig
        {
            public bool IsEnabled { get; set; } = true;
        }

        public class EnabledPluginConfigWrapper
        {
            public bool IsEnabled { get; set; } = true;
            public object PluginConfig { get; set; }

            public Dictionary<string, object> ToDictionary()
            {
                var dict = new Dictionary<string, object>
                {
                    ["IsEnabled"] = IsEnabled
                };

                if (PluginConfig is Dictionary<object, object> existingDict)
                {
                    foreach (var kvp in existingDict)
                    {
                        dict[kvp.Key.ToString()] = kvp.Value;
                    }
                }

                return dict;
            }
        }
    }
}
