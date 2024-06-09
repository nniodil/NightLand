using UnityEngine;

public class ImageScript : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * -600);
    }
}
