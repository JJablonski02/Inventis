namespace Inventis.Domain;

public abstract class Entity : IEntity
{
	public Ulid Id { get; }
	public uint Version { get; protected set; }

	protected Entity()
	{
		Id = Ulid.NewUlid();
	}

	protected Entity(Ulid id)
	{
		Id = id;
	}
}
