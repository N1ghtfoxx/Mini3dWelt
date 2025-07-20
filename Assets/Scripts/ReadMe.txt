##### WICHTIGER Hinweis:

Bitte folgende Anweisungen beachten, um das Spiel korrekt zu starten:

1. **Unity Version**: Stelle sicher, dass du Unity6 installiert hast, um Kompatibilit�tsprobleme zu vermeiden.
2. **Unity-Packet**: Importiere das entsprechende Unity-Paket in dein Unity-Projekt.
3. **Cinemachine**: Stelle sicher, dass das Cinemachine-Paket installiert ist, um die Kamerafunktionen und die Steuerung zu nutzen.
   - f�ge das Cinemaine-Brain zu deiner Hauptkamera hinzu.
   - w�hle die Cinemachine Virtual Camera aus und stelle Tracking Target auf 'EmptyKapsel' ein.
   - unter Input Axis Controller findest du den Punkt Driven Axes. Weise hier CM Default/Look zu
     (zu finden unter Packages/Cinemachine/Runtime/Input). Das erm�glicht die Maussteuerung der Kamera.
4. **Text Mesh Pro**: Stelle sicher, dass das Text Mesh Pro-Paket installiert ist, um die Benutzeroberfl�che korrekt anzuzeigen.

----

# Dungeon Maze

Ein 3D-Labyrinth-Abenteuer in atmosph�rischer Dungeon-Optik aus der Ego-Perspektive.

## Spielbeschreibung

Begib dich in ein geheimnisvolles Labyrinth und finde deinen Weg zum Master-Raum. Sammle Schl�ssel, um verschlossene T�ren zu �ffnen,
�ffne Truhen und finde wertvolle Edelsteine und halte Ausschau nach Nahrung, um deine Reise durch die dunklen G�nge zu �berstehen.

## Spielziel

Das Hauptziel ist es, den Master-Raum zu erreichen. Um dorthin zu gelangen, m�ssen verschiedene Schl�ssel gefunden und die entsprechenden T�ren ge�ffnet werden.

## Steuerung

- **Bewegung**: WASD-Tasten und Maus
- **Interaktion**: `E` - Zum Einsammeln von Gegenst�nden und �ffnen von T�ren und Truhen
- **Inventar**: `I` - Inventar �ffnen/schlie�en

## Gameplay-Elemente

### Gegenst�nde
- **Schl�ssel**: Erm�glichen das �ffnen bestimmter T�ren
- **Truhen**: Enthalten wertvolle Edelsteine 
- **Nahrung**: Kann eingesammelt und im Inventar aufbewahrt werden

### Besondere Orte
- **Geheimg�nge**: Abk�rzungen durch das Labyrinth (findest du sie alle?)

### Interaktionen
- Alle Interaktionen mit Objekten erfolgen �ber die `E`-Taste
- Gefundene Gegenst�nde werden automatisch im Inventar gespeichert
- Das Inventar kann jederzeit mit der `I`-Taste eingesehen werden

## Features

- **Automatische Speicherung**: Der Spielfortschritt wird automatisch gespeichert
- **Immersive Ego-Perspektive**: Detaillierte 3D-Umgebung im Dungeon-Stil aus der First-Person-Sicht
- **Inventar-System**: Verwaltung gesammelter Gegenst�nde
- **Schl�ssel-T�ren-Mechanik**: Strategisches Gameplay durch Schl�ssel-basierte Progression
- **Tag-Nacht-Zyklus**: Dynamische Beleuchtung mit wechselnden Lichtverh�ltnissen je nach Tageszeit

## Technische Details

### Entwicklung
- **Engine**: Unity
- **Entwicklungsunterst�tzung**: KI-gest�tzte Programmierung mit Claude und ChatGPT
- **Plattform**: PC

### Assets und Grafiken

#### Eigenentwickelte 3D-Models
Die folgenden Assets wurden komplett eigenst�ndig erstellt:
- Truhe
- Schl�ssel
- Edelstein
- M�nze
- Schinken

**Tools**: Blender (3D-Modellierung) und Adobe Substance 3D Painter (Texturierung)
**Materials & Textures**: Alle Materialien und Texturen wurden der Adobe Substance 3D Painter Bibliothek entnommen und angepasst.

#### Externe Assets

**Umgebungs-Assets**: Unity Asset Store 

## Quellen 
** Level Design Assets - hand-painted dungeon free (Boden, W�nde, Pfeiler, T�ren, Torb�gen, Laternen) ** 
https://assetstore.unity.com/packages/3d/environments/stylized-hand-painted-dungeon-free-173934
** Green Gemstone-Icon UpperPanel und Inventory **
https://assetstore.unity.com/packages/2d/environments/free-game-items-131764

---

*Entwickelt mit Unity Engine und KI-Unterst�tzung*