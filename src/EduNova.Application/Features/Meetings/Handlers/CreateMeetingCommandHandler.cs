using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Commands;
using EduNova.Application.Features.Meetings.Responses;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Meetings.Handlers
{
    public sealed class CreateMeetingCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateMeetingCommandHandler> logger)
        : IRequestHandler<CreateMeetingCommand, Result<CreateMeetingResponse>>
    {
        public async Task<Result<CreateMeetingResponse>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            // 1. توليد كود دخول عشوائي مكون من 6 حروف وأرقام
            var joinCode = GenerateJoinCode();

            var meetingId = Guid.NewGuid();

            var meeting = new Meeting
            {
                Id = meetingId,
                Title = request.Title,
                Description = request.Description,
                StartTime = request.StartTime,
                IsVideoMeeting = request.IsVideoMeeting,
                JoinCode = joinCode,
                IsActive = true,
                RoomId = meetingId.ToString("N")
            };

            // 3. إضافة الاجتماع لقاعدة البيانات وحفظ التغييرات
            context.Meetings.Add(meeting);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Meeting created successfully with ID {MeetingId} and JoinCode {JoinCode}", meeting.Id, meeting.JoinCode);

            // 4. تجهيز الرد اللي هيرجع للفرونت إند
            var response = new CreateMeetingResponse
            {
                MeetingId = meeting.Id,
                JoinCode = meeting.JoinCode
            };

            return Result.Success(response);
        }

        // دالة مساعدة لتوليد كود الدخول
        private static string GenerateJoinCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
