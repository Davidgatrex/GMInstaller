# GMInstaller
Unified installer for the Generally Mambo Suite (Work in progress)

## Introduction

### Mambo files
Mambo files (*.mambo) are manifest files for the Generally Mambo Installer (From now on, just "mambo installer"). They specify several things such as Name, Version, Remote URL, Compression method, Interaction with windows' registry keys, etc (Mambo file spec will be uploaded ASAP). Not every field is mandatory.

## Usage
### Installation, uninstallation and update of the installer
To install the installer, you just need to download the latest version from the "Releases" page, extract the Zip file and run the executable. Once the program is open, you should see an entry with the name "GMInstaller" in the list on the left. Click it and then click "Install" if you want to install it, "Uninstall" if you want to uninstall it (Can also be done directly through your current copy) or "Update" if you want to update your current copy. Once it's installed, you can delete the files you've just downloaded.

### Installation, uninstallation and update of programs
For programs, the process is very similar to the installer, with the only difference that you must download the mambo file and the compressed file (Optional. Only needed if the mambo doesn't declare the "URL" field. All of the official mambos have it, so you only have to check it for unofficial mambos. The way to check it is by trying to install it. If it says something like "File not found in disk. Downloading it from -URL-", it has the URL parameter and you don't have to do anything else. It will install unless there's an unexpected issue with the url or internet connection) that is within the same release and copy them into your "mambos" folder (If you don't have it, you must create one, wherever you want and with the name you want, and get into the installer, click the three dots next to "Mambo files directory path" and select the file or write it into the text box and click "Save Path" -> "Yes"). Then, open the installer (or click "Reload List" if it was already open), select the program in the list on the left and click the corresponding button ("Install", "Update" or "Uninstall")
