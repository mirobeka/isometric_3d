using UnityEngine;

public class AutoFold : MonoBehaviour {

    private GameObject normalObject;
    private GameObject foldedObject;
    private bool shouldBeFolded = false;
    private float timer = 0.0f;
    public AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public void BeFolded() {
        shouldBeFolded = true;
    }

    private void Start() {
        normalObject = transform.Find("FullSize").gameObject;

        // Tento celý blok môže ísť preč, viď koment vnútri
        Transform foldedTransform = transform.Find("CutSize");
        if(foldedTransform != null){
            foldedObject = foldedTransform.gameObject;
            Debug.Log("found folded version");

            // FoldedObject sa ani nepoužíva, môžeme úplne vypnúť
            foldedObject.SetActive(false);
        }

        // prehodenie kolajdru z normalObject na tento objekt a vypnutie pôvodného
        // ak sa nevypne pôvodný, tak hráča vedia dvere vystreliť do preč a ostane len čierna obrazovka
        gameObject.AddComponent<MeshFilter>().mesh = normalObject.GetComponent<MeshFilter>().sharedMesh;
        gameObject.AddComponent<MeshCollider>().convex = true;
        normalObject.GetComponent<MeshCollider>().enabled = false;
    }

    // easing krivka má najmenšiu hodnotu okolo 0.2, t.j. celý objekt sa zmenší na 20% veľkosti
    // týmto sa simuluje CutSize objekt
    // keď je 0.0, tak preblikávajú nejaké svetlá zo spodu
    // niektoré objekty v scéne nemáš ako prefaby (tie veľké čierne plochy), tak som im musel ručne skopírovať hodnotu krivky
    private void Update() {
        timer += Time.deltaTime * (shouldBeFolded ? -1 : +1);
        timer = Mathf.Clamp(timer, 0, 1);
        normalObject.transform.localScale = new Vector3(1, easing.Evaluate(timer), 1);

        shouldBeFolded = false;
    }
}

