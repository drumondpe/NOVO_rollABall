using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    public void Jogar()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

}
