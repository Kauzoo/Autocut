using System.Diagnostics;
using System.Net;

namespace Autocut;

public static class IO
{
    // TODO Make sure Console Fore- and Background color are always contrasting

    #region ConsoleIO

    public enum TimestampType
    {
        TSV, CSV, NULL
    }

    public struct IOPaths
    {
        public string InputVid;
        public string Timestamp;
        public string Outpath;
        public TimestampType TimestampType;
        public string VidExtension;

        public IOPaths(string vid, string time, string opath, TimestampType timestampType, string vidExtension)
        {
            InputVid = vid;
            Timestamp = time;
            Outpath = opath;
            TimestampType = timestampType;
            VidExtension = vidExtension;
        }
    }
    
    public static IOPaths ReadConsole()
    {
        Console.WriteLine("Welcome to Autocut");
        var inputVid = ReadInputVid();
        var tstamp = ReadTimestampFile();
        var outpath = ReadOutputPath();
        return new IOPaths(inputVid.path, tstamp.path, outpath, tstamp.type, inputVid.extension);
    }

    internal static (string path, string extension) ReadInputVid()
    {
        Console.WriteLine("Enter input video file path:");
        var vidPath = Console.ReadLine();
        if (!ValidateVidPath(vidPath))
            ReadInputVid();
        return (vidPath, Path.GetExtension(vidPath))!;
    }

    internal static (string path, TimestampType type) ReadTimestampFile()
    {
        Console.WriteLine("Enter timestamp file path:");
        var tmpPath = Console.ReadLine();
        if (!ValidateTimestampPath(tmpPath, out var type))
            ReadTimestampFile();
        return (tmpPath, type)!;
    }

    internal static string ReadOutputPath()
    {
        Console.WriteLine("Enter output path (Folder):");
        var outPath = Console.ReadLine();
        if (!ValidateOutputPath(outPath))
            ReadOutputPath();
        return outPath!;
    }

    #endregion

    #region Validation

    private static bool ValidatePath(string? path)
    {
        if (path is null)
        {
            Console.WriteLine("ERROR: Missing Path");
            return false;
        }

        if (!Path.Exists(path) || !File.Exists(path))
        {
            Console.WriteLine("ERROR: File or Path does not exist");
            return false;
        }

        if (!Path.IsPathFullyQualified(path))
        {
            // TODO Add examples of fully qualified paths
            Console.WriteLine("ERROR: Path is not fully qualified");
            return false;
        }

        return true;
    }

    private static bool ValidateVidPath(string? path)
    {
        if (!ValidatePath(path))
            return false;

        // TODO Add validation for supported video file extensions
        if (!Path.HasExtension(path))
        {
            Console.WriteLine("ERROR: File or Path does not have an extension");
            return false;
        }

        return true;
    }

    private static bool ValidateTimestampPath(string? path, out TimestampType timestampType)
    {
        timestampType = TimestampType.NULL;
        if (!ValidatePath(path))
            return false;
        // TODO Add .csv support
        if (Path.HasExtension(path) && Path.GetExtension(path) == "tsv")
        {
            timestampType = TimestampType.TSV;
            return true;
        }

        Console.WriteLine("ERROR: File or Path does not have an extension or is not a .tsv");
        return false;
    }

    private static bool ValidateOutputPath(string? path)
    {
        if (!ValidatePath(path))
            return false;
        if (File.Exists(path))
        {
            Console.WriteLine("ERROR: Path is a File. Expected Directory");
            return false;
        }

        if (!Path.EndsInDirectorySeparator(path))
            path += Path.DirectorySeparatorChar;
        return true;
    }
    #endregion

    public static void ParseCommandLineArguments(string[] args)
    {
        /**
         * Planned args:
         * -h --help    :   Show help print
         * -i <Path>    :   Input vid path
         * -o <Path>    :   Output (Folder) path
         * -t <Path>    :   Timestamp file path
         */
        throw new NotImplementedException();
    }
}

public enum VidExtension
{
    mp4
}