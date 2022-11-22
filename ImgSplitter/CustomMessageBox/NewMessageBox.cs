using System;
using System.Collections.Generic;
using System.Text;

namespace ImgSplitter.CustomMessageBox
{
    internal class NewMessageBox : MessageBoxWindow
    {
        public new void Show()
        {
            ShowDialog();
        }
    }
}
