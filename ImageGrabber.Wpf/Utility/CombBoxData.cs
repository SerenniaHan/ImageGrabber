namespace ImageGrabber.Wpf.Utility;

public class CombBoxData<T>
{
    public string DisplayMember { get; private set; }
    public T ValueMember { get; private set; }

    public CombBoxData(string displayMember, T valueMember)
    {
        DisplayMember = displayMember;
        ValueMember = valueMember;
    }
}