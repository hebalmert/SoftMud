namespace Spix.Helper.Mappings;

public interface IMapperService
{
    TTarget Map<TSource, TTarget>(TSource source);
}