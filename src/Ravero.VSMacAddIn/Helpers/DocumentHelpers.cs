using System;
using MonoDevelop.Ide;
using System.IO;
using MonoDevelop.Projects;
using Xwt;
using System.Linq;
using System.Text.RegularExpressions;
using MonoDevelop.Core;


namespace Ravero.VSMacAddIn.Helpers
{
    public static class DocumentHelpers
    {
        /// <summary>
        /// Validates if a class name is suitable for creating a new file class
        /// in the project.
        /// </summary>
        /// <returns>A Tuple representing the validation state (bool) and the validation message (string).</returns>
        /// <param name="className">The name of the class being validation.</param>
        public static (bool, string) ValidateClassName(string className) {
            // Check if the name is a valid class identifier
            if (!IsValidClassName(className))
                return (false, "The name is not a valid C# identifier for a class.");

            // Can't create the file it its not a .NET Project
            if (!(IdeApp.Workspace.CurrentSelectedProject is DotNetProject project))
                return (false, "Invalid project type");

            var folder = project.BaseDirectory;
            var fileName = GetClassFileName(className);
            var filePath = Path.Combine(folder, fileName);

            // Check if a file with the name already exists in the file system
            if (File.Exists(filePath))
                return (false, "A file with that name already exists on the project folder.");

            // Check if a file with the name already exists in the project
            if (project.Items.Any(i => i.ItemName == fileName))
                return (false, "A file with that name already exists on the project.");

            return (true, null);
        }

        /// <summary>
        /// Create a new Class File on the Current Selected Project.
        /// </summary>
        /// <param name="className">The new Class Name.</param>
        public static ProjectFile CreateClass(string className) {
            // Can't create the file it its not a .NET Project
            // This is a bit of defensive programming since the call should be pre-validated.
            if (!(IdeApp.Workspace.CurrentSelectedProject is DotNetProject project))
                return null;

            // Create the file at the root of the project
            var folder = project.BaseDirectory;
            var fileName = GetClassFileName(className);
            var filePath = Path.Combine(folder, fileName);

            // Create the file and add to the project
            var defaultNamespace = project.DefaultNamespace;
            var content = ApplyCreateFileTemplate(defaultNamespace, className);

            File.WriteAllText(filePath, content);
            return project.AddFile(fileName, "Compile");
        }

        /// <summary>
        /// Validates if the provided name is a valid C# Class Identifier.
        /// </summary>
        /// <returns><see langword="true"/> if the class name is a valid identifier.</returns>
        /// <param name="className">The name of the class.</param>
        static bool IsValidClassName(string className) => Regex.IsMatch(className, @"[A-Za-z_][A-Za-z0-9_]*");

        /// <summary>
        /// Gets the name of the class file.
        /// </summary>
        /// <returns>The class file name.</returns>
        /// <param name="className">The name of the class.</param>
        static string GetClassFileName(string className) => $"{className}.cs";

        /// <summary>
        /// Applies the Tokens to Create the content of a new Class File.
        /// </summary>
        /// <returns>The content of a new class file.</returns>
        /// <param name="rootNamespace">Root namespace.</param>
        /// <param name="className">Class name.</param>
        static string ApplyCreateFileTemplate(string rootNamespace, string className) =>
            classTemplate
                .Replace("{{rootNamespace}}", rootNamespace)
                .Replace("{{className}}", className);

        #region Templates

        /// <summary>
        /// File Template was made by using a constant string and some tokens
        /// to replace with values to create the actual class.
        /// Probably isn't the best way to do this, but I'll upgrade to use file
        /// templates when I have time to research on embbeding files in the
        /// extensions.
        /// </summary>
        const string classTemplate = @"using System;

namespace {{rootNamespace}}
{
    public class {{className}}
    {
        public {{className}}() {
        }
    }
}";

        #endregion

    }
}
