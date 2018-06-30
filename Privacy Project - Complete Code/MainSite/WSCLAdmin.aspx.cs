using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Reflection;
using KAnonymityModule;

/*
    Web Service Privacy, Compatibility and k-Anonymity
   - Integration of k-Anonymity of a WS method into the compatibility checking process of an existing privacy framework     
   Michael Edwards 
   CIS 695 - Graduate Project
  
*/

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && FileUpload.PostedFile != null)
        {
            if (FileUpload.PostedFile.FileName.Length > 0)
            {
                string strWSCLContents = "";
                lblFileSelected.Text = "File Selected: '" + this.FileUpload.FileName.ToString() + "'";

                string strPathAndFile = Server.MapPath(this.FileUpload.FileName);

                using (StreamReader streamReader = new StreamReader(strPathAndFile))
                {
                    strWSCLContents = streamReader.ReadToEnd();
                    txtWSCLDisplay.Text = strWSCLContents.Trim();
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        KAnonymity KAnon = new KAnonymity();

        //***********************************

        // delete existing table contents so we can start from scratch
        KAnon.delete_WSCL_Tables();

        GridView1.DataBind();
        GridView2.DataBind();
        GridView3.DataBind();

        //***********************************

        string strReturnMessage = "";
        string strWSCLContents = "";

        // User has previously selected one of these:
        // AllMethPassThroughCounted, AllMethPassThroughNotCounted, or OnlyEndpointMeth
        string strRadioButtonItemSelected_KAnonType = rblKAnonType.SelectedValue.ToString();

        try
        {
            strWSCLContents = txtWSCLDisplay.Text;
        }
        catch (Exception ex)
        {
            // make sure user didn't select a file that can not be put into a string
            strReturnMessage = ex.Message.ToString();
        }

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
                        strReturnMessage = "All rows have been added to the required intermediate tables (see below).<BR>" + 
                                           "Unused K-Anon related PP items have been deleted.<BR>" ;

                        strReturnMessage = strReturnMessage + KAnon.DeleteUnusedKanon_PrivacyRuleSetItems_WebService();

                        // Populate table WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd
                        strReturnMessage = strReturnMessage +
                                           KAnon.AddTransitionInfoToTable_PrivacyRuleSetItems_WebService();                            
                        
                        //*******************************************************
                       
                        GridView1.DataBind();
                        GridView2.DataBind();
                        GridView3.DataBind();
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

        lblMessageToUser.Text = strReturnMessage.Trim();

    }

    protected void btnClearTables_Click(object sender, EventArgs e)
    {
        KAnonymity KAnon = new KAnonymity();
        KAnon.delete_WSCL_Tables();

        GridView1.DataBind();
        GridView2.DataBind();
        GridView3.DataBind();
    }

    protected string[,] getSetOfWebServicesAndTheirMethods()
    {
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        String[,] WebServices_And_Methods = new string[20, 2]; ;

        int intCount = 0;

        try
        {
            conn.Open();

            //***************************************

            SqlCommand cmd = new SqlCommand("spGetAllWebSvcAndTheirMeth");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            SqlDataReader reader;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {               
                WebServices_And_Methods[intCount, 0] = reader["WS_ID"].ToString();
                WebServices_And_Methods[intCount, 1] = reader["MethodName"].ToString();

                intCount++;
            }            
        }
        catch (Exception ex)
        {
            var dataFile = Server.MapPath("~/App_Data/ErrorLog.txt");
            File.AppendAllText(@dataFile, "WSCL Admin, getSetOfWebServicesAndTheirMethods: " + ex.Message.ToString());
        }

        return WebServices_And_Methods;
    }
    
}
