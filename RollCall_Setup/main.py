import time
import requests
import tkinter as tk
from tkinter import messagebox, ttk
import threading

# 替换为你的GitHub仓库的API地址和当前程序的版本号
GITHUB_API_URL = "https://api.github.com/repos/MengdeUser/RollCall/releases/latest"
CURRENT_VERSION = "v1.0.1"
DOWNLOAD_URL = "https://gh-proxy.com/github.com/MengdeUser/RollCall/releases/download/{latest_version}/RollCall_Windows.exe"

def get_latest_version():
    try:
        response = requests.get(GITHUB_API_URL)
        response.raise_for_status()
        data = response.json()
        return data['tag_name']
    except requests.RequestException as e:
        print(f"获取最新版本时出错: {e}")
        return None

def check_for_updates():
    latest_version = get_latest_version()
    with open("version.txt", "w")as file:
        file.write(latest_version)
    if latest_version:
        if latest_version > CURRENT_VERSION:
            response = messagebox.askyesno("可用更新", f"新版本可用: {latest_version}. 是否要更新？")
            if response:
                download_and_install(latest_version)
            else:
                start_external_program()
        else:
            start_external_program()
    else:
        start_external_program()

def download_and_install(version):
    download_url = DOWNLOAD_URL.format(latest_version=version)
    download_window = tk.Toplevel(root)
    download_window.title("RollCall下载界面")
    # 获取屏幕宽度和高度
    screen_width = download_window.winfo_screenwidth()
    screen_height = download_window.winfo_screenheight()
    # 设置窗口宽度和高度
    window_width = 600
    window_height = 140
    # 计算窗口居中的坐标
    x = (screen_width - window_width) // 2
    y = (screen_height - window_height) // 2
    # 设置窗口位置
    download_window.geometry(f"{window_width}x{window_height}+{x}+{y}")
    download_window.iconbitmap('RollCall.ico')
    download_window.configure(bg='#303030')
    download_window.overrideredirect(True)
    download_window.attributes('-topmost', True)
    download_window.resizable(False, False)
    status_label = tk.Label(download_window, text="正在下载中，请稍后...", font=("HarmonyOS Sans SC Black", 24), background="#384651", foreground="#59A0D6")
    status_label.pack(pady=15)
    progress_bar = ttk.Progressbar(download_window, length=560)
    progress_bar.pack(pady=15)

    def download_file():
        response = requests.get(download_url, stream=True)
        total_size = int(response.headers.get('content-length', 0))
        block_size = 8192
        downloaded_size = 0
        with open(f"RollCall_Windows_{version}.exe", "wb") as f:
            for data in response.iter_content(block_size):
                downloaded_size += len(data)
                f.write(data)
                progress_bar["value"] = (downloaded_size / total_size) * 100
                status_label.config(text=f"下载中... {downloaded_size / 1024 / 1024:.2f} MB / {total_size / 1024 / 1024:.2f} MB")
                root.update_idletasks()
        status_label.config(text="下载完成！")
        time.sleep(5)
        start_external_program()

    threading.Thread(target=download_file).start()

def start_external_program():
    root.destroy()

root = tk.Tk()
root.withdraw()  # 隐藏主窗口
check_for_updates()
root.mainloop()
