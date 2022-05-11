﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using GLib;
using Application = Gtk.Application;
using IoPath = System.IO.Path;
using XDM.Core.Lib.Common;
using Translations;
using UI = Gtk.Builder.ObjectAttribute;
using XDM.GtkUI.Utils;

namespace XDM.GtkUI.Dialogs.DownloadComplete
{
    public class DownloadCompleteDialog : Window, IDownloadCompleteDialog
    {
        public string FileNameText
        {
            get => TxtFileName.Text;
            set => TxtFileName.Text = value;
        }

        public string FolderText
        {
            get => TxtLocation.Text;
            set => TxtLocation.Text = value;
        }

        public IApp? App { get; set; }
        public event EventHandler<DownloadCompleteDialogEventArgs>? FileOpenClicked;
        public event EventHandler<DownloadCompleteDialogEventArgs>? FolderOpenClicked;
        public event EventHandler? DontShowAgainClickd;

        [UI] private Image ImgFileIcon;
        [UI] private Label TxtFileName;
        [UI] private Label TxtLocation;
        [UI] private Button BtnOpenFolder;
        [UI] private Button BtnOpen;
        [UI] private LinkButton TxtDontShowCompleteDialog;

        private DownloadCompleteDialog(Builder builder) : base(builder.GetRawOwnedObject("window"))
        {
            builder.Autoconnect(this);
            SetDefaultSize(400, 200);
            KeepAbove = true;
            Title = TextResource.GetText("CD_TITLE");
            SetPosition(WindowPosition.Center);

            BtnOpen.Label = TextResource.GetText("CTX_OPEN_FILE");
            BtnOpenFolder.Label = TextResource.GetText("CTX_OPEN_FOLDER");
            TxtDontShowCompleteDialog.Label = TextResource.GetText("MSG_DONT_SHOW_AGAIN");
            TxtFileName.StyleContext.AddClass("large-font");
            ImgFileIcon.Pixbuf = GtkHelper.LoadSvg("file-download-line", 64);

            BtnOpen.Clicked += BtnOpen_Click;
            BtnOpenFolder.Clicked += BtnOpenFolder_Click;
            TxtDontShowCompleteDialog.Clicked += TxtDontShowCompleteDialog_MouseDown;
        }

        private void TxtDontShowCompleteDialog_MouseDown(object? sender, EventArgs e)
        {

        }

        private void BtnOpen_Click(object? sender, EventArgs e)
        {
            FileOpenClicked?.Invoke(sender, new DownloadCompleteDialogEventArgs
            {
                Path = IoPath.Combine(TxtLocation.Text, TxtFileName.Text)
            });
            Close();
        }

        public void ShowDownloadCompleteDialog()
        {
            this.Show();
        }

        private void BtnOpenFolder_Click(object? sender, EventArgs e)
        {
            FolderOpenClicked?.Invoke(sender, new DownloadCompleteDialogEventArgs
            {
                Path = TxtLocation.Text,
                FileName = TxtFileName.Text
            });
            Close();
        }

        public static DownloadCompleteDialog CreateFromGladeFile()
        {
            var builder = new Builder();
            builder.AddFromFile(IoPath.Combine(AppDomain.CurrentDomain.BaseDirectory, "glade", "download-complete-window.glade"));
            return new DownloadCompleteDialog(builder);
        }
    }
}
