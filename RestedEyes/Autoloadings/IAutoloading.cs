namespace RestedEyes.Autoloadings
{
    public interface IAutoloading
    {
        bool IsAutoloading(string programmPath);

        void AutoloadingProgramm(string programmPath);
    }
}
