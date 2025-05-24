using EasyPeasyFirstPersonController;
using SmallHedge.SoundManager;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public RectTransform bar;
    public RectTransform ball;
    public RectTransform greenZone;

    public float ballSpeed = 300f;
    public float greenZoneSpeed = 150f;

    private bool ballMovingRight = true;
    private bool greenMovingRight = true;

    public FirstPersonController firstPersonController;

    void Update()
    {
        if (ball == null)
        {
            Debug.LogError("Ball is still null!");
            return;
        }

        MoveBall();
        MoveGreenZone();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    void MoveBall()
    {
        float direction = ballMovingRight ? 1f : -1f;
        ball.anchoredPosition += new Vector2(ballSpeed * direction * Time.deltaTime, 0);

        float halfWidth = bar.rect.width / 2;
        if (ball.anchoredPosition.x >= halfWidth)
            ballMovingRight = false;
        else if (ball.anchoredPosition.x <= -halfWidth)
            ballMovingRight = true;
    }

    void MoveGreenZone()
    {
        float direction = greenMovingRight ? 1f : -1f;
        greenZone.anchoredPosition += new Vector2(greenZoneSpeed * direction * Time.deltaTime, 0);

        float halfWidth = bar.rect.width / 2;
        float greenHalf = greenZone.rect.width / 2;

        if (greenZone.anchoredPosition.x + greenHalf >= halfWidth)
            greenMovingRight = false;
        else if (greenZone.anchoredPosition.x - greenHalf <= -halfWidth)
            greenMovingRight = true;
    }

    void CheckHit()
    {
        float ballX = ball.anchoredPosition.x;
        float greenMin = greenZone.anchoredPosition.x - greenZone.rect.width / 2;
        float greenMax = greenZone.anchoredPosition.x + greenZone.rect.width / 2;

        if (ballX >= greenMin && ballX <= greenMax)
        {
            SoundManager.PlaySound(SoundType.BoosterTake);
            int number = Random.Range(0, 2);
            if (number == 0)
            {
                greenZoneSpeed *= 1.5f;
            }
            else
            {
                ballSpeed *= 1.5f;
            }


            firstPersonController.ApplyPermanentSpeedBoost(1.2f);
            Debug.Log("worked");
        }
        else
        {
            int number = Random.Range(0, 2);
            if (number == 0)
            {
                greenZoneSpeed *= 0.8f;
            }
            else
            {
                ballSpeed *= 0.8f;
            }
            firstPersonController.ApplyPermanentSpeedBoost(0.8f);
            Debug.Log("Missed.");

        }
    }
}
