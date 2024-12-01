using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject objectiveText;
    BeginningCell cell;
    [SerializeField] public int nbOfTransportersForVictory;

    private IEnumerator NextLevel(int index)
    {
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }
    private void Start()
    {
        cell=BeginningCell.instance;
        objectiveText.GetComponent<TextMeshProUGUI>().text = $"Transporters necessary to win : {nbOfTransportersForVictory}";
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
                int indexOfScene;
                endText.SetActive(true);
                if (cell.nbTransporters < nbOfTransportersForVictory)
                {
                    endText.GetComponent<TextMeshProUGUI>().text = $"{cell.nbTransporters} transporters left, {nbOfTransportersForVictory} were necessary to win. Try again.";
                    indexOfScene=SceneManager.GetActiveScene().buildIndex;
                }
                else
                {
                    endText.GetComponent<TextMeshProUGUI>().text = $"{cell.nbTransporters} transporters left. That's enough material for the next colony. Good Job.";
                    indexOfScene = SceneManager.GetActiveScene().buildIndex+1;
                }
                StartCoroutine(NextLevel(indexOfScene));
                Time.timeScale = 0;
                
            }
        }
    }
}
