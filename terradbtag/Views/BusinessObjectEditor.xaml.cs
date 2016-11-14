using System.Windows;

namespace terradbtag.Views
{
    /// <summary>
    /// Interaktionslogik für BusinessObjectEditor.xaml
    /// </summary>
    public partial class BusinessObjectEditor : Window
    {
        public BusinessObjectEditor()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
