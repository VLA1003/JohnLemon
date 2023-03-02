using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;
    public AudioSource alertSound;
    public GameObject alertSign;

    bool m_IsPlayerInRange;
    public bool m_PlayerIsNotDetectedAfterAlert;
    public float detectionTime;
    public float detectionTimeLimit = 2;
    public float alertTime;
    public float alertTimeCooldown = 2;

    void Start()
    {
        detectionTime = 0;
        alertTime = 0;
        alertSign.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
            m_PlayerIsNotDetectedAfterAlert = false;
            alertSign.SetActive(true);
            alertSound.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            m_PlayerIsNotDetectedAfterAlert = true;
            detectionTime = 0;
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    detectionTime += Time.deltaTime;
                    if (detectionTime > detectionTimeLimit)
                    {
                        gameEnding.CaughtPlayer();
                    }
                }
            }
        }
        if (m_PlayerIsNotDetectedAfterAlert == true)
        {
            alertTime += Time.deltaTime;
            if (alertTime > alertTimeCooldown)
            {
                alertSign.SetActive(false);
                alertTime = 0;
                m_PlayerIsNotDetectedAfterAlert = false;
            }
        }

    }
}
