using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;
using System.ServiceModel;
using System.Text;

namespace KAnonymityModule
{
    public class KAnonymity
    {
        // spAdd_WSCL_Transition: add to table WSCL_Transitions
        // spAdd_WSCL_Transition_AllPossibleRoutes: add to table WSCL_Transition_AllPossibleRoutes
        // spAdd_WSCL_Transition_AllPossibleRoutes_IntIDs: add to table WSCL_Transitions_AllPossibleRoutes
        // spAdd_WSCL_Transition_KAnonymity: add to table WSCL_Transitions_KAnonymity
        // spAdd_WSCL_Transition_PrivacyRuleSetItem_WebService_RowsToAdd: tries to add to table WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd

        string[] arrOfMethodNames;
        string[,] arrOfSourceAndDest;
        int arrCount;
        const int C_intMaxNumOfWSID_ToAddToWSPP = 5;

        public KAnonymity()
        { }       

        public string processWSCLToGetKAnon(int intWebServiceID, string strWSCLContents)
        {
            // Assumptions: 
            // (the client (or service) gets WS Privacy Policies, and populates its database with the full set of 
            //  WS, method and param info, before an attempt is made to process WSCL)
            // 1) web service is already on client (or service) table ClientAndServices and therefore we have a WS_ID for it
            // 2) method is already on client (or service) table WSMethods and therefore we have a Method_ID for it

            // we look for each pair of methods on the WSMethods table
            // * if we find them, we get the method ids and insert a row to the table WS_Transitions

            string strResults = "";

            strResults = processWSCLInteractions_PutInArray(strWSCLContents);

            if (strResults == "")
            {
                strResults = processWSCLTransitions_PutInArray(strWSCLContents);
                if (strResults == "")
                {
                    strResults = populate_WSTransitionsTable();

                    if (strResults == "")
                    {
                        strResults = populate_WSTransitionsAllPossibleRoutes_Table();
                    }
                }
            }

            return strResults;
        }

        //********************************************************************************

        public string processWSCLInteractions_PutInArray(string strWSCLContents)
        {
            string strReturnMessage = "";
            string strMethodName = "";

            XElement XmlFromWSCLFile;
            try
            {
                XmlFromWSCLFile = XElement.Parse(strWSCLContents);
            }
            catch (Exception exc)
            {
                strReturnMessage = "Error parsing XML (XElement.Parse): " + exc.Message.ToString();
                return strReturnMessage;
            }

            //**************************************************

            try
            {
                XDocument xdoc = XDocument.Parse(strWSCLContents);
                XNamespace z = "http://www.w3.org/2002/02/wscl10";

                arrOfMethodNames = new string[50];
                arrCount = 0;

                foreach (XElement element in xdoc.Descendants(z + "Interaction"))
                {                    
                    strMethodName = element.Attribute("id").Value;

                    if ((strMethodName != "Start") && (strMethodName != "End"))
                    {
                        arrOfMethodNames[arrCount] = strMethodName.Trim();
                        arrCount++;
                    }
                }

                return strReturnMessage;

            }
            catch (Exception ex)
            {
                strReturnMessage = "Error: " + ex.Message.ToString();

                Console.WriteLine("KAnonymity Class, processWSCLInteractions_PutInArray: " + ex.Message.ToString());

                return strReturnMessage;
            }

        }

        public string processWSCLTransitions_PutInArray(string strWSCLContents)
        {
            string strReturnMessage = "";
            string strSource = "";
            string strDestination = "";

            XElement XmlFromWSCLFile;
            try
            {
                XmlFromWSCLFile = XElement.Parse(strWSCLContents);
            }
            catch (Exception exc)
            {
                strReturnMessage = "Error parsing XML (XElement.Parse): " + exc.Message.ToString();
                return strReturnMessage;
            }

            //**************************************************

            try
            {
                XDocument xdoc = XDocument.Parse(strWSCLContents); 
                XNamespace z = "http://www.w3.org/2002/02/wscl10";

                arrOfSourceAndDest = new string[50, 2];
                arrCount = 0;

                foreach (XElement element in xdoc.Descendants(z + "Transition"))
                {
                    foreach (XElement element2 in element.Descendants(z + "SourceInteraction"))
                    {
                        strSource = element2.Attribute("href").Value;
                    }
                    foreach (XElement element2 in element.Descendants(z + "DestinationInteraction"))
                    {
                        strDestination = element2.Attribute("href").Value;
                    }

                    arrOfSourceAndDest[arrCount, 0] = strSource.Trim();
                    arrOfSourceAndDest[arrCount, 1] = strDestination.Trim();
                    arrCount++;
                }

                return strReturnMessage;

            }
            catch (Exception ex)
            {
                strReturnMessage = "Error: " + ex.Message.ToString();

               Console.WriteLine("KAnonymity Class, processWSCLTransitions_PutInArray: " + ex.Message.ToString());

                return strReturnMessage;
            }

        }

