namespace StaticVoid.OrmPerfomance.Runner
{
    public interface IShell 
    {
        string Name { get; set; }
        string HelloString { get; }
        bool CanSayHello{ get; }

        void SayHello(string name);
    }
}
