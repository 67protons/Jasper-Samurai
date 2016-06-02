using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuSelection : MonoBehaviour 
{
    public GameObject SelectionPanel, CutscenePanel;
    public Sprite[] cutscenes;
    public AudioSource mainTrack;

    public void startGame()
    {
        SelectionPanel.SetActive(false);
        PlayCinematic();
        //SceneManager.LoadScene(1);
        StartCoroutine(EndCutscene());
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void PlayCinematic()
    {
        Image panelImage = CutscenePanel.GetComponent<Image>();
        Animator panelAnimator = CutscenePanel.GetComponent<Animator>();
        panelAnimator.Play("fadeIn");

        //for (int i = 0; i < 3; i++)
        //{
        //    cutscene.sprite = cutscenes[i];
        //}
    }

    private IEnumerator EndCutscene()
    {
        yield return new WaitForSeconds(18f);
        float startTime = Time.time;
        while (Time.time < startTime + 2f)
        {
            mainTrack.volume = (2 - (Time.time - startTime));
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(1);
    }
}
