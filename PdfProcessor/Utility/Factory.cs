using System;

namespace Abbey.PdfProcessor.Utility
{
    public struct ClassSpec
    {
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
    }

    internal static class Factory
    {
        public static T CreateInstance<T>(ClassSpec classSpec)
        {
            return CreateInstance<T>(classSpec.AssemblyName, classSpec.ClassName);
        }

        public static T CreateInstance<T>(string assemblyName, string className)
        {
            return (T) Activator.CreateInstance(assemblyName, className).Unwrap();
        }
    }
}