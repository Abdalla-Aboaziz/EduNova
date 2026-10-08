using EduNova.Application.Features.Note.Command;
using EduNova.Domain.Entities;
using FluentValidation;

namespace EduNova.Application.Features.Note.Validations
{
    public class UpdateNoteValidation : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid note id.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(4000)
                .WithMessage("Content cannot exceed 4000 characters.");

            RuleFor(x => x.Color)
                .Must(color => Enum.IsDefined(typeof(NoteColor), color))
                .WithMessage("Invalid note color.");

        }
    }
}