        public string populate_WSTransitionsTable()
        {
            string strResults = "";
            string filePath = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\WSTransitionsTable.csv";

            try
            {               
                var stream = File.CreateText(filePath);

                for (int i = 0; i < arrCount; i++)
                {                   
                  
                   string csvRow = string.Format("{0},{1}", arrOfSourceAndDest[i, 0].ToString(), arrOfSourceAndDest[i, 1].ToString());
                     
                   stream.WriteLine(csvRow);                 
                }

                stream.Close();
              
            }
            catch (Exception ex)
            {
                strResults = ex.Message.ToString();

                  Console.WriteLine( "KAnonymity Class, populate_WSTransitionsTable: " + ex.Message.ToString());
            }

            return strResults;
        }

        public string populate_WSTransitionsAllPossibleRoutes_Table()
        {
            string strResults = "";
            string filePath = @"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\KPCMTest\KPCMTest\WSTransitionsAllPossibleRoutes.csv";
       
                try
            {            
                // first we put the same rows on that table that we just put on the WS_Transitions table
                // as long as they do not exist on table WSCL_Transitions_AllPossibleRoutes yet
                var stream = File.CreateText(filePath);
                string csvRow = "";
                for (int i = 0; i < arrCount; i++)
                {                   
                  
                   csvRow = string.Format("{0},{1}", arrOfSourceAndDest[i, 0].ToString(), arrOfSourceAndDest[i, 1].ToString());
                     
                   stream.WriteLine(csvRow);                 
                }
                
                    //then we add all possible route
                for (int i = 0; i < arrCount; i++)
                {
                    for(int j = 0; j < arrCount; j++){
                    if(arrOfSourceAndDest[j, 0].ToString()== arrOfSourceAndDest[i, 1].ToString())
                    csvRow = string.Format("{0},{1}", arrOfSourceAndDest[i, 0].ToString(), arrOfSourceAndDest[j, 1].ToString());
                    }
                    
                    stream.WriteLine(csvRow);
                }
                
                stream.Close();
              
            }
            
            catch (Exception ex)
            {
                strResults = ex.Message.ToString();

               Console.WriteLine("KAnonymity Class, populate_WSTransitionsAllPossibleRoutes_Table: " + ex.Message.ToString());
            }

            return strResults;
        }

        public void WSTransitionsAllPossibleRoutes_InitialSetOfInserts(string strSourceMethodName, string strDestinationMethodName)
        {
            try
            {
                int intWebServiceID = 0;               

                string filePath ="WSCL_Transition_AllPossibleRoutes";
               
            //read...
            }
            catch (Exception ex)
            {
               Console.WriteLine("KAnonymity Class, WSTransitionsAllPossibleRoutes_InitialSetOfInserts: " + ex.Message.ToString());
            }
        }

        //public void WSTransitionsAllPossibleRoutes_InsertsToCoverAllRoutes(ref string strReturnMessage)
        //{

        //    try
        //    {
        //        int intWebServiceID = 0;
      
        //        while (reader.Read())
        //        {
        //            // this is a potential row we may want to add to the table WSCL_Transitions_AllPossibleRoutes
        //            string strSource_Method_ID = reader.GetValue(0).ToString();      // source id
        //            string strDestination_Method_ID = reader.GetValue(1).ToString(); // destination id

