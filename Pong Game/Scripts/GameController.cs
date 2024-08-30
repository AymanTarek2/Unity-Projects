using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject ball;

    

    public Text scoreTextLeft;
    public Text scoreTextRight;

    public Starter starter;

    private bool started = false;

    private int scoreLeft;
    private int scoreRight;
    
    private BallController ballController;

    private Vector3 startingPosition;
    void Start()
    {
        this.ballController = this.ball.GetComponent<BallController>();
        this.startingPosition = this.ball.transform.position;
    }


    void Update()
    {
        if (this.started)
            return;

        if (Input.GetKey(KeyCode.Space)) {
            this.started = true;
            Debug.Log("e7m1");
            this.starter.StartCountDown();
            Debug.Log("e7m2");
        }
    }

    public void scoreGoalLeft()
    {
        Debug.Log("scoreGoalLeft");
        scoreRight++;
        UpdateUI();
        ResetBall();
    }
    public void scoreGoalRight()
    {
        Debug.Log("scoreGoalRight");
        scoreLeft++;
        UpdateUI();
        ResetBall();
    }
    private void UpdateUI()
    {
        scoreTextLeft.text = this.scoreLeft.ToString();
        scoreTextRight.text = this.scoreRight.ToString();
    }

    private void ResetBall()
    {
        this.ballController.Stop();
        this.ball.transform.position = this.startingPosition;
        this.starter.StartCountDown();
    }

    public void StartGame()
    {
        this.ballController.Go();
    }
}
