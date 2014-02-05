using System;
using UnityEngine;


namespace InControl
{
	public enum LogMessageType
	{
		Info,
		Warning,
		Error
	}


	public struct LogMessage
	{
		public string text;
		public LogMessageType type;
	}


	public class Logger
	{
		public delegate void LogMessageHandler( LogMessage message );
		public static event LogMessageHandler OnLogMessage;


		public static void LogInfo( string text )
		{
			if (OnLogMessage != null)
			{
				var logMessage = new LogMessage() { text = text, type = LogMessageType.Info };
				OnLogMessage( logMessage );
			}
		}


		public static void LogWarning( string text )
		{
			if (OnLogMessage != null)
			{
				var logMessage = new LogMessage() { text = text, type = LogMessageType.Warning };
				OnLogMessage( logMessage );
			}
		}


		public static void LogError( string text )
		{
			if (OnLogMessage != null)
			{
				var logMessage = new LogMessage() { text = text, type = LogMessageType.Error };
				OnLogMessage( logMessage );
			}
		}
	}
}

