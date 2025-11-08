"""Inventory system for the P.E.C.O. minimarket and fuel management."""

from __future__ import annotations

from dataclasses import dataclass, field
from typing import Dict, Iterable, List, Optional


@dataclass
class InventoryItem:
    """Represents a sellable item in the minimarket shelves."""

    name: str
    category: str
    price: float
    unit: str = "buc"
    taxable: bool = True


@dataclass
class StockRecord:
    """Tracks stock levels for an inventory item."""

    item: InventoryItem
    on_shelf: int = 0
    in_depot: int = 0

    def total(self) -> int:
        return self.on_shelf + self.in_depot

    def move_to_shelf(self, quantity: int) -> None:
        if quantity < 0:
            raise ValueError("Quantity must be positive.")
        if quantity > self.in_depot:
            raise ValueError("Not enough stock in depot to move to shelf.")
        self.in_depot -= quantity
        self.on_shelf += quantity

    def move_to_depot(self, quantity: int) -> None:
        if quantity < 0:
            raise ValueError("Quantity must be positive.")
        if quantity > self.on_shelf:
            raise ValueError("Not enough shelf stock to move back to depot.")
        self.on_shelf -= quantity
        self.in_depot += quantity

    def sell(self, quantity: int) -> float:
        if quantity <= 0:
            raise ValueError("Quantity must be positive.")
        if quantity > self.on_shelf:
            raise ValueError("Not enough stock on shelf for sale.")
        self.on_shelf -= quantity
        return quantity * self.item.price


@dataclass
class Inventory:
    """Aggregates stock records for the entire minimarket."""

    records: Dict[str, StockRecord] = field(default_factory=dict)

    def add_item(self, record: StockRecord) -> None:
        if record.item.name in self.records:
            raise ValueError(f"Item {record.item.name} already registered")
        self.records[record.item.name] = record

    def categories(self) -> List[str]:
        return sorted({record.item.category for record in self.records.values()})

    def find(self, name: str) -> Optional[StockRecord]:
        return self.records.get(name)

    def items_by_category(self, category: str) -> Iterable[StockRecord]:
        for record in self.records.values():
            if record.item.category == category:
                yield record

    def sell(self, item_name: str, quantity: int) -> float:
        record = self.find(item_name)
        if not record:
            raise ValueError(f"Item {item_name} not found")
        return record.sell(quantity)

    def snapshot(self) -> List[Dict[str, object]]:
        """Return a serialisable snapshot of the inventory."""

        return [
            {
                "item": record.item.name,
                "category": record.item.category,
                "unit": record.item.unit,
                "price": record.item.price,
                "on_shelf": record.on_shelf,
                "in_depot": record.in_depot,
                "total": record.total(),
            }
            for record in self.records.values()
        ]
