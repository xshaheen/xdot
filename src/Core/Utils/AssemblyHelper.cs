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
	        return
		        GetAssemblyFiles(folderPath, searchOption)
		        .Select(item => AssemblyLoadContext.Default.LoadFromAssemblyPath(item))
		        .ToList();
        }

        public static IEnumerable<string> GetAssemblyFiles(string folderPath, SearchOption searchOption) {
            return Directory.EnumerateFiles(folderPath, "*.*", searchOption).Where(s =>
                s.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".exe", StringComparison.OrdinalIgnoreCase));
        }

        public static IReadOnlyList<Type?> GetAllTypes(Assembly assembly) {
            try {
                return assembly.GetTypes();
            } catch (ReflectionTypeLoadException ex) {
                return ex.Types;
            }
        }
    }
}
