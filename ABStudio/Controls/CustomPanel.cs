using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using ABStudio.Misc;

namespace ABStudio.Controls
{
    public class CustomPanel : Panel
    {
        public event MouseEventHandler CTRLMouseWheel;


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (((Common.GetAsyncKeyState(0xA2) & 0x8000) > 0) || ((Common.GetAsyncKeyState(0xA3) & 0x8000) > 0)) // LCONTROL or RCONTROL held
            {
                CTRLMouseWheel.Invoke(this, e);
                ((HandledMouseEventArgs)e).Handled = true;
            }
            else
            {
                base.OnMouseWheel(e);
            }
        }
    }
}
