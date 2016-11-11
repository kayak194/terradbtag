using System.Windows;

namespace projektseminar_test
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
