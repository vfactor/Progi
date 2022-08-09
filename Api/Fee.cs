namespace Api;

internal abstract class Fee
{
    public readonly string Name;
    public readonly int Order;
    protected readonly Rule[] _rules;

    public Fee(string name, int order, Rule rule)
    {
        Name = name;
        Order = order;
        _rules = new Rule[] { rule };
    }
    public Fee(string name, int order, Rule[] rules)
    {
        Name = name;
        Order = order;
        _rules = rules;        
    }
    public virtual double Reverse(double total)
    {
        var a = _rules[0].Reverse(total);
        return (a <= total) ? total - a : total;
    }
}
internal class Base : Fee
{
    public Base(int order) : base(nameof(Base), order, new Rule(0,0.10,10,50)){}
    public override double Reverse(double total)
    {
        var r = _rules[0];
        var a = r.Reverse(total);

        if (a <= r.Min)
            return total - r.Min;

        if (r.Min < a && a < r.Max)
            return total - a;

        if (a >= r.Max)
            return total - r.Max;

        return total;
    }
}
internal class Vendor : Fee
{
    public Vendor(int order) : base(nameof(Vendor), order, new Rule(0,0.02)){}
}
internal class Extra : Fee
{
    public Extra(int order) : base(nameof(Extra), order, new Rule[] { 
                                                                        new Rule(20,0,3001,3001),
                                                                        new Rule(15,0,1001,3000),
                                                                        new Rule(10,0,501,1000),
                                                                        new Rule(5,0,1,500)
                                                                    }){}
    public override double Reverse(double total)
    {
        foreach (var r in _rules)
        {
            var a = total - r.Reverse(total);
            if (r.Min <= a && a <= r.Max || a >= r.Max)
                return a;
        }

        return total;
    }
}
internal class Storage : Fee
{
    public Storage(int order) : base(nameof(Storage), order, new Rule(100)){}
}

public class FeesCollection
{
    private readonly IOrderedEnumerable<Fee> _feesAsc;
    private readonly IOrderedEnumerable<Fee> _feesDesc;

    public FeesCollection()
    {
        var fees = new Fee[] { new Base(1), new Vendor(2), new Extra(3), new Storage(4) };

        _feesAsc = fees.OrderBy(f => f.Order);
        _feesDesc = fees.OrderByDescending(f => f.Order);
    }
    public double Reverse(double total)
    {
        double net = total;

        foreach(var f in _feesDesc)
        {
            net = f.Reverse(net);
        }

        return net;
    }
}