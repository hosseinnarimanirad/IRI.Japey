﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SqlServerTypes
{
    /// <summary>
    /// Utility methods related to CLR Types for SQL Server 
    /// </summary>
    public static class Utilities
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        static List<IntPtr> loadedAssemblies;

        static Utilities()
        {
            loadedAssemblies = new List<IntPtr>();
        }

        private static void LoadNativeAssembly(string nativeBinaryPath, string assemblyName)
        {
            var path = Path.Combine(nativeBinaryPath, assemblyName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{path} not found");
            }

            var ptr = LoadLibrary(path);
            if (ptr == IntPtr.Zero)
            {
                throw new Exception(string.Format(
                    "Error loading {0} (ErrorCode: {1})",
                    assemblyName,
                    Marshal.GetLastWin32Error()));
            }

            lock (loadedAssemblies)
            {
                loadedAssemblies.Add(ptr);
            }
        }

        ///// <summary>
        ///// Loads the required native assemblies for the current architecture (x86 or x64)
        ///// </summary>
        ///// <param name="rootApplicationPath">
        ///// Root path of the current application. Use Server.MapPath(".") for ASP.NET applications
        ///// and AppDomain.CurrentDomain.BaseDirectory for desktop applications.
        ///// </param>
        //public static void LoadNativeAssembliesv11(string rootApplicationPath)
        //{
        //    var nativeBinaryPath = IntPtr.Size > 4
        //        ? Path.Combine(rootApplicationPath, @"SqlServerTypes\x64\")
        //        : Path.Combine(rootApplicationPath, @"SqlServerTypes\x86\");

        //    LoadNativeAssembly(nativeBinaryPath, "msvcr100.dll");
        //    LoadNativeAssembly(nativeBinaryPath, "SqlServerSpatial110.dll");
        //}

        //Add support to new version
        public static void LoadNativeAssembliesv12(string rootApplicationPath)
        {
            var nativeBinaryPath = Environment.Is64BitProcess
                ? Path.Combine(rootApplicationPath, @"SqlServerTypes\x64\")
                : Path.Combine(rootApplicationPath, @"SqlServerTypes\x86\");

            LoadNativeAssembly(nativeBinaryPath, "msvcr100.dll");
            LoadNativeAssembly(nativeBinaryPath, "SqlServerSpatial120.dll");
        }

        public static void LoadNativeAssembliesv13(string rootApplicationPath)
        {
            var nativeBinaryPath = Environment.Is64BitProcess
                ? Path.Combine(rootApplicationPath, @"SqlServerTypes\x64\")
                : Path.Combine(rootApplicationPath, @"SqlServerTypes\x86\");

            //LoadNativeAssembly(nativeBinaryPath, "msvcr100.dll");
            LoadNativeAssembly(nativeBinaryPath, "msvcr120.dll");
            LoadNativeAssembly(nativeBinaryPath, "SqlServerSpatial130.dll");
        }

        public static void LoadNativeAssembliesv14(string rootApplicationPath)
        {
            var nativeBinaryPath = Environment.Is64BitProcess
            ? Path.Combine(rootApplicationPath, @"SqlServerTypes\x64\")
            : Path.Combine(rootApplicationPath, @"SqlServerTypes\x86\");

            LoadNativeAssembly(nativeBinaryPath, "msvcr120.dll");
            LoadNativeAssembly(nativeBinaryPath, "SqlServerSpatial140.dll");
        }

        public static void LoadNativeAssembliesv14()
        {
            LoadNativeAssembliesv14(AppDomain.CurrentDomain.BaseDirectory);
        }


        public static void UnloadNativeAssemblies()
        {
            lock (loadedAssemblies)
            {
                foreach (IntPtr ptr in loadedAssemblies)
                {
                    if (ptr != IntPtr.Zero)
                    {
                        FreeLibrary(ptr);
                    }
                }

                loadedAssemblies.Clear();
            }
        }
    }
}