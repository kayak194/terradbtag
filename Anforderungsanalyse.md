# 3. Anforderungsanalyse

Während den ersten zwei Gruppenmeetings wurden die Anwendungsfälle für Folksonomien, Social
Tagging und Tag-Clouds im ERP-System von TecWare ausgearbeitet. Anhand dessen wurden folgende Prinzipsskizzen erstellt, wie auch Anforderungen schriftlich formuliert. Folgene Ergebnisse wurden dabei erzielt:

##3.1 Prinzipskizzen:

1. Prinzipskizze 
<pre><pre><pre> <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/01Prinzipienskizze.png" width="200px" height="200px" /><br>
2. Prinzipskizze 
<pre><pre><pre>  <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/02Prinzipienskizze.png" width="200px" height="200px" /><br>
3. Prinzipskizze 
<pre><pre><pre>  <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/03Prinzipienskizze.png" width="200px" height="200px" />

##3.2 Anforderungen:

**Glossar**:

Systemtags: Tags, die bei Erstellung eines Objekts vom System generiert werden.
Usertags: Tags, die vom User selbst erstellt und an das Objekt angehangen werden. 
Tags: 	Menge aller System- und Usertags.

Klassifikation: 
    0 = Text
		1 = Datum
		2 = Nummer

**System**:

Das System muss in der Lage sein, eine Vielzahl von Tags anzuzeigen.
Das System muss in der Lage sein, nach bestimmten Tags zu filtern
Das System muss in der Lage sein, eine Liste von Objekten (Bestellnummer, Artikel, Artikelnummer, Kundennummer) zu verkleinern, nachdem nach einem bestimmten Tag gefiltert wurde.
Das System muss in der Lage sein, einem Objekt Systemtags anzuhängen.
Das System muss in der Lage sein, aus den in der Liste angezeigten Tags, eine WordCloud zu generieren und anzuzeigen.
Das System muss in der Lage sein, Systemtags zu klassifizieren. 
Das System muss in der Lage sein, zu einem einzelnen Objekt eine WordCloud zu generieren.
Das System muss in der Lage sein, in der Eingabemaske eine alphabetische Sortierung vorzunehemen.
Das System muss in der Lage sein, eine Filterliste in der Suchleiste zu generieren.
neu: Das System muss in der Lage sein einen Suchverlauf anzuzeigen.



**Nutzer**:

Der Nutzer muss in der Lage sein, nach Schlagworten in einer Eingabemaske zu suchen.
Der Nutzer muss in der Lage sein, eigene Tags an ein Objekt hinzuzufügen.
Der Nutzer muss in der Lage sein, Tags in der WordCloud ausblenden zu können.
Der Nutzer muss in der Lage sein, eigene Usertags zu ändern/löschen.
Der Nutzer muss in der Lage sein, Usertags zu klassifizieren.
neu: Der Nutzer muss in der Lage sein, durch eine Interaktion einen Tag als Filter zu setzen.
neu: Der Nutzer muss in der Lage sein, einen Tag auszuwählen.
neu: Der Nutzer muss in der Lage sein, durch eine Interaktion einen Tag auszublenden.


**Administrator**:

Der Administrator muss in der Lage sein, Tags zu ändern / löschen.