        //            int intSource_Method_ID;
        //            int intDestination_Method_ID;
        //            // make sure to check the success flags at some point
        //            bool success1 = int.TryParse(strSource_Method_ID, out intSource_Method_ID);
        //            bool success2 = int.TryParse(strDestination_Method_ID, out intDestination_Method_ID);

        //            //**************************************************************************

        //            try
        //            {
        //                System.Data.SqlClient.SqlConnection con2 = new System.Data.SqlClient.SqlConnection();
        //                con2.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

        //                SqlCommand cmd2 = new SqlCommand("spAdd_WSCL_Transition_AllPossibleRoutes_IntIDs");
        //                cmd2.CommandType = CommandType.StoredProcedure;
        //                SqlDataReader reader2;
        //                SqlParameter param4 = new SqlParameter("@ws_id", SqlDbType.Int);
        //                SqlParameter param5 = new SqlParameter("@source_method_id", SqlDbType.Int);
        //                SqlParameter param6 = new SqlParameter("@destination_method_id", SqlDbType.Int);
        //                cmd2.Connection = con2;

        //                cmd2.Parameters.Add(param4).Value = intWebServiceID;
        //                cmd2.Parameters.Add(param5).Value = intSource_Method_ID;
        //                cmd2.Parameters.Add(param6).Value = intDestination_Method_ID;
        //                con2.Open();
        //                reader2 = cmd2.ExecuteReader();
        //                reader2.Read();
                        
        //                strReturnMessage = reader2.GetValue(0).ToString();

        //                con2.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
        //                File.AppendAllText(@dataFile, "KAnonymity Class, WSTransitionsAllPossibleRoutes_InsertsToCoverAllRoutes (A): " + ex.Message.ToString());
        //            }

        //            //**************************************************************************

        //        }

        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
        //        File.AppendAllText(@dataFile, "KAnonymity Class, WSTransitionsAllPossibleRoutes_InsertsToCoverAllRoutes (B): " + ex.Message.ToString());

        //    }
        //}

        //********************************************************************************

