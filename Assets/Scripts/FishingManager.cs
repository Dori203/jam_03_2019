using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> fishPrefabs;
    [SerializeField] private List<int> fishCaughtList = new List<int>();
    public static FishingManager SharedInstance;

    [SerializeField] private FishType currentFishArea;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float minPokeTime;
    [SerializeField] private float maxPokeTime;
    [SerializeField] private Animator rodAnimator;
    [SerializeField] private Animator xAnimator;
    [SerializeField] private ParticleSystem catchParticle;
    [SerializeField] private ParticleSystem pokeParticles;
    [SerializeField] private float fishConsumeTime;
    [SerializeField] private int minPokes;
    [SerializeField] private int maxPokes;
    [SerializeField] private GameObject fishBox;

    private float nextFishTimer = 0f;
    private float hungerTimer = 0f;
    private int fishCount;
    private int pokeCount;

    [System.Serializable]
    public enum FishType
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        None
    }
    enum rodState
    {
        Idle,
        Waiting,
        Catch,
    }
    rodState rod = rodState.Idle;

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip poke_sound;
    [SerializeField] private AudioClip catch_sound;
    [SerializeField] private AudioClip escape_sound;

    void Awake()
    {
        SharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        nextFishTimer -= Time.deltaTime;
        hungerTimer -= Time.deltaTime;

        //consume fish
        if (hungerTimer <= 0)
        {
            consumeFish();
            StartCountdown(fishConsumeTime, fishConsumeTime + 2f, true);
        }

        if(currentFishArea != FishType.None)
        {
            switch (rod)
            {
                case rodState.Idle:
                    if (nextFishTimer <= 0)
                    {
                        StartCountdown(minPokeTime, maxPokeTime);
                        SetPokes();
                        Poke();
                        rod = rodState.Waiting;
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        StartCountdown(minIdleTime, maxIdleTime);
                        rodAnimator.Play("pull");
                        rod = rodState.Idle;
                    }

                    break;

                case rodState.Waiting:
                    if (nextFishTimer <= 0)
                    {
                        Poke();
                        StartCountdown(minPokeTime, maxPokeTime);
                    }

                    if ((pokeCount) == 0)
                    {
                        rodAnimator.Play("hold");
                        // Debug.Log("hold");
                        rod = rodState.Catch;
                        break;
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
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
                    if (nextFishTimer <= 0)
                    {
                        rodAnimator.Play("pull");
                        // Debug.Log("escape");
                        xAnimator.Play("appear");
                        audio.PlayOneShot(escape_sound);
                        StartCountdown(minIdleTime, maxIdleTime);
                        rod = rodState.Idle;
                        break;
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        //rodAnimator.Play("catch");
                        rodAnimator.Play("pull");
                        // Debug.Log("catch");
                        audio.PlayOneShot(catch_sound);
                        fishCount += 1;
                        SpawnFish();
                        fishHit((int)currentFishArea);
                        Debug.Log("caught fish with type index:");
                        Debug.Log((int)currentFishArea);
                        StartCountdown(minIdleTime, maxIdleTime);
                        rod = rodState.Idle;
                        break;
                    }

                    break;
            }
        }
        else
        {
            StartCountdown(minIdleTime, maxIdleTime);
            rod = rodState.Idle;
        }

    }

    public void fishHit(int fishTypeIndex)
    {
        //check if fish already caught.
        if (!fishCaughtList.Contains(fishTypeIndex))
        {
            fishCaughtList.Add(fishTypeIndex);

            //increase fishing score.
            GameManager.Instance.incFishingScore();

            //broadcast
            GameManager.Instance.NewFishCaught(fishTypeIndex);
        }
    }

    public void setFishingArea(FishType fishType)
    {
        currentFishArea = fishType;
    }

    public FishType getFishingArea()
    {
        return currentFishArea;
    }

    private void consumeFish()
    {
        Debug.Log("consuming fish!");
        //if there are fish caught - consume one of them (don't remove scoring given for capture)
        if (fishCount > 0)
        {
            fishCount--;
            Debug.Log("trying to remove a fish");
            //deactivate a random child of fishbox.
            Transform fish = fishBox.transform.GetChild(Random.Range(0, fishBox.transform.childCount));
            fish.SetParent(null);
            fish.gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.decFishingHealth(Random.Range(1, 9 - fishCount));
        }

    }

    private void StartCountdown(float min, float max, bool hunger = false)
    {
        if (hunger)
        {
            hungerTimer = Random.Range(min, max);
        }
        else
        {
            nextFishTimer = Random.Range(min, max);
        }
    }

    private void SetPokes()
    {
        pokeCount = Random.Range(minPokes, maxPokes);
    }

    private void Poke()
    {
        pokeCount -= 1;
        rodAnimator.Play("poke");
        pokeParticles.Play();
        audio.PlayOneShot(poke_sound);
        // Debug.Log("poke");
    }

    private void SpawnFish()
    {
        catchParticle.Play();
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        int typeIndex = (int)currentFishArea;
        Debug.Log("Instantiating a fish at index");
        Debug.Log(typeIndex);
        GameObject currentFish = Instantiate(fishPrefabs[typeIndex], fishBox.transform.position + offset,
        Quaternion.Euler(90, Random.Range(0, 360), 0), transform);
        currentFish.transform.SetParent(fishBox.transform);
    }

}
