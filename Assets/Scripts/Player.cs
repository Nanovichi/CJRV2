using UnityEngine;

public class Player : MonoBehaviour
{
    public Minigame minigame;
    private void Start()
    {
        minigame.gameObject.SetActive(true);    
    }
}
