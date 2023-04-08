using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hadi.Cms.ApplicationService.Services
{
    public class AuthorizeService
    {
        private static string[] _acceptedFeature;
        private static Dictionary<Guid, string[]> _acceptedFeatureCatch = new Dictionary<Guid, string[]>();

        private UserRoleService _userRoleService;
        private RoleFeatureService _roleFeatureService;

        public AuthorizeService()
        {
            _userRoleService = new UserRoleService();
            _roleFeatureService = new RoleFeatureService();
        }

        public List<IRoleDto> GetUserRoles(Guid userId)
        {
            var userRoles = _userRoleService.GetList(r => r.UserId == userId).Select(x => x.RoleDto).ToList();
            return userRoles;
        }

        public List<IFeatureDto> GetUserFeatures(Guid userId)
        {
            var RoleIds = _userRoleService.GetList(r => r.UserId == userId).Select(x => x.RoleId).ToList();
            var roleFeatures = _roleFeatureService.GetList(r => RoleIds.Contains(r.RoleId)).Select(x => x.FeatureDto).ToList();
            return roleFeatures;
        }

        public string[] GetUserFeaturesFormatted(Guid userId)
        {
            List<string> result = new List<string>();
            var RoleIds = _userRoleService.GetList(r => r.UserId == userId).Select(x => x.RoleId)
                .ToList();
            var features = _roleFeatureService.GetList(r => RoleIds.Contains(r.RoleId))
                .Select(x => x.FeatureDto).ToList();

            foreach (var feature in features)
            {
                result.Add(feature.AreaName + "-" + feature.ControllerName.Replace("Controller", "") + "-" +
                           feature.ActionName);
            }

            return result.ToArray();
        }

        public static string[] GetUserAcceptedFeature(User user)
        {
            if (!_acceptedFeatureCatch.Keys.Contains(user.Id))
            {
                if (user.UserName.ToLower() != "administrator")
                {
                    _acceptedFeature = new AuthorizeService().GetUserFeaturesFormatted(user.Id);
                    _acceptedFeatureCatch.Add(user.Id, _acceptedFeature);
                }
                else
                {
                    _acceptedFeature = new string[] { "ALL" };
                    _acceptedFeatureCatch.Add(user.Id, _acceptedFeature);
                }
            }

            return _acceptedFeatureCatch[user.Id];
        }
    }
}
