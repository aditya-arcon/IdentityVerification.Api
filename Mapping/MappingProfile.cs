using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;

namespace IdentityVerification.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Template, TemplateDto>().ReverseMap();
            CreateMap<CreateTemplateDto, Template>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.LastUpdated, o => o.Ignore());
            CreateMap<UpdateTemplateDto, Template>()
                .ForMember(d => d.LastUpdated, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore());

            CreateMap<FormField, FormFieldDto>().ReverseMap();
            CreateMap<CreateFormFieldDto, FormField>();
            CreateMap<UpdateFormFieldDto, FormField>();

            CreateMap<FieldCategory, FieldCategoryDto>().ReverseMap();
            CreateMap<CreateFieldCategoryDto, FieldCategory>();
            CreateMap<UpdateFieldCategoryDto, FieldCategory>();

            CreateMap<TemplateFormField, TemplateFormFieldDto>().ReverseMap();
            CreateMap<CreateTemplateFormFieldDto, TemplateFormField>();
            CreateMap<UpdateTemplateFormFieldDto, TemplateFormField>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>()
                .ForMember(d => d.CreatedAt, o => o.Ignore());
            CreateMap<UpdateUserDto, User>()
                .ForMember(d => d.Email, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore());

            CreateMap<ResponseSubmission, ResponseSubmissionDto>().ReverseMap();
            CreateMap<CreateResponseSubmissionDto, ResponseSubmission>()
                .ForMember(d => d.SubmittedAt, o => o.Ignore());
            CreateMap<UpdateResponseSubmissionDto, ResponseSubmission>()
                .ForMember(d => d.SubmittedAt, o => o.Ignore())
                .ForMember(d => d.UserID, o => o.Ignore())
                .ForMember(d => d.TemplateID, o => o.Ignore());

            CreateMap<UserResponse, UserResponseDto>().ReverseMap();
            CreateMap<CreateUserResponseDto, UserResponse>();
            CreateMap<UpdateUserResponseDto, UserResponse>();
        }
    }
}
