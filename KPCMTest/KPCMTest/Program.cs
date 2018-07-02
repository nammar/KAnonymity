

using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CompatibilityModule;
using System.Data;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using KAnonymityModule;


namespace KPCMTest
{

    class Program
    {      
        String[,] WebServices_MethodsAndParams;
        int intWebSvcMethParamCt;

        // indexes of array fields in WebServices_MethodsAndParams
        const int iWSName = 0;
        const int iMethodName = 1;
        const int iParamName = 2;
        const int iParamType = 3;
        const int iRefParam = 4;

        private WebService1.WebService1Client proxy1;
        private WebService2.WebService2Client proxy2;
        private WebService3.WebService3Client proxy3;
        private WebService4.WebService4Client proxy4;
        private WebService5.WebService5Client proxy5;

        static void Main(string[] args)
        {
            
            string strWebService = "WebService1";
            string strMethod = "getLab1Results";
           
            // WS ID (which web service to call)
            const string c_strWebService1 = "WebService1";
            const string c_strWebService2 = "WebService2";
            const string c_strWebService3 = "WebService3";
            const string c_strWebService4 = "WebService4";
            const string c_strWebService5 = "WebService5";
            const int c_intWebService1 = 1;
            const int c_intWebService2 = 2;
            const int c_intWebService3 = 3;
            const int c_intWebService4 = 4;
            const int c_intWebService5 = 5;
            int intWebService = 0;

              const string C_strAllMethPassThroughCounted = "AllMethPassThroughCounted";
            const string C_strAllMethPassThroughNotCounted = "AllMethPassThroughNotCounted";
            const string C_strOnlyEndpointMeth = "OnlyEndpointMeth";

            bool blnCompatible = false;
            string strDynamicCall_TypeName = "";
            string strDynamicCall_ConstructorArgument = "";

            Compatibility comp = new Compatibility();
            Type typeWebServiceClient = null; 


            /*****program starts here******/
           Program p = new Program();

           string strPathAndFile = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\WSCLInputFile - 5 Methods.txt";
            string strReturnMessage = "";
        string strWSCLContents = "";
             KAnonymity KAnon = new KAnonymity();

            //1. Read the file
            using (StreamReader streamReader = new StreamReader(strPathAndFile))
                {
                    strWSCLContents = streamReader.ReadToEnd();
                   Console.Write(strWSCLContents.Trim());
                }

            //2. process WSCLs
         //   p.getSetOfWebServicesAndTheirMethods();
           
        // User has previously selected one of these:
        // AllMethPassThroughCounted, AllMethPassThroughNotCounted, or OnlyEndpointMeth
        string strRadioButtonItemSelected_KAnonType = C_strAllMethPassThroughCounted;

       

        if (strWSCLContents != "")
        {
            //*******************************************************
            // Note WS is set to zero since we only infer the WSID from method name in WSCL
            // Populate tables: WSTransitionsTable, WSTransitionsAllPossibleRoutes_Table
            strReturnMessage = KAnon.processWSCLToGetKAnon(0, strWSCLContents);
            //*******************************************************

            if (strReturnMessage == "")
            {
                try
                {
                    //*******************************************************
                    // Populate table: WSCL_Transitions_KAnonymity
                    // Here is where we make use of the KAnon type radio button selection that
                    // the user made.
                    strReturnMessage = KAnon.populate_WSCL_Transitions_KAnonymity(strRadioButtonItemSelected_KAnonType);
                    //*******************************************************

                    strReturnMessage = ""; //initialize (since returning a 1 always; chg later)

                    // find out which WS Methods we want to add info to database for 

                    // Populate table WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd
                    strReturnMessage = strReturnMessage +
                                    KAnon.populate_WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd(strRadioButtonItemSelected_KAnonType);

                    //*******************************************************
                    // Possibly insert to: PrivacyRuleSetItems_WebService
                    if (strReturnMessage.Trim() == "")
                    {
                        // no error message so far, so we have successfully added to the required intermediate tables. 
                        strReturnMessage = "All rows have been added to the required intermediate tables (see below)" +
                                           "Unused K-Anon related PP items have been deleted.";

                        strReturnMessage = strReturnMessage + KAnon.DeleteUnusedKanon_PrivacyRuleSetItems_WebService();

                        // Populate table WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd
                        strReturnMessage = strReturnMessage +
                                           KAnon.AddTransitionInfoToTable_PrivacyRuleSetItems_WebService();
                    }
                }
                catch (Exception ex)
                {
                    strReturnMessage = ex.Message.ToString();
                }
            }
        }
        else
        {
            strReturnMessage = "Please choose file";
        }

        Console.WriteLine(strReturnMessage.Trim());


        /************WSCL processing ends************/


            //p.getServiceProxies();
                     
            Guid guid = new Guid();

            // check to make sure the client is compatible with the web service method 
            if (strWebService == "WebService1")
                intWebService = 1;
           
            switch (strWebService)
            {
                case c_strWebService1:
                    intWebService = c_intWebService1;
                    strDynamicCall_TypeName = "WebService1.WebService1Client";
                    strDynamicCall_ConstructorArgument = "BasicHttpBinding_IWebService1";
                    typeWebServiceClient = typeof(WebService1.WebService1Client);
                    break;
                case c_strWebService2:
                    intWebService = c_intWebService2;
                    strDynamicCall_TypeName = "WebService2.WebService2Client";
                    strDynamicCall_ConstructorArgument = "BasicHttpBinding_IWebService2";
                    typeWebServiceClient = typeof(WebService2.WebService2Client);
                    break;
                case c_strWebService3:
                    intWebService = c_intWebService3;
                    strDynamicCall_TypeName = "WebService3.WebService3Client";
                    strDynamicCall_ConstructorArgument = "BasicHttpBinding_IWebService3";
                    typeWebServiceClient = typeof(WebService3.WebService3Client);
                    break;
                case c_strWebService4:
                    intWebService = c_intWebService4;
                    strDynamicCall_TypeName = "WebService4.WebService4Client";
                    strDynamicCall_ConstructorArgument = "BasicHttpBinding_IWebService4";
                    typeWebServiceClient = typeof(WebService4.WebService4Client);
                    break;
                case c_strWebService5:
                    intWebService = c_intWebService5;
                    strDynamicCall_TypeName = "WebService5.WebService5Client";
                    strDynamicCall_ConstructorArgument = "BasicHttpBinding_IWebService5";
                    typeWebServiceClient = typeof(WebService5.WebService5Client);
                    break;
                default:
                    Console.WriteLine( "Error: web service selected is not available");
                    return;
            }

            //***************************************************************

            string test_results = "";
           // string strReturnMessage = "";
            string strCompatibilityMessage = "";
           // check to make sure the client is compatible with the web service method 
            var watch = Stopwatch.StartNew();
            blnCompatible = comp.checkClientCompatWithServiceMethod(intWebService, strMethod, ref strCompatibilityMessage, guid, " ");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken: {0}ms", watch.Elapsed.TotalMilliseconds);

            //************************************************************
            if (blnCompatible)
            {
                // Compatible
                System.IO.File.WriteAllText(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\Results.txt", "Web Service " + intWebService.ToString() + ", Method " + strMethod + "\n\n" +
                                         strCompatibilityMessage);                
            }
            else
            {
                Console.WriteLine(strCompatibilityMessage);
                // incompatible   
                    System.IO.File.WriteAllText(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\Results.txt", "Web Service " + intWebService.ToString() + ", Method " + strMethod + "\n\n" +
                    strCompatibilityMessage);
            }


           WebService1.WebService1Client proxy1;
         WebService2.WebService2Client proxy2;
         WebService3.WebService3Client proxy3;
        WebService4.WebService4Client proxy4;
         WebService5.WebService5Client proxy5;

            //************************************************************
            // if compatible, then proceed with calling the specified method
            if (blnCompatible)
            {
                // We go through the names of parameters that pertain to method that is going to be called
                
                string[] arrayOfParam = new string[2];
                arrayOfParam[0] = "patient_id";
                arrayOfParam[1] = "test_results";

                // dynamically call the web service and method specified by the user in the GUI
                // this allows us to have a dynamic number of parameters
                //strReturnMessage = (string)Invoker.CreateAndInvoke
                //    (
                //    typeWebServiceClient,
                //    strDynamicCall_TypeName,                                // ex) "WebService1.WebService1Client"
                //    new string[] { strDynamicCall_ConstructorArgument },    // ex) "BasicHttpBinding_IWebService1" 
                //    strMethod,                                              // ex) "getLabResults"
                //    arrayOfParam                                            // note: this will contain the return ref param value
                //    );

                // ORIGINAL CODE (example):
                proxy1 = new WebService1.WebService1Client("BasicHttpBinding_IWebService1");
                
                strReturnMessage = proxy1.getLab1Results(0, ref test_results);

                // Note: in order to get the full set of parameters after the fact, whether by-value or by-ref param,
                // we need to look at arrayOfParam
                test_results = arrayOfParam[1].ToString();  // get the return ref param value

                Console.WriteLine(strReturnMessage + test_results);
            }
            else
            {
                //lblCompatCheckResults.Text = "Client Privacy Requirements and WS Method Privacy Policies <u>are not compatible</u>. Method will not be called.";
            }


        }

