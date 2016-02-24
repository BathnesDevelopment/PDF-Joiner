# PDF-Joiner
Joins PDF files into a single file and allows you to control the order in which the files are joined.

## Usage and history
The PDF Joiner was created from a demand to have a simple and easy to use tool without having to resort to either Internet options for joining PDF files (often not appropriate), or rolling out full versions of PDF software to users that did not otherwise require them.

## Instructions for use

On loading the application there are 5 buttons

### Add files
Displays a file picker, which is customised to show PDF files.  This supports multiple selections so a user could select multiple files within a partiucular folder.

### Remove
If an item is currently selected in the file list then this will remove that item from the list.

### Clear
Clears all the files in the list.  Useful for instances where 

### Up
When files are listed in the file list then the Up button will take any selected file and move it up one in the list.

### Down
When files are listed in the file list then the Down button will take any selected file and move it down one in the list.

### Join PDFs
Performs the join of the PDFs in the file list, in the order that they are currently listed.

## Instructions for build

The PDF Joiner has a number of dependencies that are managed by the NuGet Package manager.

On loading the solution in Visual Studio you should be load the missing packages and then build the solution.

## Deployment

At B&NES we are using Microsoft AppV for application virtualisation so this app has been deployed using that method.

## Third party tools

The solution uses a couple of third party tools:

### iTextSharp 5.5.8
Licence: GNU AFFERO GENERAL PUBLIC LICENSE v3
A copy of the licence is downloaded when you install the package.

### MahApps Metro 1.2.4.0
Licence: Microsoft Public License (Ms-PL)