using Mapster;


namespace EduNova.Application.Features.Note.MappingConfig
{
    public class NoteMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Note, Responses.NoteResponse>();
        }
    }
}
