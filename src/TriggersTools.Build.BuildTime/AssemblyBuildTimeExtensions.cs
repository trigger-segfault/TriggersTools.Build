using System;
using System.Linq;
using System.Reflection;

namespace TriggersTools.Build {
	/// <summary>Extensions for quickly accessing an assemby's build time.</summary>
	public static class AssemblyBuildTimeExtensions {

		#region HasBuildTime

		/// <summary>Gets if the assembly has a <see cref="AssemblyBuildTimeAttribute"/>.</summary>
		/// 
		/// <param name="assembly">The assembly to check.</param>
		/// <returns>True if the assembly has a <see cref="AssemblyBuildTimeAttribute"/>.</returns>
		public static bool HasBuildTime(this Assembly assembly) {
			return assembly.GetAttribute() != null;
		}

		#endregion

		#region GetBuildTime

		/// <summary>Gets the Coordinate Universal build time of the assembly.</summary>
		/// 
		/// <param name="assembly">The assembly to get the build date for.</param>
		/// <returns>
		/// The Coordinate Universal build time. -or- <see cref="DateTime.MinValue"/> if no <see
		/// cref="AssemblyBuildTimeAttribute"/> was present.
		/// </returns>
		public static DateTime GetUtcBuildTime(this Assembly assembly) {
			return assembly.GetAttribute()?.UtcBuildTime ?? DateTime.MinValue;
		}

		/// <summary>Gets the local build time of the assembly.</summary>
		/// 
		/// <param name="assembly">The assembly to get the build date for.</param>
		/// <returns>
		/// The local build time. -or- <see cref="DateTime.MinValue"/> if no <see cref=
		/// "AssemblyBuildTimeAttribute"/> was present.
		/// </returns>
		public static DateTime GetBuildTime(this Assembly assembly) {
			return assembly.GetAttribute()?.BuildTime ?? DateTime.MinValue;
		}

		#endregion

		#region GetBuildDate

		/// <summary>Gets the Coordinate Universal build date of the assembly.</summary>
		/// 
		/// <param name="assembly">The assembly to get the build date for.</param>
		/// <returns>
		/// The Coordinate Universal build date. -or- <see cref="DateTime.MinValue"/> if no <see
		/// cref="AssemblyBuildTimeAttribute"/> was present.
		/// </returns>
		public static DateTime GetUtcBuildDate(this Assembly assembly) {
			return assembly.GetAttribute()?.UtcBuildDate ?? DateTime.MinValue;
		}

		/// <summary>Gets the local build date of the assembly.</summary>
		/// 
		/// <param name="assembly">The assembly to get the build date for.</param>
		/// <returns>
		/// The local build date. -or- <see cref="DateTime.MinValue"/> if no <see cref=
		/// "AssemblyBuildTimeAttribute"/> was present.
		/// </returns>
		public static DateTime GetBuildDate(this Assembly assembly) {
			return assembly.GetAttribute()?.BuildDate ?? DateTime.MinValue;
		}

		#endregion

		#region Private

		/// <summary>Gets the <see cref="AssemblyBuildTimeAttribute"/> from the assembly.</summary>
		private static AssemblyBuildTimeAttribute GetAttribute(this Assembly assembly) {
#if NET40
			return assembly.GetCustomAttributes(typeof(AssemblyBuildTimeAttribute), true)
				.FirstOrDefault() as AssemblyBuildTimeAttribute;
#else
			return assembly.GetCustomAttributes(typeof(AssemblyBuildTimeAttribute))
				.FirstOrDefault() as AssemblyBuildTimeAttribute;
#endif
		}

		#endregion
	}
}
