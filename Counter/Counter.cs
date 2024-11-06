using System.ComponentModel;
using System.Xml.Serialization;

namespace Counter;

[Serializable]
public class Counter : INotifyPropertyChanged
{
    [XmlElement("Id")]
    public int Id { get; set; }
    [XmlElement("InitialValue")]
    public int InitialValue { get; set; }

    private int _currentValue;
    [XmlElement("CurrentValue")]
    public int CurrentValue
    {
        get => _currentValue;
        set
        {
            if (_currentValue != value)
            {
                _currentValue = value;
                OnPropertyChanged(nameof(CurrentValue));
            }
        }
    }

    public Counter(int initialValue)
    {
        InitialValue = initialValue;
        CurrentValue = initialValue;
    }

    public Counter() { }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}