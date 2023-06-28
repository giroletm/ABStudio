using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace ABStudio.Controls
{
    public class PictureBoxDB : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }
        public bool AllowSRectCreation { get; set; } = true;
        public event EventHandler<SizableRectEventArgs> RectResize;
        public event EventHandler SelectedSRectChanged;
        public event EventHandler<SizableRectEventArgs> SRectCreated;
        public object SelectedSRect { 
            get => selectedSRect;
            set
            {
                selectedSRect = value as SizableRect;
                RaiseSelectedSRectChanged();
                this.Invalidate();
            }
        }

        private List<SizableRect> srects = new List<SizableRect>();
        private SizableRect selectedSRect = null;

        public PictureBoxDB() : base()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.DoubleBuffered = true;
            this.UpdateStyles();

            InterpolationMode = InterpolationMode.Default;
        }

        private void RaiseRectResize(SizableRect srect)
        {
            if (RectResize != null)
            {
                SizableRectEventArgs srea = new SizableRectEventArgs(srect, srect.rect, srect.LinkedObject);
                RectResize.Invoke(this, srea);
            }
        }

        private void RaiseSelectedSRectChanged()
        {
            if (SelectedSRectChanged != null)
                SelectedSRectChanged.Invoke(this, EventArgs.Empty);
        }

        private void RaiseSRectCreated(SizableRect srect)
        {
            if (SRectCreated != null)
            {
                SizableRectEventArgs srea = new SizableRectEventArgs(srect, srect.rect, srect.LinkedObject);
                SRectCreated.Invoke(this, srea);
            }
        }

        public object AddSRect(Rectangle rect, object linkedObject=null)
        {
            SizableRect srect = new SizableRect(rect, this);
            srect.LinkedObject = linkedObject;

            srects.Add(srect);

            this.Invalidate();

            return srect;
        }

        public void EditSRect(object srect, Rectangle rect)
        {
            (srect as SizableRect).rect = rect;

            this.Invalidate();
        }

        public void RemoveSRect(object srect)
        {
            SizableRect sizableRect = srect as SizableRect;
            srects.Remove(sizableRect);

            if (SelectedSRect == sizableRect)
                SelectedSRect = null;

            this.Invalidate();
        }

        public void ClearSRects()
        {
            srects.Clear();
            SelectedSRect = null;

            this.Invalidate();
        }

        public object GetSRectLinkedObject(object srect)
        {
            return (srect as SizableRect).LinkedObject;
        }

        public void SetSRectLinkedObject(object srect, object linkedObject)
        {
            (srect as SizableRect).LinkedObject = linkedObject;
        }

        public Point ConvertControlPointToPicturePoint(Point pt)
        {
            if (this.Image == null)
                return pt;

            int w = (int)Math.Round((pt.X * this.Image.Width) / (double)this.Width);
            int h = (int)Math.Round((pt.Y * this.Image.Height) / (double)this.Height);

            return new Point(w, h);
        }

        public Point ConvertPicturePointToControlPoint(Point pt)
        {
            if (this.Image == null)
                return pt;

            int w = (int)Math.Round((pt.X * this.Width) / (double)this.Image.Width);
            int h = (int)Math.Round((pt.Y * this.Height) / (double)this.Image.Height);

            return new Point(w, h);
        }

        public Rectangle ConvertControlRectToPictureRect(Rectangle rect)
        {
            Point orig = ConvertControlPointToPicturePoint(rect.Location);
            Point s = ConvertControlPointToPicturePoint(new Point(rect.X + rect.Width, rect.Y + rect.Height));

            return new Rectangle(orig, new Size(s.X - orig.X, s.Y - orig.Y));
        }

        public Rectangle ConvertPictureRectToControlRect(Rectangle rect)
        {
            Point orig = ConvertPicturePointToControlPoint(rect.Location);
            Point s = ConvertPicturePointToControlPoint(new Point(rect.X + rect.Width, rect.Y + rect.Height));

            return new Rectangle(orig, new Size(s.X - orig.X, s.Y - orig.Y));
        }

        private MouseEventArgs ConvertMEA(MouseEventArgs e)
        {
            Point pt = ConvertControlPointToPicturePoint(e.Location);
            return new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Image != null)
            {

                e.Graphics.InterpolationMode = InterpolationMode;
                //base.OnPaint(e);

                double zoomX = (double)this.Width / (double)this.Image.Width;
                double zoomY = (double)this.Height / (double)this.Image.Height;
                e.Graphics.DrawImage(this.Image, (int)Math.Round(zoomX / 2), (int)Math.Round(zoomY / 2), this.Width, this.Height);


                foreach (SizableRect srect in srects)
                {
                    try
                    {
                        srect.Draw(e.Graphics, selectedSRect == srect);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.Message);
                    }
                }
            }

            Contract.Requires(e != null);
            PaintEventHandler handler = (PaintEventHandler)Events[typeof(Control).GetField("EventPaint", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)];
            if (handler != null) handler(this, e);
        }

        bool isDown = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            e = ConvertMEA(e);

            SizableRect formerSelectedSRect = selectedSRect;

            if (e.Button == MouseButtons.Left)
            {
                if (selectedSRect == null || !selectedSRect.IsPointInRect(e.Location))
                {
                    selectedSRect = null;
                    foreach (SizableRect srect in srects)
                    {
                        if (srect.IsPointInRect(e.Location))
                        {
                            selectedSRect = srect;
                            break;
                        }
                    }
                }

                if (selectedSRect != null)
                    selectedSRect.MouseDown(e);
            }
            else if(e.Button == MouseButtons.Right)
            {
                if(AllowSRectCreation && !isDown)
                {
                    selectedSRect = AddSRect(new Rectangle(e.Location, new Size(5, 5))) as SizableRect;
                    RaiseSRectCreated(selectedSRect);
                    isDown = true;
                }
            }

            if (formerSelectedSRect != selectedSRect)
                RaiseSelectedSRectChanged();

            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            e = ConvertMEA(e);

            isDown = false;

            if (selectedSRect != null)
                selectedSRect.MouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            e = ConvertMEA(e);

            if (selectedSRect != null && selectedSRect.MouseMove(e))
                RaiseRectResize(selectedSRect);
        }

        public class SizableRectEventArgs : EventArgs
        {
            private object srect;
            private Rectangle rect;
            private object linkedObject = null;

            public object SRect { get => srect; }
            public Rectangle Rect { get => rect; }
            public object LinkedObject { get => linkedObject; }

            public SizableRectEventArgs(object srect, Rectangle rect, object linkedObject)
            {
                this.srect = srect;
                this.rect = rect;
                this.linkedObject = linkedObject;
            }
        }

        private class SizableRect
        {
            private PictureBoxDB mPictureBox;
            public Rectangle rect;
            public bool AllowDeformingDuringMovement = false;
            public object LinkedObject = null;
            private bool mIsClick = false;
            private bool mMove = false;
            private int oldX;
            private int oldY;
            private int sizeNodeRect = 5;
            private PosSizableRect nodeSelected = PosSizableRect.None;

            private enum PosSizableRect
            {
                UpMiddle,
                LeftMiddle,
                LeftBottom,
                LeftUp,
                RightUp,
                RightMiddle,
                RightBottom,
                BottomMiddle,
                None

            };

            public SizableRect(Rectangle r, PictureBoxDB owner)
            {
                rect = r;
                mIsClick = false;
                this.mPictureBox = owner;
            }

            public void Draw(Graphics g, bool isSelected)
            {
                Color col = isSelected ? Color.Red : Color.DarkGreen;
                Pen pen = new Pen(col);

                g.DrawRectangle(pen, mPictureBox.ConvertPictureRectToControlRect(rect));

                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    g.DrawRectangle(pen, GetRect(pos, true));
                }
            }

            public void MouseDown(MouseEventArgs e)
            {
                mIsClick = true;

                nodeSelected = PosSizableRect.None;
                nodeSelected = GetNodeSelectable(e.Location);

                if (rect.Contains(new Point(e.X, e.Y)))
                {
                    mMove = true;
                }
                oldX = e.X;
                oldY = e.Y;
            }

            public void MouseUp(MouseEventArgs e)
            {
                mIsClick = false;
                mMove = false;
            }

            public bool MouseMove(MouseEventArgs e)
            {
                ChangeCursor(e.Location);
                if (mIsClick == false)
                {
                    return false;
                }

                Rectangle backupRect = rect;

                switch (nodeSelected)
                {
                    case PosSizableRect.LeftUp:
                        rect.X += e.X - oldX;
                        rect.Width -= e.X - oldX;
                        rect.Y += e.Y - oldY;
                        rect.Height -= e.Y - oldY;
                        break;
                    case PosSizableRect.LeftMiddle:
                        rect.X += e.X - oldX;
                        rect.Width -= e.X - oldX;
                        break;
                    case PosSizableRect.LeftBottom:
                        rect.Width -= e.X - oldX;
                        rect.X += e.X - oldX;
                        rect.Height += e.Y - oldY;
                        break;
                    case PosSizableRect.BottomMiddle:
                        rect.Height += e.Y - oldY;
                        break;
                    case PosSizableRect.RightUp:
                        rect.Width += e.X - oldX;
                        rect.Y += e.Y - oldY;
                        rect.Height -= e.Y - oldY;
                        break;
                    case PosSizableRect.RightBottom:
                        rect.Width += e.X - oldX;
                        rect.Height += e.Y - oldY;
                        break;
                    case PosSizableRect.RightMiddle:
                        rect.Width += e.X - oldX;
                        break;

                    case PosSizableRect.UpMiddle:
                        rect.Y += e.Y - oldY;
                        rect.Height -= e.Y - oldY;
                        break;

                    default:
                        if (mMove)
                        {
                            rect.X = rect.X + e.X - oldX;
                            rect.Y = rect.Y + e.Y - oldY;
                        }
                        break;
                }
                oldX = e.X;
                oldY = e.Y;

                if (rect.Width < 5 || rect.Height < 5)
                {
                    rect = backupRect;
                }

                TestIfRectInsideArea();

                mPictureBox.Invalidate();

                return !rect.Equals(backupRect);
            }

            private void TestIfRectInsideArea()
            {
                // Test if rectangle still inside the area.
                if (rect.X < 0) rect.X = 0;
                if (rect.Y < 0) rect.Y = 0;
                if (rect.Width <= 0) rect.Width = 1;
                if (rect.Height <= 0) rect.Height = 1;

                if (rect.X + rect.Width > mPictureBox.Image.Width)
                {
                    rect.X = mPictureBox.Image.Width - rect.Width - 1;
                }
                if (rect.Y + rect.Height > mPictureBox.Image.Height)
                {
                    rect.Y = mPictureBox.Image.Height - rect.Height - 1;
                }
            }

            public bool IsPointInRect(Point pt)
            {
                return rect.Contains(pt) ||(GetNodeSelectable(pt) != PosSizableRect.None);
            }

            private Rectangle CreateRectSizableNode(int x, int y, bool convert = false)
            {
                if(convert)
                {
                    Point pt = mPictureBox.ConvertPicturePointToControlPoint(new Point(x, y));
                    x = pt.X;
                    y = pt.Y;
                }

                return new Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect);
            }

            private Rectangle GetRect(PosSizableRect p, bool convert = false)
            {
                switch (p)
                {
                    case PosSizableRect.LeftUp:
                        return CreateRectSizableNode(rect.X, rect.Y, convert);

                    case PosSizableRect.LeftMiddle:
                        return CreateRectSizableNode(rect.X, rect.Y + rect.Height / 2, convert);

                    case PosSizableRect.LeftBottom:
                        return CreateRectSizableNode(rect.X, rect.Y + rect.Height, convert);

                    case PosSizableRect.BottomMiddle:
                        return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y + rect.Height, convert);

                    case PosSizableRect.RightUp:
                        return CreateRectSizableNode(rect.X + rect.Width, rect.Y, convert);

                    case PosSizableRect.RightBottom:
                        return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height, convert);

                    case PosSizableRect.RightMiddle:
                        return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height / 2, convert);

                    case PosSizableRect.UpMiddle:
                        return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y, convert);
                    default:
                        return new Rectangle();
                }
            }

            private PosSizableRect GetNodeSelectable(Point p)
            {
                foreach (PosSizableRect r in Enum.GetValues(typeof(PosSizableRect)))
                {
                    if (GetRect(r).Contains(p))
                    {
                        return r;
                    }
                }
                return PosSizableRect.None;
            }

            private void ChangeCursor(Point p)
            {
                mPictureBox.Cursor = GetCursor(GetNodeSelectable(p));
            }

            /// <summary>
            /// Get cursor for the handle
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            private Cursor GetCursor(PosSizableRect p)
            {
                switch (p)
                {
                    case PosSizableRect.LeftUp:
                        return Cursors.SizeNWSE;

                    case PosSizableRect.LeftMiddle:
                        return Cursors.SizeWE;

                    case PosSizableRect.LeftBottom:
                        return Cursors.SizeNESW;

                    case PosSizableRect.BottomMiddle:
                        return Cursors.SizeNS;

                    case PosSizableRect.RightUp:
                        return Cursors.SizeNESW;

                    case PosSizableRect.RightBottom:
                        return Cursors.SizeNWSE;

                    case PosSizableRect.RightMiddle:
                        return Cursors.SizeWE;

                    case PosSizableRect.UpMiddle:
                        return Cursors.SizeNS;
                    default:
                        return Cursors.Default;
                }
            }
        }
    }
}
