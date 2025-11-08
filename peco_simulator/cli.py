"""Command line interface for managing the P.E.C.O. simulator."""

from __future__ import annotations

from dataclasses import dataclass
from typing import Callable, Dict

from .gas_station import GasStation
from .restock import RestockCalculator


@dataclass
class Command:
    description: str
    handler: Callable[[GasStation], None]


class GasStationCLI:
    def __init__(self, station: GasStation) -> None:
        self.station = station
        self.commands: Dict[str, Command] = {
            "descriere": Command("Afișează detalii despre locație", self.describe),
            "inventar": Command("Afișează inventarul curent", self.show_inventory),
            "restock": Command("Rulează calculatorul de restock și mută marfa", self.run_restock),
            "organizeaza": Command("Afișează planul de organizare", self.show_organization),
            "iesire": Command("Părăsește simularea", self.exit_program),
        }
        self._running = False

    def run(self) -> None:
        self._running = True
        print("Bun venit la P.E.C.O. - simulator de benzinărie horror.")
        while self._running:
            print("\nComenzi disponibile:")
            for name, command in self.commands.items():
                print(f" - {name}: {command.description}")
            cmd = input(">>> ").strip().lower()
            command = self.commands.get(cmd)
            if not command:
                print("Comandă necunoscută. Încearcă din nou.")
                continue
            command.handler(self.station)

    def describe(self, station: GasStation) -> None:
        print(station.describe())

    def show_inventory(self, station: GasStation) -> None:
        print("Inventar P.E.C.O.:")
        for entry in station.inventory.snapshot():
            print(
                f" - {entry['item']} ({entry['category']}): {entry['on_shelf']} pe raft, "
                f"{entry['in_depot']} în depozit (total {entry['total']})"
            )

    def run_restock(self, station: GasStation) -> None:
        calculator = RestockCalculator.from_snapshot(station.inventory.records.values())
        recommendation = station.restock_shelves(calculator)
        print(recommendation.format())

    def show_organization(self, station: GasStation) -> None:
        print(station.describe_organization())

    def exit_program(self, _: GasStation) -> None:
        print("Închidere simulare. Ai grijă la umbrele din jurul pompelor...")
        self._running = False


def main() -> None:
    station = GasStation()
    station.register_default_stock()
    cli = GasStationCLI(station)
    cli.run()


if __name__ == "__main__":
    main()
