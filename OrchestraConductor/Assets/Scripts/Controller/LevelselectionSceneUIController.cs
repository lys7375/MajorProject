using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelselectionSceneUIController : MonoBehaviour
{
    public Button game0;
    public Button game1;
    public Button game2;

    // Start is called before the first frame update
    void Start()
    {
        game0.onClick.AddListener(Game0ButtonOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Game0ButtonOnClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
