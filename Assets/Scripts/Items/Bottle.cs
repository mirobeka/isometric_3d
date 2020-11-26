using UnityEngine;

public class Bottle : Interactable
{
    private bool playable;

    void Awake(){
        playable = false;
    }

    public override void Interact(){
        if (!playable)
            return;
        GetComponent<Animator>().Play("PourAlcohol");
        // spusti ďalšiu animáciu
        //GameManager.Instance.
    }

    void OnEnable()
    {
        playable = true;
    }

    void OnDisable()
    {
        playable = false;
    }
}
