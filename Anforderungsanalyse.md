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

Das System muss in der Lage sein, eine Vielzahl von Tags anzuzeigen.<br>
Das System muss in der Lage sein, nach bestimmten Tags zu filtern<br>
Das System muss in der Lage sein, eine Liste von Objekten (Bestellnummer, Artikel, Artikelnummer, Kundennummer) zu verkleinern, nachdem nach einem bestimmten Tag gefiltert wurde.<br>
Das System muss in der Lage sein, einem Objekt Systemtags anzuhängen.<br>
Das System muss in der Lage sein, aus den in der Liste angezeigten Tags, eine WordCloud zu generieren und anzuzeigen.<br>
Das System muss in der Lage sein, Systemtags zu klassifizieren. <br>
Das System muss in der Lage sein, zu einem einzelnen Objekt eine WordCloud zu generieren.<br>
Das System muss in der Lage sein, in der Eingabemaske eine alphabetische Sortierung vorzunehemen.<br>
Das System muss in der Lage sein, eine Filterliste in der Suchleiste zu generieren.<br>
Das System muss in der Lage sein einen Suchverlauf anzuzeigen.<br>



**Nutzer**:

Der Nutzer muss in der Lage sein, nach Schlagworten in einer Eingabemaske zu suchen.<br>
Der Nutzer muss in der Lage sein, eigene Tags an ein Objekt hinzuzufügen.<br>
Der Nutzer muss in der Lage sein, Tags in der WordCloud ausblenden zu können.<br>
Der Nutzer muss in der Lage sein, eigene Usertags zu ändern/löschen.<br>
Der Nutzer muss in der Lage sein, Usertags zu klassifizieren.<br>
Der Nutzer muss in der Lage sein, durch eine Interaktion einen Tag als Filter zu setzen.<br>
Der Nutzer muss in der Lage sein, einen Tag auszuwählen.<br>
Der Nutzer muss in der Lage sein, durch eine Interaktion einen Tag auszublenden.<br>


**Administrator**:

Der Administrator muss in der Lage sein, Tags zu ändern / löschen.


##3.3 Entwurf der GUIs zur Eingabe von Tags:

Um eine Vorstellung von der zukünftigen Oberfläche des ERP - Systems zu bekommen, wurden von den Studenten grafisch fünf GUIs entworfen. Diese sollten der Eingabe von Tags für sogenannte Datenobjekte dienen.
Die Aufgabe der Studenten bestand darin, sich eine Form der Eingabemöglichkeit für Tags zu überlegen wobei auch neue
Dialoge entstehen können. Die Ergebnisse wurden im Gruppenmeeting präsentiert und deren Verwendung, im Austausch mit Herrn Stein  diskutiert. 

1. Inline – vs. Popup – Editor
<pre><pre><pre>  <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/InlVSPopup.PNG" width="200px" height="160px" /><br>
2. Beispielmaske (ohne Tag Eingabe)
<pre><pre><pre>  <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/BeispielMaskeOhne%20TE.PNG" width="200px" height="160px" /><br>
3. Beispielmaske (mit Inline – Tag Eingabe)
  <pre><pre><pre><img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/SAPmitInline.PNG" width="200px" height="160px" /><br>
4. Beispielmaske (mit Popup – Tag Eingabe)
  <pre><pre><pre><img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/SAPmitPopUpPNG.PNG" width="200px" height="160px" /><br>
5. Twitter – like“ Textbox (Inline)
<pre><pre><pre>  <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/TwitterLIke.PNG" width="240px" height="120px" /><br>
6. IDE – like“ Editor (Inline)
 <pre><pre><pre><img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/IDELike.PNG" width="220px" height="160px" /><br>
7. Evernote – like“ Tagbox (Inline)
<pre><pre><pre> <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/EvernoteLike.PNG" width="220px" height="160px" /><br>
8. Classic“ Listbox (Popup)
<pre><pre><pre>  <img src="https://github.com/vardoo/terradbtag/blob/Dokumentation/Images/ClassikListBox.PNG" width="220px" height="160px" />









