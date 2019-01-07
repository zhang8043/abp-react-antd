using System;
using System.IO;
using System.Linq;
using Abp.Reflection.Extensions;

namespace Precise.Web
{
    /// <summary>
    /// 该类用于查找Web项目的根路径;
    /// 单元测试（查找视图）和实体框架核心命令行命令（查找conn字符串）。
    /// </summary>
    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(PreciseCoreModule).GetAssembly().Location);
            if (coreAssemblyDirectoryPath == null)
            {
                throw new Exception("找不到Precise.Core组件的位置!");
            }

            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            while (!DirectoryContains(directoryInfo.FullName, "Precise.sln"))
            {
                if (directoryInfo.Parent == null)
                {
                    throw new Exception("找不到内容根文件夹!");
                }

                directoryInfo = directoryInfo.Parent;
            }
            var webHostFolder = Path.Combine(directoryInfo.FullName, "Precise.Web.Host");
            if (Directory.Exists(webHostFolder))
            {
                return webHostFolder;
            }

            throw new Exception("找不到Web项目的根文件夹!");
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }
}
