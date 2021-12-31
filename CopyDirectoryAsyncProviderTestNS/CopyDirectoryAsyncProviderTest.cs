using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CopyDirectoryAsyncProviderNS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyDirectoryAsyncProviderTestNS
{
    [TestClass]
    public class CopyDirectoryAsyncProviderTest
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var sourceDirName0 = "sourceDirName0";
            var sourceDirName1 = Path.Combine(sourceDirName0, "sourceDirName1");
            var sourceDirName1_1 = Path.Combine(sourceDirName1, "sourceDirName1-1");
            var sourceDirName1_1_1 = Path.Combine(sourceDirName1_1, "sourceDirName1-1-1");
            var sourceDirName1_1_2 = Path.Combine(sourceDirName1_1, "sourceDirName1-1-2");
            var sourceDirName1_2 = Path.Combine(sourceDirName1, "sourceDirName1-2");
            var targetDirName0 = "targetDirName0";

            var sourceDirNameList = new[]
            {
                sourceDirName0,
                sourceDirName1,
                sourceDirName1_1,
                sourceDirName1_1_1,
                sourceDirName1_1_2,
                sourceDirName1_2
            };

            var sourceFileName0_1 = Path.Combine(sourceDirName0, "sourceFileName0-1.txt");
            var sourceFileName0_2 = Path.Combine(sourceDirName0, "sourceFileName0-2.txt");
            var sourceFileName1_1 = Path.Combine(sourceDirName1, "sourceFileName1-1.txt");
            var sourceFileName1_2 = Path.Combine(sourceDirName1, "sourceFileName1-2.txt");
            var sourceFileName1_1_1 = Path.Combine(sourceDirName1_1, "sourceFileName1-1-2.txt");
            var sourceFileName1_1_2 = Path.Combine(sourceDirName1_1, "sourceFileName1-1-2.txt");
            var sourceFileName1_1_1_1 = Path.Combine(sourceDirName1_1_1, "sourceFileName1-1-1-1.txt");
            var sourceFileName1_1_1_2 = Path.Combine(sourceDirName1_1_1, "sourceFileName1-1-1-2.txt");
            var sourceFileName1_1_2_1 = Path.Combine(sourceDirName1_1_2, "sourceFileName1-1-2-1.txt");
            var sourceFileName1_1_2_2 = Path.Combine(sourceDirName1_1_2, "sourceFileName1-1-2-2.txt");
            var sourceFileName1_2_1 = Path.Combine(sourceDirName1_2, "sourceFileName1-2-1.txt");
            var sourceFileName1_2_2 = Path.Combine(sourceDirName1_2, "sourceFileName1-2-2.txt");

            var sourceFileNameList = new[]
            {
                sourceFileName0_1,
                sourceFileName0_2,
                sourceFileName1_1,
                sourceFileName1_2,
                sourceFileName1_1_1,
                sourceFileName1_1_2,
                sourceFileName1_1_1_1,
                sourceFileName1_1_1_2,
                sourceFileName1_1_2_1,
                sourceFileName1_1_2_2,
                sourceFileName1_2_1,
                sourceFileName1_2_2
            };

            var targetDirNameList = sourceDirNameList
                .Select(sourceDirName => sourceDirName.Replace(sourceDirName0, targetDirName0))
                .ToList();

            var targetFileNameList = sourceFileNameList
                .Select(sourceFileName => sourceFileName.Replace(sourceDirName0, targetDirName0))
                .ToList();

            foreach (var dirName in sourceDirNameList)
                Directory.CreateDirectory(dirName);
            foreach (var fileName in sourceFileNameList)
                await using (File.Create(fileName))
                {
                }

            await CopyDirectoryAsyncProvider.CopyDirectoryAsync(
                new DirectoryInfo(sourceDirName0),
                new DirectoryInfo(targetDirName0),
                CancellationToken.None
            );

            foreach (var targetDirName in targetDirNameList)
            {
                Debug.WriteLine(targetDirName);
                Assert.IsTrue(Directory.Exists(targetDirName));
            }

            foreach (var targetFileName in targetFileNameList)
            {
                Debug.WriteLine(targetFileName);
                Assert.IsTrue(File.Exists(targetFileName));
            }
        }
    }
}