using UnityEngine;

namespace Peco.Horror.World
{
    /// <summary>
    /// Static container for narrative elements that describe the Transylvanian setting.
    /// Attach this to a GameObject in the initial scene and populate the serialized fields
    /// to expose location details inside the UI or other systems.
    /// </summary>
    [CreateAssetMenu(menuName = "Peco/World Descriptor", fileName = "WorldDescriptor")]
    public class WorldDescriptor : ScriptableObject
    {
        [Header("Locație")]
        [SerializeField] private string locationName = "P.E.C.O. – Transilvania";
        [SerializeField, TextArea] private string description =
            "Benzinăria P.E.C.O. se află pe un drum pustiu din Ardeal, cu indicatorul \"Cluj-Napoca 50 KM\".\n" +
            "Noaptea, farurile singurelor mașini care trec iluminează siluetele munților și ceața grea.";

        [Header("Atmosferă")]
        [SerializeField, TextArea] private string ambientNotes =
            "Mișcări de umbre lungi, huruitul pompei de combustibil și un radio vechi ce redă știri întrerupte.";

        [Header("Organizare")]
        [SerializeField, TextArea] private string organizationNotes =
            "Rafturile sunt împărțite pe categorii: cafea și patiserie caldă aproape de intrare, conserve și apă pe culoarul central, " +
            "iar dulciurile și produsele locale lângă casa de marcat. Depozitul din spate păstrează stocul de rezervă.";

        public string LocationName => locationName;
        public string Description => description;
        public string AmbientNotes => ambientNotes;
        public string OrganizationNotes => organizationNotes;
    }
}
