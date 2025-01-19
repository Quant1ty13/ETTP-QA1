using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
    public GameObject Tip;
    public PlayerInput playerInput;
    public PlayerHandler playerHandler;
    private bool TipShown;
    [SerializeField] private TextMeshProUGUI tipHeader;
    [SerializeField] private TextMeshProUGUI tipDetails;
    [SerializeField] private RawImage tipImage;

    [Header("Tip Details")]
    [SerializeField] private string TipHeader;
    [SerializeField, TextArea(2, 5)] private string TipDetails;
    [SerializeField] private Texture TipImage;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && TipShown == false)
        {
            Time.timeScale = 0;
            playerHandler.rb2d.velocity = Vector2.zero;
            playerHandler.enabled = false;
            playerInput.enabled = false;
            TipShown = true;
            Tip.SetActive(true);
            tipHeader.text = TipHeader;
            tipDetails.text = TipDetails;
            tipImage.texture = TipImage;
        }
    }

    public void ExitTip()
    {
        Time.timeScale = 1;
        playerInput.enabled = true;
        Tip.SetActive(false);
        playerHandler.enabled = true;
    }
}
