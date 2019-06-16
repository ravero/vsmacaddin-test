using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;

namespace Ravero.VSMacAddIn.Commands
{
    public class InsertGuidCommand : CommandHandler
    {
        protected override void Run() {
            var doc = IdeApp.Workbench.ActiveDocument;
            var guid = Guid.NewGuid().ToString();
            doc.Editor.InsertAtCaret(guid);
        }

        protected override void Update(CommandInfo info) {
            var doc = IdeApp.Workbench.ActiveDocument;
            info.Enabled = (doc != null && doc.Editor != null);
        }
    }
}
