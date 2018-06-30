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
using System.Configuration;

/*
   Web Service Privacy, Compatibility and k-Anonymity
   - Integration of k-Anonymity of a WS method into the compatibility checking process of an existing privacy framework     
   Michael Edwards 
   CIS 695 - Graduate Project  
  
   Client Assumptions: 
   1) Client page developer sets a web reference to every WS that will be called in the future. 
      Now the methods are available to the code on the client side. 
   2) Developer adds methods names to table WSMethods and their corresponding parameter names to 
      table WSMethodParameters. In addition, developer adds name of WS to table ClientAndServices.       
   3) When the client page is loaded, user is asked to select WS and method. Then he or she must 
      then specify parameter values. 
   4) The user then clicks Submit. 
   
  WS Assumptions
   1) Administrators on WS side have processed WSCL and added KAnon information to Privacy Policies. 
      (and this WSCL reflects the setup on the WS side as it currently stands). 
   2) Web Services make general methods available. They also make available a method for client to 
      obtain Privacy Policy information for the WS.  
*/

public partial class _Default : System.Web.UI.Page
{
    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();

    String[,] WebServices_MethodsAndParams;
    int intWebSvcMethParamCt;
    // indexes of array fields in WebServices_MethodsAndParams
    const int iWSName = 0;
    const int iMethodName = 1;
    const int iParamName = 2;
    const int iParamType = 3;
    const int iRefParam = 4;


    protected void Page_Load(object sender, EventArgs e)
    {
        // Assumption: on client side, we know all the potential web services (and their methods) 
        // we may want to go against. We parsed the required information from web service WSDL files and 
        // have previously done the following:
        // 1) populated table ClientAndServices with the names of all the web services
        //    and table WSMethods with names of all methods of each web service
        // 2) added a web reference to the client for each one of these web services

        // Here we go against the ClientAndServices and WSMethods tables to get the list just mentioned.                   
        
        if (Session["CompatibilityMessage"] != null)
        {
            txtCompatMessage.Text = Session["CompatibilityMessage"].ToString();
        }

        txtCompatMessage.Font.Bold = true;
        txtResults.Font.Bold = true;

        if (!Page.IsPostBack)
        {
            txtUniqueIdentifierForResubmit.Text = "";
            getSetOfWebServicesAndTheirMethods();
            getClientPrivacyMatchThreshold();

            //****************************************************
            // delete existing on table Client_WSProxyNames
            DeleteExisting_Client_WSProxyNames();

            // get existing web references and put them on table Client_WSProxyNames
            // Note: the following allows us to get all names of WS within the App_WebReferences folder
            var folder = System.Web.HttpContext.Current.Server.MapPath("~/App_WebReferences/");
            string[] foldersPath = Directory.GetDirectories(folder);
            string strFolderName = "";
            foreach (string folderName in foldersPath)
            {
                strFolderName = Path.GetFileName(folderName);
                AddToTable_Client_WSProxyNames(strFolderName);
            }
            //****************************************************
        }
      
    }

