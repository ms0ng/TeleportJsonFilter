using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        const string REGEX_PATTERN = @"""position""\s*:\s*\[(.+),(.+),(.+)]}";
        const string JSON_PATH = ".\\json";
        const string UNMATCH_PATH = ".\\json_unmatch";
        const string FILTED_PATH = ".\\json_filted";
        static readonly Regex regex = new Regex(REGEX_PATTERN);
        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Handler.Instance.SetParams(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]));
            }
            Console.WriteLine($"正则匹配表达式：{REGEX_PATTERN}");

            if (!Directory.Exists(JSON_PATH)) Directory.CreateDirectory(JSON_PATH);
            if (!Directory.Exists(UNMATCH_PATH)) Directory.CreateDirectory(UNMATCH_PATH);
            if (!Directory.Exists(FILTED_PATH)) Directory.CreateDirectory(FILTED_PATH);

            string[] files = Directory.GetFiles(".\\json", "*.json");
            foreach (string fileName in files)
            {
                string fileContent = File.ReadAllText(fileName);
                Match match = regex.Match(fileContent);
                if (match.Success)
                {
                    Vector3 v3 = new Vector3(
                         Convert.ToSingle(match.Groups[1].Value),
                         Convert.ToSingle(match.Groups[2].Value),
                         Convert.ToSingle(match.Groups[3].Value)
                        );
                    //Console.WriteLine($"{fileName} :  {v3}");
                    Handler.Instance.AddPoint(v3, out bool success);
                    if (!success)
                    {
                        Console.WriteLine($"移动文件到{FILTED_PATH}文件夹下： {fileName}");
                        File.Move(fileName, fileName.Replace(JSON_PATH, FILTED_PATH));
                    }
                }
                else
                {
                    Console.WriteLine($"内容不匹配，移动文件到{UNMATCH_PATH}文件夹下：{fileName}");
                    File.Move(fileName, fileName.Replace(JSON_PATH, UNMATCH_PATH));
                }
            }

            Console.WriteLine($"过滤{files.Length}个文件后总共剩下了{Handler.Instance.AllPoints.Count}个点");
            Console.ReadKey(true);
        }
    }
}
