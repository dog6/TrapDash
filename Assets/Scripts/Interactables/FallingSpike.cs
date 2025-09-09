using System.Collections;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{

    public GameObject spike;
    private GameObject spikePrefab;
    [SerializeField] private float gravityScale;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float fallDelay;
    private Rigidbody2D spikeBody;
    private Vector2 defaultPosition;

    void Start()
    {
        defaultPosition = transform.position;
        spikePrefab = spike;

        if (spike.GetComponent<Rigidbody2D>())
        {
            spikeBody = spike.GetComponent<Rigidbody2D>();
            spikeBody.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            LogIfNoSpike();
        }
    }

    void LogIfNoSpike()
    {
        if (spike != null)
        {
            Debug.LogError($"Spike '{spike.transform.name}' is missing Rigidbody2D");
        } else
        {
            Debug.LogError($"No spike gameObject assigned for fallingSpike {transform.name}");
        }
    }

    // Triggers spike to fall
    public void Fall() => StartCoroutine(BeginFall());

    IEnumerator BeginFall()
    {
        yield return new WaitForSeconds(fallDelay);
        Debug.Log($"{spikeBody.name} is being dropped");
        spikeBody.bodyType = RigidbodyType2D.Dynamic; // spike starts falling
        spikeBody.gravityScale = this.gravityScale;
        StartCoroutine(Respawn());
    }


    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        GameObject newSpike = Instantiate(spikePrefab, transform);
        newSpike.transform.position = transform.position;
        spikeBody = newSpike.GetComponent<Rigidbody2D>();
        spikeBody.bodyType = RigidbodyType2D.Static;
    }


}