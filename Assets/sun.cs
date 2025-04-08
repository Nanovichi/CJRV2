using UnityEngine;

public class sun : MonoBehaviour
{
    private LightAnchor lightAnchor;



    private void Awake()
    {
        lightAnchor = GetComponent<LightAnchor>();  
    }
    // Update is called once per frame
   
}