    public void DeleteExisting_Client_WSProxyNames()
    { 
        try
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

            SqlCommand cmd = new SqlCommand("spDeleteAll_Client_WSProxyNames");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            con.Open();
            var retInfo = cmd.ExecuteNonQuery();
            con.Close();

        }
        catch (Exception ex)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Default page, AddToTable_Client_WSProxyNames: " + ex.Message.ToString());
        }

    }

    protected void AddToTable_Client_WSProxyNames(string strClient_WSProxyName)
    {
        try
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

            SqlCommand cmd = new SqlCommand("spAdd_Client_WSProxyNames");
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = new SqlParameter("@WSProxyName", SqlDbType.NVarChar);
            SqlParameter param2 = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar);
            param2.Direction = ParameterDirection.Output;
            param2.Size = 500;

            cmd.Connection = con;
            cmd.Parameters.Add(param1).Value = strClient_WSProxyName;
            cmd.Parameters.Add(param2);

            con.Open();
            var retInfo = cmd.ExecuteNonQuery();
            string strReturnMessage = cmd.Parameters["@ReturnMessage"].Value.ToString();
            con.Close();
            cmd.Parameters.Remove(param1);
            cmd.Parameters.Remove(param2);
        }
        catch (Exception ex)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Default page, AddToTable_Client_WSProxyNames: " + ex.Message.ToString());
        }
    }  

    protected void getSetOfWebServicesAndTheirMethods()
    {
        WebServices_MethodsAndParams = new string[100, 5];

        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        try
        {
            conn.Open();

            // Note: this stored procedure will only return info for WS that the client
            // has previously set a reference to.
            // This is what helps to ensure that the Visual Studio web references are in sync with,
            // in agreement with, what is on the database

            SqlCommand cmd = new SqlCommand("spGetAllWebSvcAndTheirMethAndParam");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            SqlDataReader reader;
            reader = cmd.ExecuteReader();

            lstWebServices.Items.Clear();
            intWebSvcMethParamCt = 0;

            while (reader.Read())
            {
                // WSName, MethodName, ParamName, ParamType, RefParam
                WebServices_MethodsAndParams[intWebSvcMethParamCt, iWSName] = reader["WSName"].ToString();
                WebServices_MethodsAndParams[intWebSvcMethParamCt, iMethodName] = reader["MethodName"].ToString();
                WebServices_MethodsAndParams[intWebSvcMethParamCt, iParamName] = reader["ParamName"].ToString();
                WebServices_MethodsAndParams[intWebSvcMethParamCt, iParamType] = reader["ParamType"].ToString();
                WebServices_MethodsAndParams[intWebSvcMethParamCt, iRefParam] = reader["RefParam"].ToString();

                ListItem item = lstWebServices.Items.FindByText(reader["WSName"].ToString());
                if (item == null)
                {
                    // not on list yet, so add (we do not want to put duplicates on this list)
                    lstWebServices.Items.Add(reader["WSName"].ToString());
                }

                intWebSvcMethParamCt++;
            }

            Session["WebServicesMethAndParams"] = WebServices_MethodsAndParams;
            Session["WebSvcMethParamCt"] = intWebSvcMethParamCt;
            string strWebService = "";

            if (lstWebServices.Items.Count == 1)
            {
                // we only have one web service in list, so we can get and display all methods for that web service
                strWebService = lstWebServices.Items[0].Text;
                lstWebServices.Items[0].Selected = true;

                // now get and display methods for this web service
                for (int x = 0; x < intWebSvcMethParamCt; x++)
                {
                    if (WebServices_MethodsAndParams[x, iWSName] == strWebService)
                    {
                        ListItem item = lstMethods.Items.FindByText(WebServices_MethodsAndParams[x, iMethodName]);
                        if (item == null)
                        {
                            // not on list yet, so add (we do not want to put duplicates on this list)
                            lstMethods.Items.Add(WebServices_MethodsAndParams[x, iMethodName]);
                        }
                    }

                }
            }

            //****************************************

            if (lstMethods.Items.Count == 1)
            {
                // we only have one method in list, so we can get and display all parameters for that web service
                string strMethod = "";

                strMethod = lstMethods.Items[0].Text;
                lstMethods.Items[0].Selected = true;

                // now get and display parameters for this method
                for (int x = 0; x < intWebSvcMethParamCt; x++)
                {
                    if (
                            (WebServices_MethodsAndParams[x, iWSName] == strWebService) &&
                            (WebServices_MethodsAndParams[x, iMethodName] == strMethod)
                        )
                    {
                        ListItem item = lstParam.Items.FindByText(WebServices_MethodsAndParams[x, iParamName]);
                        if (item == null)
                        {
                            string strItemTextToAdd = "";
                            strItemTextToAdd = WebServices_MethodsAndParams[x, iParamName] + " (" + WebServices_MethodsAndParams[x, iParamType].Trim() + ")";
                            // not on list yet, so add (we do not want to put duplicates on this list)
                            if (WebServices_MethodsAndParams[x, iRefParam] == "True")
                            {
                                strItemTextToAdd = strItemTextToAdd + " Output Param";
                            }

                            lstParam.Items.Add(strItemTextToAdd);
                        }
                    }
                }
            }

            //**************************************

        }
        catch (Exception exc)
        {
            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Default Page, getSetOfWebServicesAndTheirMethods: " + exc.Message.ToString());
        }
        finally
        {
            conn.Close();
        }
    }

    protected void lstWebServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstMethods.Items.Clear();
        lstParam.Items.Clear();
        txtResults.Text = "";
        txtUniqueIdentifierForResubmit.Text = "";
        lblMessageToUser.Text = "";
        lblCompatCheckResults.Text = "";
        
        // to capture asp listbox click, need to set the listbox's AutoPostBack property to true
        // and hook up the SelectedIndexChange event to a method (which we do here)

        for (int i = 0; i < lstWebServices.Items.Count; i++)
        {
            if (lstWebServices.Items[i].Selected == true)
            {                
                Session["SelectedWebService"] = lstWebServices.SelectedItem.Text;
            }
        }

        intWebSvcMethParamCt = (int)Session["WebSvcMethParamCt"];
        WebServices_MethodsAndParams = (String[,])Session["WebServicesMethAndParams"];
        
        // now get and display methods for this web service
        for (int x = 0; x < intWebSvcMethParamCt; x++)
        {
            if (WebServices_MethodsAndParams[x, iWSName] == Session["SelectedWebService"].ToString())
            {
                ListItem item = lstMethods.Items.FindByText(WebServices_MethodsAndParams[x, iMethodName]);
                if (item == null)
                {
                    // not on list yet, so add (we do not want to put duplicates on this list)
                    lstMethods.Items.Add(WebServices_MethodsAndParams[x, iMethodName]);
                }                
            }
        }

        //***********************************************************

        if (lstMethods.Items.Count == 1)
        {
            Session["SelectedMethod"] = lstMethods.Items[0].ToString();

            // now get and display parameters for this method
            for (int x = 0; x < intWebSvcMethParamCt; x++)
            {
                if (
                      (WebServices_MethodsAndParams[x, iWSName] == Session["SelectedWebService"].ToString()) &&
                      (WebServices_MethodsAndParams[x, iMethodName] == Session["SelectedMethod"].ToString())
                  )
                {
                    ListItem item = lstParam.Items.FindByText(WebServices_MethodsAndParams[x, iParamName]);
                    if (
                         (item == null) 
                        )
                    {
                        string strItemTextToAdd = "";
                        strItemTextToAdd = WebServices_MethodsAndParams[x, iParamName] + " (" + WebServices_MethodsAndParams[x, iParamType].Trim() + ")";
                        // not on list yet, so add (we do not want to put duplicates on this list)
                        if (WebServices_MethodsAndParams[x, iRefParam] == "True")
                        {
                            strItemTextToAdd = strItemTextToAdd + " Output Param";
                        }
                        lstParam.Items.Add(strItemTextToAdd);
                    }
                }
            }
        }   

        //***********************************************************

    }

    protected void lstMethods_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstParam.Items.Clear();
        txtResults.Text = "";
        txtUniqueIdentifierForResubmit.Text = "";

        // to capture asp listbox click, need to set the listbox's AutoPostBack property to true
        // and hook up the SelectedIndexChange event to a method (which we do here)
       
        for (int i = 0; i < lstMethods.Items.Count; i++)
        {
            if (lstWebServices.Items[i].Selected == true)
            {
                Session["SelectedWebService"] = lstWebServices.SelectedItem.Text;
            }
            if (lstMethods.Items[i].Selected == true)
            {
                Session["SelectedMethod"] = lstMethods.SelectedItem.Text;
            }
        }

        intWebSvcMethParamCt = (int)Session["WebSvcMethParamCt"];
        WebServices_MethodsAndParams = (String[,])Session["WebServicesMethAndParams"];

        // now get and display methods for this web service
        for (int x = 0; x < intWebSvcMethParamCt; x++)
        {
            if (
                 (WebServices_MethodsAndParams[x, iWSName] == Session["SelectedWebService"].ToString()) &&
                 (WebServices_MethodsAndParams[x, iMethodName] == Session["SelectedMethod"].ToString())
               )
            {
                string strItemTextToAdd = WebServices_MethodsAndParams[x, iParamName] + " (" + WebServices_MethodsAndParams[x, iParamType].Trim() + ")";
                if (WebServices_MethodsAndParams[x, iRefParam] == "True")
               {
                   strItemTextToAdd = strItemTextToAdd + " Output Param";
               }

               ListItem item = lstParam.Items.FindByText(strItemTextToAdd);

               if (item == null)                       
                {
                    // not on list yet, so add (we do not want to put duplicates on this list)
                    lstParam.Items.Add(strItemTextToAdd);
                }
            }
        }
    }
            
    protected void btnSubmit_Click(object sender, EventArgs e)
    {            
        string strWebService = "";
        string strMethod = "";
        lblMessageToUser.Text = "";
        txtCompatMessage.Text = "";
        txtResults.Text = "";
        lblCompatCheckResults.Text = "";

        //**************************************************
        // edit on user input to GUI

        if (Session["SelectedWebService"] != null)
        {
            strWebService = Session["SelectedWebService"].ToString();
        }
        if (strWebService == "") 
        {
            lblMessageToUser.Text = "Error: Please select web service.";
            return;
        }

        //**************************************************

        if (Session["SelectedMethod"] != null)
        {
            strMethod = Session["SelectedMethod"].ToString();
        }
        if (strMethod == "")
        {
            lblMessageToUser.Text = "Error: Please select method to call.";
            return;
        }

        //************************************************
        
        string strParamsSpecified = txtParamSpecified.Text.Trim();
        if  (
              (strParamsSpecified == "") &&
              (lstParam.Items.Count > 0)
            )
        {
            lblMessageToUser.Text = "Error: Please specify required parameters.";
            return;
        }

        string[] arrIndividualParam = strParamsSpecified.Split(',');

        int arrCount = arrIndividualParam.Count();

        if  (arrCount  != lstParam.Items.Count)
        {
            lblMessageToUser.Text = "Required number of parameters were not specified.";
            return;
        }

        //************************************************

        float flClientThreshold = 0;
        bool blnIsFloat = float.TryParse(txtClientThreshold.Text.ToString(), out flClientThreshold);

        if (blnIsFloat)
        {
            if ((flClientThreshold >= 0) && (flClientThreshold <= 1))
            {
                updateClientPrivacyMatchThreshold(flClientThreshold);
            }
            else
            {
                lblMessageToUser.Text = "Client Threshold must be >=0 and <= 1.";
                return;
            }
        }
        else
        {
            lblMessageToUser.Text = "Client Threshold must be numeric.";
            return;
        }

        //************************************************        
        
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

        bool blnCompatible = false;
        string strDynamicCall_TypeName = "";
        string strDynamicCall_ConstructorArgument = "";
        Compatibility comp = new Compatibility();
        Type typeWebServiceClient = null; 
       
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
                lblMessageToUser.Text = "Error: web service selected is not available";
                return;
        }        
        
        //***************************************************************

        string test_results = "";
        string strReturnMessage = "";
        string strCompatibilityMessage = "";

        string strUniqueIdentifierForResubmit = txtUniqueIdentifierForResubmit.Text.Trim();
        Guid gid = new Guid();
        if (strUniqueIdentifierForResubmit != "")        
        {
            try
            {
                gid = new Guid(strUniqueIdentifierForResubmit);
            }
            catch (Exception ex)
            {
                lblCompatCheckResults.Text = "Unique identifier entered is not a GUID in the form required (for example, 0f8fad5b-d9cb-469f-a165-70867728950e)";
                return;
            }
        }        
        string strIncentive = "";

        // check to make sure the client is compatible with the web service method  
        blnCompatible = comp.checkClientCompatWithServiceMethod(intWebService, strMethod, ref strCompatibilityMessage, ref gid, ref strIncentive);

        //************************************************************
        if (blnCompatible)
        {
            // Compatible
            // No need to give Submission Identifier because user is not going to resubmit
            txtCompatMessage.Text = "Web Service " + intWebService.ToString() + ", Method " + strMethod + "\n\n" + 
                                     strCompatibilityMessage;
            Session["CompatibilityMessage"] = txtCompatMessage.Text;
        }
        else 
        {            
            // incompatible
            
            if (strIncentive == "No Incentive")
            {
                // no incentive offered by WS
                txtCompatMessage.Text = "Web Service " + intWebService.ToString() + ", Method " + strMethod + "\n\n" + 
                                        "Submission Identifier: " + gid.ToString() + "\n" +
                                        "There are no additional WS offers.\n" +
                                        "Please change incompatible privacy requirements and resubmit with identifier.\n\n" +
                                        strCompatibilityMessage;
                Session["CompatibilityMessage"] = txtCompatMessage.Text;
            }
            else
            {
                // incentive offered by WS, display to user
                txtCompatMessage.Text = "Web Service " + intWebService.ToString() + ", Method " + strMethod + "\n\n" + 
                                        "Submission Identifier: " + gid.ToString() + "\n" +
                                        "Incentive offered by WS: " + strIncentive + "\n" +
                                        "* To accept offer, change incompatible privacy requirements and resubmit with identifier.\n" +
                                        "* To seek another offer, keep privacy requirements as they are and resubmit with identifier.\n\n" +                                        
                                        strCompatibilityMessage;
                Session["CompatibilityMessage"] = txtCompatMessage.Text;
            }

        }

        //************************************************************

        // if compatible, then proceed with calling the specified method
        if (blnCompatible)
        {
            lblCompatCheckResults.Text = "Client Privacy Requirements and WS Method Privacy Policies <u>are compatible</u>. Now calling method."; 

            string strParamDescription = "";
            int intIndexOfLeftParen = 0;
            int intIndexOfRightParen = 0;
            string strSubString = "";

            // We go through the names of parameters that pertain to method that is going to be called
            int intNewSize = 0;
            object[] arrayOfParam = new object[0]; 
            for (int x = 0; x < lstParam.Items.Count; x++)
            {
                intNewSize++;
                Array.Resize(ref arrayOfParam, intNewSize);
                strParamDescription = lstParam.Items[x].ToString();

                if (strParamDescription.Contains("Output Param"))
                {
                    // this is an output param, so we just send an empty string to the method
                    // the user should not have specified any value for this parameter
                    arrayOfParam[intNewSize - 1] = "";
                }
                else
                {
                    // this is not an output parameter; we use the user-specified value
                    intIndexOfLeftParen = strParamDescription.IndexOf("(");
                    intIndexOfRightParen = strParamDescription.IndexOf(")");
                    strSubString = strParamDescription.Substring(intIndexOfLeftParen + 1, intIndexOfRightParen - intIndexOfLeftParen - 1);

                    // this would need to be expanded later to handle the full range of types
                    // (at present, handle int and string)
                    Type myClassType = null;
                    if (strSubString == "int")
                    {
                        int intNum;
                        bool IsInt = int.TryParse(arrIndividualParam[x].ToString(), out intNum);

                        if (IsInt)
                        {
                            strSubString = "System.Int32";
                            myClassType = Type.GetType(strSubString);
                            arrayOfParam[intNewSize - 1] = doCast(arrIndividualParam[x], myClassType);
                        }
                        else
                        {
                            lblMessageToUser.Text = "Error: the value entered (" + arrIndividualParam[x].ToString()
                                                                        + ") is not an int";
                            return;
                        }
                    }
                    else
                    {   
                        // put the user-specified param value in the array that will store all specified values
                        arrayOfParam[intNewSize - 1] = arrIndividualParam[x]; 
                    }
                }
            }

            // dynamically call the web service and method specified by the user in the GUI
            // this allows us to have a dynamic number of parameters
            strReturnMessage = (string)Invoker.CreateAndInvoke
                (
                typeWebServiceClient,
                strDynamicCall_TypeName,                                // ex) "WebService1.WebService1Client"
                new object[] { strDynamicCall_ConstructorArgument },    // ex) "BasicHttpBinding_IWebService1" 
                strMethod,                                              // ex) "getLabResults"
                arrayOfParam                                            // note: this will contain the return ref param value
                );

            // ORIGINAL CODE (example):
            //proxy1 = new WebService1.WebService1Client("BasicHttpBinding_IWebService1");
            //strReturnMessage = proxy1.getLabResults(patient_id, ref test_results);

            // Note: in order to get the full set of parameters after the fact, whether by-value or by-ref param,
            // we need to look at arrayOfParam
            test_results = arrayOfParam[1].ToString();  // get the return ref param value

            txtResults.Text = strReturnMessage + test_results;
        }
        else
        {
            lblCompatCheckResults.Text = "Client Privacy Requirements and WS Method Privacy Policies <u>are not compatible</u>. Method will not be called."; 
        }
    }

    public static class Invoker
    {
        public static object CreateAndInvoke(Type typeWebServiceClient, string typeName, object[] constructorArgs, string methodName, object[] methodArgs)
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

    public void getClientPrivacyMatchThreshold()
    {
        // we go against the stored procedure and populate an array
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("spGet_ClientPrivacyMatchThreshold");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            SqlDataReader reader;
            reader = cmd.ExecuteReader();            

            // we should get one and only one row back
            reader.Read();
            txtClientThreshold.Text = reader["Priv_Match_Threshold"].ToString();                           
        }
        catch (Exception exc)
        {
            var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "Default.aspx, getClientPrivacyMatchThreshold: " + exc.Message.ToString());
        }
        finally
        {
            conn.Close();
        }
    }

    public void updateClientPrivacyMatchThreshold(float flNewThreshold)
    {
        // we go against the stored procedure and populate an array
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;
        
        try
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

            SqlCommand cmd = new SqlCommand("spUpdate_ClientPrivacyMatchThreshold");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader;
            SqlParameter param1 = new SqlParameter("@new_threshold", SqlDbType.Float);
            cmd.Connection = con;
          
            cmd.Parameters.Add(param1).Value = flNewThreshold;
            con.Open();
            reader = cmd.ExecuteReader();
            con.Close();
            cmd.Parameters.Remove(param1);
           
        }
        catch (Exception ex)
        {
            string strResults = ex.Message.ToString();

            var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "KAnonymity Class, populate_WSTransitionsTable: " + ex.Message.ToString());
        }
    }   

       
    
}
