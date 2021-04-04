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
using System.IO;

namespace CRUDNotesApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string path = @"..\..\notes\";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CollapseAll()
        {
            ReadNoteBlock.Visibility = Visibility.Collapsed;
            NotesListBox.Visibility = Visibility.Collapsed;
            CloseButton.Visibility = Visibility.Collapsed;
            NotesLabel.Visibility = Visibility.Collapsed;
            ActionButton.Visibility = Visibility.Collapsed;
            NewNoteLabel.Visibility = Visibility.Collapsed;
            NewNoteName.Visibility = Visibility.Collapsed;
            NewTextLabel.Visibility = Visibility.Collapsed;
            NewText.Visibility = Visibility.Collapsed;
            NotesLabel.Content = "Notes:";
            NewText.Text = "";
            NewNoteName.Text = "";
        }

        //gets all the notes in the note directory
        private string[] GetNotes(string path)
        {
            string[] notes = new string[Directory.GetFiles(path).Length];

            for (int i = 0; i < Directory.GetFiles(path).Length; i++)
            {
                notes[i] = System.IO.Path.GetFileName(Directory.GetFiles(path)[i]);
            }

            return notes;
        }

        //used to pass the selected note to other methods
        private string ChoosenNote(string path)
        {
            return path + NotesListBox.SelectedItem;
        }
        
        //menu first button
        private void CreateNoteButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseAll();
            NewNoteLabel.Visibility = Visibility.Visible;
            NewNoteName.Text = "";
            NewNoteName.Visibility = Visibility.Visible;
            ActionButton.Content = "Create new note";
            ActionButton.Visibility = Visibility.Visible;
            NewTextLabel.Visibility = Visibility.Visible;
            NewText.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Visible;
        }

        //menu second button
        private void ReadNotesButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseAll();
            if(GetNotes(path).Length == 0)
            {
                MessageBox.Show("There aren't any notes.");
                return;
            }
            NotesLabel.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Visible;
            NotesListBox.UnselectAll();
            NotesListBox.Visibility = Visibility.Visible;
            NotesListBox.ItemsSource = GetNotes(path);
            ActionButton.Content = "Read note";
            ActionButton.Visibility = Visibility.Visible;
        }

        //menu third button
        private void UpdateNoteButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseAll();

            if (GetNotes(path).Length == 0)
            {
                MessageBox.Show("There aren't any notes.");
                return;
            }

            NotesListBox.UnselectAll();
            NotesListBox.ItemsSource = GetNotes(path);
            NotesLabel.Visibility = Visibility.Visible;
            NotesListBox.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Visible;
            ActionButton.Content = "Open selected";
            ActionButton.Visibility = Visibility.Visible;
        }
        
        //menu fourth button
        private void DeleteNotesButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseAll();

            if (GetNotes(path).Length == 0)
            {
                MessageBox.Show("There aren't any notes.");
                return;
            }

            NotesLabel.Visibility = Visibility.Visible;
            NotesListBox.UnselectAll();
            NotesListBox.ItemsSource = GetNotes(path);
            NotesListBox.Visibility = Visibility.Visible;
            ActionButton.Content = "Delete note";
            ActionButton.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Visible;
        }

        //closes the app
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //action button
        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ActionButton.Content == "Read note")
            {
                if (ItemIsSelected())
                {
                    NotesListBox.Visibility = Visibility.Collapsed;
                    ActionButton.Visibility = Visibility.Collapsed;

                    StreamReader reader = new StreamReader(ChoosenNote(path));
                    ReadNoteBlock.Visibility = Visibility.Visible;
                    NotesLabel.Content = NotesListBox.SelectedItem;

                    using (reader)
                    {
                        ReadNoteBlock.Text = reader.ReadToEnd();
                    }
                }
                else
                {
                    return;
                }
            }
            else if((string)ActionButton.Content == "Create new note")
            {
                StreamWriter writer = new StreamWriter(path + NewNoteName.Text + ".txt");

                using (writer)
                {
                    writer.Write(NewText.Text);
                }

                MessageBox.Show($"New note {NewNoteName.Text}.txt created.");
                NewNoteName.Text = "";
                NewText.Text = "";
            }
            else if((string)ActionButton.Content == "Open selected")
            {
                if (ItemIsSelected())
                {
                    StreamReader reader = new StreamReader(ChoosenNote(path));
                    
                    NotesListBox.Visibility = Visibility.Collapsed;
                    ActionButton.Content = "Update note";
                    
                    NotesLabel.Content = NotesListBox.SelectedItem;
                    NotesLabel.Visibility = Visibility.Visible;
                    using (reader)
                    {
                        NewText.Text = reader.ReadToEnd();
                    }
                    NewText.Visibility = Visibility.Visible;
                }
                else
                {
                    return;
                }
            }
            else if((string)ActionButton.Content == "Update note")
            {
                StreamWriter writer = new StreamWriter(path + (string)NotesListBox.SelectedItem, false);

                using (writer)
                {
                    writer.Write(NewText.Text);
                }

                MessageBox.Show($"{System.IO.Path.GetFileName(ChoosenNote(path))} updated succesfully.");
                ActionButton.Content = "Open selected";
                NewText.Visibility = Visibility.Hidden;
                NewText.Text = "";
                NotesListBox.UnselectAll();
                NotesListBox.Visibility = Visibility.Visible;
                NotesLabel.Content = "Notes:";
            }
            else if((string)ActionButton.Content == "Delete note")
            {
                if (ItemIsSelected())
                {
                    File.Delete(ChoosenNote(path));
                    NotesListBox.ItemsSource = GetNotes(path);
                }
                else
                {
                    return;
                }
            }
        }

        //collapses all sub elements
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseAll();
            NotesLabel.Content = "Notes:";
        }
        private bool ItemIsSelected()
        {
            if(NotesListBox.SelectedItem == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
