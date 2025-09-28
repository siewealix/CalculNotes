namespace CalculNotes;

public partial class Calcul_note_de_session_normale : ContentPage
{
	public Calcul_note_de_session_normale()
	{
		InitializeComponent();
	}

    private void OnCalculerClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(noteCCEntry.Text) && !string.IsNullOrWhiteSpace(noteNFEntry.Text))
        {
            if (double.TryParse(noteCCEntry.Text, out double noteCC) && double.TryParse(noteNFEntry.Text, out double noteNF))
            {
                // Vérification des contraintes SN
                if (noteCC < 0 || noteCC > 20)
                {
                    DisplayAlert("Erreur", "La note de contrôle continu doit être comprise entre 0 et 20.", "OK");
                    return;
                }

                // Vérification des contraintes NF
                if (noteNF < 0 || noteNF > 20)
                {
                    DisplayAlert("Erreur", "La note finale doit être comprise entre 0 et 20.", "OK");
                    return;
                }

                // Calcul de la note de SN nécessaire
                double noteSN = (noteNF - (0.3 * noteCC)) / 0.7;

                // Vérification si la note de SN est hors des limites
                if (noteSN > 20)
                {
                    DisplayAlert("Note de SN nécessaire", "La note de sesion normale nécessaire dépasse 20.", "OK");
                }
                else if (noteSN < 0)
                {
                    DisplayAlert("Note de SN nécessaire", "La note de sesion normale nécessaire est inférieure à 0.", "OK");
                }
                else
                {
                    // Affichage de la note de SN nécessaire
                    DisplayAlert("Note de SN nécessaire", $"La note de sesion normale nécessaire est : {noteSN:F2}", "OK");
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