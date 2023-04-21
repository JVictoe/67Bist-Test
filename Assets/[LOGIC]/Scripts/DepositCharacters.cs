using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepositCharacters : MonoBehaviour
{
    [SerializeField] private Image timeIndicator = default;
    [SerializeField] private GameObject indicatoObject = default;

    public float timeToRemove;
    public bool inPlace;
    private float time;

    private PlayerController player;

    private void Update()
    {
        if (inPlace && player.PlayerCombat.Childs.Count >= 1)
        {
            time += Time.deltaTime;
            timeIndicator.fillAmount = time / 2;
            if (time > timeToRemove)
            {
                time = 0f;
                timeIndicator.fillAmount = 0f;
                RemoveCharacter();
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            indicatoObject.SetActive(false);
            player = other.gameObject.GetComponent<PlayerController>();
            inPlace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        indicatoObject.SetActive(true);
        player = null;
        time = 0f;
        timeIndicator.fillAmount = 0f;
        inPlace = false;
    }

    private void RemoveCharacter()
    {
        if (player != null)
        {
            List<Transform> t = player.PlayerCombat.Childs;

            int index = t.Count - 1;

            t[index].parent = null;
            t[index].gameObject.SetActive(false);
            t.RemoveAt(index);

            PlayerStatusManager.Instance.AddMoney(10);
            PlayerStatusManager.Instance.SetMaxPlayer(true);
        }
    }
}
