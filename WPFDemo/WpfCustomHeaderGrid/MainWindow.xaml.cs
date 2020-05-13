using System.Linq;
using System.Windows;

namespace WpfCustomHeaderGrid {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            dataGrid.Loaded += dataGrid_Loaded;
        }

        void dataGrid_Loaded(object sender, RoutedEventArgs e) {
            dataGrid.Loaded -= dataGrid_Loaded;
            dataGrid.ItemsSource = System.IO.Directory.GetDirectories("c:\\", "*",
                                            System.IO.SearchOption.TopDirectoryOnly)
                                            .Select(f => new FileName() { Name = f });
        }
    }

    /// <summary>
    /// Small class to hold the data to be displayed in the data grid
    /// </summary>
    public class FileName {
        public string Name { get; set; }
    }
}
