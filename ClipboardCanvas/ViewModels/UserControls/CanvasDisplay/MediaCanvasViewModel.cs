﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Core;
using Windows.Storage;

using ClipboardCanvas.Helpers.SafetyHelpers;
using ClipboardCanvas.Helpers.SafetyHelpers.ExceptionReporters;
using ClipboardCanvas.Models;
using ClipboardCanvas.Enums;
using ClipboardCanvas.ModelViews;
using ClipboardCanvas.DataModels.PastedContentDataModels;
using System.Threading;

namespace ClipboardCanvas.ViewModels.UserControls.CanvasDisplay
{
    public class MediaCanvasViewModel : BasePasteCanvasViewModel
    {
        #region Private Members

        private readonly IDynamicCanvasControlView _view;

        private MediaContentType _mediaContentType => contentType as MediaContentType;

        #endregion

        #region Protected Members

        protected override ICollectionsContainerModel AssociatedContainer => _view?.CollectionContainer;

        #endregion

        #region Private Properties

        private TimeSpan __Position
        {
            get => ControlView.Position;
            set => ControlView.Position = value;
        }

        private bool __IsLoopingEnabled
        {
            get => ControlView.IsLoopingEnabled;
            set => ControlView.IsLoopingEnabled = value;
        }

        private double __Volume
        {
            get => ControlView.Volume;
            set => ControlView.Volume = value;
        }

        #endregion

        #region Public Properties

        public static List<string> Extensions => new List<string>() {
            // Video
            ".mp4", ".webm", ".ogg", ".mov", ".qt", ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".mkv",
            // Audio
            ".mp3", ".m4a", ".wav", ".wma", ".aac", ".adt", ".adts", ".cda",
        };

        private MediaSource _ContentMedia;
        public MediaSource ContentMedia
        {
            get => _ContentMedia;
            set => SetProperty(ref _ContentMedia, value);
        }

        private bool _ContentMediaLoad;
        public bool ContentMediaLoad
        {
            get => _ContentMediaLoad;
            set => SetProperty(ref _ContentMediaLoad, value);
        }

        public IMediaCanvasControlView ControlView { get; set; }

        #endregion

        #region Constructor

        public MediaCanvasViewModel(IDynamicCanvasControlView view, CanvasPreviewMode canvasMode)
            : base(StaticExceptionReporters.DefaultSafeWrapperExceptionReporter, new MediaContentType(), canvasMode)
        {
            this._view = view;
        }

        #endregion

        #region Override

        public override async Task<SafeWrapperResult> TryLoadExistingData(ICollectionsContainerItemModel itemData, CancellationToken cancellationToken)
        {
            SafeWrapperResult result = await base.TryLoadExistingData(itemData, cancellationToken);

            if (result)
            {
                UpdateMediaControl();
            }

            return result;
        }

        public override async Task<SafeWrapperResult> TrySaveData()
        {
            return await Task.FromResult(SafeWrapperResult.S_SUCCESS);
        }

        protected override async Task<SafeWrapperResult> SetData(DataPackageView dataPackage)
        {
            return await Task.FromResult(SafeWrapperResult.S_SUCCESS);
        }

        protected override async Task<SafeWrapperResult> SetData(StorageFile file)
        {
            return await Task.FromResult(SafeWrapperResult.S_SUCCESS);
        }

        protected override async Task<SafeWrapper<StorageFile>> TrySetFileWithExtension()
        {
            return await Task.FromResult(new SafeWrapper<StorageFile>(associatedFile, SafeWrapperResult.S_SUCCESS));
        }

        protected override async Task<SafeWrapperResult> TryFetchDataToView()
        {
            ContentMediaLoad = true;
            ContentMedia = MediaSource.CreateFromStorageFile(sourceFile);

            return await Task.FromResult(SafeWrapperResult.S_SUCCESS);
        }

        protected override void OnReferencePasted()
        {
            // Change the source
            _ContentMedia = MediaSource.CreateFromStorageFile(sourceFile);
        }

        #endregion

        #region Public Helpers

        public void UpdateMediaControl()
        {
            if (ControlView != null && _mediaContentType != null)
            {
                this.__Position = _mediaContentType.savedPosition;
                this.__IsLoopingEnabled = App.AppSettings.CanvasSettings.MediaCanvas_IsLoopingEnabled;
                this.__Volume = App.AppSettings.CanvasSettings.MediaCanvas_UniversalVolume;
            }
        }

        #endregion

        #region IDisposable

        public override void Dispose()
        {
            if (ControlView != null && ContentMediaLoad)
            {
                if (AssociatedContainerCanvas?.ContentType is MediaContentType mediaContentType)
                {
                    mediaContentType.savedPosition = __Position;
                }

                App.AppSettings.CanvasSettings.MediaCanvas_IsLoopingEnabled = __IsLoopingEnabled;
                App.AppSettings.CanvasSettings.MediaCanvas_UniversalVolume = __Volume;
            }

            base.Dispose();

            ContentMedia?.Dispose();
            ContentMedia = null;
        }

        #endregion
    }
}
