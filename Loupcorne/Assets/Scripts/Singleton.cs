using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;
	public static T Instance
	{
		get
		{
			if(_instance == null)
			{
				//Debug.Log("Looking for instance of " + typeof(T));
				_instance = GameObject.FindObjectOfType(typeof(T)) as T;
			
				if(_instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
				}
			}
			return _instance;
		}
	}
}
