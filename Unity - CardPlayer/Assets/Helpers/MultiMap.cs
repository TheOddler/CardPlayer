using System;
using System.Collections.Generic;

public class MultiMap<TKey,TValue>: Dictionary<TKey, HashSet<TValue>>
{
	public MultiMap(): base() {}
	
	public void Add(TKey key, TValue value)
	{
		if (key == null) throw new ArgumentNullException();
		
		HashSet<TValue> valuesSet = null;
		if(!this.TryGetValue(key, out valuesSet))
		{
			valuesSet = new HashSet<TValue>();
			base.Add(key, valuesSet);
		}
		valuesSet.Add(value);
	}
	
	public void AddValueToKeys(IEnumerable<TKey> keys, TValue value)
	{
		foreach(var key in keys)
		{
			Add(key, value);
		}
	}
	
	public bool ContainsValue(TKey key, TValue value)
	{
		if (key == null) return false;
		
		HashSet<TValue> values = null;
		if(this.TryGetValue(key, out values))
		{
			return values.Contains(value);
		}
		return false;
	}
	
	public void Remove(TKey key, TValue value)
	{
		if (key == null) throw new ArgumentNullException();
		
		HashSet<TValue> valuesSet = null;
		if(this.TryGetValue(key, out valuesSet))
		{
			valuesSet.Remove(value);
			if(valuesSet.Count <= 0)
			{
				this.Remove(key);
			}
		}
	}
	
	public void RemoveValueFromKeys(IEnumerable<TKey> keys, TValue value)
	{
		foreach(var key in keys)
		{
			Remove(key, value);
		}
	}
	
	public HashSet<TValue> GetValues(TKey key)
	{
		HashSet<TValue> toReturn = null;
		if(!base.TryGetValue(key, out toReturn))
		{
			toReturn = new HashSet<TValue>();
		}
		return toReturn;
	}
}