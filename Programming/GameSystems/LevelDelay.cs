using UnityEngine;
using System.Collections;

public class LevelDelay : MonoBehaviour
{
    public AudioSource musicPlayer;
    private AudioSimple countdown; 

    void Awake()
    {
        countdown = this.GetComponent<AudioSimple>();
    }

	void Start ()
    {
        countdown.PlayAudioClip();
        GameManager.Instance.startGametime = false;
        GameManager.Instance.StopPlayerMovement();

        StartCoroutine(DelayLevel());
	}
	
    IEnumerator DelayLevel()
    {
        yield return new WaitForSeconds(countdown.Source.clip.length);
        GameManager.Instance.startGametime = true;
        GameManager.Instance.StartPlayerMovement();
        musicPlayer.Play();
    }

}
