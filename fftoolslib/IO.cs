using System;
using System.Collections.Generic;

namespace fftoolslib;

public class PathHelpers
{
    /// <summary>
    /// Validate that a (File)path exists, is fully qualified and not null.
    /// Also removes quotes.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>true if all checks pass; false if any checks fail</returns>
    public static bool ValidateFilePath(ref string? path)
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

    /// <summary>
    /// Split a string into substring using space as delim. 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string[] SplitInput(string? input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        List<string> buf = new();
        List<char> wordbuf = new(200);
        int i = 0;
        while (i < input.Length)
        {
            // Skip over whitespace
            if (input[i++] == ' ')
                continue;
            // Iterate over a string
            char delim = (input[i] == '"') ? '"' : ' ';
            while (input[i] != delim && i < input.Length)
            {
                wordbuf.Add(input[i++]);
            }
            buf.Add(string.Join("", wordbuf.ToArray()));
        }
        return buf.ToArray();
    }

    public static List<string> SplitPaths(string? path)
    {
        var paths = new List<string>(SplitInput(path));
        paths.RemoveAll(p => !ValidatePath(path: ref p));
        return paths;
    }

    public static void FindFiles()
    {
        throw new NotImplementedException();
    }

    public static bool ValidateVidPath(ref string? path)
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

    /// <summary>
    /// Removes leading and trailing quotes from a path string if they exist
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string RemoveQuotes(string path)
    {
        if (path[0] == '"' && path[^1] == '"')
        {
            return path[1..^1];
        }
        return path;
    }

    /// <summary>
    /// Removes leading and trailing quotes from a path string if they exist
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static void RemoveQuotes(ref string path)
    {
        if (path[0] == '"' && path[^1] == '"')
            path = path[1..^1];
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
        if (path[^1] != '"')
            path += '"';
        return path;
    }

    /// <summary>
    /// Add quotes to a path string
    /// This is best practice in order for use in cojunction with ffmpeg
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static void AddQuotes(ref string path)
    {
        if (path[0] != '"')
            path = '"' + path;
        if (path[^1] != '"')
            path += '"';
    }
}
