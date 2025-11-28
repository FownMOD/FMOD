using FMOD.API;
using FMOD.FMOD;
using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FMOD.Loader
{
    public class Load
    {
        public static List<MOD> LoadsMOD = new List<MOD>();
        public static List<LabApi.Loader.Features.Plugins.Plugin> LoadsLabPlugins = new List<LabApi.Loader.Features.Plugins.Plugin>();
        public static bool LoadAllMod(bool LoadLabPlugin = true)
        {
            LoadsMOD.Clear();
            LoadsLabPlugins.Clear();
            var mods = LoadTypesFromAssemblies(typeof(MOD), Paths.GetPluginsDir(Server.Port));
            var labplugins = LoadTypesFromAssemblies(typeof(Plugin), Paths.GetPluginsDir(Server.Port));
            foreach (var i in mods)
            {
                LoadMod(i);
            }
            if (LoadLabPlugin)
            {
                foreach(var i in labplugins)
                {
                    LoadLabPlugins(i);
                }
            }
            return true;
        }
        public static bool UnLoadAllPlugin(bool UnLoadLabPlugins = true)
        {
            LoadsMOD.Clear();
            LoadsLabPlugins.Clear();
            var mods = LoadTypesFromAssemblies(typeof(MOD), Paths.GetPluginsDir(Server.Port));
            var labplugins = LoadTypesFromAssemblies(typeof(Plugin), Paths.GetPluginsDir(Server.Port));
            foreach (var i in mods)
            {
                MOD mod = (MOD)Activator.CreateInstance(i);
                mod.OnDisable();
            }
            foreach (var i in labplugins)
            {
                Plugin plugin = (Plugin)Activator.CreateInstance(i);
                plugin.Disable();
            }
            return true;
        }
        public static bool LoadLabPlugins(Type type)
        {
            try
            {
                LabApi.Loader.Features.Plugins.Plugin plugin = (LabApi.Loader.Features.Plugins.Plugin)Activator.CreateInstance(type);
                LoadsLabPlugins.Add(plugin);
                Type ConfigType = GetLabPluginConfigType(plugin);
                string baseConfigPath = Paths.ConfigDir ?? "Config";
                string serverPort = Server.Port.ToString() ?? "default";
                string configPath = Path.Combine(baseConfigPath, serverPort, "Plugins");
                Directory.CreateDirectory(configPath);
                string PluginName = plugin.Name ?? "UnknownPlugin";
                string Author = plugin.Author ?? "UnknownAuthor";
                string Description = plugin.Description ?? "No description";
                string modConfigFile = Path.Combine(configPath, $"{PluginName}.yaml");
                Log.Debug($"加载LabAPI插件中[{PluginName}]制作者[{Author}]版本[{plugin.Version}]");
                if (File.Exists(modConfigFile))
                {
                    var configs = GetPropertiesWithValuesFromYaml(modConfigFile);
                    var isEnabledEntry = configs.FirstOrDefault(x => x.Key == "IsEnabled");
                    if (isEnabledEntry.Key != null)
                    {
                        bool isEnabled = Convert.ToBoolean(isEnabledEntry.Value);
                        if (isEnabled)
                        {
                            plugin.Enable();
                            Log.Debug($"已启用LabAPI插件: {PluginName}");
                        }
                        else
                        {
                            plugin.Disable();
                            LoadsLabPlugins.Remove(plugin);
                            Log.Debug($"已禁用LabAPI插件: {PluginName}");
                        }
                    }
                    else
                    {
                        WriteTextToTOP(modConfigFile, "IsEnabled: true");
                        plugin.Enable();
                        Log.Debug($"已启用LabAPI插件(添加IsEnabled): {PluginName}");
                    }
                }
                else
                {
                    if (ConfigType == null)
                    {
                        Log.Error($"LabAPI插件{plugin.Name}未指定Config");
                        return false;
                    }

                    string pluginConfig = ConvertTypeToYaml(ConfigType);
                    bool hasIsEnabledProperty = ConfigType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Any(p => p.Name == "IsEnabled" && p.PropertyType == typeof(bool));

                    if (hasIsEnabledProperty)
                    {
                        File.WriteAllText(modConfigFile, pluginConfig);
                        plugin.Enable();
                        Log.Debug($"已启用LabAPI插件(新建配置): {PluginName}");
                    }
                    else
                    {
                        string contentWithIsEnabled = "IsEnabled: true" + Environment.NewLine + pluginConfig;
                        File.WriteAllText(modConfigFile, contentWithIsEnabled);
                        plugin.Enable();
                        Log.Debug($"已启用LabAPI插件(新建配置+IsEnabled): {PluginName}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"加载LabAPI插件时发生错误: {ex.Message}");
                Log.Error($"堆栈跟踪: {ex.StackTrace}");
                return false;
            }
        }
        public static Type GetLabPluginConfigType(Plugin plugin)
        {
            if (plugin == null)
                return null;

            Type pluginType = plugin.GetType();
            while (pluginType != null && pluginType != typeof(object))
            {
                if (pluginType.IsGenericType && pluginType.GetGenericTypeDefinition() == typeof(Plugin<>))
                {
                    return pluginType.GetGenericArguments()[0];
                }
                pluginType = pluginType.BaseType;
            }

            return null;
        }
        public static List<Type> LoadTypesFromAssemblies(Type mainType, string paths)
        {
            if (mainType == null)
                throw new ArgumentNullException(nameof(mainType));

            if (string.IsNullOrEmpty(paths))
                throw new ArgumentNullException(nameof(paths));

            var resultTypes = new List<Type>();

            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                var dllFiles = Directory.GetFiles(paths, "*.dll", SearchOption.AllDirectories);

                foreach (var dllFile in dllFiles)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dllFile);
                        var types = assembly.GetExportedTypes();
                        var compatibleTypes = types.Where(t =>
                            t.IsClass &&
                            !t.IsAbstract &&
                            mainType.IsAssignableFrom(t));

                        resultTypes.AddRange(compatibleTypes);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"加载程序集 {Path.GetFileName(dllFile)} 时发生错误: {ex.Message}");
                    }
                }

                return resultTypes;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string dependenceDir = Paths.DependenceDir;

                if (string.IsNullOrEmpty(dependenceDir) || !Directory.Exists(dependenceDir))
                    return null;
                string assemblyName = new AssemblyName(args.Name).Name + ".dll";
                string assemblyPath = Path.Combine(dependenceDir, assemblyName);
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
                var allDlls = Directory.GetFiles(dependenceDir, "*.dll", SearchOption.AllDirectories);
                var matchingDll = allDlls.FirstOrDefault(dll =>
                    Path.GetFileNameWithoutExtension(dll).Equals(new AssemblyName(args.Name).Name,
                    StringComparison.OrdinalIgnoreCase));

                if (matchingDll != null)
                {
                    return Assembly.LoadFrom(matchingDll);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"解析依赖 {args.Name} 时发生错误: {ex.Message}");
            }

            return null;
        }

        public static List<Type> LoadTypesFromAssembliesWithFilter(Type mainType, string paths, Func<Type, bool> filter = null)
        {
            var allTypes = LoadTypesFromAssemblies(mainType, paths);

            if (filter != null)
            {
                return allTypes.Where(filter).ToList();
            }

            return allTypes;
        }

        public static List<T> CreateInstancesFromAssemblies<T>(string paths) where T : class
        {
            var types = LoadTypesFromAssemblies(typeof(T), paths);
            var instances = new List<T>();

            foreach (var type in types)
            {
                try
                {
                    if (Activator.CreateInstance(type) is T instance)
                    {
                        instances.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"创建类型 {type.Name} 实例时发生错误: {ex.Message}");
                }
            }

            return instances;
        }
        public static bool LoadMod(Type MainMod)
        {
            if (!typeof(MOD).IsAssignableFrom(MainMod))
            {
                return false;
            }

            MOD mod = (MOD)Activator.CreateInstance(MainMod);
            LoadsMOD.Add(mod);
            string modName = mod.Name;
            string configPath = Path.Combine(Paths.ConfigDir, Server.Port.ToString(),"Plugins");
            string modConfigFile = Path.Combine(configPath, $"{modName}.yaml");
            Log.Debug($"加载插件中[{modName}]制作者[{mod.Author}]版本[{mod.Version}]");
            try
            {
                if (CheckFileExists(configPath, $"{modName}.yaml"))
                {
                    var configs = GetPropertiesWithValuesFromYaml(modConfigFile);
                    var isEnabledEntry = configs.FirstOrDefault(x => x.Key == "IsEnabled");
                    if (isEnabledEntry.Key != null)
                    {
                        bool isEnabled = Convert.ToBoolean(isEnabledEntry.Value);
                        if (isEnabled)
                        {
                            mod.OnEnabled();
                        }
                        else
                        {
                            Log.Debug($"插件[{mod.Name}]已关闭");
                            mod.OnDisable();
                            LoadsMOD.Remove(mod);
                        }
                    }
                    else
                    {
                        WriteTextToTOP(modConfigFile, "IsEnabled: true");
                        mod.OnEnabled();
                    }
                }
                else
                {
                    if (mod.Config == null)
                    {
                        Log.Error($"{mod.Name}未指定Config");
                        return false;
                    }
                    string pluginConfig = ConvertTypeToYaml(mod.ConfigType);
                    bool hasIsEnabledProperty = mod.ConfigType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Any(p => p.Name == "IsEnabled" && p.PropertyType == typeof(bool));
                    if (hasIsEnabledProperty)
                    {
                        File.WriteAllText(modConfigFile, pluginConfig);
                        mod.OnEnabled();
                    }
                    else
                    {
                        string contentWithIsEnabled = "IsEnabled: true" + Environment.NewLine + pluginConfig;
                        File.WriteAllText(modConfigFile, contentWithIsEnabled);
                        mod.OnEnabled();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"加载MOD {modName} 时发生错误: {ex.Message}");
                return false;
            }
        }

        public static List<KeyValuePair<string, object>> GetPropertiesWithValuesFromYaml(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"YAML文件未找到: {filePath}");

            try
            {
                var yamlContent = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var data = deserializer.Deserialize<Dictionary<string, object>>(yamlContent);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"解析YAML文件失败: {ex.Message}", ex);
            }
        }

        public static string ConvertTypeToYaml(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            object instance = CreateInstance(type);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(p => p.CanRead);

            var propertyValues = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var value = GetPropertyValue(property, instance);
                propertyValues[property.Name] = value;
            }

            var serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

            return serializer.Serialize(propertyValues);
        }

        private static object CreateInstance(Type type)
        {
            try
            {
                if (type.GetConstructor(Type.EmptyTypes) != null)
                {
                    return Activator.CreateInstance(type);
                }
            }
            catch
            {
            }
            return null;
        }

        public static bool CheckFileExists(string path, string fileName)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            try
            {
                string fullPath = Path.Combine(path, fileName);
                return File.Exists(fullPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"检查文件存在性时发生错误: {ex.Message}", ex);
            }
        }

        private static object GetPropertyValue(PropertyInfo property, object instance)
        {
            try
            {
                if (instance == null) return "null";

                var value = property.GetValue(instance);
                if (value == null) return "null";

                if (value is string str) return str;

                if (value is IEnumerable enumerable && !(value is string))
                {
                    var list = new List<object>();
                    foreach (var item in enumerable)
                    {
                        list.Add(item ?? "null");
                    }
                    return list.Count > 0 ? list : new List<object>();
                }

                return value;
            }
            catch
            {
                return "unknown";
            }
        }

        public static void WriteTextToTOP(string file, string text)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (!File.Exists(file))
            {
                File.WriteAllText(file, text + Environment.NewLine);
                return;
            }

            string extension = Path.GetExtension(file).ToLower();
            if (extension == ".yaml" || extension == ".yml")
            {
                WriteToYamlFileSmart(file, text);
            }
            else
            {
                WriteToGenericFile(file, text);
            }
        }

        private static void WriteToYamlFileSmart(string file, string text)
        {
            var lines = File.ReadAllLines(file).ToList();
            string newKey = ExtractKey(text);

            if (!string.IsNullOrEmpty(newKey) && KeyExists(lines, newKey))
            {
                ReplaceExistingKey(lines, newKey, text);
            }
            else
            {
                int insertPosition = FindInsertPosition(lines);
                lines.Insert(insertPosition, text);
            }

            File.WriteAllLines(file, lines);
        }

        private static void WriteToGenericFile(string file, string text)
        {
            string existingContent = File.ReadAllText(file);
            string newContent = text + Environment.NewLine + existingContent;
            File.WriteAllText(file, newContent);
        }

        private static string ExtractKey(string yamlLine)
        {
            int colonIndex = yamlLine.IndexOf(':');
            return colonIndex > 0 ? yamlLine.Substring(0, colonIndex).Trim() : null;
        }

        private static bool KeyExists(List<string> lines, string key)
        {
            return lines.Any(line =>
            {
                string trimmedLine = line.Trim();
                return !string.IsNullOrEmpty(trimmedLine) &&
                       !trimmedLine.StartsWith("#") &&
                       trimmedLine.StartsWith(key + ":");
            });
        }

        private static void ReplaceExistingKey(List<string> lines, string key, string newText)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (!string.IsNullOrEmpty(line) &&
                    !line.StartsWith("#") &&
                    line.StartsWith(key + ":"))
                {
                    lines[i] = newText;
                    return;
                }
            }
        }

        private static int FindInsertPosition(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;
                return i;
            }
            return 0;
        }
    }
}
