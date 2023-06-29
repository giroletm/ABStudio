using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABStudio.FileFormats.DAT;
using ABStudio.FileFormats.ZSTREAM;
using ABStudio.Forms;

namespace ABStudio.Misc
{
    public static class Common
    {
        /*********/
        /* Lists */
        /*********/

        private static readonly string[] typeList = new string[] { "Sprite" };
        private static readonly string[] displayList = new string[] { "Spritesheet" };
        private static readonly Func<DATFile>[] createList = new Func<DATFile>[] { NewSpritesheetFile };
        private static readonly Action<string, DATFile>[] editorList = new Action<string, DATFile>[] { NewSpritesheetEditor };

        public static string[] DisplayNames { get => displayList; }


        /**************************/
        /* New & Editor functions */
        /**************************/

        public static DATFile NewSpritesheetFile()
        {
            return DATFile.NewSpriteData();
        }

        public static void NewSpritesheetEditor(string path, DATFile file)
        {
            SpritesheetEditor editor = new SpritesheetEditor(path, file);
            editor.Show();
        }


        /*********************/
        /* Utility functions */
        /*********************/

        public static DATFile OpenFile() { string str = ""; return OpenFile(ref str); }
        public static DATFile OpenFile(ref string originalPath)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Angry Birds Classic DAT Files|*.dat|Angry Birds Classic Sheets JSON Files|*.sheet.json|All files (*.*)|*.*";
                dialog.DefaultExt = "dat";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;

                    DATFile file = null;
                    try
                    {
                        if (path.EndsWith(".sheet.json", StringComparison.OrdinalIgnoreCase))
                        {
                            ZSTREAMFile zs = new ZSTREAMFile(path);
                            file = zs.associatedDAT;
                        }
                        else
                            file = new DATFile(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Couldn't load:"
                            + Environment.NewLine + "\"" + path + "\""
                            + Environment.NewLine
                            + Environment.NewLine + "Error:"
                            + Environment.NewLine + ex.ToString(),
                            "Couldn't load DAT file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return null;
                    }

                    originalPath = path;
                    return file;
                }
            }

            return null;
        }

        public static void OpenFileInNewEditor()
        {
            string path = "";
            DATFile file = OpenFile(ref path);

            if (file != null)
                OpenEditor(path, file);
        }

        public static void SaveFile(string path, DATFile file) => SaveFile(ref path, file);
        public static void SaveFile(ref string path, DATFile file)
        {
            if (path == "")
                SaveAsFile(ref path, file);
            else
                File.WriteAllBytes(path, path.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? file.AsJSONBytes : file.AsBytes);
        }
        

        public static bool SaveAsFile(DATFile file) { string str = ""; return SaveAsFile(ref str, file); }
        public static bool SaveAsFile(ref string originalPath, DATFile file)
        {
            SaveFileDialog dialog;
            dialog = new SaveFileDialog();
            dialog.Filter = "Angry Birds Classic DAT Files|*.dat|Angry Birds Classic Sheets JSON Files|*.sheet.json|All files (*.*)|*.*";
            dialog.DefaultExt = "dat";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                originalPath = dialog.FileName;
                SaveFile(ref originalPath, file);

                return true;
            }

            return false;
        }

        public static void OpenEditor(string path, DATFile file)
        {
            bool success = false;

            for (int i = 0; i < typeList.Length; i++)
            {
                if (file.Type == typeList[i])
                {
                    editorList[i].Invoke(path, file);
                    success = true;
                    break;
                }
            }

            if (!success)
                MessageBox.Show("No associated editor was found for DAT file type \"" + file.Type + "\".", "Couldn't open editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DATFile AskForNew()
        {
            using (ComboAskForm comboAskForm = new ComboAskForm(displayList, "Choose the type of file you want to create"))
                if (comboAskForm.ShowDialog() == DialogResult.OK)
                    return MakeNew(comboAskForm.ComboIndex);

            return null;
        }

        public static string AskForType()
        {
            using (ComboAskForm comboAskForm = new ComboAskForm(displayList, "Choose the type of the file you opened"))
                if (comboAskForm.ShowDialog() == DialogResult.OK)
                    return displayList[comboAskForm.ComboIndex];

            return null;
        }

        private static DATFile MakeNew(int id)
        {
            return createList[id].Invoke();
        }

        public static DATFile MakeNew(string displayName)
        {
            return MakeNew(Array.IndexOf(displayList, displayName));
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        // https://stackoverflow.com/a/13266776/9399492
        public static string MakeRelativePath(string filePath, string referencePath, bool useWindowsDirChar=true)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            string relativePath = Uri.UnescapeDataString(referenceUri.MakeRelativeUri(fileUri).ToString());
            if (useWindowsDirChar)
                relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);

            return relativePath;
        }
    }
}
