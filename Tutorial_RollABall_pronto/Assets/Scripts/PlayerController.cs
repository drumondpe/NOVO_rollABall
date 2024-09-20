using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject restartTextObject;
    public TextMeshProUGUI timerTextObject; // Objeto de texto para o timer
    private float timer = 35.0f; // 35 segundos para a contagem regressiva

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        restartTextObject.SetActive(false);
        timerTextObject.text = FormatTime(timer); // Inicializar o texto do timer
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate() 
    {
        if (timer > 0) // Permitir movimento apenas se ainda houver tempo
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        return string.Format("Tempo: {0:00}:{1:00}", minutes, seconds);
    }

    void Update()
    {
        if (timer > 0) // Continuar a decrementar o timer se houver tempo restante
        {
            timer -= Time.deltaTime;
            timerTextObject.text = FormatTime(timer);
        }
        else if (!restartTextObject.activeSelf)
        {
            restartTextObject.GetComponent<TextMeshProUGUI>().text = "Tempo Esgotado!\nAperte 'Barra de Espaço' para tentar novamente";
            restartTextObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (transform.position.y < -3)
        {
            restartTextObject.SetActive(true);
        }
        else
        {
            restartTextObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    void SetCountText() 
    {
        countText.text = "Contagem: " + count.ToString();
        if (count >= 23 && timer > 0)
        {
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Você venceu!\nTempo restante: " + FormatTime(timer) + "\nAperte 'Barra de Espaço' para recomeçar";
            winTextObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            winTextObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            winTextObject.GetComponent<TextMeshProUGUI>().fontSize = 32;
            winTextObject.SetActive(true);
            timer = 0; // Para a contagem regressiva
        }
    }
}
