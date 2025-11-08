# P.E.C.O. – Benzinărie Horror Simulator (Unity)

Acest repository oferă implementarea de bază a simulatorului de benzinărie **P.E.C.O.** pentru Unity. Jocul este plasat pe un drum pustiu din Ardeal, la 50 KM de Cluj-Napoca, și include managementul minimarketului, al pompelor de combustibil și infrastructura pentru evenimente horror viitoare.

## Caracteristici

- **Context narativ**: ScriptableObject dedicat (`WorldDescriptor`) pentru a expune descrieri atmosferice și note de organizare în UI.
- **Inventar modular**: Rafturi, sloturi și depozit pentru minimarket configurabile direct din Inspector.
- **Calculator de restock**: Algoritm configurabil prin `RestockCalculator` pentru a planifica reaprovizionarea.
- **Pompă de combustibil**: Gestionarea volumului disponibil, calculul costului total și refil al rezervoarelor.
- **Manager central**: `PecoGasStationManager` conectează lumea, inventarul și pompele pregătind terenul pentru logica horror ulterioară.
- **HUD minimal**: Script pentru a afișa în UI informațiile despre locație și rapoartele de restock.

## Structura Unity

```
unity/PECOHorrorSimulator/
└── Assets/
    └── Scripts/
        ├── GasStation/
        │   ├── FuelPump.cs
        │   ├── GameLoopController.cs
        │   ├── PecoGasStationManager.cs
        │   ├── Inventory/
        │   │   ├── InventoryItemDefinition.cs
        │   │   ├── InventoryManager.cs
        │   │   ├── InventoryShelf.cs
        │   │   └── InventorySlot.cs
        │   ├── Restock/
        │   │   └── RestockCalculator.cs
        │   └── World/
        │       └── WorldDescriptor.cs
        └── UI/
            └── GasStationHud.cs
```

Toate scripturile sunt scrise în C# și pot fi atașate obiectelor din Unity pentru a configura simularea.

## Cum rulezi proiectul în Unity

1. **Creează un proiect 3D în Unity** (Unity 2021.3 LTS sau mai nou recomandat).
2. Copiază folderul `Assets` din `unity/PECOHorrorSimulator` în proiectul tău nou sau importă-l prin Unity Package Manager ca pachet local.
3. În scenă:
   - Creează un GameObject gol numit `P.E.C.O. Station` și atașează `PecoGasStationManager`.
   - Adaugă componente `FuelPump`, `InventoryManager`, `InventoryShelf` și configurează `InventorySlot`-urile cu `InventoryItemDefinition` ScriptableObjects (creează-le din meniu: **Create → Peco → Inventory Item**).
   - Creează un `WorldDescriptor` (**Create → Peco → World Descriptor**) și setează descrierile locației.
   - Adaugă un Canvas cu componente TextMeshPro și atașează `GasStationHud` pentru a afișa informațiile.
   - Atașează `GameLoopController` pe un GameObject din scenă pentru a simula turele și verificările de restock.
4. Rulează scena în Play Mode. Vei putea declanșa restock manual din inspector (butonul `GenerateRestockPlan` via context menu sau apelând din alte scripturi) și vei vedea rapoartele actualizate în HUD.

## Extensii recomandate

- Integrarea unui sistem de evenimente horror (lumini care se sting, NPC-uri misterioase) conectat la `GameLoopController`.
- UI suplimentar pentru gestionarea vânzărilor, a taxării combustibilului și a depozitului.
- Salvarea progresului între ture și un sistem de economie (profit/pierderi).

## Licență

Poți folosi codul ca punct de pornire pentru jocul tău P.E.C.O. în Unity. Ajustează și extinde după nevoile proiectului.
