using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;

/*  
  Web Service 5
*/

public class WS5_Implementation : IWebService5
{

    // declare a variable for the proxy
    private WebService4.WebService4Client proxy;
    String[,] ArrayAssertions_WebService = null;
    int intWSAssertCount = 0;

    // indexes for array fields of ArrayAssertions_WebService
    const int iRule_ID = 0;
    const int iRule_Item_ID = 1;
    const int iResource_ID = 2;
    const int iResource_Name = 3;
    const int iWeight = 4;
    const int iMandatory_Flag = 5;
    const int iDomain_ID = 6;
    const int iDomain_Name = 7;
    const int iScope_ID = 8;
    const int iScope = 9;
    const int iClientOrSvcName = 10;
    const int iPriv_Match_Threshold = 11;
    const int iTopic_ID = 12;
    const int iTopic = 13;
    const int iLevel_ID = 14;
    const int iLevel = 15;

    public string[][] getPrivacyPolicyArray(int WS_ID, int Target_WS_ID, string strTargetMethodName, ref Guid gid, ref string strIncentive)
    {
        // populated the 2D array
        populateServiceAssertionsArray(WS_ID, Target_WS_ID, strTargetMethodName);

        if (gid == Guid.Empty)
        {
            Guid g;
            gid = Guid.NewGuid();
        }

        //*********************************************            

        try
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

            SqlCommand cmd = new SqlCommand("spAdd_GUID_IncentivesOffered");
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param1 = new SqlParameter("@gid", SqlDbType.UniqueIdentifier);
            SqlParameter param2 = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar);
            param2.Direction = ParameterDirection.Output;
            param2.Size = 500;

            cmd.Connection = con;
            cmd.Parameters.Add(param1).Value = gid;
            cmd.Parameters.Add(param2);

            con.Open();
            var retInfo = cmd.ExecuteNonQuery();
            strIncentive = cmd.Parameters["@ReturnMessage"].Value.ToString();
            con.Close();
            cmd.Parameters.Remove(param1);
            cmd.Parameters.Remove(param2);
        }
        catch (Exception ex)
        {
            var dataFile = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "WS5, getPrivacyPolicyArray: " + ex.Message.ToString());
        }

        //*************************************************************************************

        // put in jagged array so can be passed back to client 

        string[][] JaggedArrayAssertions_WebService;
        JaggedArrayAssertions_WebService = new string[50][];

        for (int i = 0; i < 50; i++)
        {
            JaggedArrayAssertions_WebService[i] = new string[16];
            JaggedArrayAssertions_WebService[i][iRule_ID] = ArrayAssertions_WebService[i, iRule_ID];
            JaggedArrayAssertions_WebService[i][iRule_Item_ID] = ArrayAssertions_WebService[i, iRule_Item_ID];
            JaggedArrayAssertions_WebService[i][iResource_ID] = ArrayAssertions_WebService[i, iResource_ID];
            JaggedArrayAssertions_WebService[i][iResource_Name] = ArrayAssertions_WebService[i, iResource_Name];
            JaggedArrayAssertions_WebService[i][iWeight] = ArrayAssertions_WebService[i, iWeight];
            JaggedArrayAssertions_WebService[i][iMandatory_Flag] = ArrayAssertions_WebService[i, iMandatory_Flag];
            JaggedArrayAssertions_WebService[i][iDomain_ID] = ArrayAssertions_WebService[i, iDomain_ID];
            JaggedArrayAssertions_WebService[i][iDomain_Name] = ArrayAssertions_WebService[i, iDomain_Name];
            JaggedArrayAssertions_WebService[i][iScope_ID] = ArrayAssertions_WebService[i, iScope_ID];
            JaggedArrayAssertions_WebService[i][iScope] = ArrayAssertions_WebService[i, iScope];
            JaggedArrayAssertions_WebService[i][iClientOrSvcName] = ArrayAssertions_WebService[i, iClientOrSvcName];
            JaggedArrayAssertions_WebService[i][iPriv_Match_Threshold] = ArrayAssertions_WebService[i, iPriv_Match_Threshold];

            JaggedArrayAssertions_WebService[i][iTopic_ID] = ArrayAssertions_WebService[i, iTopic_ID];
            JaggedArrayAssertions_WebService[i][iTopic] = ArrayAssertions_WebService[i, iTopic];
            JaggedArrayAssertions_WebService[i][iLevel_ID] = ArrayAssertions_WebService[i, iLevel_ID];
            JaggedArrayAssertions_WebService[i][iLevel] = ArrayAssertions_WebService[i, iLevel];
        }

        //*******************************

        return JaggedArrayAssertions_WebService;
    }

    protected void populateServiceAssertionsArray(int WS_ID, int Target_WS_ID, string strTargetMethodName)
    {
        // Assumption: the Privacy Policy information for this web service is already on the client database.
        // This PP information was previously obtained from the web service via a route agreed upon by 
        // client and web service. 
        // (The service would have provided a method that would have called a service-side stored procedure like this 
        //  and returned this information to the client; then the client would have stored this
        //  PP info on the client local database - available to be accessed by the client stored procedure). 

        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();

        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        try
        {
            conn.Open();
            //***************************************
            // get the info for the web service

            SqlCommand cmd = new SqlCommand("spGetAllAssertionsForWS");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ws_id", SqlDbType.Int)).Value = WS_ID;
            cmd.Parameters.Add(new SqlParameter("@target_ws_id", SqlDbType.Int)).Value = Target_WS_ID;
            cmd.Parameters.Add(new SqlParameter("@target_method_name", SqlDbType.NVarChar)).Value = strTargetMethodName;
            cmd.Connection = conn;
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            ArrayAssertions_WebService = new string[50, 16];
            intWSAssertCount = 0;

            while (reader.Read())
            {
                ArrayAssertions_WebService[intWSAssertCount, iRule_ID] = reader["Rule_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iRule_Item_ID] = reader["Rule_Item_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iResource_ID] = reader["Resource_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iResource_Name] = reader["Resource_Name"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iWeight] = reader["Weight"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iMandatory_Flag] = reader["Mandatory_Flag"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iDomain_ID] = reader["Domain_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iDomain_Name] = reader["Domain_Name"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iScope_ID] = reader["Scope_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iScope] = reader["Scope"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iClientOrSvcName] = reader["ClientOrSvcName"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iPriv_Match_Threshold] = reader["Priv_Match_Threshold"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iTopic_ID] = reader["Topic_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iTopic] = reader["Topic"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iLevel_ID] = reader["Level_ID"].ToString();
                ArrayAssertions_WebService[intWSAssertCount, iLevel] = reader["Level"].ToString();

                ++intWSAssertCount;
            }
            conn.Close();
            //****************************************
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.ToString());
        }
        finally
        {
            conn.Close();
        }

    }

    public string getLab5Results(int patient_id, ref string test_results)
    {
        // create the proxy object so that we can call it later
        proxy = new WebService4.WebService4Client("BasicHttpBinding_IWebService4");

        string strReturnMessage = proxy.getLab4Results(patient_id, ref test_results);
        strReturnMessage = strReturnMessage + test_results;

        test_results = "WS5 Test Results are Excellent\n";
        return "WS5/getLab5Results\n" + strReturnMessage;

    }

    

}