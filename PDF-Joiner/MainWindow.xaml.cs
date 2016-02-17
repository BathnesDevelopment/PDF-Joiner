using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

/// <summary>
/// PDF Joiner WPF Application.
/// </summary>
namespace PDF_Joiner
{
    /// <summary>
    /// The Main Window of the applications
    /// Written: DRowe, 16th February 2016
    /// Normally this application would be split out with some proper separation
    /// of concerns, but given it just takes some file paths for PDFs and merges the 
    /// files together there's no point in making it too complex.
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for user selecting to add file(s)
        /// Presents an open files dialog which allows for multiple select.
        /// Then adds the filenames into the files list box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Pdf Files|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames) lbFiles.Items.Add(new { Value = filename, Text = Path.GetFileName(filename) } );
            }
            lbFiles.DisplayMemberPath = "Text";
        }

        /// <summary>
        /// Event handler for user selecting to remove file(s)
        /// Removes the selected items from the list box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            while (lbFiles.SelectedItems.Count > 0) lbFiles.Items.Remove(lbFiles.SelectedItem);
        }

        /// <summary>
        /// Event handler for user selecting to clear the current files.
        /// Just runs the clear method on the files list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lbFiles.Items.Clear();
        }

        /// <summary>
        /// Event handler for the user clicking the Join PDFs button
        /// Shows a Save dialog to get the output file name.
        /// Then triggers the combine method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (lbFiles.Items.Count == 0)
            {
                await this.ShowMessageAsync("No files", "You need to add files to be joined.");
                return;
            }

            // Get list of files to combine.
            string[] fileList = new string[lbFiles.Items.Count];
            for (int x = 0; x < lbFiles.Items.Count; x++) fileList[x] = ((dynamic)lbFiles.Items[x]).Value;

            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Pdf Files|*.pdf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Get the path to save to.
                string filename = dlg.FileName;

                // Set the progress indicator.
                pgJoin.IsIndeterminate = true;

                // Run the combine
                CombineMultiplePDFs(fileList, filename);

                // Hide the progress indicator.
                pgJoin.IsIndeterminate = false;

                lbFiles.Items.Clear();
                
                // And give a small prompt
                await this.ShowMessageAsync("File saved", "File has been saved to: " + filename);
            }
        }

        /// <summary>
        /// Event handler for user clicking the Down button.
        /// Calls the MoveItem method with the relevant direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            MoveItem(1);
        }

        /// <summary>
        /// Event handler for user clicking the Up button.
        /// Calls the MoveItem method with the relevant direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            MoveItem(-1);
        }

        /// <summary>
        /// Moves the item in the list box.
        /// </summary>
        /// <param name="direction">The direction to move the item: -1 is Up, 1 is down.</param>
        private void MoveItem(int direction)
        {
            if (lbFiles.SelectedItem == null || lbFiles.SelectedIndex < 0) return;

            // Calculate new index using move direction
            int newIndex = lbFiles.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= lbFiles.Items.Count)
                return; // Index out of range - nothing to do

            object selected = lbFiles.SelectedItem;
            // Removing removable element
            lbFiles.Items.Remove(selected);
            // Insert it in new position
            lbFiles.Items.Insert(newIndex, selected);
            // Restore selection
            lbFiles.SelectedItem = lbFiles.Items[newIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="outFile"></param>
        public static void CombineMultiplePDFs(string[] fileNames, string outFile)
        {
            using (FileStream stream = new FileStream(outFile, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfCopy pdf = new PdfCopy(pdfDoc, stream);
                pdfDoc.Open();
                foreach (string file in fileNames) pdf.AddDocument(new PdfReader(file));
                if (pdfDoc != null) pdfDoc.Close();
            }
        }
    }
}