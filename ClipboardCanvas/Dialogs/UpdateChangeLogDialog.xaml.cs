﻿using ClipboardCanvas.ViewModels.Dialogs;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System;
using ClipboardCanvas.Enums;
using ClipboardCanvas.Helpers;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClipboardCanvas.Dialogs
{
    public sealed partial class UpdateChangeLogDialog : ContentDialog, IDialog<UpdateChangeLogDialogViewModel>
    {
        public UpdateChangeLogDialogViewModel ViewModel
        {
            get => (UpdateChangeLogDialogViewModel)DataContext;
            set => DataContext = value;
        }

        public UpdateChangeLogDialog()
        {
            this.InitializeComponent();
        }

        public async new Task<DialogResult> ShowAsync() => (await base.ShowAsync()).ToDialogResult();

        // TODO: Move to view model
        private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel?.LoadUpdateDataFromGitHub(true);
        }
    }
}
