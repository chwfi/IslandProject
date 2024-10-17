public class UnboundedTaskUI : UnboundedUI
{
    private Task _task;

    public void SetUp(Task task)
    {
        _task = task;
    }

    public override void UpdateUI()
    {
        _amountText.text = $"{_task.Description}\n{_task.CurrentSuccessValue}/{_task.NeedToSuccessValue}";
    }
}
