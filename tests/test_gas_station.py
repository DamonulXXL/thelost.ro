from peco_simulator.gas_station import GasStation
from peco_simulator.restock import RestockCalculator


def test_restock_moves_items_to_shelves():
    station = GasStation()
    station.register_default_stock()
    calculator = RestockCalculator.from_snapshot(station.inventory.records.values(), default_minimum=12)

    before = station.inventory.find("Pâine de casă")
    assert before is not None
    assert before.on_shelf == 6

    recommendation = station.restock_shelves(calculator)

    after = station.inventory.find("Pâine de casă")
    assert after is not None
    assert after.on_shelf >= 12
    assert "Pâine de casă" in recommendation.amounts


def test_sell_fuel_calculates_tax():
    station = GasStation()
    station.register_default_stock()

    totals = station.sell_fuel(0, 10.0)
    assert totals["subtotal"] == 72.0
    assert round(totals["tax"], 2) == 13.68
    assert round(totals["total"], 2) == 85.68
