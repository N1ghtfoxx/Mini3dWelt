##### WICHTIGER Hinweis:

Bitte folgende Anweisungen beachten, um das Spiel korrekt zu starten:

1. **Unity Version**: Stelle sicher, dass du Unity6 installiert hast, um Kompatibilitätsprobleme zu vermeiden.
2. **Unity-Packet**: Importiere das entsprechende Unity-Paket in dein Unity-Projekt.
3. **Cinemachine**: Stelle sicher, dass das Cinemachine-Paket installiert ist, um die Kamerafunktionen und die Steuerung zu nutzen.
   - füge das Cinemaine-Brain zu deiner Hauptkamera hinzu.
   - wähle die Cinemachine Virtual Camera aus und stelle Tracking Target auf 'EmptyKapsel' ein.
   - unter Input Axis Controller findest du den Punkt Driven Axes. Weise hier CM Default/Look zu
     (zu finden unter Packages/Cinemachine/Runtime/Input). Das ermöglicht die Maussteuerung der Kamera.
4. **Text Mesh Pro**: Stelle sicher, dass das Text Mesh Pro-Paket installiert ist, um die Benutzeroberfläche korrekt anzuzeigen.

----

# Dungeon Maze

Ein 3D-Labyrinth-Abenteuer in atmosphärischer Dungeon-Optik aus der Ego-Perspektive.

## Spielbeschreibung

Begib dich in ein geheimnisvolles Labyrinth und finde deinen Weg zum Master-Raum. Sammle Schlüssel, um verschlossene Türen zu öffnen,
öffne Truhen und finde wertvolle Edelsteine und halte Ausschau nach Nahrung, um deine Reise durch die dunklen Gänge zu überstehen.

## Spielziel

Das Hauptziel ist es, den Master-Raum zu erreichen. Um dorthin zu gelangen, müssen verschiedene Schlüssel gefunden und die entsprechenden Türen geöffnet werden.

## Steuerung

- **Bewegung**: WASD-Tasten und Maus
- **Interaktion**: `E` - Zum Einsammeln von Gegenständen und Öffnen von Türen und Truhen
- **Inventar**: `I` - Inventar öffnen/schließen

## Gameplay-Elemente

### Gegenstände
- **Schlüssel**: Ermöglichen das Öffnen bestimmter Türen
- **Truhen**: Enthalten wertvolle Edelsteine 
- **Nahrung**: Kann eingesammelt und im Inventar aufbewahrt werden

### Besondere Orte
- **Geheimgänge**: Abkürzungen durch das Labyrinth (findest du sie alle?)

### Interaktionen
- Alle Interaktionen mit Objekten erfolgen über die `E`-Taste
- Gefundene Gegenstände werden automatisch im Inventar gespeichert
- Das Inventar kann jederzeit mit der `I`-Taste eingesehen werden

## Features

- **Automatische Speicherung**: Der Spielfortschritt wird automatisch gespeichert
- **Immersive Ego-Perspektive**: Detaillierte 3D-Umgebung im Dungeon-Stil aus der First-Person-Sicht
- **Inventar-System**: Verwaltung gesammelter Gegenstände
- **Schlüssel-Türen-Mechanik**: Strategisches Gameplay durch Schlüssel-basierte Progression
- **Tag-Nacht-Zyklus**: Dynamische Beleuchtung mit wechselnden Lichtverhältnissen je nach Tageszeit

## Technische Details

### Entwicklung
- **Engine**: Unity
- **Entwicklungsunterstützung**: KI-gestützte Programmierung mit Claude und ChatGPT
- **Plattform**: PC

### Assets und Grafiken

#### Eigenentwickelte 3D-Models
Die folgenden Assets wurden komplett eigenständig erstellt:
- Truhe
- Schlüssel
- Edelstein
- Münze
- Schinken

**Tools**: Blender (3D-Modellierung) und Adobe Substance 3D Painter (Texturierung)
**Materials & Textures**: Alle Materialien und Texturen wurden der Adobe Substance 3D Painter Bibliothek entnommen und angepasst.

#### Externe Assets

**Umgebungs-Assets**: Unity Asset Store 

## Quellen 
** Level Design Assets - hand-painted dungeon free (Boden, Wände, Pfeiler, Türen, Torbögen, Laternen) ** 
https://assetstore.unity.com/packages/3d/environments/stylized-hand-painted-dungeon-free-173934
** Green Gemstone-Icon UpperPanel und Inventory **
https://assetstore.unity.com/packages/2d/environments/free-game-items-131764

---

*Entwickelt mit Unity Engine und KI-Unterstützung*