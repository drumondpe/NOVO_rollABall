using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement; // Certifique-se de incluir este namespace

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
    private Vector3 startPosition;
    private bool gameWon = false; // Adicione esta variável para manter o estado de vitória

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        restartTextObject.SetActive(false);
        startPosition = transform.position;
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
        // Verificar se o jogador caiu ou ganhou e pressionou espaço
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
            winTextObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            winTextObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            winTextObject.SetActive(true);
            gameWon = true; // Definir o estado de vitória como verdadeiro
        }
    }
}
