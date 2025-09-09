using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShakeController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public float intenseAmplitude = 5f;
    public float intenseFrequency = 2f;
    public float shakeDuration = 0.5f;

    private CinemachineBasicMultiChannelPerlin noise;
    private float defaultAmplitude;
    private float defaultFrequency;

    void Start()
    {
        virtualCam = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        virtualCam.Follow = transform;
        virtualCam.LookAt = transform;
        noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        defaultAmplitude = noise.m_AmplitudeGain;
        defaultFrequency = noise.m_FrequencyGain;
    }

    public void ShakeOnDeath()
    {
        StopAllCoroutines(); // stop any ongoing shake resets
        StartCoroutine(IntenseShake());
    }

    IEnumerator IntenseShake()
    {
        noise.m_AmplitudeGain = intenseAmplitude;
        noise.m_FrequencyGain = intenseFrequency;
        yield return new WaitForSeconds(shakeDuration);

        noise.m_AmplitudeGain = defaultAmplitude;
        noise.m_FrequencyGain = defaultFrequency;
    }
}