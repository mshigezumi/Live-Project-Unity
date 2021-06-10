using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerBallTriggers : MonoBehaviour
{
    //Used by UI script
    public static bool ballInHole { get; private set; }

    private static float timeInHole;

    GameObject fireworks;
    string fireworksTag = "Fireworks";

    void Start()
    {
        ballInHole = false;
        fireworks = GameObject.FindGameObjectWithTag(fireworksTag);
        fireworks.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Play InHole sound when ball enters hole trigger
        if (collider.tag == "Hole")
        {
            // Play Hole Trigger sound
            AudioManager.instance.Play("InHole");
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Hole")
        {
            ballInHole = true;
            timeInHole += Time.deltaTime;
        }
        if (timeInHole > 1.5f)
        {
            StartCoroutine(WaitForSceneLoad());
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Hole")
        {
            ballInHole = false;
            timeInHole = 0f;
        }
    }
    private IEnumerator WaitForSceneLoad()
    {
        if (fireworks != null)
        {
            fireworks.SetActive(true);
            yield return new WaitForSeconds(5);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
