using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;

namespace CalculNotes
{
    public partial class MainPage : ContentPage
    {
        private List<Matiere> matieres = new List<Matiere>();

        private double totalCredits = 0;


        public MainPage()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
            //DeleteDatabase();
            InitializeDatabase();
            _ = RefreshMatiereListFromDatabase();
        }

         private async void InitializeDatabase()
         {
             var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
             using (var connection = new SqliteConnection($"Data Source={databasePath}"))
             {
                 await connection.OpenAsync();

                 var createTableCommand = connection.CreateCommand();
                 createTableCommand.CommandText = @"
                     CREATE TABLE IF NOT EXISTS Matiere (
                         Id INTEGER PRIMARY KEY AUTOINCREMENT,
                         Nom VARCHAR(20) NOT NULL,
                         Note REAL NOT NULL,
                         Credit REAL NOT NULL
                     );";
                 await createTableCommand.ExecuteNonQueryAsync();
             }
         }

        private void DeleteDatabase()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }

        private async Task RefreshMatiereListFromDatabase()
         {
             matieres.Clear();

             var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
             using (var connection = new SqliteConnection($"Data Source={databasePath}"))
             {
                 await connection.OpenAsync();

                 var selectCommand = connection.CreateCommand();
                 selectCommand.CommandText = "SELECT * FROM Matiere;";
                 using (var reader = await selectCommand.ExecuteReaderAsync())
                 {
                     while (await reader.ReadAsync())
                     {
                         matieres.Add(new Matiere
                         {
                             Id = reader.GetInt32(0),
                             Nom = reader.GetString(1),
                             Note = reader.GetDouble(2),
                             Credit = reader.GetDouble(3)
                         });
                     }
                 }
             }

             matiereListView.ItemsSource = null;
             matiereListView.ItemsSource = matieres;
             totalCredits = matieres.Sum(m => m.Credit);
            
             idEntry.Text = "";

        }

        private async void OnViderClicked(object sender, EventArgs e)
        {
            nomEntry.Text = "";
            noteEntry.Text = "";
            creditEntry.Text = "";
            idEntry.Text = "";

        }

        private async void CalculClicked(object sender, EventArgs e)
        {
            totalCredits = matieres.Sum(m => m.Credit);
            double totalWeightedGrade = matieres.Sum(m => m.Note * m.Credit);
            double average = totalWeightedGrade / totalCredits;
            await DisplayAlert("Votre moyenne: ", $"{average:F2}", "OK");

        }



        private async void OnAjouterClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nomEntry.Text) && !string.IsNullOrWhiteSpace(noteEntry.Text) && !string.IsNullOrWhiteSpace(creditEntry.Text))
            {
                if (double.TryParse(noteEntry.Text, out double note) && double.TryParse(creditEntry.Text, out double credit) && (note>=0 && note<=20) && (credit > 0))
                {
                    if (totalCredits + credit <= 100)
                    {
                        string nom = nomEntry.Text;
                        matieres.Add(new Matiere { Nom = nom, Note = note, Credit = credit });
                        totalCredits = matieres.Sum(m => m.Credit);
                        nomEntry.Text = "";
                        noteEntry.Text = "";
                        creditEntry.Text = "";
                        idEntry.Text = "";

                        // Insert data into database
                        await InsertMatiereIntoDatabase(nom, note, credit);

                        await DisplayAlert("Succès", "La matière a été ajoutée avec succès.", "OK");

                    }
                    else
                    {
                        await DisplayAlert("Attention", "Le total des crédits ne peut pas dépasser 30.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erreur", "Veuillez entrer des valeurs numériques valides pour la note et le crédit.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
            }
            await RefreshMatiereListFromDatabase();
        }


        private async Task InsertMatiereIntoDatabase(string nom, double note, double credit)
         {
             var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
             using (var connection = new SqliteConnection($"Data Source={databasePath}"))
             {
                 await connection.OpenAsync();

                 var insertCommand = connection.CreateCommand();
                 insertCommand.CommandText = "INSERT INTO Matiere (Nom, Note, Credit) VALUES (@Nom, @Note, @Credit);";
                 insertCommand.Parameters.AddWithValue("@Nom", nom);
                 insertCommand.Parameters.AddWithValue("@Note", note);
                 insertCommand.Parameters.AddWithValue("@Credit", credit);
                 await insertCommand.ExecuteNonQueryAsync();
             }
         }

        private async void OnMatiereItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && e.Item is Matiere selectedMatiere)
            {
                // Remplir les champs avec les informations de l'élément sélectionné
                nomEntry.Text = selectedMatiere.Nom;
                noteEntry.Text = selectedMatiere.Note.ToString();
                creditEntry.Text = selectedMatiere.Credit.ToString();
                idEntry.Text = selectedMatiere.Id.ToString();   
            }
        }

        private async void OnSupprimerClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(idEntry.Text) && int.TryParse(idEntry.Text, out int id))
            {
                try
                {
                    var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
                    using (var connection = new SqliteConnection($"Data Source={databasePath}"))
                    {
                        await connection.OpenAsync();

                        var deleteCommand = connection.CreateCommand();
                        deleteCommand.CommandText = "DELETE FROM Matiere WHERE Id = @Id;";
                        deleteCommand.Parameters.AddWithValue("@Id", id);
                        await deleteCommand.ExecuteNonQueryAsync();

                        await DisplayAlert("Succès", "La matière a été supprimée avec succès.", "OK");

                        // Rafraîchir la liste après suppression
                        await RefreshMatiereListFromDatabase();

                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur", $"Une erreur s'est produite lors de la suppression de la matière : {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner la matière à supprimer dans le tableau.", "OK");
            }
        }

        private async void OnModifierClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(idEntry.Text) && int.TryParse(idEntry.Text, out int id))
            {
                if (!string.IsNullOrWhiteSpace(nomEntry.Text) && !string.IsNullOrWhiteSpace(noteEntry.Text) && !string.IsNullOrWhiteSpace(creditEntry.Text))
                {
                    if (double.TryParse(noteEntry.Text, out double note) && double.TryParse(creditEntry.Text, out double credit) && (note >= 0 && note <= 20) && (credit > 0))
                    {
                        try
                        {
                            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
                            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
                            {
                                await connection.OpenAsync();
                                var updateCommand = connection.CreateCommand();
                                updateCommand.CommandText = "UPDATE Matiere SET Nom = @Nom, Note = @Note, Credit = @Credit WHERE Id = @Id;";
                                updateCommand.Parameters.AddWithValue("@Nom", nomEntry.Text);
                                updateCommand.Parameters.AddWithValue("@Note", note);
                                updateCommand.Parameters.AddWithValue("@Credit", credit);
                                updateCommand.Parameters.AddWithValue("@Id", id);
                                await updateCommand.ExecuteNonQueryAsync();

                                await DisplayAlert("Succès", "La matière a été modifiée avec succès.", "OK");

                                // Rafraîchir la liste après modification
                                await RefreshMatiereListFromDatabase();

                            }

                            // Calculate and display average if totalCredits equals 30

                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Erreur", $"Une erreur s'est produite lors de la modification de la matière : {ex.Message}", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Erreur", "Veuillez entrer des valeurs numériques valides pour la note et le crédit.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner la matière à modifier dans le tableau.", "OK");
            }
        }

    }

    public class Matiere
     {
         public int Id { get; set; }
         public string Nom { get; set; }
         public double Note { get; set; }
         public double Credit { get; set; }
     }

    }



