using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartSceneScreens : MonoBehaviour
{
    public List<GameObject> _textsMP;

    private int numberOfPokes;
    private TMP_Text _petitspointsText;
    private bool onCoroutine;
    void Start()
    {
        numberOfPokes = 0;
        for (int i = 1; i < _textsMP.Count; i++)
        {
            _textsMP[i].SetActive(false);
        }
    }

    public void RedButtonPoked()
    {
        // we increase the number of pokes
        numberOfPokes += 1;

        // and change the text on the screen
        if (!onCoroutine && numberOfPokes < _textsMP.Count)
        {
            _textsMP[numberOfPokes - 1].SetActive(false);
            _textsMP[numberOfPokes].SetActive(true);

            // for the trois petits points
            if (numberOfPokes == 2)
            {
                StartCoroutine(TroisPetitsPoints());
            }
        }

        // once the player has pressed enough times, we load the scene
        // - TODO - Animation du player qui rentre dans l'ecran ? 
        if (numberOfPokes >= _textsMP.Count)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator TroisPetitsPoints()
    {
        onCoroutine = true;
        for (int i = 0; i<3; i++)
        {
            yield return new WaitForSeconds(1.0f);
            _textsMP[2].GetComponent<TextMeshProUGUI>().text += ".";
        }

        yield return new WaitForSeconds(1.0f);
        // pokes have no effects during the coroutine
        numberOfPokes = 2;
        onCoroutine = false;
    }
}
