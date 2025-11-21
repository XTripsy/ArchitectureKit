using System;

public interface IStateRegistry
{
    void IRegister(string id, IState factory);
    IState ICreate(string id);
}