        public string populate_WSCL_Transitions_KAnonymity(string strRadioButtonItemSelected_KAnonType)
        {
            // In the previous processing, we added rows (from WSCL file info), where the rows
            // were not there already, to tables WSTransitionsTable and WSTransitionsAllPossibleRoutes.

            // Now, we go against the following table to see if we need to add rows to it:
            // WSCL_Transition_KAnonymity (if the rows are not there already). 

            // This is an intermediate table that will be used further on. 
            // Final goal down the line: provide an additional possible KAnon that can be chosen
            // when setting up assertions for the web service side 

            string strReturnMessage = "";

            const string C_strAllMethPassThroughCounted = "AllMethPassThroughCounted";
            const string C_strAllMethPassThroughNotCounted = "AllMethPassThroughNotCounted";
            const string C_strOnlyEndpointMeth = "OnlyEndpointMeth";
            string strStoredProcedure = "";

            switch(strRadioButtonItemSelected_KAnonType)
            {
                case C_strAllMethPassThroughCounted:
                    strStoredProcedure = "spAdd_WSCL_Transition_KAnonymity_AllMethPassThroughCounted"; 
                    break;
                case C_strAllMethPassThroughNotCounted:
                    strStoredProcedure = "spAdd_WSCL_Transition_KAnonymity_AllMethPassThroughNotCounted"; 
                    break;
                case C_strOnlyEndpointMeth:
                    strStoredProcedure = "spAdd_WSCL_Transition_KAnonymity_OnlyEndpointMethods"; 
                    break;
                default:
                    var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                    File.AppendAllText(@dataFile, "KAnonymity Class, populate_WSCL_Transitions_KAnonymity: strRadioButtonItemSelected_KAnonType not specified");
                    return "Error: strRadioButtonItemSelected_KAnonType not specified";
            }
             

            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

                // previously: spAdd_WSCL_Transition_KAnonymity
                SqlCommand cmd = new SqlCommand(strStoredProcedure);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader;
                cmd.Connection = con;
                con.Open();
                reader = cmd.ExecuteReader();                                
               
                con.Close();
            }
            catch (Exception ex)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "KAnonymity Class, populate_WSCL_Transitions_KAnonymity: " + ex.Message.ToString());
            }
            return strReturnMessage;

        }

        //*********************************************************************************

        public string populate_WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd(string strRadioButtonItemSelected_KAnonType)
        {
            // In the previous processing, we added rows (from WSCL file info), where the rows
            // were not there already, to tables WSTransitionsTable and WSTransitionsAllPossibleRoutes.

            // Now, we go against the following table to see if we need to add rows to it:
            // WSCL_Transition_PrivacyRuleSetItem_WebService_RowsToAdd (if the rows are not there already). 

            // This is an intermediate table that will be used further on. 
            // Final goal down the line: provide an additional possible KAnon that can be chosen
            // when setting up assertions for web service side  

            string strReturnMessage = "";

            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

                SqlCommand cmd = new SqlCommand("spAdd_WSCL_Transition_PrivacyRuleSetItem_WebService_RowsToAdd");
                cmd.CommandType = CommandType.StoredProcedure;                
                SqlParameter param1 = new SqlParameter("@method_name", SqlDbType.NVarChar);
                SqlParameter param2 = new SqlParameter("@KAnonType", SqlDbType.NVarChar);
                cmd.Connection = con;
                con.Open();

                //***********************************
                for (int i = 0; i < arrOfMethodNames.Count(); i++)
                {
                    if (arrOfMethodNames[i] != null)
                    {
                        cmd.Parameters.Add(param1).Value = arrOfMethodNames[i].ToString();
                        cmd.Parameters.Add(param2).Value = strRadioButtonItemSelected_KAnonType.ToString();
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Close();
                        reader = null;
                        cmd.Parameters.Remove(param1);
                        cmd.Parameters.Remove(param2);
                    }
                }
                //***********************************

                con.Close();
            }
            catch (Exception ex)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "KAnonymity Class, populate_WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd: " + ex.Message.ToString());
            }

            return strReturnMessage;
        }

        public string DeleteUnusedKanon_PrivacyRuleSetItems_WebService()
        {   
            string strReturnMessage = "";

            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

                SqlCommand cmd = new SqlCommand("spDeleteUnusedKanon_PrivacyRuleSetItems_WebService");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = con;               
                con.Open();               
                var retInfo = cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "KAnonymity Class, DeleteUnusedKanon_PrivacyRuleSetItems_WebService: " + ex.Message.ToString());
            }

            return strReturnMessage;

        }

        public string AddTransitionInfoToTable_PrivacyRuleSetItems_WebService()
        {
            string strReturnMessage = "";

            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

                SqlCommand cmd = new SqlCommand("spAdd_PrivacyRuleSetItems_WebService");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param2 = new SqlParameter("@method_name", SqlDbType.NVarChar);
                SqlParameter param3 = new SqlParameter("@ReturnMessage", SqlDbType.NVarChar);
                param3.Direction = ParameterDirection.Output;
                param3.Size = 500;

                cmd.Connection = con;
                con.Open();
                //***********************************
                
                for (int i = 0; i < C_intMaxNumOfWSID_ToAddToWSPP; i++)
                {
                    if (arrOfMethodNames[i] == null)
                    {
                        break; // no more methods to look at
                    }
                    else
                    {
                        cmd.Parameters.Add(param2).Value = arrOfMethodNames[i].ToString();
                        cmd.Parameters.Add(param3);
                       
                        var retInfo = cmd.ExecuteNonQuery();
                        strReturnMessage = strReturnMessage + cmd.Parameters["@ReturnMessage"].Value.ToString();                        
                        
                        cmd.Parameters.Remove(param2);
                        cmd.Parameters.Remove(param3);
                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "KAnonymity Class, AddTransitionInfoToTable_PrivacyRuleSetItems_WebService: " + ex.Message.ToString());
            }

            return strReturnMessage;
        }

        //********************************************************************************

        public void delete_WSCL_Tables()
        {
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;
                SqlCommand cmd = new SqlCommand("spDelete_WSCL_Transition_And_AllPossibleRoutes");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader;
                cmd.Connection = con;
                con.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                con.Close();
            }
            catch (Exception ex)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "KAnonymity Class, delete_WSCL_Tables: " + ex.Message.ToString());
            }
        }
        
    }
}

