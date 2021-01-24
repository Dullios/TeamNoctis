using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	private static T _instance = null;
	private static bool applicationIsQuitting = false;


	public static T instance
	{
		get
		{
			return Instance;
		}
	}


	static T Instance
	{

		get
		{
			if (applicationIsQuitting)
			{
				return null;
			}

			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType(typeof(T)) as T;
				if (_instance == null)
				{

					_instance = new GameObject().AddComponent<T>();
					_instance.gameObject.name = _instance.GetType().Name;
					//print("CREATING NEW " + _instance.gameObject.name);
				}
			}
			return _instance;
		}
	}

	public static bool HasInstance
	{
		get
		{
			return !IsDestroyed;
		}
	}

	static bool IsDestroyed
	{
		get
		{
			if (_instance == null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	protected virtual void Awake()
	{
		Init();
		_instance = this as T;

	}

	protected void Init()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;//Avoid doing anything else
		}

	}

	/// <summary>
	/// Unity can sometimes destroy in random order and create 
	/// a new singleton instance onDestroy
	/// this ensures that shit won't happen.
	/// </summary>
	protected virtual void OnDestroy()
	{
		if (_instance == this)
		{
			//Debug.Log("DESTROYING SINGLETON OF TYPE " + gameObject.name);
			_instance = null;
		}
	}

	protected virtual void OnApplicationQuit()
	{
		_instance = null;
		//print("the application quits");
	}

}