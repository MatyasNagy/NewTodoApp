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

namespace NewTodoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly IUserRepository     userRepository;
        public readonly IFeladatRepository  feladatRepository;
        public readonly Ilabels labelsRepository;
       


        public MainWindow()
        {
            userRepository    = new UserRepository();
            feladatRepository = new FeladatRepository();
            labelsRepository = new LabelsRepository();

            InitializeComponent();
            PopulateUserek();
            PopulateFeladatok();
        }

        void PopulateUserek()
        {
            List<User> userek = userRepository.UserekBeolvasas();
            ListBoxUserek.Items.Clear();
            ListBoxTulajdonos.Items.Clear();
            foreach (var item in userek)
            {
                ListBoxUserek.Items.Add(item.UserName.ToString());
                ListBoxTulajdonos.Items.Add(item.UserName.ToString());
            }
        }

        void PopulateFeladatok()
        {
            List<Feladat> feladatok = feladatRepository.FeladatBeolvasas();
            ListboxFeladatok.Items.Clear();
            foreach (var item in feladatok)
            {
                ListboxFeladatok.Items.Add(item.Cim);
            }
        }


        private void ButtonUserHozzaad_Click(object sender, RoutedEventArgs e)
        {
            string username= TextBoxUserek.Text.ToString();
            User user = new User(username);
            userRepository.UserHozzaad(user);
            PopulateUserek();
        }

        private void ButtonHozzaad_Click(object sender, RoutedEventArgs e)
        {
            string cim    = TextBoxCim.Text;
            string leiras = TextBoxLeiras.Text;
            DateTime date = DatePicklerDatum.SelectedDate.Value;
            Feladat ujfeladat = new Feladat(cim, leiras, date);
            User selecteduser = new User(ListBoxTulajdonos.SelectedItem.ToString());
            feladatRepository.Listahozad(ujfeladat, selecteduser);
            PopulateFeladatok();
            PopulateCCB();
            PopulateFeladatokCCBoxFa();
        }

        private void TextBoxCim_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxCim.Text="";
        }

        private void TextBoxLeiras_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxLeiras.Text = "";
        }

        private void ButtonMegtekint_Click(object sender, RoutedEventArgs e)
        {
            string fa = ListboxFeladatok.SelectedItem.ToString();
            //int index = ListboxFeladatok.Items.IndexOf(a);
            var message = feladatRepository.Megtekint(fa);
            string ms="";
            foreach (string item in message)
            {
                ms += item+" ";
            }
            MessageBox.Show(ms);
        }

        //3.lap programrésze ------------------------------------------------------------------------------------------------

        private void CCBox_Initialized(object sender, EventArgs e)
        {
            PopulateCCB();
        }

        private void CCBoxFa_Initialized(object sender, EventArgs e)
        {
            PopulateFeladatokCCBoxFa();
        }

        void PopulateCCB()
        {
            var labels = labelsRepository.Lista();
            CCBox.Items.Clear();
            foreach (string label in labels)
            {
                CCBox.Items.Add(new Xceed.Wpf.Toolkit.CheckListBox().Name=label);
            }
        }

        private void BTNListahozad_Click(object sender, RoutedEventArgs e)
        {
            labelsRepository.Listahozad(TBXLabelneve.Text);
            PopulateCCB();
        }

        void PopulateFeladatokCCBoxFa()
        {
            List<Feladat> feladatok = feladatRepository.FeladatBeolvasas();
            CCBoxFa.Items.Clear();
            foreach (var item in feladatok)
            {
                CCBoxFa.Items.Add(item.Cim);
            }
        }

        private void BTNListabolelvesz_Click(object sender, RoutedEventArgs e)
        {
            var selitem = CCBox.SelectedItems;
            foreach (var item in selitem)
            {
                string s = item.ToString();
                labelsRepository.Listabolelvesz(s);
            }
            PopulateCCB();
        }

        private void BTNFaladthozLabel_Click(object sender, RoutedEventArgs e)
        {
            List<string> labelekString = new List<string>();
            List<string> feladatokString = new List<string>();

            foreach (var item in CCBox.SelectedItems)
            {
                labelekString.Add(item.ToString());
            }

            foreach (var item in CCBoxFa.SelectedItems)
            {
                feladatokString.Add(item.ToString());
            }

            labelsRepository.LabelFaRendez(labelekString, feladatokString);


        }
    }
}
