using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RegisterChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI")]

    public Animator transicion;
    public string sceneName;
    private float transitionTime = 1f;
    void Start()
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transicion.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}