        public static class Invoker
        {
            public static object CreateAndInvoke(Type typeWebServiceClient, string typeName, object[] constructorArgs, string methodName, string[] methodArgs)
            {
                object instance = Activator.CreateInstance(typeWebServiceClient, constructorArgs);
                MethodInfo method = typeWebServiceClient.GetMethod(methodName);
                return method.Invoke(instance, methodArgs);
            }
        }

        public static dynamic doCast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

        private List<string> getServiceProxies()
        {
            List<string> list = new List<string>();

            // get existing web references and put them on table Client_WSProxyNames
            // Note: the following allows us to get all names of WS within the App_WebReferences folder
            var folder = System.Web.HttpContext.Current.Server.MapPath(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\Service References\");
            string[] foldersPath = Directory.GetDirectories(folder);
            string strFolderName = "";
            foreach (string folderName in foldersPath)
            {
                strFolderName = Path.GetFileName(folderName);
                list.Add(strFolderName);
            }
            return list;
        }

        protected void getSetOfWebServicesAndTheirMethods()
        {
            List<String> lstWebServices = new List<String>();
           

            WebServices_MethodsAndParams = new string[100, 5];

         
            try
            {
             
                // Note: this stored procedure will only return info for WS that the client
                // has previously set a reference to.
                // This is what helps to ensure that the Visual Studio web references are in sync with,
                // in agreement with, what is on the database

                string filePath = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\ServicesAndMethods.csv";
                StreamReader reader = new StreamReader(filePath);
               // string[,] strArray = null;
               // int Row = 0;

                lstWebServices.Clear();
                intWebSvcMethParamCt = 0;

                while (!reader.EndOfStream)
                {
                    string[] Line = reader.ReadLine().Split(',');

                    // WSName, MethodName, ParamName, ParamType, RefParam
                    WebServices_MethodsAndParams[intWebSvcMethParamCt, iWSName] = Line[0].ToString();
                    WebServices_MethodsAndParams[intWebSvcMethParamCt, iMethodName] = Line[1].ToString();
                    WebServices_MethodsAndParams[intWebSvcMethParamCt, iParamName] = Line[2].ToString();
                    WebServices_MethodsAndParams[intWebSvcMethParamCt, iParamType] = Line[3].ToString();
                    WebServices_MethodsAndParams[intWebSvcMethParamCt, iRefParam] = Line[4].ToString();

                   
                }


            }
            catch (Exception exc)
            {
                //var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
                //File.AppendAllText(@dataFile, 
                  Console.Write("Default Page, getSetOfWebServicesAndTheirMethods: " + exc.Message.ToString());
            }
            finally
            {
                
            }
        }
    
 



} 

}
