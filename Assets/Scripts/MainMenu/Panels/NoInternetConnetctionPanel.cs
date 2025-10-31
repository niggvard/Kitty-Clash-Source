using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoInternetConnetctionPanel : PanelUI<bool>
{
    [SerializeField] private GameObject fade;

    private static NoInternetConnetctionPanel instance;
    private GameObject currentFade;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            try
            {
                Destroy(instance.gameObject);
            }
            catch { }
        }
        instance = this;
    }

    protected override void Start()
    {
        base.Start();

        DontDestroyOnLoad(gameObject);
        StopAllCoroutines();
        StartCoroutine(InternetCheck());
    }

    private IEnumerator InternetCheck()
    {
        while (true)
        {
            if (currentFade != null)
                Destroy(currentFade);

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                ShowPanel(true);
                yield break;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                currentFade = Instantiate(fade);
                SceneManager.LoadScene(1);
            }

            yield return new WaitForSeconds(1);
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }
}
