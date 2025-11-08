"""Definitions related to the world and atmospheric details of the simulator."""

from dataclasses import dataclass
from typing import List


@dataclass
class Landmark:
    """Represents a point of interest near the gas station."""

    name: str
    description: str
    distance_km: float


@dataclass
class WorldContext:
    """Metadata describing the setting for the P.E.C.O. gas station."""

    region: str
    road_description: str
    nearby_city_indicator: str
    landmarks: List[Landmark]

    @classmethod
    def default(cls) -> "WorldContext":
        """Return the canonical description of the bleak Transylvanian setting."""

        return cls(
            region="Transilvania, România",
            road_description=(
                "Drum național pustiu, mărginit de păduri de molid și dealuri cu ceață densă. "
                "Noaptea liniștea este spartă doar de țârâitul insectelor și vântul ce lovește pompele P.E.C.O."
            ),
            nearby_city_indicator="Indicator rutier: Cluj-Napoca 50 KM",
            landmarks=[
                Landmark(
                    name="Câmpul de Turbină",
                    description="Turbine eoliene abandonate care scârțâie la fiecare rafală de vânt.",
                    distance_km=12.0,
                ),
                Landmark(
                    name="Hanul Părăsit",
                    description=(
                        "Ruinele unui han din piatră unde localnicii spun că auzim cântări când se lasă ceața."
                    ),
                    distance_km=4.5,
                ),
            ],
        )

    def describe(self) -> str:
        """Return a human readable description of the setting."""

        lines = [
            f"Regiune: {self.region}",
            self.road_description,
            self.nearby_city_indicator,
            "Puncte de interes în apropiere:",
        ]
        for landmark in self.landmarks:
            lines.append(f" - {landmark.name} ({landmark.distance_km} km): {landmark.description}")
        return "\n".join(lines)
