﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KPCMTest.WebService2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WebService2.IWebService2")]
    public interface IWebService2 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebService2/getLab2Results", ReplyAction="http://tempuri.org/IWebService2/getLab2ResultsResponse")]
        string getLab2Results(int patient_id, ref string test_results);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebService2/getPrivacyPolicyArray", ReplyAction="http://tempuri.org/IWebService2/getPrivacyPolicyArrayResponse")]
        string[][] getPrivacyPolicyArray(int WS_ID, int Target_WS_ID, string strTargetMethodName, ref System.Guid gid, ref string strIncentive);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebService2Channel : KPCMTest.WebService2.IWebService2, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebService2Client : System.ServiceModel.ClientBase<KPCMTest.WebService2.IWebService2>, KPCMTest.WebService2.IWebService2 {
        
        public WebService2Client() {
        }
        
        public WebService2Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebService2Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebService2Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebService2Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string getLab2Results(int patient_id, ref string test_results) {
            return base.Channel.getLab2Results(patient_id, ref test_results);
        }
        
        public string[][] getPrivacyPolicyArray(int WS_ID, int Target_WS_ID, string strTargetMethodName, ref System.Guid gid, ref string strIncentive) {
            return base.Channel.getPrivacyPolicyArray(WS_ID, Target_WS_ID, strTargetMethodName, ref gid, ref strIncentive);
        }
    }
}