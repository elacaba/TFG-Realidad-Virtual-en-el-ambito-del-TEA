using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public Animator transicion;
    private float transitionTime = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selection()
    {
            Debug.Log("que pasa");
            StartCoroutine(LoadLevel(sceneName));

    }

    IEnumerator LoadLevel(string sceneName)
    {
        transicion.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

    }
}
