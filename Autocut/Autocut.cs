namespace Autocut
{
    using System.Diagnostics;
    using System.IO;

    public static class Autocut
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Autocut");
            Console.WriteLine("Enter input video file path:");
            var vid_path = Console.ReadLine();
            if (vid_path == null)
            {
                Console.WriteLine("Missing Video File Path. Exiting");
                return;
            }

            Console.WriteLine("Enter timestamp file path:");
            var tmp_path = Console.ReadLine();
            if (tmp_path == null)
            {
                Console.WriteLine("Missing Timestamp File Path. Exiting");
                return;
            }

            run(vid_path, ParseInput(tmp_path));
        }

        public struct Segment
        {
            public Timestamp Start, End;
            public string OutPath;

            public Segment(string line)
            {
                var s = line.Split("\t");
                if (s.Length != 3)
                {
                    Console.WriteLine("Error while parsing segment string");
                    throw new FormatException();
                }

                Start = new Timestamp(s[0]);
                End = new Timestamp(s[1]);
                OutPath = s[2];
            }
        }

        public struct Timestamp
        {
            public string Hour, Minute, Second;

            public Timestamp(string time)
            {
                var times = time.Split(":");
                if (times.Length != 3)
                {
                    Console.WriteLine("Error in timestamp specification");
                    throw new FormatException();
                }

                foreach (var t in times)
                {
                    if (Validate(t)) continue;
                    Console.WriteLine("Error in timestamp specification");
                    throw new FormatException();
                }

                Hour = times[0];
                Minute = times[1];
                Second = times[2];
            }

            private bool Validate(string time) => (time.Length == 2 && char.IsDigit(time[0]) && char.IsDigit(time[1]));

            public override string ToString() => $"{Hour}:{Minute}:{Second}";
        }

        public static Segment[] ParseInput(string path)
        {
            var content = File.ReadAllLines(path);
            var segments = new Segment[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                segments[i] = new Segment(content[i]);
            }

            return segments;
        }

        public static void run(string inputPath, Segment[] segments)
        {
            foreach (var seg in segments)
            {
                Process process = new Process();
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