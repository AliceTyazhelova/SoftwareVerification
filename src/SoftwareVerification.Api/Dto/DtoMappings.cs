using SoftwareVerification.Api.Dto;
using SoftwareVerification.Domain.Entities;

namespace SoftwareVerification.Api.Dtos;

public static class DtoMappings
{
    public static VerificationObjectDto ToDto(this VerificationObject entity)
    {
        return new VerificationObjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Version = entity.Version,
            Type = entity.Type.ToString(), 
            ReceivedAt = entity.ReceivedAt,
            Result = entity.Result.ToString(),
            ExpertComment = entity.ExpertComment
        };
    }
}