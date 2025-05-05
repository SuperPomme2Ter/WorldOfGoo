using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject objectiveText;
    [SerializeField] public int nbOfTransportersForVictory;

    GameObject refreshButton;
    Transform buttonPos;

    public void NextLevel()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void Start()
    {
        objectiveText.GetComponent<TextMeshProUGUI>().text = $"Transporters necessary to win : {nbOfTransportersForVictory}";
        refreshButton=endText.transform.parent.GetChild(2).gameObject;
        buttonPos=endText.transform.GetChild(0).GetChild(0);
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<StationCell>(out var finalStation))
        {
            if (collision is BoxCollider2D)
            {
                endText.SetActive(true);
                if (transform.parent.GetChild(1).childCount < nbOfTransportersForVictory)
                {
                    endText.GetComponent<TextMeshProUGUI>().text = $"{transform.parent.GetChild(1).childCount} transporters left, {nbOfTransportersForVictory} were necessary to win. Try again.";
                    buttonPos.gameObject.SetActive(false);
                    refreshButton.transform.position = buttonPos.position;
                }
                else
                {
                    endText.GetComponent<TextMeshProUGUI>().text = $"{transform.parent.GetChild(1).childCount} transporters left. That's enough material for the next colony. Good Job.";

                }
                Time.timeScale = 0;
                
            }
        }
    }
}
