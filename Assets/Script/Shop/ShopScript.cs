using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public static ShopScript Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    [System.Serializable] public class ShopItem
    {
        [Header("Knife Icon")]
        public Sprite icon;
        [Header("Knife UI Image")]
        public Sprite KnifeSprite;
        [Space]
        public int Price;
        public bool IsPurchased = false;
        [Range(0,1)]
        public int isOpened = 0; // Zero = close | One = Open
    }

    public List<ShopItem> ShopItemsList;

    public Animator animator;

    GameObject ItemTemplate;
    GameObject g;
    [SerializeField] Transform ShopScrollView;
    Button buyButton;

    public Profile profile;

    public class Avatar
    {
        public Sprite Image;
    }

    public List<Avatar> AvatarList;
    [SerializeField] GameObject AvatarUITemplate;

    public Color openedColor;
    public Color clickColor;
    public Color openItemColor;

    [Header("Apple Shop")]
    public bool isPurchasedApple;
    public Sprite apple;


    private void Start()
    {
        ItemTemplate = ShopScrollView.GetChild(0).gameObject;

        int len = ShopItemsList.Count;
        for (int i = 0; i < len; i++)
        {
            ShopItemsList[i].isOpened = PlayerPrefs.GetInt(ShopItemsList[i].isOpened + "itemOpened");

            g = Instantiate(ItemTemplate, ShopScrollView);
            if (ShopItemsList[i].icon != null && !isPurchasedApple) { g.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ShopItemsList[i].icon; }
            if (isPurchasedApple) {g.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = apple; }
            if (ShopItemsList[i].Price != 0) { g.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = ShopItemsList[i].Price.ToString(); }
            else { g.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = ""; }
            g.transform.GetChild(0).GetComponent<Image>().color = clickColor;
            buyButton = g.transform.GetChild(0).GetComponent<Button>();
            buyButton.interactable = !ShopItemsList[i].IsPurchased;
            buyButton.AddEventListener(i, OnClickedButton);

            if (ShopItemsList[i].isOpened == 1) { OpenThisItem(i); }
        }
        Destroy(ItemTemplate.gameObject);
    }

    void OnClickedButton(int itemIndex)
    {
        ShopItemsList[itemIndex].IsPurchased = true;

        ShopItemsList[itemIndex].isOpened = 1;

        PlayerPrefs.SetInt("ItemOpened", ShopItemsList[itemIndex].isOpened);



        ShopScrollView.GetChild(itemIndex).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ShopItemsList[itemIndex].KnifeSprite;
        ShopScrollView.GetChild(itemIndex).GetComponent<RawImage>().color = openedColor;

        ShopScrollView.GetChild(itemIndex).GetChild(0).GetComponent<Image>().color = openItemColor;

        AddKnife(ShopItemsList[itemIndex].KnifeSprite);

        profile.CurrentAvatar(ShopItemsList[itemIndex].KnifeSprite);

    }


    void OpenThisItem(int itemIndex)
    {
        ShopItemsList[itemIndex].IsPurchased = true;

        ShopScrollView.GetChild(itemIndex + 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = ShopItemsList[itemIndex].KnifeSprite;
        ShopScrollView.GetChild(itemIndex + 1).GetComponent<RawImage>().color = openedColor;
        ShopScrollView.GetChild(itemIndex + 1).GetChild(0).GetComponent<Image>().color = openItemColor;
    }

    void AddKnife(Sprite img)
    {
        if (AvatarList == null)
            AvatarList = new List<Avatar>();

        Avatar ava = new Avatar() { Image = img };
        AvatarList.Add(ava);

        AvatarUITemplate.transform.GetComponent<Image>().sprite = ava.Image;

        animator.SetTrigger("NewKnife");
    }
}
