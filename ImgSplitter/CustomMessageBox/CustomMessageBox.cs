using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ImgSplitter.CustomMessageBox
{
    internal class CustomMessageBox
    {
        public static void Show(string message, string title)
        {
            using (MessageBoxWindow msg = new MessageBoxWindow())
            {
                msg.Title = title;
                msg.TxtTitle.Text = title;
                msg.TxtMessage.Text = message;
                msg.BtnOk.Content = "OK";
                msg.BtnOk.Focus();
                msg.ShowDialog();
            }
        }
        public static void ShowNoWarning(string message, string title)
        {
            using (MessageBoxWindow msg = new MessageBoxWindow())
            {
                msg.Title = title;
                msg.TxtTitle.Text = title;
                msg.TxtMessage.Text = message;
                msg.TxtMessage.Width = 185;
                msg.Icon.Visibility = Visibility.Collapsed;
                msg.BtnOk.Content = "OK";
                msg.BtnOk.Focus();
                msg.ShowDialog();
            }
        }
    }
}
