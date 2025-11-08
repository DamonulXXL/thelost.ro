# P.E.C.O. – Benzinărie Horror Simulator

Acest proiect conține infrastructura de bază pentru simularea unei benzinării izolate din Transilvania, România. Benzinăria poartă numele **P.E.C.O.** și se află pe un drum național pustiu, cu indicator rutier „Cluj-Napoca 50 KM”. Simulatorul acoperă managementul minimarketului, organizarea spațiului și operațiunile de alimentare cu combustibil, pregătind terenul pentru viitoare evenimente horror.

## Caracteristici

- Context narativ complet al locației, inclusiv repere și atmosferă unică ardelenească.
- Inventar pentru minimarket cu rafturi, produse tradiționale, cafea și gustări rapide.
- Management al depozitului și al restock-ului printr-un calculator dedicat.
- Pompă de combustibil cu calcul automat al taxelor.
- Plan de organizare a rafturilor și zonelor din minimarket.
- Interfață de linie de comandă pentru a explora și opera benzinăria.

## Descărcare și instalare

1. **Clonează proiectul** folosind Git:

   ```bash
   git clone https://github.com/<utilizator>/<repo>.git peco-simulator
   cd peco-simulator
   ```

   Înlocuiește `<utilizator>/<repo>` cu locația reală a repository-ului.

2. **Creează un mediu virtual (opțional, dar recomandat)** și instalează dependențele de dezvoltare:

   ```bash
   python -m venv .venv
   source .venv/bin/activate  # Windows: .venv\Scripts\activate
   pip install -r requirements-dev.txt
   ```

   Dependențele sunt minime (pytest pentru teste); simulatorul propriu-zis rulează doar cu Python standard.

## Utilizare

1. Asigură-te că ești în rădăcina proiectului și, dacă ai creat unul, mediu virtual este activ.
2. Rulează simulatorul din linia de comandă:

   ```bash
   python -m peco_simulator.cli
   ```

   Vei putea consulta descrierea locației, inventarul și poți genera restock-uri.

3. Pentru a rula testele automate și a verifica integritatea simulatorului:

   ```bash
   pytest
   ```

## Structura proiectului

- `peco_simulator/world.py` – contextul și descrierea lumii.
- `peco_simulator/inventory.py` – gestionarea obiectelor din minimarket.
- `peco_simulator/restock.py` – logica pentru calculatorul de restock.
- `peco_simulator/gas_station.py` – logica principală a benzinăriei, depozit și pompe.
- `peco_simulator/cli.py` – interfața de linie de comandă.
- `tests/` – teste unitare pentru funcționalitate cheie.

## Direcții viitoare

- Evenimente horror dinamice și sistem de noapte.
- Interacțiuni cu NPC-uri și clienți misterioși.
- Extinderea depozitului și gestionarea furnizorilor.
- Integrarea unor mini-jocuri pentru securitate și supraviețuire.

Acest punct de plecare oferă structura necesară pentru a continua dezvoltarea experienței horror în jurul benzinăriei P.E.C.O.
