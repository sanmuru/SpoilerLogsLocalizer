using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace sll
{
    static class Program
    {
        static string DictonaryDirectory = null;
        static HashSet<string> SpoilerLogFiles = new HashSet<string>();

        static bool ProcessCmdLine(string[] args)
        {
            args = args ?? Environment.GetCommandLineArgs();

            foreach (var arg in args)
            {
                if (arg == null || string.IsNullOrEmpty(arg.Trim())) continue;

                Match match = Regex.Match(arg, @"^(/|-{1,2})(?<PARAM>[^/\-:]*)");
                if (match.Success)
                {
                    string param = match.Groups["PARAM"].Value;
                    if (string.IsNullOrEmpty(param))
                    {
                        Console.WriteLine("未指定命令行参数。");
                        return false;
                    }
                    else if (param == "dictdir")
                    {
                        if (DictonaryDirectory == null)
                        {
                            string dir;
                            int i = arg.IndexOf(':');
                            if (i < 0) dir = "";
                            else dir = arg.Substring(i + 1);
                            
                            if (string.IsNullOrEmpty(dir))
                            {
                                Console.WriteLine("无法找到文件夹“”。");
                                return false;
                            }
                            else if (Directory.Exists(dir))
                                DictonaryDirectory = Path.GetFullPath(dir);
                            else
                            {
                                Console.WriteLine("无法找到文件夹“{0}”。", dir);
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("多次指定命令行参数。{0}", param);
                            return false;
                        }
                    }
                }
                else
                {
                    SpoilerLogFiles.Add(Path.GetFullPath(arg));
                }
            }

            if (DictonaryDirectory == null)
            {
                DictonaryDirectory = Path.Combine(Environment.CurrentDirectory, "dict");
                if (!Directory.Exists(DictonaryDirectory))
                    DictonaryDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dict");
                if (!Directory.Exists(DictonaryDirectory))
                {
                    Console.WriteLine("无法找到字典文件夹。");
                    return false;
                }
            }

            return true;
        }

        static void Main(string[] args)
        {
            if (!ProcessCmdLine(args)) return; // 处理命令行参数过程中出现错误，立即退出。
        }
    }
}
