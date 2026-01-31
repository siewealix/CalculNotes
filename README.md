# CalculNotes

Application **.NET MAUI** multiplateforme pour calculer des notes (contrôle continu, session normale, note finale). Le projet vise à offrir un outil simple et lisible pour saisir des notes et obtenir des moyennes rapidement.

## Fonctionnalités principales

- Calcul de la **note de contrôle continu**.
- Calcul de la **note de session normale**.
- Calcul de la **note finale**.
- Interface MAUI pensée pour une utilisation rapide sur mobile et desktop.

## Prérequis

- **.NET 8 SDK**
- Les workloads MAUI nécessaires pour votre plateforme cible :
  - Android (ex. `android`)
  - iOS / MacCatalyst (ex. `ios`, `maccatalyst`)
  - Windows (ex. `maui-windows`)

> Astuce : installez les workloads via `dotnet workload install maui` puis, si besoin, ajoutez les plateformes.

## Lancer le projet

Depuis la racine du repo :

```bash
dotnet build
```

Exemples de lancement (à adapter à votre OS) :

```bash
# Android

dotnet build -t:Run -f net8.0-android

# Windows

dotnet build -t:Run -f net8.0-windows10.0.19041.0
```

## Structure rapide

- `MainPage.xaml` : page principale.
- `Calcul note de contrôle continu.xaml` : écran de calcul du contrôle continu.
- `Calcul note de session normale.xaml` : écran de calcul de la session normale.
- `Calcul note finale.xaml` : écran de calcul de la note finale.
- `Resources/` : icônes, images et ressources.

## Développement

- Projet MAUI mono-solution : `CalculNotes.sln`.
- Le fichier `CalculNotes.csproj` contient les cibles et dépendances MAUI.

---

Si vous souhaitez ajouter des règles de calcul spécifiques (pondérations, coefficients, etc.), elles peuvent être centralisées dans les pages XAML/C# associées pour garder le comportement lisible et facile à ajuster.
