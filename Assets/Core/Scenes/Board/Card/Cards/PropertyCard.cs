using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity card Object used to instanciate 'entityCards' Dictionary<int, EntityCard>
/// See DecksController.
/// </summary>
namespace StarterCore.Core.Scenes.Board.Card.Cards
{
    public class PropertyCard
    {
        public int index;
        public List<string> icons = new List<string>();
        public string id;
        public string about;
        public string label;
        public string comment;
        public List<string> parentsClassList = new List<string>();//List of parent classes (Direct, not inferred).
        public List<string> ChildrenClassList = new List<string>();//List of sub classes.
        public List<Color32> colors = new List<Color32>();
    }
}