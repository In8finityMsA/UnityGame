﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerBugsScript : MonoBehaviour
{
    public int penalty;
    public int reward;
    public GameObject image;

    public float timeToWaitOk;
    
    Rect cameraRect;
    public GameObject background;
    public GameObject resulttext;
    Canvas canv;
    public GameObject top;
    public float timer;
    public GameObject bottom;
    public GameObject right;
    public GameObject left;
    public static ManagerBugsScript Instance { get; private set; }
    // Start is called before the first frame update
    public GameObject bugPrefab;
    public List<GameObject> bugs = new List<GameObject>();
    public float speed;
    public int bugAmount;
    private bool isClicked = false;

    public bool IsPlaying = false;
    public GameObject BackPanel;
    public GameObject HintBox;
    public GameObject EndButton;

    public void StartGame()
    {
        IsPlaying = true;
        HintBox.SetActive(false);
        
        BackPanel.GetComponent<UnityEngine.UI.Image>().color = Color.clear;
    }

    public float Parabola(float x)
    {
        return x * x;
    }
    public float Sq(float x)
    {
        return (float)System.Math.Sqrt(x);
    }
    private void Awake()
    {
        timeToWaitOk = 1;
        image.SetActive(false);
        resulttext.SetActive(false);
        timer = 10;
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        bottom.transform.position = new Vector3(cameraRect.x, cameraRect.y, 0);
        top.transform.position = new Vector3(cameraRect.x, cameraRect.y + cameraRect.height, 0);
        left.transform.position = new Vector3(cameraRect.x, cameraRect.y, 0);
        right.transform.position = new Vector3(cameraRect.x + cameraRect.width, cameraRect.y + cameraRect.height, 0);

        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 scale = transform.localScale;
        scale.x *= cameraSize.x / spriteSize.x;
        scale.y *= cameraSize.y / spriteSize.y;
        background.transform.position = Vector2.zero; // Optional
        background.transform.localScale = scale;


        bugAmount = 5;
        speed = 2;
        Instance = this;
        System.Random rand = new System.Random();
        for (int i = 0; i < bugAmount; i++)
        {
            bugs.Add(Instantiate(bugPrefab, new Vector3(rand.Next((int)(cameraRect.x * 50), (int)((cameraRect.x + cameraRect.width) * 50))/100, rand.Next((int)(cameraRect.y * 50), (int)((cameraRect.y + cameraRect.height) * 50)) / 100 , 0),  Quaternion.identity));
        }
        canv = gameObject.GetComponentInChildren<Canvas>();

    }
    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            SceneManager.LoadScene("MainScene");
        }

    }
    void Start()
    {
        Debug.Log("set act false");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && bugs.Count != 0)
            {

                for (int i = 0; i < bugs.Count; i++)
                {
                    bugs[i].SetActive(false);
                }
                resulttext.SetActive(true);
                image.SetActive(true);
                resulttext.GetComponent<Text>().text = "Баги вас одолели(..." + '\n' + $"Ваши потери составили: {penalty}";
                timeToWaitOk -= Time.deltaTime;
                if (timeToWaitOk <= 0)
                {
                    //MainManager.Instance.Money -= penalty;
                    //MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);

                    

                    Thread.Sleep(2000);
                    EndButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "OK(";
                    EndButton.SetActive(true);
                }               
            }
            if (bugs.Count == 0)
            {
                MainManager.Instance.Money += reward;
                MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);

                resulttext.SetActive(true);
                image.SetActive(true);
                resulttext.GetComponent<Text>().text = "Все починилось, все работает!" + '\n' + $"Вы заработали {reward}";
                EndButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "OK!";
                EndButton.SetActive(true);
            }
        }      
    }
}
