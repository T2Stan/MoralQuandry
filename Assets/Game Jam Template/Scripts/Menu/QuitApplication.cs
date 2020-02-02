using UnityEngine;
using System.Collections;

public class QuitApplication : MonoBehaviour {

	[Header("Audio Sources")]
	[SerializeField] private AudioSource QuitSound;

	public void QuitWithSound()
	{
		QuitSound.Play();
		StartCoroutine(Wait(QuitSound.clip.length));
	}

	private IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
		Quit();
	}

	public void Quit()
	{
		//If we are running in a standalone build of the game
	#if UNITY_STANDALONE
		//Quit the application
		Application.Quit();
	#endif

		//If we are running in the editor
	#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
	#endif
	}
}
