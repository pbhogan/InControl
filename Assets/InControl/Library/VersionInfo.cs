using System;


namespace InControl
{
	public struct VersionInfo
	{
		public const int Major = 1;
		public const int Minor = 1;
		public const int Patch = 2;
		public const int Build = 1449;


		public override string ToString()
		{
			return string.Format( "{0}.{1}.{2} build {3}", Major, Minor, Patch, Build );
		}
	}
}











































































































































