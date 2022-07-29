using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using System.Reflection;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public AuthorizationBehaviour(
            ICurrentUserService currentUserService, 
            IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                // Must be authenticated user
                ThrowExceptionIfUserNotAuthenticated(_currentUserService.UserId);

                // Role-based authorization
                await HandleRoleAuthorization(authorizeAttributes, _currentUserService.UserId!);

                // Policy-based authorization
                await HandlePolicyAuthorization(authorizeAttributes, _currentUserService.UserId!);
            }

            // User is authorized / authorization not required
            return await next();
        }

        private void ThrowExceptionIfUserNotAuthenticated(string? userId)
        {
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
        }

        private async Task HandleRoleAuthorization(IEnumerable<AuthorizeAttribute> authorizeAttributes, string userId)
        {
            var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

            if (authorizeAttributesWithRoles.Any())
            {
                var authorized = false;

                foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                {
                    foreach (var role in roles)
                    {
                        var isInRole = await _identityService.IsInRoleAsync(userId, role.Trim());
                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }

                // Must be a member of at least one role in roles
                if (!authorized)
                {
                    throw new ForbiddenAccessException();
                }
            }
        }

        private async Task HandlePolicyAuthorization(IEnumerable<AuthorizeAttribute> authorizeAttributes, string userId)
        {
            var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

            if (authorizeAttributesWithPolicies.Any())
            {
                foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                {
                    var authorized = await _identityService.AuthorizeAsync(userId, policy);

                    if (!authorized)
                    {
                        throw new ForbiddenAccessException();
                    }
                }
            }
        }
    }
}
