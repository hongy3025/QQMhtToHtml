using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QQMhtToHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "QQMht To Html";
            string outdir = ".";
            string hFile = Path.GetFileNameWithoutExtension(args[0]);
            string d = $@"{outdir}\{hFile}_images";
            string mht, html = string.Empty;
            try
            {
                mht = File.ReadAllText(args[0]);
            }
            catch
            {
                return;
            }
            MHTMLParser parser = new MHTMLParser(mht);
            List<string[]> nodes;
            Console.WriteLine("Processing data...");
            nodes = parser.DecompressString();
            if (nodes.Count > 0)
            {
                int c = nodes.Count - 1;
                if (nodes.Count > 1)
                {

                    Console.WriteLine($"{c} image(s) found.");
                    Directory.CreateDirectory(d);
                    Console.WriteLine($"create dir {d}");
                }
                html = nodes[0][2];
                for (int i = 1; i < nodes.Count; i++)
                {
                    string ext = nodes[i][0].Split("/".ToArray())[1],
                        name = nodes[i][1].Split(".".ToArray())[0];
                    if (ext == "jpeg")
                    {
                        ext = "jpg";
                    }
                    string iFile = $@"{d}\{name}.{ext}";
                    byte[] bytes = Convert.FromBase64String(nodes[i][2]);
                    File.WriteAllBytes(iFile, bytes);
                    html = html.Replace($"{name}.dat", $@"{iFile}");
                    Console.WriteLine($"Processing image...({i}/{c}): {iFile}");
                }
                hFile = $@"{outdir}\{hFile}.html";
                File.WriteAllText(hFile, html);
                Console.Write("All done.");
            }
        }
    }
}