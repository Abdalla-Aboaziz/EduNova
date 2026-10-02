using Mapster;

namespace EduNova.Application.Common.Mappings;

/// <summary>
/// Central Mapster mapping configuration.
/// Register all custom type mappings here.
/// This config is registered in DependencyInjection.cs via TypeAdapterConfig.GlobalSettings.
/// </summary>
public static class MappingConfig
{
    /// <summary>
    /// Call this method to register all Mapster mapping configurations.
    /// Add your custom .Map() configurations inside this method.
    /// </summary>
    public static TypeAdapterConfig GetConfiguredMappingConfig()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        // Scan this assembly for all IRegister implementations
        config.Scan(typeof(MappingConfig).Assembly);

        return config;
    }
}
