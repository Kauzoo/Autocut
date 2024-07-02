import sys
import os
import ffmpeg
import argparse

FFMPEG_PATH = ".\\FFmpeg\\ffmpeg.exe"

INPUT_FILE = ""
TIMESTAMP_FILE = ""
OUTPUT_DIR = ""

parser = argparse.ArgumentParser(description='Extract segments from a video file. Powered by FFmpeg.')

def run():
    input_extension = os.path.splitext(INPUT_FILE)[1]
    entries = parse_timestampfile(TIMESTAMP_FILE, OUTPUT_DIR)
    for e in entries:
        os.system(f"{FFMPEG_PATH} -ss {e.start} -to {e.end} -i \"{INPUT_FILE}\" -c copy \"{e.output}{input_extension}\"")


def gui_entry(input_file, timestamp_file, output_dir):
    global INPUT_FILE
    global TIMESTAMP_FILE
    global OUTPUT_DIR
    INPUT_FILE = input_file
    TIMESTAMP_FILE = timestamp_file
    OUTPUT_DIR = output_dir
    run()


def interacticve_input():
    while True:
        input_path = input("Enter input video path:")
        if parse_input_file(input_path):
            break

    while True:
        output_directory = input("Enter output directory:")
        if parse_output_file(output_directory):
            break

    while True:
        timestamp_directory = input("Enter timestamp path directory:")
        if parse_timestamp_file_path(timestamp_directory):
            break
    
    run()


def main():
    """Used as entry point for the command line version"""
    parse_arguments()
    run()


def parse_arguments():
    global TIMESTAMP_FILE
    global OUTPUT_DIR
    global INPUT_FILE
    global parser
    parser.add_argument('--timestampfile', dest='timestamppath', action='store',
                        help='path to timestamp file', required=False)
    parser.add_argument('--input', dest='inputpath', action='store',
                        help='path to (video) file', required=False)
    parser.add_argument('--output', dest='outputpath', action='store',
                        help='path to folder for output', required=False)
    parser.add_argument('--interactive', dest='interactive_flag', action='store_true',
                        help='start programm in interactive mode', default=True)

    args = parser.parse_args()

    if args.interactive_flag:
        interacticve_input()
        input("Finished")
        exit()

    # Validate timestamppath
    if os.path.isfile(args.timestamppath):
        extension = os.path.splitext(args.timestamppath)[1]
        print(f"Extension {extension}")
        if extension != ".tsv" and extension != ".csv":
            print(f"timestampfile {args.timestamppath} is not a valid format (.tsv / .csv)")
            exit()
        TIMESTAMP_FILE = args.timestamppath
        TIMESTAMP_FILE = remove_quotes(TIMESTAMP_FILE)
        print(f"wtf: {TIMESTAMP_FILE}")
    else:
        print(f"file {args.timestamppath} does not exist")
        exit()


    if not os.path.isdir(args.outputpath):
        print(f"path {args.outputpath} does not exist")
    OUTPUT_DIR = args.outputpath
    OUTPUT_DIR = remove_quotes(OUTPUT_DIR)

    if not os.path.isfile(args.inputpath):
        print(f"file {args.timestamppath} does not exist")
    INPUT_FILE = args.inputpath
    INPUT_FILE = remove_quotes(INPUT_FILE)



def parse_input_file(path):
    global INPUT_FILE
    path = path.strip()
    path = path.strip("\"")
    path = path.strip("\n")
    if not os.path.isfile(path):
        print(f"ERROR: File {path} does not exist.")
        print_path_help()
        return False
    INPUT_FILE = path
    INPUT_FILE = remove_quotes(INPUT_FILE)
    return True


def parse_output_file(path):
    global OUTPUT_DIR
    path = path.strip()
    path = path.strip("\"")
    path = path.strip("\n")
    if not os.path.isdir(path):
        print(f"ERROR: Path {path} does not exist")
        print_path_help()
        return False
    OUTPUT_DIR = path
    OUTPUT_DIR = remove_quotes(OUTPUT_DIR)
    return True


def parse_timestamp_file_path(path):
    global TIMESTAMP_FILE
    path = path.strip()
    path = path.strip("\"")
    path = path.strip("\n")
    if os.path.isfile(path):
        extension = os.path.splitext(path)[1]
        print(f"Extension {extension}")
        if extension != ".tsv" and extension != ".csv":
            print(f"Timestampfile {path} is not a valid format (.tsv / .csv)")
            print_timestamp_file_help()
            return False
        TIMESTAMP_FILE = path
        TIMESTAMP_FILE = remove_quotes(path)
    else:
        print(f"ERROR: File {path} does not exist")
        print_path_help()
        return False
    return True
    

class Entry:
    def __init__(self, output, start, end):
        self.output = output
        self.start = start
        self.end = end

class Timestamp:
    def __init__(self, hour, minute, second):
        self.hour = hour
        self.minute = minute
        self.second = second
    def __str__(self):
        return f"{self.hour}:{self.minute}:{self.second}"
    
def remove_quotes(path):
    path.strip()
    path.strip("\"")
    return path

def add_quotes(path):
    return f"\"{path}\""


