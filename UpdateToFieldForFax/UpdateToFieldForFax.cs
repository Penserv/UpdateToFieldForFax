using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace UpdateToFieldForFax
{
    public class UpdateToFieldForFax : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider) 
        {
            ITracingService service1 = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext service2 = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (!service2.InputParameters.Contains("Target") || !(service2.InputParameters["Target"] is Entity))
                return;
            Entity inputParameter = (Entity)service2.InputParameters["Target"];
            if (inputParameter.LogicalName != "email")
                return;
            try
            {
                ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(service2.UserId));
                if (inputParameter.Contains("pps_faxnumber") && inputParameter.Attributes["pps_faxnumber"] != null && inputParameter.Attributes["pps_faxnumber"].ToString() != string.Empty && inputParameter.Attributes["pps_faxnumber"].ToString() != "")
                {
                    service1.Trace("3");
                    Entity entity = new Entity("activityparty");
                    entity["participationtypemask"] = (object)new OptionSetValue(0);
                    service1.Trace("4");
                    entity["addressused"] = (object)(inputParameter.Attributes["pps_faxnumber"] + "@fax.penserv.com");
                    entity["fullname"] = (object)(inputParameter.Attributes["pps_faxnumber"] + "@fax.penserv.com");
                    service1.Trace("5");
                    inputParameter.Attributes["to"] = (object)new Entity[1]
                    {
            entity
                    };
                    service1.Trace("6");
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the Distribution Log Text plug-in.", (Exception)ex);
            }
            catch (Exception ex)
            {
                service1.Trace("FollowupPlugin: {0}", (object)ex.ToString());
                throw;
            }
        }
    }
}
