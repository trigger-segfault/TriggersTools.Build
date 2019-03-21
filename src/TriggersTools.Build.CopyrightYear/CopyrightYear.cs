using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TriggersTools.Build {
	/// <summary>
	///  A task to update the copyright's year.
	/// </summary>
	public class CopyrightYear : Task {
		#region Patterns

		/// <summary>
		///  The base regex pattern for the year token.
		/// </summary>
		private const string YearToken = "{YEAR}";
		/// <summary>
		///  The regex pattern for the assembly info file's copyright.
		/// </summary>
		private const string AssemblyCopyrightPattern =
			@"\s*(?:System\.Reflection\.)?AssemblyCopyright\s*\(\s*\$?@?""(?'copyright'.*)""\s*\)\s*";
		/// <summary>
		///  The regex pattern for C# assembly info files.
		/// </summary>
		private const string AssemblyInfoCSPattern =
			@"\[\s*assembly:" + AssemblyCopyrightPattern + @"\]";
		/// <summary>
		///  The regex pattern for F# assembly info files.
		/// </summary>
		private const string AssemblyInfoFSPattern =
			@"\[<\s*assembly:" + AssemblyCopyrightPattern + @">\]";
		/// <summary>
		///  The regex pattern for Visual Basic assembly info files.
		/// </summary>
		private const string AssemblyInfoVBPattern =
			@"<\s*Assembly:" + AssemblyCopyrightPattern + @">";
		/// <summary>
		///  The regex pattern for C#, F#, and Visual Basic assembly info files.
		/// </summary>
		private const string AssemblyInfoPattern =
			@"^\s*(?:" + AssemblyInfoCSPattern + "|" +
						 AssemblyInfoFSPattern + "|" +
						 AssemblyInfoVBPattern + @")\s*$";

		/// <summary>
		///  The regex for C# assembly info files.
		/// </summary>
		private Regex AssemblyInfoRegex => new Regex(AssemblyInfoPattern, RegexOptions.Multiline);

		#endregion

		#region Settings
		
		[Required]
		/// <summary>
		///  The path of the project directory.
		/// </summary>
		public string ProjectDir { get; set; }
		//[Required]
		/// <summary>
		///  The input copyright property.
		/// </summary>
		public string CopyrightInput { get; set; }
		//[Required]
		/// <summary>
		///  The input assembly info file path with the copyright.
		/// </summary>
		public string AssemblyInfoInput { get; set; }
		//[Required]
		/// <summary>
		///  The output assembly info file path with the copyright.
		/// </summary>
		public string AssemblyInfoOutput { get; set; }

		/// <summary>
		///  The output copyright property.
		/// </summary>
		[Output]
		public ITaskItem Copyright { get; set; }
		/// <summary>
		///  The output assembly info file with the copyright.
		/// </summary>
		[Output]
		public ITaskItem AssemblyInfo { get; set; }

		#endregion

		#region Execute

		/// <summary>
		///  Executes the task to update the copyright's year.
		/// </summary>
		/// <returns>True if the task succeeds.</returns>
		public override bool Execute() {
			Log.LogMessage(MessageImportance.Normal, "Applying Copyright Year");

			if (AssemblyInfoInput == null && AssemblyInfoOutput == null &&
				CopyrightInput == null)
			{
				Log.LogError($"{nameof(CopyrightInput)} or {nameof(AssemblyInfoOutput)} " +
					$"must be defined!");
			}

			if (AssemblyInfoInput != null && AssemblyInfoOutput == null) {
				Log.LogError($"{nameof(AssemblyInfoOutput)} must be defined if " +
					$"{nameof(AssemblyInfoInput)} is defined!");
			}

			bool assemblyInfo = AssemblyInfoOutput != null;
			bool property = CopyrightInput != null;

			try {
				string year = DateTime.UtcNow.Year.ToString();
				bool found = false;

				if (assemblyInfo)
					ReplaceAssemblyInfo(year, ref found);
				if (property)
					ReplaceProperty(year, ref found);

				if (!found) {
					Log.LogWarning($"No {YearToken} token was found to replace!");
				}
				
				return true;
			}
			catch (Exception ex) {
				Log.LogError(ex.ToString());
				return false;
			}
		}

		/// <summary>
		///  Replaces the copyright year in an assembly info file and writes a new file.
		/// </summary>
		/// <param name="year">The year to replace with.</param>
		private void ReplaceAssemblyInfo(string year, ref bool found) {
			string file = AssemblyInfoInput;
			string outFile = AssemblyInfoOutput;
			if (!Path.IsPathRooted(file))
				file = Path.Combine(ProjectDir, file);
			if (!Path.IsPathRooted(outFile))
				outFile = Path.Combine(ProjectDir, outFile);
			string dir = Path.GetDirectoryName(file);
			if (!File.Exists(file)) {
				//Log.LogMessage(MessageImportance.High, $"Could not find {file}!");
				return;
			}

			string text = File.ReadAllText(file);

			Match attributeMatch = AssemblyInfoRegex.Match(text);
			Group copyrightGroup = attributeMatch.Groups["copyright"];
			if (attributeMatch.Success && copyrightGroup.Success) {
				string copyright = copyrightGroup.Value;
				if (!copyright.Contains(YearToken)) {
					Log.LogWarning($"Could not find {YearToken} token in " +
						$"{nameof(AssemblyCopyrightAttribute)}!");
				}
				else {
					found = true;
					copyright = copyright.Replace(YearToken, year);
					text = ReplaceGroup(copyrightGroup, text, copyright);
				}
			}
			else {
				//Log.LogWarning($"Could not find {nameof(AssemblyCopyrightAttribute)} in assembly info file!");
			}

			// Output the new assembly info file so it can be referenced
			Directory.CreateDirectory(dir);
			File.WriteAllText(outFile, text);
			AssemblyInfo = new TaskItem(outFile);
		}

		/// <summary>
		///  Replaces the copyright year in a property.
		/// </summary>
		/// <param name="year">The year to replace with.</param>
		private void ReplaceProperty(string year, ref bool found) {
			string copyright = CopyrightInput;
			if (string.IsNullOrEmpty(copyright)) {
				//Log.LogMessage(MessageImportance.High, $"Copyright property was empty!");
				return;
			}
			
			if (!copyright.Contains(YearToken)) {
				Log.LogWarning($"Could not find {YearToken} token in Copyright property!");
			}
			else {
				found = true;
				copyright = copyright.Replace(YearToken, year);
			}

			// Output the new copyright
			Copyright = new TaskItem(copyright);
		}

		#endregion

		#region Helpers
		
		/// <summary>
		///  Replaces the group match with the specified string.
		/// </summary>
		/// <param name="group">The group to replace.</param>
		/// <param name="input">The string to replace.</param>
		/// <param name="value">The value to replace the group with.</param>
		/// <returns>The replaced string.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="input"/>, or <paramref name="value"/> is null.
		/// </exception>
		private static string ReplaceGroup(Group group, string input, string value) {
			if (!group.Success)
				return input;
			input = input.Remove(group.Index, group.Length);
			input = input.Insert(group.Index, value);
			return input;
		}

		#endregion
	}
}
