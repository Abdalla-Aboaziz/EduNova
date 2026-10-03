using EduNova.Application.Features.Lecture.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Lecture.Validations
{
    public class UploadLectureCommandValidator : AbstractValidator<UploadLectureCommand>
    {
        private static readonly string[] VideoExtensions = [".mp4", ".mov", ".webm"];
        private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private const long MaxVideoSize = 500L * 1024 * 1024;
        private const long MaxImageSize = 5L * 1024 * 1024;

        public UploadLectureCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.SubjectId).NotEmpty();

            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Description).MaximumLength(2000);

            RuleFor(x => x.Order).GreaterThan(0);

            RuleFor(x => x.DurationInSeconds).GreaterThan(0);

            RuleFor(x => x.Video)
                .NotNull()
                .Must(f => f.Length > 0).WithMessage("The video file is empty.")
                .Must(f => f.Length <= MaxVideoSize).WithMessage("The video must not exceed 500 MB.")
                .Must(f => VideoExtensions.Contains(Path.GetExtension(f.FileName).ToLowerInvariant()))
                    .WithMessage("Allowed video types: mp4, mov, webm.")
                .Must(f => f.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                    .WithMessage("The file is not a video.");

            When(x => x.Thumbnail is not null, () =>
            {
                RuleFor(x => x.Thumbnail!)
                    .Must(f => f.Length > 0).WithMessage("The thumbnail file is empty.")
                    .Must(f => f.Length <= MaxImageSize).WithMessage("The thumbnail must not exceed 5 MB.")
                    .Must(f => ImageExtensions.Contains(Path.GetExtension(f.FileName).ToLowerInvariant()))
                        .WithMessage("Allowed image types: jpg, jpeg, png, webp.")
                    .Must(f => f.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                        .WithMessage("The file is not an image.");
            });
        }
    }
}
