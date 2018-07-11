using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RadialButton : MonoBehaviour, IPointerExitHandler,IPointerEnterHandler,IPointerClickHandler {

    public Image circle;
    public Image icon;
    public string title;
    public RadialMenu myMenu;
    public float speed = 10f;
    public int Index { get; set; }

    Color defaultColor;

    public void Anim()
    {
        StartCoroutine(AnimateButtonIn());
    }

    IEnumerator AnimateButtonIn() {
        transform.localScale = Vector3.zero;
        float timer = 0f;
        while (timer < (1 / speed)) {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * timer * speed;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myMenu.selected = this;
        defaultColor = circle.color;
        circle.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myMenu.selected = null;
        circle.color = defaultColor;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        AnimalsManager.Instance.SwapToAnimalNumber(Index);
    }
}
