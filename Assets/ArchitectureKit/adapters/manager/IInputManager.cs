public interface IInputManager
{
    void IRegisterActionInput(string state, IAction action);
    void IActiveActionInput(string mapping);
    int IGetIndexCatalogInputAction(string state, InputCatalog group);
}
