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
using System.Data.Entity;

namespace TRIntant.Windows
{
    public partial class MainWindow : Window
    {
        TREntities TRContext = new TREntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btPrintTeacher_Click(object sender, RoutedEventArgs e)
        {
            TRContext.Преподаватель.Load();
            lvTeacher.ItemsSource = TRContext.Преподаватель.Local.ToBindingList<Преподаватель>();
        }
        private void btPrintStudents_Click(object sender, RoutedEventArgs e)
        {
            TRContext.Ученик.Load();
            lvStudent.ItemsSource = TRContext.Ученик.Local.ToBindingList<Ученик>();
        }

        private void RemoveTeacherEnity(int pk)
        {
            if (TRContext.Преподаватель.Find(pk) != null)
            {
                TRContext.Преподаватель.Remove(TRContext.Преподаватель.Find(pk));
                TRContext.SaveChanges();
                MessageBox.Show($"Преподаватель с id = {pk} удален");
            }
            else
            {
                MessageBox.Show($"Преподаватель с id = {pk} не найден");
            }
        }
        private void RemoveStudentEntity(int pk)
        {
            if (TRContext.Ученик.Find(pk) != null)
            {
                TRContext.Ученик.Remove(TRContext.Ученик.Find(pk));
                TRContext.SaveChanges();
                MessageBox.Show($"Ученик с id = {pk} удален");
            }
            else
            {
                MessageBox.Show($"Ученик с id = {pk} не найден");
            }
        }

        private void AddTeacherEntity()
        {
            EditAddTeacherWindow windowEditTeacher = new EditAddTeacherWindow();
            Преподаватель teacherEntity = new Преподаватель();

            windowEditTeacher.spDataTeacherEntity.DataContext = teacherEntity;
            windowEditTeacher.ShowDialog();

            if (windowEditTeacher.DialogResult ?? false)
            {
                if (teacherEntity != null)
                {
                    TRContext.Преподаватель.Add(teacherEntity);
                    TRContext.SaveChanges();
                    MessageBox.Show("Преподаватель добавлен");
                }
            }
            else
            {
                MessageBox.Show("Изменения не сохранены");
            }
        }
        private void AddStudentEntity()
        {
            EditAddStudentWindow windowEditStudent = new EditAddStudentWindow();
            Ученик studentEntity = new Ученик();

            windowEditStudent.spDataStudentEntity.DataContext = studentEntity;
            windowEditStudent.ShowDialog();

            if (windowEditStudent.DialogResult ?? false)
            {
                if (studentEntity != null)
                {
                    TRContext.Ученик.Add(studentEntity);
                    TRContext.SaveChanges();
                    MessageBox.Show("Ученик добавлен");
                }
            }
            else
            {
                MessageBox.Show("Изменения не сохранены");
            }
        }

        private void EditTeacherEntity(int pk)
        {
            if (TRContext.Преподаватель.Find(pk) != null)
            {
                EditAddTeacherWindow windowEditTeacher = new EditAddTeacherWindow();
                Преподаватель teacherEntity = TRContext.Преподаватель.Find(pk);

                windowEditTeacher.spDataTeacherEntity.DataContext = teacherEntity;

                windowEditTeacher.ShowDialog();

                if (windowEditTeacher.DialogResult ?? false)
                {
                    TRContext.SaveChanges();
                    MessageBox.Show("Изменения сохранены");
                }
                else
                {
                    TRContext.Entry(teacherEntity).State = EntityState.Unchanged;
                    lvTeacher.Items.Refresh();
                    MessageBox.Show("Изменения не сохранены");
                }
            }
            else
            {
                MessageBox.Show($"Преподаватель с id = {pk} не найден");
            }
        }
        private void EditStudentEntity(int pk)
        {
            if (TRContext.Ученик.Find(pk) != null)
            {
                EditAddStudentWindow windowEditStudent = new EditAddStudentWindow();
                Ученик studentEntity = TRContext.Ученик.Find(pk);

                windowEditStudent.spDataStudentEntity.DataContext = studentEntity;

                windowEditStudent.ShowDialog();

                if (windowEditStudent.DialogResult ?? false)
                {
                    TRContext.SaveChanges();
                    MessageBox.Show("Изменения сохранены");
                }
                else
                {
                    TRContext.Entry(studentEntity).State = EntityState.Unchanged;
                    lvStudent.Items.Refresh();
                    MessageBox.Show("Изменения не сохранены");
                }
            }
            else
            {
                MessageBox.Show($"Ученик с id = {pk} не найден");
            }
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            if (СheckEnterСonditions(rbRemoveTeacher.IsChecked ?? false, rbRemoveStudent.IsChecked ?? false, tbRemoveId.Text, out int pk))
            {
                if (rbRemoveStudent.IsChecked ?? false)
                {
                    RemoveStudentEntity(pk);
                    return;
                }

                if (rbRemoveTeacher.IsChecked ?? false)
                {
                    RemoveTeacherEnity(pk);
                    return;
                }
            }
        }
        private void tbRemoveId_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbRemoveId.Clear();
        }
        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            if (СheckEnterСonditions(rbEditTeacher.IsChecked ?? false, rbEditStudent.IsChecked ?? false, tbEditId.Text, out int pk))
            {
                if (rbEditStudent.IsChecked ?? false)
                {
                    EditStudentEntity(pk);
                    return;
                }

                if (rbEditTeacher.IsChecked ?? false)
                {
                    EditTeacherEntity(pk);
                    return;
                }
            }

        }
        private void tbEditId_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbEditId.Clear();
        }
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (СheckEnterСonditions(rbAddTeacher.IsChecked ?? false, rbAddStudent.IsChecked ?? false))
            {
                if (rbAddStudent.IsChecked ?? false)
                {
                    AddStudentEntity();
                    return;
                }

                if (rbAddTeacher.IsChecked ?? false)
                {
                    AddTeacherEntity();
                    return;
                }
            }

        }
        private bool СheckEnterСonditions(bool rbTeacher, bool rbStudent, string tbId, out int pk)
        {

            if (!rbTeacher & !rbStudent)
            {
                MessageBox.Show("Не выбран объект изменения (Ученик или Преподаватель)");
                pk = 0;
                return false;

            }

            if (!int.TryParse(tbId, out pk))
            {
                MessageBox.Show("id введен не верно");
                return false;
            }
            return true;
        }
        private bool СheckEnterСonditions(bool rbTeacher, bool rbStudent)
        {

            if (!rbTeacher & !rbStudent)
            {
                MessageBox.Show("Не выбран объект изменения (Ученик или Преподаватель)");
                return false;
            }
            return true;
        }
    }
}