
public class Updateable<T>
{
	public T Value { get; private set; }
	public event System.Action<T> UpdatedTo;
	
	public Updateable(T value)
	{
		Value = value;
	}
	
	public Updateable() { }
	
	public void UpdateValue(T value)
	{
		Value = value;
		if (UpdatedTo != null) UpdatedTo(value);
	}
}
