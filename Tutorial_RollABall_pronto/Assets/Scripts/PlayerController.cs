using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Vector3 startPosition;
    private bool gameWon = false;
    private float timer = 0.0f; // Variável para manter o tempo decorrido

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        restartTextObject.SetActive(false);
        startPosition = transform.position;
        timerTextObject.text = "Tempo: 00:00"; // Inicializar o texto do timer
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void Update()
    {
        if (!gameWon) // Atualizar o timer apenas se o jogo não estiver ganho
        {
            timer += Time.deltaTime; // Aumentar o timer com o tempo desde o último frame
            int minutes = (int)timer / 60; // Converter o tempo total em minutos
            int seconds = (int)timer % 60; // Converter o tempo restante em segundos
            timerTextObject.text = string.Format("Tempo: {0:00}:{1:00}", minutes, seconds); // Atualizar o texto do timer
        }

        if ((transform.position.y < -3 || gameWon) && Input.GetKeyDown(KeyCode.Space))
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
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText() 
    {
        countText.text = "Contagem: " + count.ToString();
        if (count >= 23)
        {
            int minutes = (int)timer / 60;
            int seconds = (int)timer % 60;
            string finalTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            // Incluímos a mensagem de tempo decorrido na mensagem de vitória existente
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Você venceu!\nTempo total: " + finalTime + "\n\nAperte 'Barra de Espaço' para recomeçar";
            winTextObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            winTextObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            winTextObject.GetComponent<TextMeshProUGUI>().fontSize = 32;
            winTextObject.SetActive(true);
            gameWon = true; // Marca o jogo como ganho para parar o timer
        }
    }
}
