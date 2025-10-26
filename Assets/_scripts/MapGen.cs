using JetBrains.Annotations;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;


public class MapGen : MonoBehaviour
{
    // Variables 
    private int[] tileLayout;
    private int tileCount;
    private int maxR;
    private int minR;
    private List<int> lastR;
    //R means rooms 

    private int bossRid;
    private int hiddenRid;
    private int shopRid;
    private int itemRid;
    //Rid is Room Id

    public Room setR;
    private float Rsize;
    private Queue<int> Rqueue;
    private List<Room> RGen;

    [Header("Sprites")]
    [SerializeField] public Sprite boss;
    [SerializeField] public Sprite shop;
    [SerializeField] public Sprite item;
    [SerializeField] public Sprite hidden;







    void Start()
    {
        minR = 8;
        maxR = 16;
        Rsize = 1.9f;
        RGen = new();

        MakeDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeDungeon();
        }
    }


    void MakeDungeon()
    {
        for(int i =0; i<RGen.Count; i++)
        {
            Destroy(RGen[i].gameObject);
        }

        RGen.Clear();
        tileLayout = new int[100];
        // this sets the x and y of the area we can change this to get different size dim
        tileCount = default;
        Rqueue = new Queue<int>();  
        lastR = new List<int>();

        visitR(45);
        // center of the matrix I want to test 50 and different numbers 

        GenDungeon();


    }
    void GenDungeon()
    {
        while(Rqueue.Count > 0)
        {
            int index = Rqueue.Dequeue();
            int x = index % 10;

            bool created = false;

            if (x > 1) created |= visitR(index - 1);
            if (x < 9) created |= visitR(index + 1);
            if (index > 20) created |= visitR(index - 10);
            if (index < 70) created |= visitR(index + 10);
            //|= this mean if its already true it keeps it true 

            if(created == false)
                lastR.Add(index);
        }
        if (tileCount < minR)
        {
            MakeDungeon();
            return;
        }
        makeSR();
    }

    void makeSR()
    {
        bossRid = lastR.Count > 0 ? lastR[lastR.Count - 1] : -1;

        if(bossRid != -1)
        {
            lastR.RemoveAt(lastR.Count - 1);
        }
        itemRid = RandLastR();
        shopRid = RandLastR();
        hiddenRid = FindHR();

        if(itemRid == -1 || shopRid == -1 || hiddenRid == -1|| bossRid == -1)
        {
            MakeDungeon();
            return; 
        }
        makeR(hiddenRid);

        SRicons();


    }

    void SRicons() 
    {
        foreach(var rooms in RGen)
        {
            if(rooms.index == itemRid)
            {
                rooms.makeSRicons(item);
            }

            if (rooms.index == shopRid)
            {
                rooms.makeSRicons(shop);
            }

            if (rooms.index == bossRid)
            {   
                rooms.makeSRicons(boss);
            }

            if (rooms.index == hiddenRid)
            {
                rooms.makeSRicons(hidden);
            }
        }
    }

    int RandLastR()
    {
        if (lastR.Count == 0) return -1;

        int randR = Random.Range(0,lastR.Count);
        int index = lastR[randR];

        lastR.RemoveAt(randR);
        return index;
    }

    int FindHR()
    {
        for (int attempt = 0; attempt < 900; attempt++)
        {
            int x = Mathf.FloorToInt(Random.Range(0f, 1f) * 9) + 1;
            int y = Mathf.FloorToInt(Random.Range(0f, 1f) * 8) + 2;

            int index = y * 10 + x;

            if (tileLayout[index] != 0) { continue; }

            if (bossRid == index - 1 || bossRid == index + 1 || bossRid == index + 10 || bossRid == index - 10) { continue; }

            if(index -1 <0|| index+1 >tileLayout.Length || index - 10<0 ||index+10> tileLayout.Length) {continue;}

            int adj = FindAdjNum(index);

            if(adj >=3 || (attempt > 300 && adj >= 2)||(attempt >600 && adj >= 1))
            {
                return index;
            }

        }
        return -1;
    }

    private int FindAdjNum(int index)
    {
        return tileLayout[index - 10] + tileLayout[index - 1] + tileLayout[index + 1] + tileLayout[index+10];
    }

    private bool visitR(int index)
    {
        if (tileLayout[index] !=0)
            return false;
        if (FindAdjNum(index) > 1)
            return false;
        if(tileCount>maxR)
            return false;
        if (Random.value < 0.5f)
            return false;

        Rqueue.Enqueue(index);
        tileLayout[index] = 1;
        tileCount++;

        makeR(index);
        return true;
    }

    private void makeR(int index)
    {
        int x = index % 10;
        int y = index / 10;
        Vector2 pos = new Vector2(x*Rsize, -y*Rsize);

        Room newRoom = Instantiate(setR, pos, Quaternion.identity);
        newRoom.value = 1;
        newRoom.index = index;

        RGen.Add(newRoom);
    }



}
