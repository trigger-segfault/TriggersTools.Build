using System;

namespace TriggersTools.Build {
	/// <summary>An attribute specifying the build time of an assembly.</summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class AssemblyBuildTimeAttribute : Attribute {

		#region Members/Properties

		/// <summary>Gets the Coordinate Universal build time of the assembly.</summary>
		public DateTime UtcBuildTime { get; }
		/// <summary>Gets the local build time of the assembly.</summary>
		public DateTime BuildTime => UtcBuildTime.ToLocalTime();

		/// <summary>Gets the Coordinate Universal build date of the assembly.</summary>
		public DateTime UtcBuildDate => UtcBuildTime.Date;
		/// <summary>Gets the local build date of the assembly.</summary>
		public DateTime BuildDate => BuildTime.Date;

		/// <summary>Gets the Coordinate Universal build time of day of the assembly.</summary>
		public TimeSpan UtcBuildTimeOfDay => UtcBuildTime.TimeOfDay;
		/// <summary>Gets the local build time of day of the assembly.</summary>
		public TimeSpan BuildTimeOfDay => BuildTime.TimeOfDay;

		#endregion

		#region Constructor
		
		/// <summary>Constructs the build time attribute by parsing the dateTime.</summary>
		/// 
		/// <param name="utcTicksOrDateTime">
		/// If this is just a number, the <see cref="UtcBuildTime"/> will be parsed from ticks.
		/// Otherwise the <see cref="UtcBuildTime"/> will be parsed from <see cref="DateTime"/>.
		/// </param>
		public AssemblyBuildTimeAttribute(string utcTicksOrDateTime) {
			if (long.TryParse(utcTicksOrDateTime, out long ticks))
				UtcBuildTime = new DateTime(ticks, DateTimeKind.Utc);
			else
				UtcBuildTime = DateTime.Parse(utcTicksOrDateTime).ToUniversalTime();
		}

		#endregion
	}
}
