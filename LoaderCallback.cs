using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool firstUpdate = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            Loader.LoaderCallBack();
        }
    }
}
