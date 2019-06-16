using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using Ravero.VSMacAddIn.Dialogs;

namespace Ravero.VSMacAddIn
{
    /// <summary>
    /// Provides startup experience for the Addin.
    /// </summary>
    public class StartupHandler : CommandHandler
    {
        protected override void Run() {
            if (!PropertyService.HasValue(Constants.Properties.HasDisplayedWelcomeKey)) {
                var dialog = new WelcomeDialog();
                dialog.Show();
            }
        }
    }
}
