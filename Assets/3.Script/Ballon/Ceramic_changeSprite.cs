using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceramic_changeSprite : MonoBehaviour
{
    [SerializeField]
    Sprite Ceramic_0;
    [SerializeField]
    Sprite Ceramic_1;
    [SerializeField]
    Sprite Ceramic_2;
    [SerializeField]
    Sprite Ceramic_3;
    [SerializeField]
    Sprite Ceramic_4;

    [SerializeField]
    int hp = 10;
    SpriteRenderer spriteRenderer;
    BloonManager bloonmanger;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bloonmanger = GetComponent<BloonManager>();
    }
    private void Update()
    {
        hp = bloonmanger.hp;
        switch (hp)
        {
            case 10: 
                spriteRenderer.sprite = Ceramic_0;
                break;
            case 8:
                spriteRenderer.sprite = Ceramic_1;
                break;
            case 6:
                spriteRenderer.sprite = Ceramic_2;
                break;
            case 4:
                spriteRenderer.sprite = Ceramic_3;
                break;
            case 2:
                spriteRenderer.sprite = Ceramic_4;
                break;
        }
    }
}
