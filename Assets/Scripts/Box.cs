using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteArray;
    [SerializeField] private SpriteRenderer blockPrefab;
    [SerializeField] private SpriteRenderer borderPrefab; 
    [SerializeField] private TextMeshProUGUI hitText;
    
    private int hitPoints;
    private Color32 blockColor;
    private Color32 borderColor;
    private Sprite blockSprite;
    private int specialBoxType;
    private Vector2 boxGridPos;

    void Start()
    {
        borderColor = borderPrefab.color;
        blockPrefab.color = blockColor;
        blockPrefab.sprite = blockSprite;
        hitText.text = hitPoints.ToString();
    }

    
    public void SetAbsolutePosX(int x)
    {
        boxGridPos.x = x;
    }

    public void SetAbsolutePosY(int y)
    {
        boxGridPos.y = y;
    }

    public void UpdateAbsolutePosY()
    {
        boxGridPos.y -= 1;
    }

    public void SetTeamColor(Color32 teamValue)
    {
        blockColor = teamValue;
    }

    public void SetBoxSprite(int spriteIndex)
    {
        specialBoxType = spriteIndex;
        blockSprite = spriteArray[specialBoxType];
    }

    public void SetHitPoints(int value)
    {
        hitPoints = value;
        //Debug.Log("hitPoints = " + hitPoints);
    }

    private void Flashing()
    {
        StartCoroutine(FadeOut());
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        for (int i = 255; i >= 5; i -= 50)
        {
            borderColor.a = (byte)i;
            blockColor.a = (byte)i;
            borderPrefab.color = borderColor;
            blockPrefab.color = blockColor;
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator FadeIn()
    {
        for (int j = 5; j <= 255; j += 50)
        {
            borderColor.a = (byte)j;
            blockColor.a = (byte)j;
            borderPrefab.color = borderColor;
            blockPrefab.color = blockColor;
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void DestroyEffect()
    {
        StartCoroutine(DissolveBox());
    }

    IEnumerator DissolveBox()
    {
        int j;
        blockPrefab.material.shader = Shader.Find("Shader Graphs/Dissolve");
        for (float i = 1f; i >= 0f; i -= 0.1f)
        {
            j = (int)i*250;
            borderColor.a = (byte)j;
            borderPrefab.color = borderColor;
            blockPrefab.material.SetFloat("_Fade", i);
            yield return new WaitForSeconds(0.05f);
        }
        DestroyBoxSound.destroyedBoxesCount++;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //When a special box destroyed by anoher special box
        if (specialBoxType > 0)
            DestroyBySpecialBox(specialBoxType);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When box is hit
        if (collision.gameObject.tag == "Ball")
        {
            //Before being destroyed, destroy appropriate boxes
            if (hitPoints < 2)
            {
                DestroyBySpecialBox(specialBoxType);
                DestroyEffect();
            }
            else
            {
                //Otherwise decrease hit points
                hitPoints--;
                Flashing();
                hitText.text = hitPoints.ToString();
            }
        }
    }

    private void DestroyBySpecialBox(int specialBoxType)
    {
        Box[] boxes = FindObjectsOfType<Box>();
        //Debug.Log("Number of boxes = " + boxes.Length);
        switch (specialBoxType)
        {
            case 1: //destroy all surrounding boxes
                foreach (Box box in boxes)
                {
                    if ((((boxGridPos.x - 1) <= box.boxGridPos.x) && ((boxGridPos.x + 1) >= box.boxGridPos.x) && ((boxGridPos.y + 1) == box.boxGridPos.y) || ((boxGridPos.y - 1) == box.boxGridPos.y))
                        || ((((boxGridPos.x - 1) == box.boxGridPos.x) || ((boxGridPos.x + 1) == box.boxGridPos.x)) && (boxGridPos.y == box.boxGridPos.y)))
                        box.DestroyEffect();
                }
                break;
            case 2: //destroy all boxes in same row
                foreach (Box box in boxes)
                {
                    if (boxGridPos.y == box.boxGridPos.y)
                        box.DestroyEffect();
                }
                break;
            case 3: //destroy all boxes in same column
                foreach (Box box in boxes)
                {
                    if ((boxGridPos.x == box.boxGridPos.x) && (box.boxGridPos.y <= 5) && (box.boxGridPos.y >= -5))
                        box.DestroyEffect();
                }
                break;
            case 4: //destroy all boxes in same row and column 
                foreach (Box box in boxes)
                {
                    if ((boxGridPos.y == box.boxGridPos.y) || ((boxGridPos.x == box.boxGridPos.x) && (box.boxGridPos.y <= 5) && (box.boxGridPos.y >= -5)))
                        box.DestroyEffect(); 
                }
                break;
        }
    }

    public void NextLevel()
    {
        if (boxGridPos.y < 6 || boxGridPos.y > 7)
        {
            gameObject.transform.position -= new Vector3(0, 1.28f);
            UpdateAbsolutePosY();
        }
        else if (boxGridPos.y == 7)
        {
            gameObject.transform.position -= new Vector3(0, (2 * 1.28f));
            UpdateAbsolutePosY();
            UpdateAbsolutePosY();
        }
        else { }
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
    }*/
}