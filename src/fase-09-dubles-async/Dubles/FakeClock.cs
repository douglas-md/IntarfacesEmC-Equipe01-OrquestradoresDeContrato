
public sealed class FakeClock : IClock

{

    public DateTimeOffset Now { get; set; } = DateTimeOffset.UtcNow;



    public void Avancar(TimeSpan tempo) => Now = Now.Add(tempo);

}