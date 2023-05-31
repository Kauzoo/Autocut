using System.Diagnostics;
using System.Net;

namespace Autocut;

public static class InfoWriter
{
    public static void ParseInfoWriterCSV()
    {
        IO.ValidateFilePath(ref )
    }

    internal static void ReadInfoWriterFile()
    {
        Console.WriteLine("Enter input video file path:");
        var path = Console.ReadLine();
        if(!IO.ValidateFilePath(ref path))
            ReadInfoWriterFile();
    }

    internal static bool ValidateInfoWriterFile(string path)
    {
        var content = File.ReadAllLines(path);
        foreach(var line in content)
        {
            string timestamp = line.Split(',')[0];
            
        }
    }
}

