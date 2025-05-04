using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppCRM
{
    /// <summary>
    /// Logique d'interaction pour PagePrincipale.xaml
    /// </summary>
    public partial class PagePrincipale : Window
    {
        public int nb = 0;
        public PagePrincipale()
        {
            InitializeComponent();
        }

        private void btnRedirectionClient_Click(object sender, RoutedEventArgs e)
        {
            if (nb == 0) {
                this.Close();
                MainWindow main = new MainWindow();
                main.Visibility = Visibility.Visible;
                nb++;
            }
           
        }

        private void btnQuitterApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRedirectionProduit_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
                PageProduit pageProduit = new PageProduit();
                pageProduit.Visibility = Visibility.Visible;
               
        }

        private void btnRedirectionFact_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
