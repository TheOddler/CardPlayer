using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public interface CardInfoGatherer
{
	ICollection<string> PotentialHits { get; }
	IEnumerator LoadInfoFor(CardInfo card);
}
