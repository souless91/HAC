using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace HAC2Beta2
{
    public class PNGBox : Panel
    {
        public PNGBox()
        {
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }
    }
}
