// <copyright file="ExtensionHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Tests
{
    public static class ExtensionHelpers
    {
        public static string GetPath(string path)
        {
            var assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return System.IO.Path.Join(assemblyPath, path);
        }
    }
}
