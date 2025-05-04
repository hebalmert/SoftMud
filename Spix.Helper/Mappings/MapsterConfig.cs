using Mapster;
using Spix.Core.EntitiesNet;

namespace Spix.Helper.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Node, Node>()
            .Ignore(dest => dest.Corporation!)
            .Ignore(dest => dest.IpNetwork!)
            .Ignore(dest => dest.Operation!)
            .Ignore(dest => dest.Mark!)
            .Ignore(dest => dest.MarkModel!)
            .Ignore(dest => dest.Zone!)
            .Ignore(dest => dest.FrecuencyType!)
            .Ignore(dest => dest.Frecuency!)
            .Ignore(dest => dest.Channel!)
            .Ignore(dest => dest.Security!);
    }
}