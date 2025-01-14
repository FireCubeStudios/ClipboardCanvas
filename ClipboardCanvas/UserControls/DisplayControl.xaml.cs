﻿using ClipboardCanvas.Helpers.Filesystem;
using ClipboardCanvas.Models;
using ClipboardCanvas.ModelViews;
using ClipboardCanvas.UnsafeNative;
using ClipboardCanvas.ViewModels.Pages;
using ClipboardCanvas.ViewModels.UserControls;
using System.Collections;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using System;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using ClipboardCanvas.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ClipboardCanvas.UserControls
{
    public sealed partial class DisplayControl : UserControl, IDisplayControlView
    {
        public DisplayControlViewModel ViewModel
        {
            get => (DisplayControlViewModel)DataContext;
            set => DataContext = value;
        }

        public IWindowTitleBarControlModel WindowTitleBarControlModel { get; set; }

        public INavigationToolBarControlModel NavigationToolBarControlModel { get; set; }

        public IPasteCanvasPageModel PasteCanvasPageModel
        {
            get
            {
                if ((DisplayFrame.Content as Page)?.DataContext is CanvasPageViewModel viewModel)
                {
                    return viewModel;
                }

                return null;
            }
        }

        public DisplayControl()
        {
            this.InitializeComponent();

            this.ViewModel = new DisplayControlViewModel(this);
        }

        // TODO: Move the event handler to the ViewModel
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the rest when the view is loaded
            await InitialApplicationChecksHelpers.HandleFileSystemPermissionDialog(WindowTitleBarControlModel);

            await InitialApplicationChecksHelpers.CheckVersionAndShowDialog();

            await this.ViewModel.InitializeAfterLoad();
        }
    }
}
