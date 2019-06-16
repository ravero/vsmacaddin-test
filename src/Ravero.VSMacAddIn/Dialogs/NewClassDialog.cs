using System;
using Gtk;
using Ravero.VSMacAddIn.Helpers;
using static Ravero.VSMacAddIn.Helpers.DocumentHelpers;
using MonoDevelop.Ide;

namespace Ravero.VSMacAddIn.Dialogs
{
    public class NewClassDialog : Dialog
    {
        const int WINDOW_WIDTH = 300;
        const int WINDOW_HEIGTH = 100;

        readonly Label classNameCaption;
        readonly Entry classNameEntry;
        readonly Button confirmButton;
        readonly Button cancelButton;

        public NewClassDialog() {
            // Window Setup
            WindowPosition = WindowPosition.CenterAlways;
            SetSizeRequest(WINDOW_WIDTH, WINDOW_HEIGTH);
            Resizable = false;
            Title = "New Class";
            Events = Gdk.EventMask.AllEventsMask;
            CanFocus = true;

            // Elements Setup
            classNameCaption = new Label("Enter the class name:");
            classNameCaption.Layout.Alignment = Pango.Alignment.Left;

            classNameEntry = new Entry();
            classNameEntry.CanFocus = true;
            classNameEntry.Events = Gdk.EventMask.AllEventsMask;
            //classNameEntry.Text = 
            classNameEntry.IsEditable = true;

            confirmButton = new Button();
            confirmButton.Label = "OK";
            confirmButton.Clicked += OnConfirmButtonClicked;;

            cancelButton = new Button();
            cancelButton.Label = "Cancel";
            cancelButton.Clicked += (sender, e) => {
                Hide();
            };

            // Layout Setup
            var line1 = new HBox {
                classNameCaption,
                classNameEntry
            };
            var line2 = new HBox {
                confirmButton,
                cancelButton
            };

            VBox.Add(line1);
            VBox.Add(line2);
            ShowAll();
        }

        protected override void OnShown() {
            GrabFocus();
            classNameEntry.GrabFocus();
            base.OnShown();
        }

        void OnConfirmButtonClicked(object sender, EventArgs e) {
            // Validate if the provided class name is valid
            var (isValid, errorMessage) = ValidateClassName(classNameEntry.Text);
            if (!isValid) {
                Xwt.MessageDialog.ShowError(errorMessage, "Can't create class");
                return;
            }

            // If validation is fine passes with the creation
            var file = CreateClass(classNameEntry.Text);
            if (file != null) {
                IdeApp.Workbench.OpenDocument(file.FilePath, file.Project, true);
                Hide();
            }
        }
    }
}
