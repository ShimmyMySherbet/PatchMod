# PatchMod
An Unturned Module that allows you to make scheduled rocket changes the next time the server restarts.

This module loads before RocketMod, so that it can access RocketMod's files before it loads.

It creates a sort of 'mirror' folder in your server folder. Any files/folders placed in this folder will be moved to rocket (overwriting where needed).

PatchMod also provides a file to schedule file deletions.


## Example 1:

<b>PatchMod Files:</b>

<ul><li>MyServer\PatchMod\Patch\Plugins\MyPlugin.dll</li></ul>

<b>RocketMod Files:</b>

<ul><li>MyServer\Rocket\Plugins\MyPlugin.dll</li></ul>

The next time the server starts, PatchMod will move MyPlugin.dll to Rocket Plugins, overwriting the current version. So this allows you to schedule plugin updates.

## Example 2:

<b>PatchMod Files:</b>

<ul><li>MyServer\PatchMod\Patch\NewFolder\File.txt</li></ul>

<b>RocketMod Files:</b>

<ul><li>N/A</li></ul>

This will create a folder named "NewFolder" in the rocket folder, then move "File.txt" into that folder.

## Example 3:

<b>PatchMod Files:</b>


<ul><li>MyServer\PatchMod\Patch\PatchMod.RemList.txt</li></ul>


<b>RocketMod Files:</b>

<ul>
<li>MyServer\Rocket\Plugins\MyPlugin\</li>
<li>MyServer\Rocket\Plugins\MyPlugin\MyPlugin.configuration.xml</li>
<li>Server\Rocket\Plugins\MyPlugin.dll</li>
</ul>

<b>PatchMod.RemList.txt Contents:</b>
<i>

#Put paths to file/folders to delete in here.<br>
Plugins\MyPlugin.dll<br>
Plugins\MyPlugin
</i>

This will delete MyPlugin.dll, and delete it's config file, including all the files within it.

The Remove List is cleared after each time it is used, so you don't have to worry about it double deleting.


