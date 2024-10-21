using UnityEngine;
using TMPro;
using System.Collections;


public class GhostText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI myText;

    private Coroutine myCoroutine;

    public void HandleMiss()
    {
        // Ativa o próprio GameObject
        gameObject.SetActive(true);

        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);

        }

        // Randomize the myText string between 3 different cases
        int randomCase = Random.Range(0, 3); // Generates a random number between 0 and 2

        if (myText != null)
        {
            switch (randomCase)
            {
                case 0:
                    myText.text = "Pelo menos ganhaste a medalha de participação.";
                    break;
                case 1:
                    myText.text = "Não te preocupes, perder faz parte do jogo... mas tu estás te especializando!";
                    break;
                case 2:
                    myText.text = "Quem precisa de vitória quando se tem tanta criatividade na hora de perder?";
                    break;
            }

            // Start coroutine to disable the GameObject after 5 seconds
            myCoroutine = StartCoroutine(DisableAfterTime(5f));
        }
        else
        {
            Debug.LogWarning("myText is not assigned!");
        }
    }

    // Coroutine to disable the GameObject after a delay
    private IEnumerator DisableAfterTime(float delay)
    {
        // Wait for the specified delay (5 seconds in this case)
        yield return new WaitForSeconds(delay);

        // Disable the GameObject
        gameObject.SetActive(false);
    }
}
