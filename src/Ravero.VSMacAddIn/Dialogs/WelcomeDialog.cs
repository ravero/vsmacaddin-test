using System;
using Gtk;
using MonoDevelop.Core;

namespace Ravero.VSMacAddIn.Dialogs
{
    public class WelcomeDialog : Dialog
    {
        const int WINDOW_WIDTH = 400;
        const int WINDOW_HEIGHT = 150;

        readonly Label informationLabel;
        readonly Button confirmButton;

        public WelcomeDialog() {
            // Window Setup
            WindowPosition = WindowPosition.CenterAlways;
            SetSizeRequest(WINDOW_WIDTH, WINDOW_HEIGHT);
            Resizable = false;
            Title = "Welcome to my Sample!";
            Events = Gdk.EventMask.AllEventsMask;
            CanFocus = true;

            // Elements Setup
            informationLabel = new Label("Welcome to my Sample Visual Studio for Mac Extension. This is window is provided to display some welcome message.");
            informationLabel.Layout.Alignment = Pango.Alignment.Center;

            confirmButton = new Button();
            confirmButton.Label = "OK";
            confirmButton.Clicked += (s, e) => {
                PropertyService.Set(Constants.Properties.HasDisplayedWelcomeKey, true);
                PropertyService.SaveProperties();
                Hide();
            };

            // Layout Setup
            VBox.SetSizeRequest(WINDOW_WIDTH, WINDOW_HEIGHT);
            VBox.Add(informationLabel);
            VBox.Add(confirmButton);
            VBox.Events = Gdk.EventMask.AllEventsMask;
            VBox.CanFocus = true;

            ShowAll();
        }

        protected override void OnShown() {
            GrabFocus();
            base.OnShown();
        }
    }
}
