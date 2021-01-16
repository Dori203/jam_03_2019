using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGame : MonoBehaviour {
    [SerializeField] private List<GameObject> fishTypes;
    [SerializeField] private GameObject fishBox;
    private int fishCount;

    [SerializeField] private ParticleSystem catchParticle;
    [SerializeField] private ParticleSystem pokeParticles;

    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float timer = 0f;

    [SerializeField] private float minPokeTime;
    [SerializeField] private float maxPokeTime;

    [SerializeField] private int minPokes;
    [SerializeField] private int maxPokes;

    private int pokeCount;

    [SerializeField] private AnimationClip pokeAnimation;
    [SerializeField] private AnimationClip catchAnimation;
    [SerializeField] private Animator rodAnimator;
    [SerializeField] private Animator xAnimator;

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip poke_sound;
    [SerializeField] private AudioClip catch_sound;
    [SerializeField] private AudioClip escape_sound;


    enum rodState {
        Idle,
        Waiting,
        Catch,
    }

    rodState rod = rodState.Idle;

    // Start is called before the first frame update
    void Start() {
        StartCountdown(minIdleTime, maxIdleTime);
        pokeCount = 0;
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        switch (rod) {
            case rodState.Idle:
                if (timer <= 0) {
                    StartCountdown(minPokeTime, maxPokeTime);
                    SetPokes();
                    Poke();
                    rod = rodState.Waiting;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    StartCountdown(minIdleTime, maxIdleTime);
                    rodAnimator.Play("pull");
                    rod = rodState.Idle;
                }

                break;

            case rodState.Waiting:
                if (timer <= 0) {
                    Poke();
                    StartCountdown(minPokeTime, maxPokeTime);
                }

                if ((pokeCount) == 0) {
                    rodAnimator.Play("hold");
                    // Debug.Log("hold");
                    rod = rodState.Catch;
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    rodAnimator.Play("pull");
                    // Debug.Log("escape");
                    audio.PlayOneShot(escape_sound);
                    StartCountdown(minIdleTime, maxIdleTime);
                    rod = rodState.Idle;
                    xAnimator.Play("appear");
                    break;
                }

                break;

            case rodState.Catch:
                if (timer <= 0) {
                    rodAnimator.Play("pull");
                    // Debug.Log("escape");
                    xAnimator.Play("appear");
                    audio.PlayOneShot(escape_sound);
                    StartCountdown(minIdleTime, maxIdleTime);
                    rod = rodState.Idle;
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    //rodAnimator.Play("catch");
                    rodAnimator.Play("pull");
                    // Debug.Log("catch");
                    audio.PlayOneShot(catch_sound);
                    fishCount += 1;
                    StartCountdown(minIdleTime, maxIdleTime);
                    rod = rodState.Idle;
                    SpawnFish();
                    break;
                }

                break;
        }
    }

    private void StartCountdown(float min, float max) {
        timer = Random.Range(min, max);
    }

    private void SetPokes() {
        pokeCount = Random.Range(minPokes, maxPokes);
    }

    private void SpawnFish() {
        catchParticle.Play();
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        int typeIndex = Random.Range(0, fishTypes.Count - 1);
        {
            GameObject currentFish = Instantiate(fishTypes[typeIndex], fishBox.transform.position + offset,
                Quaternion.Euler(90, Random.Range(0, 360), 0), transform);
            currentFish.transform.SetParent(fishBox.transform);
        }
    }

    private void Poke() {
        pokeCount -= 1;
        rodAnimator.Play("poke");
        pokeParticles.Play();
        audio.PlayOneShot(poke_sound);
        // Debug.Log("poke");
    }
}