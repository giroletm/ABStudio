# AB Studio
A general purpose Angry Birds modding tool

## What is this?
AB Studio is a WIP tool that aims to be able to edit every single file format used in [Angry Birds](https://wikipedia.org/wiki/Angry_Birds) games.

For now, it only edits [AB Classic](https://en.wikipedia.org/wiki/Angry_Birds_(video_game)) spritesheets (in both DAT and SHEET.JSON formats).
Hence why windows say "AB Classic Studio" for now.

But hopefully, it'll one day support:
* Composprites
* Fonts
* Localization files
* More?

## Building

In order to build AB Studio, follow the following steps:
* Open the ``ABStudio.sln`` file in [Visual Studio](https://visualstudio.microsoft.com/) (both 2019 and 2022 were tested to work).
* Depending of your usage, either run the tool in debug mode using the play icon the top, or change the building mode from ``Debug`` to ``Release`` in the combo box at the left side of the play button, and open the ``Generate`` toolbox (from the toolbar on the top) and click on ``Generate solution``.
* You will find your built executable in the ``bin`` folder in the project's directory, in the folder that corresponds to your building mode.

## Code organization

The tool is split in various editors that can be opened at any time using an input file.

All editors are independent and can be opened in as many instances as you wish.

## Contributing

Feel free to fork the repo and write editors for file formats that aren't supported yet, or to make fixes to existing ones.

Then, send a pull request. If your changes are of good quality, I'll merge them into the repo.

However, please be careful to keep the code's organization clean. There's no point in writing new things if it takes twice the amount of time to clean it up afterwards.

## Licensing
Check the [LICENSE](https://github.com/giroletm/ABStudio/blob/main/LICENSE) file.

