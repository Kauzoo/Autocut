import tkinter as tk
from tkinter import ttk
from tkinter import filedialog as fd
from tkinter.messagebox import showinfo
import autocut

TIMESTAMP_PATH = ""
INPUT_VIDEO_PATH = ""
OUTPUT_DIR = ""



TIMESTAMP_LIST = []
ENTRY_LIST = []

root = tk.Tk()
root.title("Autocout. Praise FFmpeg")
root.minsize(300, 300)  # width, height
root.geometry("300x300+50+50")

CURRENT_HOURS = tk.StringVar(root, 0)
CURRENT_MINUTES = tk.StringVar(root, 0)
CURRENT_SECONDS = tk.StringVar(root, 0)

CURRENT_NAME = tk.StringVar(root)


def select_timestamp_file():
    filetypes = (
        ('Tab Seperated Values', '*.tsv'),
        ('Comma Seperated Values', '*.csv'),
        ('All files', '*.*')
    )

    filename = fd.askopenfilename(
        title='Open a file',
        initialdir='/',
        filetypes=filetypes)

    showinfo(
        title='Selected File',
        message=filename
    )
    global TIMESTAMP_PATH
    TIMESTAMP_PATH = filename


def select_input_file():
    # TODO more file types
    filetypes = (
        ('Video files', '*.mp4'),
        ('All files', '*.*')
    )

    filename = fd.askopenfilename(
        title='Open a file',
        initialdir='/',
        filetypes=filetypes)

    showinfo(
        title='Selected File',
        message=filename
    )
    global INPUT_VIDEO_PATH
    INPUT_VIDEO_PATH = filename


def select_output_dir():
    filename = fd.askdirectory(
        title='Open a file',
        initialdir='/')

    showinfo(
        title='Selected Directory',
        message=filename
    )
    global OUTPUT_DIR
    OUTPUT_DIR = filename

def run():
    autocut.gui_entry(INPUT_VIDEO_PATH, TIMESTAMP_PATH, OUTPUT_DIR)
    showinfo(
        title='Finished',
        message="Segments have been extracted."
    )


# BUTTONS
# open timestamp button
open_timestamp_button = ttk.Button(
    root,
    text='Open Timestamp File',
    command=select_timestamp_file
)

# open input video button
open_video_button = ttk.Button(
    root,
    text='Open Video',
    command=select_input_file
)

open_dir_button = ttk.Button(
    root,
    text='Select Output Directory',
    command=select_output_dir
)

go_button = ttk.Button(
    root,
    text='Extract Segments',
    command=run
)



# TIMESTAMP INPUT WINDOW
def open_input_window():
    # Create secondary (or popup) window.
    timestamp_input_window = tk.Toplevel()
    timestamp_input_window.title("Create new timestamp")
    timestamp_input_window.config(width=300, height=200)

    timestamp_input_window.columnconfigure(0, weight=4)
    timestamp_input_window.columnconfigure(1, weight=1)

    input_frame = create_input_frame(timestamp_input_window)
    input_frame.grid(column=0, row=0)

    button_frame = create_button_frame(timestamp_input_window)
    button_frame.grid(column=1, row=0)


def confirm_timestamp_input_window():
    global TIMESTAMP_LIST
    global CURRENT_HOURS
    global CURRENT_MINUTES
    global CURRENT_SECONDS
    t = autocut.Timestamp(CURRENT_HOURS.get(), CURRENT_MINUTES.get(), CURRENT_SECONDS.get())
    TIMESTAMP_LIST.append(t)
    print(t)


