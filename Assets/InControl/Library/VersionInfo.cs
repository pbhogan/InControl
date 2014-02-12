using System;


namespace InControl
{
	public struct VersionInfo
	{
		public const int Major = 1;
		public const int Minor = 0;
		public const int Patch = 1;
		public const int Build = 1198;


		public override string ToString()
		{
			return string.Format( "{0}.{1}.{2} build {3}", Major, Minor, Patch, Build );
		}
	}
}











































































































































