using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Ravero.VSMacAddIn.Dialogs;

namespace Ravero.VSMacAddIn.Commands
{
    public class CreateClassCommand : CommandHandler
    {
        protected override void Run() {
            var dialog = new NewClassDialog();
            dialog.Show();
        }

        protected override void Update(CommandInfo info) {
            // Limits visibility of this feature to C# source files only.
            // Don't know if it's the best method to determine the Active Document
            //  type but I think it makes the most sense.
            var doc = IdeApp.Workbench.ActiveDocument;
            info.Visible = doc?.Editor?.MimeType == "text/x-csharp";
        }
    }
}
