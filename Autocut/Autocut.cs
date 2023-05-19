namespace Autocut
{
    using System.Diagnostics;
    using System.IO;

    internal static class Autocut
    {
        public static void Main(string[] args)
        {
            IO.IOPaths con;
            Console.Title = "Autocut | powered by ffmpeg";
            if (args.Length == 0)
                con = IO.ReadConsole();
            else
                con = IO.ParseCommandLineArguments(args);
            run(con.InputVid, ParseTimestamps(con));
        }

        internal struct Segment
        {
            public string OutPath;
            public Timestamp Start, End;
        }

        internal readonly struct Timestamp
        {
            public readonly string Hour, Minute, Second;

            public Timestamp(string time)
            {
                var times = time.Split(':');
                if (times.Length != 3)
                {
                    throw new FormatException("Timestamp to long or short");
                }

                foreach (var t in times)
                {
                    if (Validate(t)) continue;
                    throw new FormatException("Timestamp contains non Digit character");
                }

                Hour = times[0];
                Minute = times[1];
                Second = times[2];
            }

            private bool Validate(string time) => (time.Length == 2 && char.IsDigit(time[0]) && char.IsDigit(time[1]));

            public override string ToString() => $"{Hour}:{Minute}:{Second}";
        }

        internal static Segment[] ParseTimestamps(IO.IOPaths con)
        {
            char delim;
            switch (con.TimestampType)
            {
                case IO.TimestampType.TSV:
                    delim = '\t';
                    break;
                case IO.TimestampType.CSV:
                    throw new NotImplementedException();
                default:
                    Console.WriteLine("ERROR: Unknown error");
                    throw new Exception();
            }

            var content = File.ReadAllLines(con.Timestamp);
            var segments = new Segment[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                segments[i] = new Segment();
                var s = content[i].Split(delim);
                if (s.Length != 3)
                {
                    Console.WriteLine($"ERROR: Failed to parse Segment at Line {i + 1}. To many or few elements");
                    throw new FormatException();
                }

                if (Path.IsPathFullyQualified(s[0]))
                {
                    Console.WriteLine($"Detected full path at Line {i + 1}");
                    if (!Path.HasExtension(s[0]) || Path.GetExtension(s[0]) != con.VidExtension)
                    {
                        Console.WriteLine($"ERROR: Missing file extension or non matching file extension");
                        throw new FormatException();
                    }

                    segments[i].OutPath = s[0];
                }
                else
                {
                    foreach (var c in Path.GetInvalidFileNameChars())
                    {
                        if (!s[0].Contains(c)) continue;
                        Console.WriteLine($"ERROR: Filename {s[0]} at Line {i + 1} contains invalid character {c}");
                        try
                        {
                            new Timestamp(s[0]);
                            Console.WriteLine($"ERROR: Detected possible Timestamp at Line {i + 1} Column 0. Expected Filename or Path.");
                        }
                        catch (FormatException e)
                        {
                            throw e;
                        }
                        throw new FormatException();
                    }

                    // TODO Handle file extension
                    segments[i].OutPath = con.Outpath + s[0] + con.VidExtension;
                }

                try
                {
                    segments[i].Start = new Timestamp(s[1]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"ERROR: Failed to parse start Timestamp at Line {i + 1}. {e.Message}");
                    throw;
                }

                try
                {
                    segments[i].End = new Timestamp(s[2]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"ERROR: Failed to parse end Timestamp at Line {i + 1}. {e.Message}");
                    throw;
                }
            }

            return segments;
        }

        internal static void run(string inputPath, Segment[] segments)
        {
            // TODO Add support for encoding instead of copy
            
            foreach (var seg in segments)
            {
                var process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = "ffmpeg.exe";
                process.StartInfo.Arguments = $"-ss {seg.Start} -to {seg.End} -i {inputPath} -c copy {seg.OutPath}";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
                process.WaitForExit(); // Waits here for the process to exit.    
            }

            Console.WriteLine("Done");
        }
    }
}