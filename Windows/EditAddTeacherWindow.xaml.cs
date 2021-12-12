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
using System.Windows.Shapes;

namespace TRIntant.Windows
{
    public partial class EditAddTeacherWindow : Window
    {
        private bool validationError = true;
        public EditAddTeacherWindow()
        {
            InitializeComponent();
        }

        private void btCloseEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (validationError)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не все значения в нужном формате");
            }
        }
        private void tbLessonDate_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                validationError = false;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                validationError = true;
            }
        }
    }
}