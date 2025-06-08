# 🎬 CaptionsConverter

A simple command-line tool that automatically detects and fixes subtitle file encoding issues (like `è`, `ð`, `æ`) and replaces them with proper characters (`č`, `đ`, `ć`). It also converts the file to UTF-8 encoding for universal compatibility.

> Designed for `.srt` subtitle files downloaded from the internet that display incorrect characters due to legacy encodings like Windows-1252.

---

## ✅ Features

- 🔍 Automatically detects character encoding (via [Ude](https://github.com/errepi/ude))
- 🔄 Converts to UTF-8
- 🛠️ Replaces common Western fallback characters with proper Serbian/Croatian letters
- 🗂 Supports batch processing of subtitle files in a folder

---

## 💾 Download

👉 [**Click here to download the latest version (.zip)**](https://github.com/your-username/CaptionsConverter/releases/latest)

> Unzip it and run the `CaptionsConverter.exe` in terminal or command prompt.

---

## 🛠 Usage

```bash
CaptionsConverter.exe "C:\Path\To\SubtitleFolder"
```

Optional second argument for file extension:
```bash
CaptionsConverter.exe "C:\MyFolder" "*.str"
```
---

## 🆘 Help Output
```
Usage: ./CaptionsConverter.exe <FolderPath> [file extension]
Files will be overwritten
Path must use / or \\ or "\"
Examples: C:/user/file.zip  or  C:\\user\\file.zip or "C:\user\file.zip"

Default for file extension is .srt
```

---

## ⚠ Notes
The program overwrites files in-place. Make a backup if needed.