def parse_timestampfile(filepath, outdir):
    """
    Parse timestamp file for futher processing. Expected formats (.tsv|.csv).
    """
    MAX_ELEMENTS = 3
    extension = os.path.splitext(filepath)[1]
    sep = ''
    if extension == ".tsv":
        sep = '\t'
    if extension == ".csv":
        sep = ','
    line_list = []
    print(extension)
    print(filepath)
    with open(filepath) as f:
        content = f.readlines()
    for line_num, line in enumerate(content):
        l = line.split(sep=sep)
        if (len(l) < MAX_ELEMENTS):
            print(f"ERROR: Less than {MAX_ELEMENTS} elements in line {line_num+1}")
            print(f"File: {filepath}")
            exit_helpfully()
        if (len(l) > MAX_ELEMENTS):
            print(f"ERROR: More than {MAX_ELEMENTS} elements in line {line_num+1}")
            print(f"File: {filepath}")
            exit_helpfully()
        line_list.append(l)
    entries = []
    for linenum, line in enumerate(line_list):
        e1 = Entry("", "", "")
        # Parse name
        filename = line[0]
        if os.path.isabs(filename):
            e1.output = filename
        else:
            e1.output = os.path.join(outdir, filename)
        # Parse start
        start_timestamp = parse_timestamp(line[1], linenum, "B", "Starttime")
        # Parse end 
        end_timestamp = parse_timestamp(line[2], linenum, "C", "Endtime")
        if int(start_timestamp.hour + start_timestamp.minute + start_timestamp.second) > int(end_timestamp.hour + end_timestamp.minute + end_timestamp.second):
            print(f"ERROR: Start-Timestamp in line {linenum+1} ({filename}) has is bigger than End-Timestamp")
            print(f"{start_timestamp} > {end_timestamp}")
            exit_helpfully()
            # TODO Add support for validation of timestamps that are longer then the input file
        e1.start = start_timestamp
        e1.end = end_timestamp
        entries.append(e1)
    return entries


def parse_timestamp(timestamp, linenum, col, other):
    SEGMENT_COUNT = 3
    segs = str.split(timestamp, ':')
    if (len(segs) > SEGMENT_COUNT):
        print(f"ERROR: Timestamp {timestamp} in line {linenum+1}, column {col} ({other}) has more than {SEGMENT_COUNT} elements")
        exit_helpfully()
    if (len(segs) < 2):
        print(f"ERROR: Timestamp {timestamp} in line {linenum+1}, column {col} ({other}) has less than {SEGMENT_COUNT} elements")
        exit_helpfully()
    if (len(segs) == 2):
        print(f"WARNING: Timestamp {timestamp} in line {linenum+1}, column {col} ({other}) only has 2 elements. Assuming missing hour segment.")
        print(f"Hour (HH) will be assumed as 00.")
        segs.insert(0, "00")
    # Validate numerics
    stamp = Timestamp("","","")
    for i, seg in enumerate(segs):
        name = ""
        if i == 0:
            name = "Hour"
        elif i == 1:
            name = "Minute"
        elif i == 2:
            name = "Second"
        seg = seg.strip('\n')
        # Validate with help messages
        if not seg.isdigit():
            print(f"ERROR @ Timestamp {timestamp} in line {linenum+1}, column {col} ({other}).")
            print(f"{name} {seg} contains non digit character")
        if len(seg) > 2:
            print(f"ERROR @ Timestamp {timestamp} in line {linenum+1}, column {col} ({other}).")
            print(f"{name} {seg} has more than two digits")
        if len(seg) == 1:
            print(f"Warning @ Timestamp {timestamp} in line {linenum+1}, column {col} ({other}).")
            print(f"{name} {seg} only has one digit. Assuming missing leading zero.")
            seg = "0" + seg
            print(f"Adding missing zero. New Value: {seg}")
        if i == 0:
            stamp.hour = seg
        elif i == 1:
            stamp.minute = seg
        elif i == 2:
            stamp.second = seg
    return stamp

def exit_helpfully():
    print_general_help()
    input("Press enter to exit")
    exit()


def print_general_help():
    print("Autocut Help\n")
    print_path_help()

    # Input file
    print("Input Video:")
    print("Supports most common video file formats like .mp4\n\n")

    # Out dir
    print("Output Dir:")
    print("Path to a directory in which the output files will be placed.\n\n")

    print_timestamp_file_help()

    # Timestamp help
    print("Timestamps:")
    print("Timestamps should be of shape HH:MM:SS where all values are digits in range 0-9.")
    print("Example: 01:55:23 -> This is equal to: 1 Hour 55 Minutes 23 Seconds")

def print_path_help():
    # Path help
    print("Paths:")
    print("Both relative and absolute (fully qualified) paths are supported.")
    print("A fully qualified path (in Windows) includes a drive letter as well as possible file extension.")
    print("Whitepace in paths should be escaped by adding double quotes around the path.")
    print("Example: \"E:\Videos\my folder with whitespace\my file with whitespace.mp4\"\n\n")


def print_timestamp_file_help():
    # Timestamp file
    print("Timestampfile:")
    print("Autocut supports timestamps files as .tsv or .csv")
    print("The file should be formated as follows:")
    print("Colum A:\tFilename")
    print("Column B:\tStarttimestamp")
    print("Column C:\tEndtimestamp")
    print("For more information refer to README and example files\n\n")

if __name__ == "__main__":
    main()
