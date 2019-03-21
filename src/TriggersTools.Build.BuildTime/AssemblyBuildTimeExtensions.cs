using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TriggersTools.Build {
	/// <summary>
	///  Extensions for quickly accessing an assemby's build time.
	/// </summary>
	public static class AssemblyBuildTimeExtensions {
		#region Constants

		/// <summary>
		///  Gets the name of the build timestamp embedded resource used to store the build time.
		/// </summary>
		public const string TimestampResource = "TriggersTools.Build.Timestamp";

		#endregion

		#region HasBuildTime

		/// <summary>
		///  Gets if the assembly has an embedded <see cref="TimestampResource"/>.
		/// </summary>
		/// 
		/// <param name="assembly">The assembly to check.</param>
		/// <returns>True if the assembly has an embedded <see cref="TimestampResource"/>.</returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> is null.
		/// </exception>
		public static bool HasBuildTime(this Assembly assembly) {
			using (Stream stream = assembly.GetManifestResourceStream(TimestampResource)) {
				return stream != null;
			}
		}

		#endregion

		#region GetBuildTime

		/// <summary>
		///  Gets the Coordinate Universal build time of the assembly.
		/// </summary>
		/// <param name="assembly">The assembly to get the build time for.</param>
		/// <returns>
		///  The Coordinate Universal build time.-or- <see cref="DateTime.MinValue"/> if no embedded
		///  <see cref="TimestampResource"/> was present.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		///  The embedded <see cref="TimestampResource"/> is corrupt.
		/// </exception>
		public static DateTime GetUtcBuildTime(this Assembly assembly) {
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));
			using (Stream stream = assembly.GetManifestResourceStream(TimestampResource)) {
				if (stream == null)
					return DateTime.MinValue;
				using (StreamReader reader = new StreamReader(stream))
					return DateTime.Parse(reader.ReadToEnd()).ToUniversalTime();
			}
		}
		/// <summary>
		///  Gets the local build time of the assembly.
		/// </summary>
		/// <param name="assembly">The assembly to get the build time for.</param>
		/// <returns>
		///  The local build time.-or- <see cref="DateTime.MinValue"/> if no embedded <see cref="TimestampResource"/>
		///  was present.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		///  The embedded <see cref="TimestampResource"/> is corrupt.
		/// </exception>
		public static DateTime GetBuildTime(this Assembly assembly) {
			DateTime date = assembly.GetUtcBuildTime();
			if (date != DateTime.MinValue)
				date = date.ToLocalTime();
			return date;
		}

		#endregion

		#region GetBuildDate

		/// <summary>
		///  Gets the Coordinate Universal build date of the assembly.
		/// </summary>
		/// <param name="assembly">The assembly to get the build date for.</param>
		/// <returns>
		///  The Coordinate Universal build date.-or- <see cref="DateTime.MinValue"/> if no embedded
		///  <see cref="TimestampResource"/> was present.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		///  The embedded <see cref="TimestampResource"/> is corrupt.
		/// </exception>
		public static DateTime GetUtcBuildDate(this Assembly assembly) {
			DateTime date = assembly.GetUtcBuildTime();
			if (date != DateTime.MinValue)
				date = date.Date;
			return date;
		}
		/// <summary>
		///  Gets the local build date of the assembly.
		/// </summary>
		/// <param name="assembly">The assembly to get the build date for.</param>
		/// <returns>
		///  The local build date.-or- <see cref="DateTime.MinValue"/> if no embedded <see cref="TimestampResource"/>
		///  was present.
		/// </returns>
		/// 
		/// <exception cref="ArgumentNullException">
		///  <paramref name="assembly"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		///  The embedded <see cref="TimestampResource"/> is corrupt.
		/// </exception>
		public static DateTime GetBuildDate(this Assembly assembly) {
			DateTime date = assembly.GetBuildTime();
			if (date != DateTime.MinValue)
				date = date.ToLocalTime().Date;
			return date;
		}

		#endregion
	}
}
