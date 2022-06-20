using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    public static string sceneName;
    public Text mensaje;
    public Animator transicion;
    public Button boton;
    private float transitionTime = 1f;
    public bool esMonitor;

    public static StartGameMonitor instance { get; private set; }
    void Start()
    {
        boton.interactable = false;
    }

    public void gettingName()
    {
        esMonitor = true;
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transicion.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Found Data persistance manager in scene");
        instance = this;
    }

    public void setSceneName()
    {
        sceneName = this.GetComponentInChildren<Transform>().transform.GetChild(1).name;
        if (sceneName != null)
            boton.interactable = true;
    }

}
