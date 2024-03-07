using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABStudio.Controls;
using ABStudio.FileFormats.DAT;
using ABStudio.FileFormats.PVR;
using ABStudio.Misc;
using StbRectPackSharp;

namespace ABStudio.Forms
{
    public partial class SpritesheetEditor : Form
    {
        private static readonly string Title = "AB Classic Spritesheet Editor";

        private string originalPath = "";
        private DATFile file = null;
        private DATFile.SpriteData data = null;
        private Bitmap spritesheet = null;
        private float zoom = 1.0f;
        private object SelectedObj => spritesheetPictureBox.SelectedSRect;
        private DATFile.SpriteData.Sprite SelectedSprite => spritesheetPictureBox.GetSRectLinkedObject(SelectedObj) as DATFile.SpriteData.Sprite;

        private bool legacyPVR = false;
        private string pvrFormat = "";

        #region Extensions management

        private static readonly string[] supportedPicExt = new string[] { "pvr", "png", "jpg", "gif", "bmp", "tiff" };
        private static readonly string[] supportedPicName = new string[] { "PowerVR Image File", "Portable Network Graphics", "Joint Photographic Experts Group", "Graphics Interchange Format", "Bitmap Image file", "Tagged Image File Format" };

        private static string GenerateSaveExtension(bool allowPVR)
        {
            string ext = "";

            for(int i = (allowPVR ? 0 : 1); i < supportedPicExt.Length; i++)
            {
                ext += supportedPicName[i] + " (*." + supportedPicExt[i]  + ")| *." + supportedPicExt[i] + "|";
            }

            return ext + "All files (*.*)|*.*";
        }

        private static string[] GenerateSaveExtensionTable(bool allowPVR)
        {
            List<string> exts = new List<string>();

            for(int i = (allowPVR ? 0 : 1); i < supportedPicExt.Length; i++)
            {
                exts.Add(supportedPicName[i] + " (*." + supportedPicExt[i]  + ")");
            }

            return exts.ToArray();
        }

        #endregion

        #region Constructor

        public SpritesheetEditor(string path, DATFile file)
        {
            spritesheet = new Bitmap(1, 1);

            InitializeComponent();

            originalPath = path;
            ChangeFile(file);
        }

        #endregion

        #region Form triggers

        private void RefreshTitle()
        {
            this.Text = Title + " - " + ((originalPath == "") ? "New file" : Path.GetFileName(originalPath));
        }

        private void SpritesheetEditor_Load(object sender, EventArgs e)
        {
        }

