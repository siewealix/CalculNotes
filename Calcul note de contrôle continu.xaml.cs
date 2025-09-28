namespace CalculNotes;

public partial class Calcul_note_de_contrôle_continu : ContentPage
{
	public Calcul_note_de_contrôle_continu()
	{
		InitializeComponent();
	}


    private void OnCalculerClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(noteSNEntry.Text) && !string.IsNullOrWhiteSpace(noteNFEntry.Text))
        {
            if (double.TryParse(noteSNEntry.Text, out double noteSN) && double.TryParse(noteNFEntry.Text, out double noteNF))
            {
                // Vérification des contraintes SN
                if (noteSN < 0 || noteSN > 20)
                {
                    DisplayAlert("Erreur", "La note de la session normale doit être comprise entre 0 et 20.", "OK");
                    return;
                }

                // Vérification des contraintes NF
                if (noteNF < 0 || noteNF > 20)
                {
                    DisplayAlert("Erreur", "La note finale doit être comprise entre 0 et 20.", "OK");
                    return;
                }

                // Calcul de la note de CC nécessaire
                double noteCC = (noteNF - (0.7 * noteSN)) / 0.3;

                // Vérification si la note de CC est hors des limites
                if (noteCC > 20)
                {
                    DisplayAlert("Note de CC nécessaire", "La note de contrôle continu nécessaire dépasse 20.", "OK");
                }
                else if (noteCC < 0)
                {
                    DisplayAlert("Note de CC nécessaire", "La note de contrôle continu nécessaire est inférieure à 0.", "OK");
                }
                else
                {
                    // Affichage de la note de CC nécessaire
                    DisplayAlert("Note de CC nécessaire", $"La note de contrôle continu nécessaire est : {noteCC:F2}", "OK");
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