using Microsoft.Win32;
using System.IO;

namespace KindleClippingsParser.Helpers
{
    public static class DiskIOHelper
    {
        #region Public methods

        public static void OpenTextFile(out bool? isFileSelected, out string fileName, string dialogTitle, string dialogFilter)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = dialogTitle;            
            dlg.DefaultExt = ".txt";
            dlg.Filter = dialogFilter;

            isFileSelected = dlg.ShowDialog();
            fileName = dlg.FileName;
        }

        public static string FindMyClippingsFile()
        {
            const string MY_CLIPPINGS_PATH = "documents\\My Clippings.txt";

            string fullPathToMyClippingsFile = null;

            //Search for disk with 'documents\My Clippings.txt' in root directory
            DriveInfo[] allSystemDrives = DriveInfo.GetDrives();
            
            foreach(DriveInfo di in allSystemDrives)
            {
                if (File.Exists(string.Format("{0}{1}", di.Name, MY_CLIPPINGS_PATH)))
                {
                    fullPathToMyClippingsFile = string.Format("{0}{1}", di.Name, MY_CLIPPINGS_PATH);
                }
            }

            return fullPathToMyClippingsFile;
        }

        #endregion Public methods
    }
}
