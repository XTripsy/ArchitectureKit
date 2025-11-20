public interface IInstaller<in T>
{
    void Install(T installer);
}