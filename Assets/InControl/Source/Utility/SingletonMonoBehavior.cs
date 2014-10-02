using UnityEngine;


namespace InControl
{
	public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }

		private static object _lock = new object();


		protected void SetSingletonInstance()
		{
			lock (_lock)
			{
				if (Instance == null)
				{
					var instances = FindObjectsOfType<T>() as T[];
					if (instances.Length > 0)
					{
						Instance = instances[0];

						if (instances.Length > 1)
						{
							Debug.LogWarning( "Multiple instances of singleton " + typeof(T) + " found." );
						}
					}
					else
					{
						Debug.LogError( "No instance of singleton " + typeof(T) + " found." );
					}
				}
			}
		}
	}
}