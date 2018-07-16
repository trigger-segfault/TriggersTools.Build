﻿using System;
using System.Diagnostics;
using System.Reflection;
using TriggersTools.Build;

namespace Framework.Test {
	class Program {
		static void Main(string[] args) {
			// NOTE: It seems updating a local (and maybe web) NuGet targets
			// package will break the console until you restart VS2017. Fun.
			Console.WriteLine("=== Framework.Test ===");
			Console.WriteLine("Checking assembly attributes...");
			Console.WriteLine();

			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			Console.WriteLine($"Copyright: {fileVersionInfo.LegalCopyright}");
			if (assembly.HasBuildTime())
				Console.WriteLine($"Build Time: {assembly.GetBuildTime()}");
			else
				Console.WriteLine($"Build Time: NOT FOUND");

			Console.WriteLine();
			Console.WriteLine("Finished - Press any key to close...");
			Console.Read();
		}
	}
}