def create_input_frame(container):
    global CURRENT_HOURS
    global CURRENT_MINUTES
    global CURRENT_SECONDS
    frame = ttk.Frame(container)

    # grid layout for the input frame
    frame.columnconfigure(0, weight=1)
    frame.columnconfigure(0, weight=2)

    # Hours
    ttk.Label(frame, text='Hours:').grid(column=0, row=0, sticky=tk.W)
    #CURRENT_HOURS = tk.StringVar(frame, 0)
    spin_box_hours = ttk.Spinbox(
    frame, from_=0, to=99, textvariable=CURRENT_HOURS, wrap=True)
    spin_box_hours.focus()
    spin_box_hours.grid(column=1, row=0, sticky=tk.W)

    # Minutes
    ttk.Label(frame, text='Minutes:').grid(column=0, row=1, sticky=tk.W)
    #CURRENT_MINUTES = tk.StringVar(frame, 0)
    spin_box_minutes = ttk.Spinbox(
    frame, from_=0, to=59, textvariable=CURRENT_MINUTES, wrap=True)
    spin_box_minutes.grid(column=1, row=1, sticky=tk.W)

    # Seconds
    ttk.Label(frame, text='Seconds:').grid(column=0, row=2, sticky=tk.W)
    #CURRENT_SECONDS = tk.StringVar(frame, 0)
    spin_box_seconds = ttk.Spinbox(
    frame, from_=0, to=59, textvariable=CURRENT_SECONDS, wrap=True)
    spin_box_seconds.grid(column=1, row=2, sticky=tk.W)


    for widget in frame.winfo_children():
        widget.grid(padx=5, pady=5)

    return frame


def create_button_frame(container):
    frame = ttk.Frame(container)

    frame.columnconfigure(0, weight=1)

    # ttk.Button(frame, text='Confirm Hours').grid(column=0, row=0)
    # ttk.Button(frame, text='Replace').grid(column=0, row=1)
    ttk.Button(frame, command=confirm_timestamp_input_window, text='Confirm').grid(column=0, row=2)
    ttk.Button(frame, command=container.destroy, text='Cancel').grid(column=0, row=3)

    for widget in frame.winfo_children():
        widget.grid(padx=5, pady=5)

    return frame


# CREATE ENTRY WINDOW
def open_input_window2():
    # Create secondary (or popup) window.
    entry_input_window = tk.Toplevel()
    entry_input_window.title("Create new timestamp")
    entry_input_window.config(width=300, height=200)

    entry_input_window.columnconfigure(0, weight=4)
    entry_input_window.columnconfigure(1, weight=1)

    input_frame2 = create_input_frame2(entry_input_window)
    input_frame2.grid(column=0, row=0)

    button_frame2 = create_button_frame2(entry_input_window)
    button_frame2.grid(column=1, row=0)

def create_input_frame2(container):
    global CURRENT_HOURS
    global CURRENT_MINUTES
    global CURRENT_SECONDS
    global CURRENT_NAME
    global TIMESTAMP_LIST

    TIMESTAMP_LIST.clear()
    TIMESTAMP_LIST.append(autocut.Timestamp("00", "00", "00"))
    TIMESTAMP_LIST.append(autocut.Timestamp("00", "00", "00"))
    frame = ttk.Frame(container)

    # grid layout for the input frame
    frame.columnconfigure(0, weight=1)
    frame.columnconfigure(0, weight=2)

    # Name
    ttk.Label(frame, text='Name:').grid(column=0, row=0, sticky=tk.W)
    name = ttk.Entry(frame, textvariable=CURRENT_NAME, width=30)
    name.focus()
    name.grid(column=1, row=0, sticky=tk.W)

    # Start
    ttk.Button(frame, command=open_input_window, text='Starttimestamp').grid(column=0, row=1, sticky=tk.W)
    ttk.Label(frame, text=TIMESTAMP_LIST[0]).grid(column=1, row=1, sticky=tk.W)

    # End
    ttk.Button(frame, command=open_input_window, text='Starttimestamp').grid(column=0, row=2, sticky=tk.W)
    ttk.Label(frame, text=TIMESTAMP_LIST[1]).grid(column=1, row=2, sticky=tk.W)

    for widget in frame.winfo_children():
        widget.grid(padx=5, pady=5)

    return frame

button_add_entry = ttk.Button(
    root,
    text="Add new entry",
    command=open_input_window2
)

# Create Label in our window
# text = Label(root, text="Nothing will work unless you do.")
# text.pack()
# text2 = Label(root, text="- Maya Angelou")
# text2.pack()
# fd.askopenfilename()


open_timestamp_button.pack(expand=True)
open_video_button.pack(expand=True)
open_dir_button.pack(expand=True)
go_button.pack(expand=True)
# button_add_entry.pack(expand=True)

# run application
root.mainloop()