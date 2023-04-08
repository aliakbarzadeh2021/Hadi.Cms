using System;
//using System.Data.Spatial;

namespace Hadi.Cms.Web.App_Start
{
    public class CustomBodyModelValidator : System.Web.Http.Validation.DefaultBodyModelValidator
    {
        public override bool ShouldValidateType(Type type)
        {
            //return type != typeof(DbGeography) && base.ShouldValidateType(type);
            return false;
        }
    }
}