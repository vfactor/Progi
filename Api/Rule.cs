namespace Api;
internal class Rule
{
    private readonly double _amount;
    private readonly double _percentage;
    public readonly double Min;
    public readonly double Max;

    public Rule(double amount = 0, double percentage = 0, double min = 1, double max = 1)
    {
        _amount = amount;
        _percentage = percentage;
        Min = min;
        Max = max;

        if (!IsValid)
            throw new ArgumentException("rule is invalid");
    }

    public double Apply(double total)
    {
        throw new NotImplementedException();
    }
    public double Reverse(double total) => _amount + (total - (total / (1 + _percentage)));
    private bool IsValid => Min >= 0 && Max >= Min && ((_amount == 0 && _percentage > 0) || (_amount > 0 && _percentage == 0));
}