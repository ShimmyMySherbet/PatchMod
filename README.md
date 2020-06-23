# PatchMod
An Unturned Module that allows you to sync RocketMod files between servers, and schedule changes for the next time the server starts.

This module loads before RocketMod, so that it can access RocketMod's files before it loads.

Features:
<ul>
  <li>Corss-Server Rocket Syncing</li>
  <li>Remote FTP Corss-Server Syncing</li>
  <li>Sync Exclusions System</li>
  <li>Platform to create custom sync soruces (e.g., from an SQL Database)</li>
  <li>Schedule file changes and/or deletions for the next server restart.</li>
</ul>

# OpenMod

Support for OpenMod is planned once it is released.

# Syncing

PatchMod provides a feature to sync RocketMod files between servers. This can be enabled and configured in Config.ini. When the server starts, and before RocketMod loads, PatchMod will sync files from the specified source. It will create new files, and update existing ones if they are different from the sync source files.

PatchMod has support to sync from a local directory, or remotely via FTP.

For both scheduled changes, and file syncing, the source folder is a mirror of the Rocket folder in your server. Meaning, a file placed in SyncSource\Plugins\ will be synced into Rocket's plugin folder.

## Sync Configuration

To sync from a specified location, whether it is a local directory or remote FTP server, you use the <i>SyncPath</i> field in the config to set where to sync from.

For a local directory, this value will be the absolute path to this folder. For FTP, it will be the remote path to the folder to sync from on the FTP server.

To enable Syncing, set <i>SyncEnabled</i> in the config to True.

To change between Local, FTP, or a custom Sync Source, set <i>SyncMode</i> to the name of that source. For a local folder it is <i>Local</i>. and for FTP it is <i>FTP</i>.

Top configure FTP, set <i>FTPHost</i> to the IP/Address of your FTP server. If your FTP server uses a port other than the FTP default, set this port in <i>FTPPort</i>. If the server requires authentication, set the Username and Password to the server in <i>FTPUsername</i> and <i>FTPPassword</i>. If you are syncing from the root directory of the FTP server, leave <i>SyncPath</i> blank. Otherwise set this value to the path to the folder you want to sync from.

If you are using a custom sync source, set this field to the name of the source. Custom Sync Sources will typically use Config.ini to store their settings. These options will appear below the PatchMod settings.

### Sync Exclusions

You can exclude files and folders from being synced in '<i>.SyncExclude</i>', located in the PatchMod folder of your server. Files/Folders placed in here will not be synced from the server, this includes local syncing, FTP syncing and custom syncing.

# Schedule Changes

PatchMod also has the means to schedule file changes for the next time the server starts. This works similarly to the Syncing feature listed above, however, it deleted the files after they have been updated in the rocket folder. This is useful to schedule plugin updates for the next time the server restarts, rather than having to restart the server.

You can find this folder under MyServer\PatchMod\Patch

You will already find it has 2 folders, Plugins and Libraries. This is to make it easier to schedule file changes in these Rocketmod folders. If you want to schedule changes in another folder, just create the folders in here. When it patches the Rocket folder, it will push all files and folders in this Patch folder to the Rocket folder. This includes subdirectories and their files.

You will also notice a file in the Patch folder called '<i>patchmod.remlist.txt</i>'. Use this file to schedule deletions. E.g, to delete UEssentials from the server, in that file, put '<i>Plugins\UEssentials.dll</i>'. Here, the next time the server restarts, UEssentials will be deleted. Each entry is placed on a new line. You can also delete folders with this.

## Downloads

See <a href="https://github.com/ShimmyMySherbet/PatchMod/releases">Release</a> for downloads.
