"""Core gameplay logic for the P.E.C.O. gas station simulator."""

from __future__ import annotations

from dataclasses import dataclass, field
from typing import Dict, Iterable, List, Tuple

from .inventory import Inventory, InventoryItem, StockRecord
from .restock import RestockCalculator, RestockRecommendation
from .world import WorldContext


@dataclass
class FuelPump:
    """Represents a fuel pump and its taxation model."""

    fuel_type: str
    price_per_liter: float
    tax_rate: float = 0.19
    capacity_liters: float = 5000.0
    available_liters: float = 3000.0

    def sell(self, liters: float) -> Tuple[float, float]:
        if liters <= 0:
            raise ValueError("Liters must be positive")
        if liters > self.available_liters:
            raise ValueError("Not enough fuel available")
        self.available_liters -= liters
        subtotal = liters * self.price_per_liter
        tax = subtotal * self.tax_rate
        return subtotal, tax

    def restock(self, liters: float) -> None:
        if liters <= 0:
            raise ValueError("Liters must be positive")
        if self.available_liters + liters > self.capacity_liters:
            raise ValueError("Exceeds pump capacity")
        self.available_liters += liters


@dataclass
class Depot:
    """Mini warehouse used to store restock inventory."""

    capacity: int
    used: int = 0

    def can_store(self, quantity: int) -> bool:
        return self.used + quantity <= self.capacity

    def add(self, quantity: int) -> None:
        if not self.can_store(quantity):
            raise ValueError("Depot capacity exceeded")
        self.used += quantity

    def remove(self, quantity: int) -> None:
        if quantity > self.used:
            raise ValueError("Not enough items in depot")
        self.used -= quantity


@dataclass
class GasStation:
    """Manages the minimarket, depot and pumps for the P.E.C.O. station."""

    name: str = "P.E.C.O."
    world: WorldContext = field(default_factory=WorldContext.default)
    inventory: Inventory = field(default_factory=Inventory)
    pumps: List[FuelPump] = field(default_factory=list)
    depot: Depot = field(default_factory=lambda: Depot(capacity=500))
    organization_plan: Dict[str, List[str]] = field(default_factory=dict)

    def register_default_stock(self) -> None:
        """Populate the gas station with default inventory and pump data."""

        items = [
            StockRecord(InventoryItem("Cofe Sel", "Băuturi calde", 8.5, "pahar"), on_shelf=10, in_depot=20),
            StockRecord(InventoryItem("Pâine de casă", "Produse de panificație", 5.0), on_shelf=6, in_depot=12),
            StockRecord(InventoryItem("Sarmale la borcan", "Mâncare tradițională", 24.0, unit="borcan"), on_shelf=4, in_depot=8),
            StockRecord(InventoryItem("Baton energizant", "Gustări rapide", 7.5), on_shelf=12, in_depot=15),
            StockRecord(InventoryItem("Apă plată Izvorul Nocturn", "Băuturi reci", 4.0, unit="sticlă"), on_shelf=18, in_depot=30),
        ]
        for record in items:
            self.inventory.add_item(record)

        self.pumps = [
            FuelPump("Benzină 95", price_per_liter=7.2, available_liters=2500.0),
            FuelPump("Motorină", price_per_liter=7.8, available_liters=2600.0),
        ]

        self.organization_plan = {
            "Băuturi calde": ["Espressor La Marzocco", "Stand termos"],
            "Băuturi reci": ["Vitrină frigorifică est", "Frigider mic lângă case"],
            "Mâncare tradițională": ["Raft central din lemn", "Promoție lângă geam"],
            "Gustări rapide": ["Gondolă din fața caselor", "Casă #2"],
            "Produse de panificație": ["Coșuri de răchită", "Raft cald lângă cuptor"],
        }

        self.depot.used = sum(record.in_depot for record in self.inventory.records.values())

    def sell_item(self, item_name: str, quantity: int) -> float:
        return self.inventory.sell(item_name, quantity)

    def sell_fuel(self, pump_index: int, liters: float) -> Dict[str, float]:
        pump = self.pumps[pump_index]
        subtotal, tax = pump.sell(liters)
        return {"subtotal": subtotal, "tax": tax, "total": subtotal + tax}

    def restock_shelves(self, calculator: RestockCalculator) -> RestockRecommendation:
        recommendation = calculator.recommend(self.inventory)
        for item_name, quantity in recommendation.amounts.items():
            record = self.inventory.find(item_name)
            if record:
                record.move_to_shelf(quantity)
                self.depot.remove(quantity)
        return recommendation

    def receive_delivery(self, deliveries: Iterable[Tuple[str, int]]) -> None:
        """Add new items to the depot and increase stock records accordingly."""

        for item_name, quantity in deliveries:
            record = self.inventory.find(item_name)
            if not record:
                raise ValueError(f"Cannot receive delivery for unknown item {item_name}")
            if not self.depot.can_store(quantity):
                raise ValueError("Depot capacity would be exceeded by delivery")
            record.in_depot += quantity
            self.depot.add(quantity)

    def organize(self, category: str, plan: List[str]) -> None:
        self.organization_plan[category] = plan

    def describe_organization(self) -> str:
        lines = ["Plan de organizare P.E.C.O.:"]
        for category, hotspots in sorted(self.organization_plan.items()):
            lines.append(f" - {category}:")
            for hotspot in hotspots:
                lines.append(f"    • {hotspot}")
        return "\n".join(lines)

    def describe(self) -> str:
        info = [f"Benzinăria {self.name}", self.world.describe(), "", self.describe_organization()]
        return "\n".join(info)
