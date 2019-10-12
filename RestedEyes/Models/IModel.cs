namespace RestedEyes.Models
{
    public interface IModel
    {
        void Attach(IModelObserver imo);

        void Break(bool isBreak);
        string Start();

        void SaveConfig(string filePath = null);
        void OpenConfig(string filePath);

        bool IsAutoloading { get; }
        void AddOrRemoveAutoloading();
        string[] AutoloadTypes();
        void ChangeAutoloadTypes(string typeName);
    }
}
