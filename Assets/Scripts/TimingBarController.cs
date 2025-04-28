using UnityEngine;
using UnityEngine.UI;

public class TimingBarController : MonoBehaviour
{
    [Header("References")]
    public RectTransform tick;
    public RectTransform greenZone;

    [Header("Settings")]
    public float baseSpeed = 500f;
    public float speedVariance = 50f;
    public float forgivenessPixels = 5f;

    [Header("Green Zone Settings")]
    public float greenMoveSpeed = 100f;
    public float minGreenZoneWidth = 30f;

    private RectTransform timingBar;
    private bool movingRight = true;
    private bool greenMovingRight = true;
    private float currentSpeed;
    private Potato potato;

    private Vector2 greenZoneStartPos;
    private float greenZoneStartWidth;

    private void Start()
    {
        timingBar = GetComponent<RectTransform>();
        RandomizeSpeed();
        potato = FindObjectOfType<Potato>();

        if (greenZone != null)
        {
            greenZoneStartPos = greenZone.anchoredPosition;
            greenZoneStartWidth = greenZone.sizeDelta.x;
        }
    }

    private void Update()
    {
        if (tick == null || timingBar == null || greenZone == null)
            return;

        UpdateTickSpeed();
        MoveTick();
        MoveAndShrinkGreenZone();
    }

    private void MoveTick()
    {
        float barWidth = timingBar.rect.width;
        float halfBar = barWidth / 2f;
        float tickX = tick.anchoredPosition.x;

        if (movingRight)
        {
            tickX += currentSpeed * Time.deltaTime;
            if (tickX >= halfBar)
            {
                tickX = halfBar;
                movingRight = false;
                RandomizeSpeed();
            }
        }
        else
        {
            tickX -= currentSpeed * Time.deltaTime;
            if (tickX <= -halfBar)
            {
                tickX = -halfBar;
                movingRight = true;
                RandomizeSpeed();
            }
        }

        tick.anchoredPosition = new Vector2(tickX, tick.anchoredPosition.y);
    }

    private void RandomizeSpeed()
    {
        currentSpeed = baseSpeed + Random.Range(-speedVariance, speedVariance);
    }

    private void UpdateTickSpeed()
    {
        if (potato == null)
            return;

        float dangerLevel = 1f - (potato.countdown / potato.maxCountdown);

        float extraSpeed = Mathf.Lerp(0f, 400f, dangerLevel);
        currentSpeed = baseSpeed + extraSpeed + Random.Range(-speedVariance, speedVariance);
    }

    private void MoveAndShrinkGreenZone()
    {
        if (potato == null) return;

        float dangerLevel = 1f - (potato.countdown / potato.maxCountdown);

        if (dangerLevel > 0.5f)
        {
            float barWidth = timingBar.rect.width;
            float halfBar = barWidth / 2f;
            Vector2 greenPos = greenZone.anchoredPosition;

            if (greenMovingRight)
            {
                greenPos.x += greenMoveSpeed * Time.deltaTime;
                if (greenPos.x > halfBar - (greenZone.rect.width / 2f))
                {
                    greenPos.x = halfBar - (greenZone.rect.width / 2f);
                    greenMovingRight = false;
                }
            }
            else
            {
                greenPos.x -= greenMoveSpeed * Time.deltaTime;
                if (greenPos.x < -halfBar + (greenZone.rect.width / 2f))
                {
                    greenPos.x = -halfBar + (greenZone.rect.width / 2f);
                    greenMovingRight = true;
                }
            }

            greenZone.anchoredPosition = greenPos;

            float newWidth = Mathf.Lerp(greenZoneStartWidth, minGreenZoneWidth, (dangerLevel - 0.5f) * 2f);
            greenZone.sizeDelta = new Vector2(newWidth, greenZone.sizeDelta.y);
        }
    }

    public bool IsInGreenZone()
    {
        if (tick == null || greenZone == null)
            return false;

        float tickCenter = tick.position.x;
        float greenLeft = greenZone.position.x - (greenZone.rect.width / 2f) - forgivenessPixels;
        float greenRight = greenZone.position.x + (greenZone.rect.width / 2f) + forgivenessPixels;

        return tickCenter > greenLeft && tickCenter < greenRight;
    }

    // ✨ New: Reset Green Zone manually
    public void ResetGreenZone()
    {
        if (greenZone != null)
        {
            greenZone.anchoredPosition = greenZoneStartPos;
            greenZone.sizeDelta = new Vector2(greenZoneStartWidth, greenZone.sizeDelta.y);
            greenMovingRight = true;
        }
    }
}
