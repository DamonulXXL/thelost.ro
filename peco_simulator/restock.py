"""Restock planning logic for P.E.C.O. gas station."""

from __future__ import annotations

from dataclasses import dataclass
from typing import Dict, Iterable

from .inventory import Inventory, StockRecord


@dataclass
class RestockRecommendation:
    """Stores restock quantities for each item."""

    amounts: Dict[str, int]

    def format(self) -> str:
        lines = ["Plan de reaprovizionare:"]
        for item_name, quantity in sorted(self.amounts.items()):
            lines.append(f" - {item_name}: {quantity} buc")
        return "\n".join(lines)


class RestockCalculator:
    """Computes restock amounts based on minimum shelf thresholds."""

    def __init__(self, minimums: Dict[str, int]) -> None:
        self.minimums = minimums

    def recommend(self, inventory: Inventory) -> RestockRecommendation:
        amounts: Dict[str, int] = {}
        for record in inventory.records.values():
            target = self.minimums.get(record.item.category, 0)
            deficit = target - record.on_shelf
            if deficit > 0:
                moveable = min(deficit, record.in_depot)
                amounts[record.item.name] = moveable
        return RestockRecommendation(amounts)

    @staticmethod
    def from_snapshot(snapshot: Iterable[StockRecord], default_minimum: int = 5) -> "RestockCalculator":
        """Create a calculator that ensures each item keeps default shelf stock."""

        minimums = {record.item.category: default_minimum for record in snapshot}
        return RestockCalculator(minimums)
