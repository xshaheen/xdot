using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using JetBrains.Annotations;

namespace X.Core.Utils {
    [PublicAPI]
    public static class AssemblyHelper {
        public static List<Assembly> LoadAssemblies(string folderPath, SearchOption searchOption) {
            var list = new List<Assembly>();

            foreach (var item in GetAssemblyFiles(folderPath, searchOption).ToArray()) {
                list.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(item));
            }

            return list;
        }

        public static IEnumerable<string> GetAssemblyFiles(
            string folderPath,
            SearchOption searchOption
        ) {
            return Directory.EnumerateFiles(folderPath, "*.*", searchOption)
                .Where(s =>
                    s.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                    || s.EndsWith(".exe", StringComparison.OrdinalIgnoreCase));
        }

        public static IReadOnlyList<Type?> GetAllTypes(Assembly assembly) {
            try {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex) {
                return ex.Types;
            }
        }
    }
}
