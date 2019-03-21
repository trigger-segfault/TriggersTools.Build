using System;
using System.Diagnostics;
using System.Reflection;
using TriggersTools.Build;

namespace Multitarget.Test {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("=== Multitarget.Test ===");
			Console.WriteLine("Checking assembly attributes...");
			Console.WriteLine();

			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			Console.WriteLine($"       Copyright: {fileVersionInfo.LegalCopyright}");
			Console.WriteLine();
			if (assembly.HasBuildTime()) {
				Console.WriteLine($"Local Build Time: {assembly.GetBuildTime()}");
				Console.WriteLine($"Local Build Date: {assembly.GetBuildDate().ToShortDateString()}");
				Console.WriteLine($"  UTC Build Time: {assembly.GetUtcBuildTime()}");
				Console.WriteLine($"  UTC Build Date: {assembly.GetUtcBuildDate().ToShortDateString()}");
			}
			else {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Build Time: NOT FOUND");
				Console.ResetColor();
			}

			Console.WriteLine();
			Console.WriteLine("Finished - Press any key to close...");
			Console.Read();
		}
	}
}