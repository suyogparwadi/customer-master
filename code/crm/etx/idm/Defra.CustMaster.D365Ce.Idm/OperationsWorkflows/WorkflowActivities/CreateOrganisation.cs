using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Linq;
using System.ServiceModel;

namespace Defra.Customer.Plugins.WorkflowActivities
{


    public class CreateOrganisation : CodeActivity
    {
        #region "Parameter Definition"
        [RequiredArgument]
        [Input("Business Type")]
        public InArgument<String> BusinessType { get; set; }
        [Input("CompanyHouseId")]
        public InArgument<String> CompanyHouseId { get; set; }
        [Input("ValidatedWithCompanyHouse")]
        public InArgument<String> ValidatedWithCompanyHouse { get; set; }
        [Input("Email")]
        public InArgument<String> Email { get; set; }
        [Input("Name")]
        public InArgument<String> Name { get; set; }
        [Output("CRMGuid")]
        public OutArgument<String> CrmGuid { get; set; }
        [Output("UniqueReference")] 
        public OutArgument<String> UniqueReference { get; set; }
        [Output("Code")]
        public OutArgument<String> Code { get; set; }
        [Output("Message")]
        public OutArgument<string> Message { get; set; }
        [Output("MessageDetail")]
        public OutArgument<string> MessageDetail { get; set; }

        #endregion


       
        protected override void Execute(CodeActivityContext executionContext)
        {
            #region "Read Parameters"
            string _BusinessType = this.BusinessType.Get(executionContext);
            string _CompanyHouseId = this.CompanyHouseId.Get(executionContext);
            string _ValidatedWithCompanyHouse = this.ValidatedWithCompanyHouse.Get(executionContext);
            string _Email = this.Email.Get(executionContext);
            string _Name = this.Name.Get(executionContext);
            int? optionSetValue;
            Int64 ErrorCode = 400; //400 -- bad request
            String _ErrorMessage = string.Empty;
            String _ErrorMessageDetail = string.Empty;
            Guid ContactId = Guid.Empty;
            Guid CrmGuid;
            #endregion

            #region "Load CRM Service from context"
            try
            {
                Common objCommon = new Common(executionContext);
                objCommon.tracingService.Trace("Load CRM Service from context --- OK");

                objCommon.tracingService.Trace("Email: {0}, _Name: Name{1} ,_ValidatedWithCompanyHouse: {2},_CompanyHouseId: {3}  ", 
                    _Email, _Name, _ValidatedWithCompanyHouse, _CompanyHouseId);


                if (String.IsNullOrWhiteSpace(_BusinessType))
                {
                objCommon.tracingService.Trace("checking business type");

                    _ErrorMessage = "Business type cannot be empty.";
                   
                }
                else if (!String.IsNullOrEmpty(_Name) && _Name.Length > 160)
                {
                    objCommon.tracingService.Trace("checking org name");

                    _ErrorMessage = "Organiation name is more than 160 characters.";

                }
                else if (!String.IsNullOrEmpty(_CompanyHouseId) && _CompanyHouseId.Length > 8)
                {
                    objCommon.tracingService.Trace("checking house id");

                    _ErrorMessage = "Company House Id cannot be more than 8 characters.";
                }
                else if (!(String.IsNullOrWhiteSpace(_ValidatedWithCompanyHouse)) && (_ValidatedWithCompanyHouse == "y" || _ValidatedWithCompanyHouse == "n"))
                {
                    objCommon.tracingService.Trace("checking validated with company house id");

                    _ErrorMessage = "Validated with company house should have y or n";

                }
                /*
                else if (!String.IsNullOrEmpty(_Email) && _Email.Length > 100)
                {
                    _ErrorMessage = "Email address cannot be more than 100 characters long.";

                }*/
               
                else
                {

                    objCommon.tracingService.Trace("After completing validation");

                    //check if business type option set exists
                    var attributeRequest = new RetrieveAttributeRequest
                    {
                        EntityLogicalName = "account",
                        LogicalName = "defra_businesstype",
                        RetrieveAsIfPublished = true
                    };

                    var attributeResponse = (RetrieveAttributeResponse)objCommon.service.Execute(attributeRequest);
                    var attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

                    var optionList = (from o in attributeMetadata.OptionSet.Options
                                      where o.Label.UserLocalizedLabel.Label == _BusinessType
                                      select new { Value = o.Value, Text = o.Label.UserLocalizedLabel.Label }).ToList();

                    if (optionList.Count == 0)
                    {
                        _ErrorMessage = String.Format("Busiensstype option {0} not found.", _BusinessType);
                    }
                    else
                    {
                        objCommon.tracingService.Trace("before checking for option set");

                        optionSetValue = optionList.FirstOrDefault().Value;

                        ///

                        //OptionSetValueCollection BusinessTypes = new OptionSetValueCollection();

                        //BusinessTypes.Add(new OptionSetValue(optionSetValue.Value)); //
                        

                        //


                        Entity Account = new Entity("account");
                        objCommon.tracingService.Trace("before assigning");
                        objCommon.tracingService.Trace(optionSetValue.ToString());

                        Account["defra_businesstype"] = ToString();

                        //Account["defra_businesstype"] = BusinessTypes;
                        Account["name"] = _Name;
                        Account["defra_companyhouseid"] = _CompanyHouseId;
                        objCommon.tracingService.Trace("after assigning");

                        if (_ValidatedWithCompanyHouse == "y")
                        {
                            Account["defra_validatedwithcompanyhouse"] = 0;
                        }
                        else if (_ValidatedWithCompanyHouse == "n")
                        {
                            Account["defra_validatedwithcompanyhouse"] = 1;
                        }
                        if (_Email != null)
                        { Account["emailaddress1"] = _Email; }
                        objCommon.tracingService.Trace("before createing guid:");
                        try
                        {
                            CrmGuid = objCommon.service.Create(Account);
                            objCommon.tracingService.Trace("after createing guid:{0}", CrmGuid.ToString());

                            this.CrmGuid.Set(executionContext, CrmGuid.ToString());

                            Entity AccountRecord = objCommon.service.Retrieve("account", CrmGuid, new Microsoft.Xrm.Sdk.Query.ColumnSet("defra_uniquereference"));
                            this.Code.Set(executionContext, ErrorCode.ToString());
                            this.UniqueReference.Set(executionContext, AccountRecord["defra_uniquereference"]);
                        }
                        catch (Exception ex)
                        {
                            objCommon.tracingService.Trace("error message:" + ex.Message);
                        }
                        
                       


                        ErrorCode = 200; //success
                    }
                }
                objCommon.tracingService.Trace("outside 1");

                this.Code.Set(executionContext, ErrorCode.ToString());
                this.Message.Set(executionContext, _ErrorMessage);
                this.MessageDetail.Set(executionContext, _ErrorMessageDetail);
                if (_ErrorMessage != string.Empty)
                {
                    this.Message.Set(executionContext, _ErrorMessage);
                }
                
            }
            catch (Exception ex)
            {

                ErrorCode = 500;
                _ErrorMessage = "Error occured while processing request";
                _ErrorMessageDetail = ex.Message;
                //throw ex;
                this.Code.Set(executionContext, ErrorCode.ToString());
                this.Message.Set(executionContext, _ErrorMessage);
                this.MessageDetail.Set(executionContext, _ErrorMessageDetail);
            }
            #endregion
        }
    }
}
