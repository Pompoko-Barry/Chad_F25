using UnityEngine;
using System.Collections;

public class SetFalse : MonoBehaviour
{
    private float lifetime = 10f;
    private DartManager dartManager;

    private Coroutine timerCoroutine;

    public float Lifetime { get => lifetime; set => lifetime = value; }
    public DartManager DartManager { get => dartManager; set => dartManager = value; }
    public Coroutine TimerCoroutine { get => timerCoroutine; set => timerCoroutine = value; }

    //for particles
   [SerializeField] private ParticleSystem deactivateEfffect;

    public void StartCountdown()
    {
        if (TimerCoroutine != null)
            StopCoroutine(TimerCoroutine);

        TimerCoroutine = StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        //wait until 0.5 secodns before deactivation to play particles
        yield return new WaitForSeconds(Lifetime - 0.5f);

        if (deactivateEfffect != null)
        {
            //deactivateEfffect.transform.position = transform.position;
            //deactivateEfffect.Play();
            ParticleSystem effect = Instantiate(deactivateEfffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }

        //wait for the remaining 0.5 seconds until setting the dart false
        yield return new WaitForSeconds(0.5f);
        

        if (DartManager != null)
        {
            DartManager.ResetDart();
        }
             gameObject.SetActive(false);
    }
}
