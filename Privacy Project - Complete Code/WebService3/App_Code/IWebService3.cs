using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

/*  
     
   Web Service 3 
 
*/

[ServiceContract]
public interface IWebService3
{
    [OperationContract]
    string getLab3Results(int patient_id, ref string test_results);

    [OperationContract]
    string[][] getPrivacyPolicyArray(int WS_ID, int Target_WS_ID, string strTargetMethodName, ref Guid gid, ref string strIncentive);
    
}



