using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Microsoft.Maui.Controls;

namespace Counter
{
    public partial class MainPage : ContentPage
    {
        private readonly string FilePath = Path.Combine(Path.GetTempPath(), "counters.xml") ;
        public ObservableCollection<Counter> Counters { get; set; }
        private int _currentMaxId = 0;

        public MainPage()
        {
            InitializeComponent();
            Counters = new ObservableCollection<Counter>();
            BindingContext = this;

            LoadCounters();
        }

        public void LoadCounters()
        {
            if (File.Exists(FilePath))
            {
                var serializer = new XmlSerializer(typeof(List<Counter>));
                using (var stream = File.OpenRead(FilePath))
                {
                    var loadedCounters = (List<Counter>)serializer.Deserialize(stream);

                    foreach (var counter in loadedCounters)
                    {
                        Counters.Add(counter);
                    }

                    _currentMaxId = Counters.Count > 0 ? Counters.Max(counter => counter.Id) : 0;
                }
            }
            else
            {
                SaveCountersCreateFile();
            }
        }

        private void SaveCountersCreateFile()
        {
            var serializer = new XmlSerializer(typeof(List<Counter>));
            using (var stream = File.Create(FilePath))
            {
                serializer.Serialize(stream, Counters.ToList());
            }
        }

        private void OnAddCounterClicked(object sender, EventArgs e)
        {
            if (int.TryParse(InitialValueEntry.Text, out int initialValue))
            {
                var counter = new Counter(initialValue) 
                { 
                    Id = ++_currentMaxId 
                };
                Counters.Add(counter);
                SaveCountersCreateFile();
            }
        }

        private void OnIncrementClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Counter counter)
            {
                counter.CurrentValue++;
                SaveCountersCreateFile();
            }
        }

        private void OnDecrementClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Counter counter)
            {
                counter.CurrentValue--;
                SaveCountersCreateFile();
            }
        }

        private string _originalPlaceholder = "Podaj wartość początkową";

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                entry.Placeholder = string.Empty;
            }
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (string.IsNullOrEmpty(entry.Text))
                {
                    entry.Placeholder = _originalPlaceholder;
                }
            }
        }
    }

}
