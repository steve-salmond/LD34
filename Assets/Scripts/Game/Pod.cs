using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DG.Tweening;

public class Pod : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The list of slots on this pod. */
    public List<PodSlot> Slots
    { get; private set; }

    /** Delay between pod advancements. */
    public float StepTime;

    /** Amount to move when advancing. */
    public Vector3 StepOffset;

    /** Number of steps before delivery. */
    public int StepsToDeliver;

    /** Number of steps between delivery and death. */
    public int StepsToDeath;

    /** Pod capsule. */
    public Transform Capsule;

    /** Overall human display. */
    public Transform Human;

    /** Fetus growth stage. */
    public Transform Fetus;

    /** Baby growth stage. */
    public Transform Baby;

    /** Child growth stage. */
    public Transform Child;

    /** Adult growth stage. */
    public Transform Adult;

    /** Growth bubble effect. */
    public ParticleSystem GrowthBubbles;

    /** Water. */
    public MeshRenderer Water;

    /** Good water color. */
    public Color GoodWaterColor;
    
    /** Bad water color. */
    public Color BadWaterColor;

    /** Return the current score for this pod. */
    public int Score
    { get { return Slots.Sum(slot => slot.Score); } }

    /** Return whether pod is acceptable for delivery. */
    public bool IsGood
    { get { return Growth >= 1; } }

    /** Get the pod's growth fraction. */
    public float Growth
    { get { return ((float) (Slots.Count(slot => slot.IsGood)) / Slots.Count); } }


    // Private Properties
    // -----------------------------------------------------

    /** Target growth fraction. */
    private float GrowthTarget;



    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Get game controller.
        var controller = GameController.Instance;

        // Configure the pod.
        StepTime = controller.PodStepTime;

        // Activate slots.
        int min = controller.PodSlotCountMin;
        int max = controller.PodSlotCountMax;
        int count = Random.Range(min, max + 1);
        var slots = GetComponentsInChildren<PodSlot>();
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(count > 0);
            count--;
        }

        // Get list of active slots.
        Slots = slots.Where(slot => slot.gameObject.activeSelf).ToList();

        // Start out with zero growth.
        SetGrowth(0);

        // Fire up the pod control routine.
        StartCoroutine(UpdateRoutine());
    }

    /** Handle objects hitting the pod. */
    private void OnTriggerEnter(Collider other)
    {
        var blob = other.GetComponent<NutrientBlob>();
        if (blob == null)
            return;

        Consume(blob);
    }


    // Coroutines
    // -----------------------------------------------------

    /** Update the pod. */
    private IEnumerator UpdateRoutine()
    {
        // Advance the pod until it's time to deliver.
        for (int i = 0; i < StepsToDeliver; i++)
            yield return StartCoroutine(Advance());

        // Deliver the pod.
        Deliver();
        yield return new WaitForSeconds(0.5f);

        // Advance until it's time to die.
        for (int i = 0; i < StepsToDeath; i++)
            yield return StartCoroutine(Advance());

        // Kill the pod.
        Destroy(gameObject);
    }

    /** Advances the pod by one step. */
    private IEnumerator Advance()
    {
        transform.DOMove(StepOffset, StepTime).SetRelative();
        yield return new WaitForSeconds(StepTime);
    }


    // Private Methods
    // -----------------------------------------------------

    /** Consume a nutrient blob. */
    private void Consume(NutrientBlob blob)
    {
        // Ask each slot if it wants the nutrient.
        foreach (var slot in Slots)
            if (slot.Consume(blob))
            {
                UpdateGrowth();
                break;
            }

        // Kill the blob.
        Destroy(blob.gameObject);
    }

    /** Deliver the pod. */
    private void Deliver()
    {
        // Update game score.
        GameController.Instance.Deliver(this);

        // Shake the pod a bit.
        transform.DOPunchScale(Vector3.one * 0.05f, 0.5f);

        // Move pod up or down depending on whether it passes.
        if (IsGood)
            Capsule.DOMoveY(10, 0.5f).SetRelative();
        else
            Capsule.DOMoveY(-10, 0.5f).SetRelative();

    }

    /** Update the growth stage. */
    private void UpdateGrowth()
    {
        // Shake the pod a bit.
        transform.DOPunchScale(Vector3.one * 0.05f, 0.5f);

        // Shake the human as it grows.
        Human.DOPunchScale(new Vector3(0.5f, 0, 0.5f), 1.5f, 5);

        // Play the growth bubble effect.
        GrowthBubbles.Play();

        // Set the new growth fraction.
        var old = GrowthTarget;
        GrowthTarget = Growth;
        DOTween.To(SetGrowth, old, GrowthTarget, 1);

        // Determine new water color.
        float fullSlots = Slots.Count(slot => slot.IsFull);
        if (fullSlots > 0)
        {
            var ratio = Slots.Count(slot => slot.IsGood) / fullSlots;
            var c = Color.Lerp(BadWaterColor, GoodWaterColor, ratio);
            Water.material = new Material(Water.material);
            Water.material.EnableKeyword("_EMISSION");
            Water.material.DOColor(c, "_EmissionColor", 1);
            Water.material.DOColor(c, 1);
        }

    }

    /** Set the growth stage value. */
    private void SetGrowth(float value)
    {
        Fetus.gameObject.SetActive(false);
        Baby.gameObject.SetActive(false);
        Child.gameObject.SetActive(false);
        Adult.gameObject.SetActive(false);

        if (value < 0.1f)
        {
            Fetus.gameObject.SetActive(true);
        }
        else if (value < 0.35f)
        {
            Baby.gameObject.SetActive(true);
        }
        else if (value < 0.7f)
        {
            Child.gameObject.SetActive(true);
        }
        else
        {
            Adult.gameObject.SetActive(true);
        }

    }

}
