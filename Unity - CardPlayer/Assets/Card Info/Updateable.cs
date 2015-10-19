
public class Updateable<T>
{
	public T Value { get; private set; }
	public bool Ready { get; private set; }
	public event System.Action<T> UpdatedTo;
	
	public Updateable(T initialValue, bool consideredReady)
	{
		Value = initialValue;
		Ready = consideredReady;
	}
	
	public Updateable()
	{
		Ready = false;
	}
	
	public void UpdateValue(T value)
	{
		Value = value;
		Ready = true;
		if (UpdatedTo != null) UpdatedTo(value);
	}
}
