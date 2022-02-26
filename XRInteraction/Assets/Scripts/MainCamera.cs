using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public TargetPrefab targetPrefab;
    float nextTime = 0;
    public int interval = 1;
    GameObject scoreBoard;
    int hits = 0;
    public int maxTargets = 20;

    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time + interval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= nextTime) {
            nextTime += interval;
            var targets = FindObjectsOfType<TargetPrefab>();
            if (targets.Length < maxTargets) {
                Instantiate(targetPrefab);
            }
        }
    }

    public void hit(){ 
        hits++;
        updateScore();
    }
    
    void updateScore(){ 
        if (scoreBoard == null) {
            scoreBoard = GameObject.Find("Score Board");
        }
        scoreBoard.GetComponent<UnityEngine.UI.Text>().text = "Score: " + hits;
    }
}
