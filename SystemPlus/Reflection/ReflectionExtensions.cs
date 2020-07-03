using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace SystemPlus.Reflection
{
    /// <summary>
    /// Extension methods and utilities for reflection activities
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Properties

        public static string? ProductName
        {
            get { return Assembly.GetEntryAssembly()?.Name(); }
        }

        public static Version? ProductVersion
        {
            get { return Assembly.GetEntryAssembly()?.Version(); }
        }

        public static string? CallerName
        {
            get { return Assembly.GetCallingAssembly()?.Name(); }
        }

        public static Version? CallerVersion
        {
            get { return Assembly.GetCallingAssembly()?.Version(); }
        }

        public static string? Name(this Assembly assembly)
        {
            return assembly?.GetName()?.Name;
        }

        public static Version? Version(this Assembly assembly)
        {
            return assembly?.GetName()?.Version;
        }

        public static string? AssemblyTitle
        {
            get
            {
                return Assembly.GetEntryAssembly()?.GetAssemblyAttribute<AssemblyTitleAttribute>()?.Title;
            }
        }

        public static string? AssemblyFileVersion
        {
            get
            {
                return Assembly.GetEntryAssembly()?.GetAssemblyAttribute<AssemblyFileVersionAttribute>()?.Version;
            }
        }

        public static string? AssemblyCopyright
        {
            get
            {
                return Assembly.GetEntryAssembly()?.GetAssemblyAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            }
        }

        public static string? AssemblyCompany
        {
            get
            {
                return Assembly.GetEntryAssembly()?.GetAssemblyAttribute<AssemblyCompanyAttribute>()?.Company;
            }
        }

        #endregion

        #region Methods

        [return: MaybeNull]
        public static T GetAssemblyAttribute<T>(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            object[] attributes = assembly.GetCustomAttributes(typeof(T), false);

            if (attributes.Length == 0)
                return default;

            return (T)attributes[0];
        }

        /// <summary>
        /// Gets all resource names in assembly folder
        /// </summary>
        public static string[] GetResourceNames(string dir)
        {
            return GetResourceNames(Assembly.GetCallingAssembly(), dir);
        }

        /// <summary>
        /// Gets all resource names in assembly folder
        /// </summary>
        public static string[] GetResourceNames(this Assembly assembly, string dir)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            dir += "/";

            string strResources = assembly.GetName().Name + ".g.resources";

            using Stream? stream = assembly.GetManifestResourceStream(strResources);

            if (stream == null)
                throw new NullReferenceException(nameof(stream));

            using ResourceReader oResourceReader = new ResourceReader(stream);

            IEnumerable<string> vResources = from p in oResourceReader.OfType<DictionaryEntry>() let strTheme = (string)p.Key where strTheme.StartsWith(dir, StringComparison.InvariantCultureIgnoreCase) select strTheme;
            return vResources.ToArray();
        }

        /// <summary>
        /// Finds instantiates and returns a collection of all types implementing the given interface
        /// </summary>
        public static IEnumerable<T> GetInstancesOfInterface<T>(this Assembly assembly, params object[] constructorArgs) where T : class
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            Type baseType = typeof(T);

            IList<T> instances = new List<T>();
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));

            foreach (Type type in types)
            {
                if (Activator.CreateInstance(type, constructorArgs) is T instance)
                    instances.Add(instance);
            }

            return instances;
        }

        /// <summary>
        /// Finds instantiates and returns a collection of all types implementing the given class
        /// </summary>
        public static IEnumerable<T> GetInstancesOfType<T>(this Assembly assembly, params object[] constructorArgs) where T : class
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            IList<T> instances = new List<T>();
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(T)));

            foreach (Type type in types)
            {
                if (Activator.CreateInstance(type, constructorArgs) is T instance)
                {
                    instances.Add(instance);
                }
            }

            return instances;
        }

        /// <summary>
        /// Gets embedded resource stream, path should be in form "Folder\File.txt"
        /// </summary>
        public static Stream? GetEmbeddedResource(this Assembly assembly, string baseNamespace, string path)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            string fullpath = baseNamespace + "." + path.Replace('\\', '.').Replace('/', '.').Trim('.');
            return assembly.GetManifestResourceStream(fullpath);
        }

        #endregion
    }
}