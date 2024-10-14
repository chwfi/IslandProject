using UnityEngine;

[CreateAssetMenu(menuName = "SO/TransitionCondition/TimerCondition")]
public class TimerCondition : TransitionCondition
{
    private float timer = 0;
    private float randomTime = 0f;

    [SerializeField] private float minTime = 1f; // 최소 랜덤 타이머 값
    [SerializeField] private float maxTime = 5f; // 최대 랜덤 타이머 값

    private bool complete = false;

    private void OnEnable()
    {
        randomTime = Random.Range(minTime, maxTime);
    }

    public override bool IsConditionValid()
    {
        if (complete)
        {
            timer = 0;
            randomTime = Random.Range(minTime, maxTime);
            complete = false;
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > randomTime)
            {
                complete = true;
                return complete;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
