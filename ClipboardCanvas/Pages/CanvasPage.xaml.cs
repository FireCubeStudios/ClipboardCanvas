﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using ClipboardCanvas.ViewModels.Pages;
using ClipboardCanvas.ModelViews;
using ClipboardCanvas.Models;
using ClipboardCanvas.DataModels.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClipboardCanvas.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CanvasPage : Page, ICanvasPageView
    {
        public CanvasPageViewModel ViewModel
        {
            get => (CanvasPageViewModel)DataContext;
            set => DataContext = value;
        }

        public IPasteCanvasModel PasteCanvasModel => PasteCanvasControl?.ViewModel;

        public ICollectionsContainerModel AssociatedCollection { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DisplayFrameNavigationParameterDataModel navigationParameter = e.Parameter as DisplayFrameNavigationParameterDataModel;
            AssociatedCollection = navigationParameter.collectionContainer;
        }

        public CanvasPage()
        {
            this.InitializeComponent();

            this.ViewModel = new CanvasPageViewModel(this);
        }

        private void PastedAsReference_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Flyout.ShowAttachedFlyout(PastedAsReference);
        }
    }
}
