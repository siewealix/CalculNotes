namespace CalculNotes;

public partial class Calcul_note_finale : ContentPage
{
	public Calcul_note_finale()
	{
		InitializeComponent();
	}

    private void OnCalculerClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(noteCCEntry.Text) && !string.IsNullOrWhiteSpace(noteSNEntry.Text))
        {
            if (double.TryParse(noteCCEntry.Text, out double noteCC) && double.TryParse(noteSNEntry.Text, out double noteSN))
            {
                // Vérification des contraintes SN
                if (noteCC < 0 || noteCC > 20)
                {
                    DisplayAlert("Erreur", "La note de contrôle continu doit être comprise entre 0 et 20.", "OK");
                    return;
                }

                // Vérification des contraintes SN
                if (noteSN < 0 || noteSN > 20)
                {
                    DisplayAlert("Erreur", "La note de session normale doit être comprise entre 0 et 20.", "OK");
                    return;
                }

                // Calcul de la note de SN nécessaire
                double noteFN = ((0.3 * noteCC) + (0.7 * noteSN));

                // Vérification si la note de SN est hors des limites
                if (noteFN > 20)
                {
                    DisplayAlert("Note de FN ", "La note finale dépasse 20.", "OK");
                }
                else if (noteSN < 0)
                {
                    DisplayAlert("Note de FN", "La note finale est inférieure à 0.", "OK");
                }
                else
                {
                    // Affichage de la note de SN nécessaire
                    DisplayAlert("Note de FN", $"La note finale est : {noteFN:F2}", "OK");
                }
            }
            else
            {
                DisplayAlert("Erreur", "Veuillez entrer des valeurs numériques valides.", "OK");
            }
        }
        else
        {
            DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
        }
    }
}