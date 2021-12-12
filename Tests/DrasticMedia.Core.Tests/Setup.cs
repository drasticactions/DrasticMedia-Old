using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Core.Tests
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            LibVLCSharp.Shared.Core.Initialize();

            var mediaPath = ExtensionHelpers.GetPath("Media");
            var mediaTestFiles = ExtensionHelpers.GetPath("MediaTestFiles");
            if (Directory.Exists(mediaPath))
            {
                Directory.Delete(mediaPath, true);
            }

            Directory.CreateDirectory(mediaPath);

            string[] files = System.IO.Directory.GetFiles(mediaTestFiles, "*.*", new EnumerationOptions() { RecurseSubdirectories = true });
            foreach (string s in files)
            {
                var destFile = s.Replace("MediaTestFiles", "Media");
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                System.IO.File.Copy(s, destFile, true);
            }
        }
    }
}
