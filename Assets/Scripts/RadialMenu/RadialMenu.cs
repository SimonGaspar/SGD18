using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {

    public GameObject buttonPrefab;
    public RadialButton selected;
    private RadialButton button;
    public float menuRadius = 100f;
    bool _showMenu = false;

    // Use this for initialization
    public void SpawnButtons (AnimalsManager.FormPrefabs []obj) {
        StartCoroutine(AnimateButtons(obj));
	}

    public IEnumerator AnimateButtons(AnimalsManager.FormPrefabs[] obj) {
        for (int i = 0; i < obj.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(transform, false);
            float theta = (2 * Mathf.PI / obj.Length) * i;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * menuRadius;
            button = newButton.GetComponent<RadialButton>();
            button.icon.sprite = obj[i].sprite;
            button.circle.color = obj[i].color;
            button.title = obj[i].title;
            button.myMenu = this;
            _showMenu = true;
            button.Index = i;
            button.Anim();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("MenuHero") && _showMenu)
        {
            _showMenu = false;
            Destroy(gameObject);
        }
    }

    public bool IsMenuShowed() {
        return _showMenu;
    }

}
