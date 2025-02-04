﻿using ClipboardCanvas.Dialogs;
using ClipboardCanvas.Enums;
using ClipboardCanvas.Helpers.Filesystem;
using ClipboardCanvas.Helpers.SafetyHelpers;
using ClipboardCanvas.Models;
using ClipboardCanvas.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;

namespace ClipboardCanvas.Helpers
{
    public static class InitialApplicationChecksHelpers
    {
        public static async Task CheckVersionAndShowDialog()
        {
            string lastVersion = App.AppSettings.ApplicationSettings.LastVersionNumber;
            string currentVersion = App.AppVersion;

            if (string.IsNullOrEmpty(lastVersion))
            {
                // No version yet, Clipboard Canvas is freshly installed
                // update the version setting with current version
                App.AppSettings.ApplicationSettings.LastVersionNumber = currentVersion;
            }
            else
            {
                // Compare two versions
                if (VersionHelpers.IsVersionDifferentThan(lastVersion, currentVersion))
                {
                    // Show the update dialog
                    await App.DialogService.ShowDialog(new UpdateChangeLogDialogViewModel());

                    // Update the last version number to be the current number
                    App.AppSettings.ApplicationSettings.LastVersionNumber = currentVersion;
                }
            }
        }

        public static async Task HandleFileSystemPermissionDialog(IWindowTitleBarControlModel windowTitleBar)
        {
            string testForFolderOnFS = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            SafeWrapper<StorageFolder> testFolderResult = await StorageHelpers.ToStorageItemWithError<StorageFolder>(testForFolderOnFS);

            if (!testFolderResult)
            {
                App.IsInRestrictedAccessMode = true;

                DialogResult dialogResult = await App.DialogService.ShowDialog(new FileSystemAccessDialogViewModel());

                if (dialogResult == DialogResult.Primary)
                {
                    // Restart the app
                    await CoreApplication.RequestRestartAsync(string.Empty);
                }
                else
                {
                    // Continue in Restricted Access
                    windowTitleBar.IsInRestrictedAccess = true;
                }
            }
            else
            {
                return;
            }
        }
    }
}