        private void SpritesheetEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && spritesheetPictureBox.Focused)
            {
                spritesheetPictureBox_KeyPressDel(sender, e);
                e.Handled = true;
            }
        }

        private void SpritesheetEditor_Resize(object sender, EventArgs e)
        {
            RefreshZoom();
        }

        #endregion

        #region Menu Strip triggers

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFile(Common.NewSpritesheetFile());
        }

        private void openHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DATFile file = Common.OpenFile(ref originalPath);

            if (file != null)
                ChangeFile(file);
        }

        private void openInNewInstanceToolStripMenuItem_Click(object sender, EventArgs e) => Common.OpenFileInNewEditor();

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CanBeSaved())
                return;

            Common.SaveFile(ref originalPath, file);

            if(!originalPath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                data.associatedZSTREAM = null;
            }

            if (!SaveSpritesheet())
                return;

            RefreshTitle();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CanBeSaved())
                return;

            if (!Common.SaveAsFile(ref originalPath, file))
                return;

            if (!SaveSpritesheet())
                return;

            RefreshTitle();
        }

        private bool CanBeSaved()
        {
            string ext = Path.GetExtension(data.filenames[0]).ToLower();

            bool isJson = originalPath.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
            bool isJsonStreamCombo = isJson && (ext == ".stream");

            if (data.filenames[0] == null || data.filenames[0] == "" || ext == "")
            {
                MessageBox.Show("Please write a filename for the picture to save with your spritesheet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (supportedPicExt.ToList().IndexOf(ext.Substring(1)) < 0 && !isJsonStreamCombo)
            {
                MessageBox.Show("Unsupported extention \"" + ext + "\", please use a supported extension like " + (isJson ? "STREAM, " : "") + "PVR or PNG.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool SaveSpritesheet()
        {
            string ext = Path.GetExtension(data.filenames[0]).ToLower();
            string fname = Path.GetDirectoryName(originalPath) + Path.DirectorySeparatorChar + data.filenames[0];

            if (ext == ".pvr")
            {
                if (this.pvrFormat == "")
                {
                    bool chosen = false;

                    while (!chosen)
                    {
                        string[] formats = new string[] { "RGBA4444", "RGBA8888", "RGB565" };
                        using (MCQAskForm mcqAskForm = new MCQAskForm(formats, "PVR image format"))
                        {
                            if (mcqAskForm.ShowDialog() == DialogResult.OK)
                            {
                                string fmt = formats[mcqAskForm.ChosenAnswer].ToLower();
                                int half = fmt.Length / 2;

                                char ch1 = (char)((0 < half) ? fmt[0] : 0);
                                char ch2 = (char)((1 < half) ? fmt[1] : 0);
                                char ch3 = (char)((2 < half) ? fmt[2] : 0);
                                char ch4 = (char)((3 < half) ? fmt[3] : 0);
                                char va1 = (char)((half < fmt.Length) ? fmt[half] : 0);
                                char va2 = (char)(((half + 1) < fmt.Length) ? fmt[half + 1] : 0);
                                char va3 = (char)(((half + 2) < fmt.Length) ? fmt[half + 2] : 0);
                                char va4 = (char)(((half + 3) < fmt.Length) ? fmt[half + 3] : 0);

                                this.pvrFormat = new string(new char[] { ch1, va1, ch2, va2, ch3, va3, ch4, va4 });

                                chosen = true;
                            }
                        }
                    }

                    chosen = false;

                    while (!chosen)
                    {
                        string[] answers = new string[] { "No", "Yes" };
                        using (MCQAskForm mcqAskForm = new MCQAskForm(answers, "Save PVR in Legacy form?"))
                        {
                            if (mcqAskForm.ShowDialog() == DialogResult.OK)
                            {
                                this.legacyPVR = (mcqAskForm.ChosenAnswer == 1) ? true : false;
                            }
                        }
                    }
                }

                PVRFile pvr = new PVRFile(spritesheet, this.pvrFormat);
                pvr.isLegacy = this.legacyPVR;
                pvr.Save(fname);
            }
            else if(ext == ".stream")
            {
                data.associatedZSTREAM.SaveBitmap(spritesheet, fname);
            }
            else
            {
                spritesheet.Save(fname);
            }

            return true;
        }

        private void importSpritesheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = GenerateSaveExtension(true);
                dialog.DefaultExt = (originalPath == "" || data.filenames.Count < 1 || data.filenames[0] == "") ? "pvr" : Path.GetExtension(data.filenames[0]).Substring(1);
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadBitmap(dialog.FileName);

                    filenameTextBox.Text = Path.GetFileName(dialog.FileName);
                }
            }
        }

        private void importSpritesFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderPicker dialog = new FolderPicker();
            if (dialog.ShowDialog() == true)
            {
                MCQAskForm maf = new MCQAskForm(new string[] { "Insert them", "Overwrite existing ones", "Don't import them" }, "In which way do you want to import borders of the imported sprites?");
                if(maf.ShowDialog() == DialogResult.OK)
                {
                    Packer packer = new Packer(256, 256);

                    List<Bitmap> bmps = new List<Bitmap>();
                    List<string> names = new List<string>();
                    foreach(string file in Directory.GetFiles(dialog.ResultPath))
                    {
                        if (supportedPicExt.ToList().IndexOf(Path.GetExtension(file).Substring(1)) < 0)
                            continue;

                        bmps.Add(new Bitmap(file));
                        names.Add(Path.GetFileNameWithoutExtension(file));
                    }

                    foreach (Bitmap bmp in bmps) {
                        Packer.PackRectForce(ref packer, bmp.Width + 4, bmp.Height + 4, bmp);
                    }

                    int left = int.MaxValue;
                    int top = int.MaxValue;
                    int right = int.MinValue;
                    int bottom = int.MinValue;

                    Bitmap full = new Bitmap(packer.Width, packer.Height);
                    using (Graphics g = Graphics.FromImage(full))
                    {
                        foreach (PackerRectangle rect in packer.PackRectangles)
                        {
                            if (rect.X < left)
                                left = rect.X;
                            if (rect.Y < top)
                                top = rect.Y;
                            if ((rect.X + rect.Width) > right)
                                right = rect.X + rect.Width;
                            if ((rect.Y + rect.Height) > bottom)
                                bottom = rect.Y + rect.Height;

                            Bitmap bmp = rect.Data as Bitmap;
                            Rectangle mainRect = new Rectangle(rect.X + 2, rect.Y + 2, bmp.Width, bmp.Height);
                            g.DrawImage(bmp, mainRect, new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

                            g.DrawImage(bmp, new Rectangle(rect.X+1, rect.Y+2, 1, bmp.Height), new Rectangle(0, 0, 1, bmp.Height), GraphicsUnit.Pixel);
                            g.DrawImage(bmp, new Rectangle(rect.X+bmp.Width+2, rect.Y+2, 1, bmp.Height), new Rectangle(bmp.Width-1, 0, 1, bmp.Height), GraphicsUnit.Pixel);

                            g.DrawImage(bmp, new Rectangle(rect.X+2, rect.Y+1, bmp.Width, 1), new Rectangle(0, 0, bmp.Width, 1), GraphicsUnit.Pixel);
                            g.DrawImage(bmp, new Rectangle(rect.X+2, rect.Y+bmp.Height+2, bmp.Width, 1), new Rectangle(0, bmp.Height-1, bmp.Width, 1), GraphicsUnit.Pixel);
                        }
                    }

                    full = full.Clone(new Rectangle(left, top, right - left, bottom - top), full.PixelFormat);

                    if (full.Width > 2048 || full.Height > 2048)
                    {
                        MessageBox.Show("Couldn't fit your sprites in a 2048x2048 spritesheet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (maf.ChosenAnswer == 1)
                        data.sprites.Clear();

                    if (maf.ChosenAnswer != 2)
                    {
                        foreach (PackerRectangle rect in packer.PackRectangles)
                        {
                            DATFile.SpriteData.Sprite sprite = new DATFile.SpriteData.Sprite();

                            Bitmap bmp = rect.Data as Bitmap;

                            sprite.name = names[bmps.IndexOf(rect.Data as Bitmap)];
                            sprite.rect = new Rectangle(rect.X + 2 - left, rect.Y + 2 - top, bmp.Width, bmp.Height);
                            sprite.orig = new Point(sprite.rect.Width / 2, sprite.rect.Height / 2);

                            data.sprites.Add(sprite);
                        }
                    }


                    spritesheet = full;
                    spritesheetPictureBox.Image = spritesheet;
                    RefreshRects();
                    RefreshZoom();

                    filenameTextBox.Text = Path.GetFileName(dialog.ResultPath) + ".png";
                }
            }
        }

        private void exportSpritesheetImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog;
            dialog = new SaveFileDialog();
            dialog.Filter = GenerateSaveExtension(false);// "Portable Network Graphics|*.png|Joint Photographic Experts Group|*.jpg|Graphics Interchange Format|*.gif|Bitmap Image file|*.bmp|Tagged Image File Format|*.tiff|All files (*.*)|*.*";
            dialog.DefaultExt = supportedPicExt[1];
            dialog.FileName = Path.GetFileNameWithoutExtension(originalPath) + "." + supportedPicExt[1];
            if (dialog.ShowDialog() == DialogResult.OK)
                spritesheet.Save(dialog.FileName);
        }

        private void exportCroppedSpritesheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderPicker dialog = new FolderPicker();
            if (dialog.ShowDialog() == true)
            {

                ComboAskForm caf = new ComboAskForm(GenerateSaveExtensionTable(false), "In which format do you want to save the cropped spritesheet?");
                if (caf.ShowDialog() == DialogResult.OK)
                {
                    foreach (DATFile.SpriteData.Sprite sprite in data.sprites)
                    {
                        if (sprite.rect.Width < 1 || sprite.rect.Height < 1)
                            continue;

                        Bitmap bmp = new Bitmap(sprite.rect.Width, sprite.rect.Height);

                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.DrawImage(spritesheet, new Rectangle(0, 0, sprite.rect.Width, sprite.rect.Height), sprite.rect, GraphicsUnit.Pixel);
                        }

                        bmp.Save(dialog.ResultPath + Path.DirectorySeparatorChar + sprite.name + "." + supportedPicExt[caf.ComboIndex+1]);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void clearAllSpritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spritesheetPictureBox.ClearSRects();
            data.sprites.Clear();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AB Studio Spritesheets Editor"
                + Environment.NewLine + "By RSM (rsm4618 @ Discord)"
                + Environment.NewLine
                + Environment.NewLine + "Libraries used:"
                + Environment.NewLine + "- PVRTexLib"
                + Environment.NewLine + "- CppSharp"
                + Environment.NewLine + "- Newtonsoft.Json"
                + Environment.NewLine + "- Aspose.Zip"
                + Environment.NewLine
                + Environment.NewLine + "Documentation:"
                + Environment.NewLine + "- RizDub"
                + Environment.NewLine + "- AB360"
                + Environment.NewLine
                + Environment.NewLine + "Special thanks:"
                + Environment.NewLine + "- AB Series Modding Hub Discord Server"
                + Environment.NewLine + "- Hidden birds",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Reloading & Refreshing utility functions

        private void ChangeFile(DATFile file)
        {
            RefreshTitle();
            this.file = file;
            this.data = file.GetAsSpriteData();
            LoadBitmap();
            RefreshRects();
            RefreshLocalSprite();

            while (data.filenames.Count < 1)
                data.filenames.Add("");

            filenameTextBox.Text = data.filenames[0];
        }

        private void LoadBitmap(string path=null)
        {
            if (path == "")
                path = null;

            bool hasSpecifiedPath = path != null;
            legacyPVR = false;
            pvrFormat = "";

            if (data.filenames.Count <= 0 && !hasSpecifiedPath)
            {
                spritesheet = new Bitmap(4, 4);
            }
            else if(data.filenames.Count == 1 || hasSpecifiedPath)
            {
                string fullPath = (hasSpecifiedPath) ? path : (Path.GetDirectoryName(originalPath) + Path.DirectorySeparatorChar + data.filenames[0]);
                string ext = Path.GetExtension(fullPath).ToLower();
                if(ext == ".pvr")
                {
                    PVRFile pvr = new PVRFile(fullPath);
                    legacyPVR = pvr.isLegacy;
                    pvrFormat = pvr.GetFormatStr();
                    spritesheet = pvr.AsBitmap();
                }
                else if(fullPath.EndsWith(".stream") || ext == ".zstream")
                {
                    if (data.associatedZSTREAM != null)
                    {
                        spritesheet = data.associatedZSTREAM.GetBitmap(fullPath);
                        pvrFormat = data.associatedZSTREAM.GetFormatAsPVR();
                        legacyPVR = false;
                    }
                    else
                        throw new Exception("Non-JSON sheets can't have a stream or zstream texture");
                }
                else
                {
                    Bitmap tmp = new Bitmap(fullPath);
                    spritesheet = new Bitmap(tmp);
                    tmp.Dispose();
                }

                if (data.associatedZSTREAM != null)
                    data.associatedZSTREAM.UpdateBitmap(spritesheet);
            }
            else
            {
                MessageBox.Show("Multi-spritesheet DAT files aren't supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            spritesheetPictureBox.Image = spritesheet;
            RefreshZoom();
        }

        private void RefreshZoom()
        {
            float w = spritesheet.Width * zoom;
            float h = spritesheet.Height * zoom;

            insideViewPanel.Size = new Size(Math.Max((int)Math.Round(w * 1.1f), imageViewPanel.Width - 6), Math.Max((int)Math.Round(h * 1.1f), imageViewPanel.Height - 6));
            spritesheetPictureBox.Location = new Point((int)Math.Round((insideViewPanel.Size.Width / 2.0f) - (w / 2.0f)), (int)Math.Round((insideViewPanel.Size.Height / 2.0f) - (h / 2.0f)));
            spritesheetPictureBox.Size = new Size((int)Math.Round(w), (int)Math.Round(h));
        }

        private void RefreshRects()
        {
            spritesheetPictureBox.ClearSRects();
            
            foreach(DATFile.SpriteData.Sprite sprite in data.sprites)
            {
                spritesheetPictureBox.AddSRect(sprite.rect, sprite);
            }
        }

        private void RefreshLocalSprite()
        {
            bool isSelected = SelectedObj != null;
            selectedSpriteGroupBox.Enabled = isSelected;

            if (isSelected)
            {
                DATFile.SpriteData.Sprite sprite = SelectedSprite;
                spriteNameTextBox.Text = sprite.name;
                spriteRectXNumUpDown.Value = sprite.rect.X;
                spriteRectYNumUpDown.Value = sprite.rect.Y;
                spriteRectWNumUpDown.Value = sprite.rect.Width;
                spriteRectHNumUpDown.Value = sprite.rect.Height;
                spriteOrigPtXNumUpDown.Value = sprite.orig.X;
                spriteOrigPtYNumUpDown.Value = sprite.orig.Y;
            }
        }

        private void RefreshSelectedRect()
        {
            if (SelectedObj != null)
                spritesheetPictureBox.EditSRect(SelectedObj, SelectedSprite.rect);
        }

        #endregion

        #region Image view panel triggers

        private void imageViewPanel_CTRLMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (zoom < 10.0f)
                    zoom /= 0.9f;
                else
                    return;
            }
            else
            {
                if (imageViewPanel.HorizontalScroll.Visible || imageViewPanel.VerticalScroll.Visible)
                    zoom *= 0.9f;
                else
                    return;
            }

            int viewportWidthBefore = imageViewPanel.Width - (imageViewPanel.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
            int viewportHeightBefore = imageViewPanel.Height - (imageViewPanel.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);

            double scrollbarLeftRatio = (double)imageViewPanel.HorizontalScroll.Value / (double)imageViewPanel.HorizontalScroll.Maximum;
            double scrollbarTopRatio = (double)imageViewPanel.VerticalScroll.Value / (double)imageViewPanel.VerticalScroll.Maximum;
            int scrollbarRightPos = (imageViewPanel.HorizontalScroll.Value + viewportWidthBefore);
            double scrollbarRightRatio = (double)(imageViewPanel.HorizontalScroll.Value + viewportWidthBefore) / (double)imageViewPanel.HorizontalScroll.Maximum;
            int scrollbarTopPos = (imageViewPanel.VerticalScroll.Value + viewportHeightBefore);
            double scrollbarBottomRatio = (double)(imageViewPanel.VerticalScroll.Value + viewportHeightBefore) / (double)imageViewPanel.VerticalScroll.Maximum;

            RefreshZoom();
            imageViewPanel.PerformLayout();

            int viewportWidthAfter = imageViewPanel.Width - (imageViewPanel.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
            int viewportHeightAfter = imageViewPanel.Height - (imageViewPanel.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);

            int zoomLeft = (int)Math.Round(imageViewPanel.HorizontalScroll.Maximum * scrollbarLeftRatio);
            int zoomTop = (int)Math.Round(imageViewPanel.VerticalScroll.Maximum * scrollbarTopRatio);

            int goRight = (int)Math.Round(imageViewPanel.HorizontalScroll.Maximum * scrollbarRightRatio) - zoomLeft - viewportWidthBefore;
            int goDown = (int)Math.Round(imageViewPanel.VerticalScroll.Maximum * scrollbarBottomRatio) - zoomTop - viewportHeightBefore;

            int zoomFinalX = (int)Math.Round(zoomLeft + (goRight * ((double)e.X / (double)viewportWidthBefore)));
            int zoomFinalY = (int)Math.Round(zoomTop + (goDown * ((double)e.Y / (double)viewportHeightBefore)));

            if (zoomFinalX < 0)
                zoomFinalX = 0;
            if (zoomFinalX > imageViewPanel.HorizontalScroll.Maximum)
                zoomFinalX = imageViewPanel.HorizontalScroll.Maximum;

            if (imageViewPanel.HorizontalScroll.Visible)
                imageViewPanel.HorizontalScroll.Value = zoomFinalX;

            if (zoomFinalY < 0)
                zoomFinalY = 0;
            if (zoomFinalY > imageViewPanel.VerticalScroll.Maximum)
                zoomFinalY = imageViewPanel.VerticalScroll.Maximum;

            if (imageViewPanel.VerticalScroll.Visible)
                imageViewPanel.VerticalScroll.Value = zoomFinalY;

            imageViewPanel.PerformLayout();

        }

        private void imageViewPanel_Scroll(object sender, ScrollEventArgs e)
        {
        }

        #endregion

        #region Spritesheet picturebox triggers

        private void spritesheetPictureBox_RectResize(object sender, PictureBoxDB.SizableRectEventArgs e)
        {
            DATFile.SpriteData.Sprite sprite = e.LinkedObject as DATFile.SpriteData.Sprite;
            sprite.rect = e.Rect;

            RefreshLocalSprite();
        }

        private void spritesheetPictureBox_SRectCreated(object sender, PictureBoxDB.SizableRectEventArgs e)
        {
            DATFile.SpriteData.Sprite sprite = new DATFile.SpriteData.Sprite();
            sprite.name = "";
            sprite.rect = e.Rect;
            sprite.orig = new Point(sprite.rect.Width / 2, sprite.rect.Height / 2);
            data.sprites.Add(sprite);

            spritesheetPictureBox.SetSRectLinkedObject(e.SRect, sprite);
        }

        private void spritesheetPictureBox_Click(object sender, EventArgs e)
        {
            (sender as Control).Focus();
        }

        private void spritesheetPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if(SelectedObj != null)
            {
                Point pt = spritesheetPictureBox.ConvertPicturePointToControlPoint(SelectedSprite.orig);
                Point pt2 = spritesheetPictureBox.ConvertPicturePointToControlPoint(SelectedSprite.rect.Location);
                e.Graphics.DrawRectangle(new Pen(Color.Red), new Rectangle(pt2.X + pt.X - 2, pt2.Y + pt.Y - 2, 5, 5));
            }
        }

        private void spritesheetPictureBox_SelectedSRectChanged(object sender, EventArgs e)
        {
            RefreshLocalSprite();
        }

        private void spritesheetPictureBox_KeyPressDel(object sender, KeyEventArgs e)
        {
            if (SelectedObj != null)
            {
                data.sprites.Remove(SelectedSprite);
                spritesheetPictureBox.RemoveSRect(SelectedObj);
                RefreshLocalSprite();
            }
        }

        #endregion

        #region Manual rectangle edit controls triggers

        private void filenameTextBox_TextChanged(object sender, EventArgs e)
        {
            while (data.filenames.Count < 1)
                data.filenames.Add("");

            data.filenames[0] = filenameTextBox.Text;
        }

        private void reloadPictureButton_Click(object sender, EventArgs e)
        {
            if (filenameTextBox.Text == "")
                MessageBox.Show("No path to reload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                LoadBitmap();
        }
        private void spriteNameTextBox_TextChanged(object sender, EventArgs e)
        {
            SelectedSprite.name = spriteNameTextBox.Text;
        }

        private void spriteRectXNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedSprite.rect.X = (int)spriteRectXNumUpDown.Value;
            RefreshSelectedRect();
        }

        private void spriteRectYNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedSprite.rect.Y = (int)spriteRectYNumUpDown.Value;
            RefreshSelectedRect();
        }

        private void spriteRectWNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedSprite.rect.Width = (int)spriteRectWNumUpDown.Value;
            //spriteOrigPtXNumUpDown.Maximum = SelectedSprite.rect.Width - 1;
            RefreshSelectedRect();
        }

        private void spriteRectHNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedSprite.rect.Height = (int)spriteRectHNumUpDown.Value;
            //spriteOrigPtYNumUpDown.Maximum = SelectedSprite.rect.Height - 1;
            RefreshSelectedRect();
        }

        private void spriteOrigPtXNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedSprite.orig.X = (int)spriteOrigPtXNumUpDown.Value;
            RefreshSelectedRect();
        }

        private void spriteOrigPtYNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedSprite.orig.Y = (int)spriteOrigPtYNumUpDown.Value;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapCButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = SelectedSprite.rect.Width / 2;
            spriteOrigPtYNumUpDown.Value = SelectedSprite.rect.Height / 2;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapTLButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = 0;
            spriteOrigPtYNumUpDown.Value = 0;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapTButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = SelectedSprite.rect.Width / 2;
            spriteOrigPtYNumUpDown.Value = 0;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapTRButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = SelectedSprite.rect.Width - 1;
            spriteOrigPtYNumUpDown.Value = 0;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapRButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = SelectedSprite.rect.Width - 1;
            spriteOrigPtYNumUpDown.Value = SelectedSprite.rect.Height / 2;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapBRButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = SelectedSprite.rect.Width - 1;
            spriteOrigPtYNumUpDown.Value = SelectedSprite.rect.Height - 1;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapBButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = SelectedSprite.rect.Width / 2;
            spriteOrigPtYNumUpDown.Value = SelectedSprite.rect.Height - 1;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapBLButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = 0;
            spriteOrigPtYNumUpDown.Value = SelectedSprite.rect.Height - 1;
            RefreshSelectedRect();
        }

        private void spriteOrigPtMapLButton_Click(object sender, EventArgs e)
        {
            spriteOrigPtXNumUpDown.Value = 0;
            spriteOrigPtYNumUpDown.Value = SelectedSprite.rect.Height / 2;
            RefreshSelectedRect();
        }

        #endregion
    }
}
