using MediatR;

namespace ECommerceAPI.Application.Features.Commands.AuthorizationEnpoints.AssignRole
{
	public class AssignRoleCommandRequest :IRequest<AssignRoleCommandResponse>
	{
		public string[] Roles { get; set; }
		public string Code { get; set; }
		public string Menu {  get; set; }
		public Type? Type { get; set; }

	}
}