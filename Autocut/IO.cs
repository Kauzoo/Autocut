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
        if (!ValidateVidPath(ref vidPath))
            ReadInputVid();
        return (vidPath, Path.GetExtension(vidPath))!;
    }

    internal static (string path, TimestampType type) ReadTimestampFile()
    {
        Console.WriteLine("Enter timestamp file path:");
        var tmpPath = Console.ReadLine();
        if (!ValidateTimestampPath(ref tmpPath, out var type))
            ReadTimestampFile();
        return (tmpPath, type)!;
    }

    internal static string ReadOutputPath()
    {
        Console.WriteLine("Enter output path (Folder):");
        var outPath = Console.ReadLine();
        if (!ValidateOutputPath(ref outPath))
            ReadOutputPath();
        return outPath!;
    }

    #endregion

    #region Validation

    private static bool ValidateFilePath(ref string? path)
    {
        if (path is null)
        {
            Console.WriteLine("ERROR: Missing Path");
            return false;
        }

        path = RemoveQuotes(path);

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

    public static bool ValidatePath(ref string? path)
    {
        if (path is null)
        {
            Console.WriteLine("ERROR: Missing Path");
            return false;
        }

        path = RemoveQuotes(path);

        if (!Path.Exists(path))
        {
            Console.WriteLine("ERROR: Path does not exist");
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

    private static bool ValidateVidPath(ref string? path)
    {
        if (!ValidateFilePath(ref path))
            return false;

        // TODO Add validation for supported video file extensions
        if (!Path.HasExtension(path))
        {
            Console.WriteLine("ERROR: File or Path does not have an extension");
            return false;
        }

        return true;
    }

    private static bool ValidateTimestampPath(ref string? path, out TimestampType timestampType)
    {
        timestampType = TimestampType.NULL;
        if (!ValidateFilePath(ref path))
            return false;
        // TODO Add .csv support
        if (Path.HasExtension(path) && Path.GetExtension(path) == ".tsv")
        {
            timestampType = TimestampType.TSV;
            return true;
        }

        Console.WriteLine("ERROR: File or Path does not have an extension or is not a .tsv");
        return false;
    }

    private static bool ValidateOutputPath(ref string? path)
    {
        if (!ValidatePath(ref path))
            return false;
        if (File.Exists(path))
        {
            Console.WriteLine("ERROR: Path is a File. Expected Directory");
            return false;
        }

        if (!Path.EndsInDirectorySeparator(path!))
            path += Path.DirectorySeparatorChar;
        return true;
    }
    #endregion

    /// <summary>
    /// Removes leading and trailing quotes from a path string if they exist
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string RemoveQuotes(string path)
    {
        if (path[0] == '"' && path[path.Length - 1] == '"')
        {
            return path.Substring(1, (path.Length - 2));
        }
        return path;
    }

    /// <summary>
    /// Add quotes to a path string
    /// This is best practice in order for use in cojunction with ffmpeg
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string AddQuotes(string path)
    {
        if (path[0] != '"')
            path = '"' + path;
        if (path[path.Length - 1] != '"')
            path = path + '"';
        return path;
    }

    public static IOPaths ParseCommandLineArguments(string[] args)
    {
        /**
         * Planned args:
         * -h --help    :   Show help print         
         * -i <Path>    :   Input vid path          [Required argument]
         * -o <Path>    :   Output (Folder) path    [Required argument]
         * -t <Path>    :   Timestamp file path     [Required argument]
         */
         bool inputVidFlag, outputFlag, timestampFlag;

        // Parse help
         if (args.Contains("-h") || args.Contains("--help"))
         {
            printHelp();
         }
         // Parse required arguments
         for (var i = 0; i < args.Length; i++)
         {
            switch(args[i])
            {

                case "-i":
                    if (i + 1 >= args.Length)
                    {
                        Console.
                    }
                    ValidateVidPath(args[i + 1])
                    break;
                case "-o":
                    break;
                case "-t":
                    break;
            }

         }


        throw new NotImplementedException();
    }

    private static string 
    public static void printHelp()
    {
        throw new NotImplementedException();
    }
}

public enum VidExtension
{
    mp4
}