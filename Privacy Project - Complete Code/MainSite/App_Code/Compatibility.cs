using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace CompatibilityModule
{
    public class Compatibility
    {
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();

        String[,] ArrayAssertions_Client;
        String[,] ArrayAssertions_WebService;
        String[,] ArrayDomainSubsumpRelationships;
        const int c_intWebClient_ID = 6;
        int intClientAssertCount = 0;
        int intWSAssertCount = 0;
        int intDomainSubsumpRelationshipsCount = 0;        
        private WebService1.WebService1Client proxy1;
        private WebService2.WebService2Client proxy2;
        private WebService3.WebService3Client proxy3;
        private WebService4.WebService4Client proxy4;
        private WebService5.WebService5Client proxy5;

        // indexes for array fields of ArrayAssertions_Client and ArrayAssertions_WebService
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

        // indexes for array fields of ArrayDomainSubsumpRelationships
        const int iSubsumed_Domain_ID = 0;
        const int iSubsuming_Domain_ID = 1;


        public bool checkClientCompatWithServiceMethod(int WS_ID, string strTargetMethodName, ref string strOverallCompatibilityMessage, ref Guid gid, ref string strIncentive)
        {
            // Check the compatibility between the client and a particular web service.
            // Note: a check is done on the method name as well as the parameters that are specified (among the 4 optional);
            // for the parameters, a check is also made for param name + "_PR" and param name "_EXP" to find resources that
            // are defined for "practice" and "expectation"

            populateClientAssertionsArray(c_intWebClient_ID, WS_ID, strTargetMethodName);
            
            //***********************************************************

            // Call web service to get privacy policy for this method
            // Assumption: service will return a jagged array (which WCF passes via XML)

            string[][] JaggedArrayAssertions_WebService;
            JaggedArrayAssertions_WebService = new string[50][];

            switch (WS_ID)
            {
                case 1:
                    proxy1 = new WebService1.WebService1Client();
                    JaggedArrayAssertions_WebService = proxy1.getPrivacyPolicyArray(WS_ID, WS_ID, strTargetMethodName, ref gid, ref strIncentive);
                    break;
                case 2:
                    proxy2 = new WebService2.WebService2Client();
                    JaggedArrayAssertions_WebService = proxy2.getPrivacyPolicyArray(WS_ID, WS_ID, strTargetMethodName, ref gid, ref strIncentive);
                    break;
                case 3:
                    proxy3 = new WebService3.WebService3Client();
                    JaggedArrayAssertions_WebService = proxy3.getPrivacyPolicyArray(WS_ID, WS_ID, strTargetMethodName, ref gid, ref strIncentive);
                    break;
                case 4:
                    proxy4 = new WebService4.WebService4Client();
                    JaggedArrayAssertions_WebService = proxy4.getPrivacyPolicyArray(WS_ID, WS_ID, strTargetMethodName, ref gid, ref strIncentive);
                    break;
                case 5:
                    proxy5 = new WebService5.WebService5Client();
                    JaggedArrayAssertions_WebService = proxy5.getPrivacyPolicyArray(WS_ID, WS_ID, strTargetMethodName, ref gid, ref strIncentive);
                    break;
                default:
                    // we should never get here
                    return false;
            }
           
            // Populate array ArrayAssertions_WebService with Privacy Policy info from web service                
            ArrayAssertions_WebService = new string[50, 16];

            for (int i = 0; i < 50; i++)
            {
                if (JaggedArrayAssertions_WebService[i][iRule_ID] == null)
                {
                    // don't put null items in the 2D assertions array
                    break;
                }              

                ArrayAssertions_WebService[i, iRule_ID] = JaggedArrayAssertions_WebService[i][iRule_ID];
                ArrayAssertions_WebService[i, iRule_Item_ID] = JaggedArrayAssertions_WebService[i][iRule_Item_ID];
                ArrayAssertions_WebService[i, iResource_ID] = JaggedArrayAssertions_WebService[i][iResource_ID];
                ArrayAssertions_WebService[i, iResource_Name] = JaggedArrayAssertions_WebService[i][iResource_Name];
                ArrayAssertions_WebService[i, iWeight] = JaggedArrayAssertions_WebService[i][iWeight];
                ArrayAssertions_WebService[i, iMandatory_Flag] = JaggedArrayAssertions_WebService[i][iMandatory_Flag];
                ArrayAssertions_WebService[i, iDomain_ID] = JaggedArrayAssertions_WebService[i][iDomain_ID];
                ArrayAssertions_WebService[i, iDomain_Name] = JaggedArrayAssertions_WebService[i][iDomain_Name];
                ArrayAssertions_WebService[i, iScope_ID] = JaggedArrayAssertions_WebService[i][iScope_ID];
                ArrayAssertions_WebService[i, iScope] = JaggedArrayAssertions_WebService[i][iScope];
                ArrayAssertions_WebService[i, iClientOrSvcName] = JaggedArrayAssertions_WebService[i][iClientOrSvcName];
                ArrayAssertions_WebService[i, iPriv_Match_Threshold] = JaggedArrayAssertions_WebService[i][iPriv_Match_Threshold];
                ArrayAssertions_WebService[i, iTopic_ID] = JaggedArrayAssertions_WebService[i][iTopic_ID];
                ArrayAssertions_WebService[i, iTopic] = JaggedArrayAssertions_WebService[i][iTopic];
                ArrayAssertions_WebService[i, iLevel_ID] = JaggedArrayAssertions_WebService[i][iLevel_ID];
                ArrayAssertions_WebService[i, iLevel] = JaggedArrayAssertions_WebService[i][iLevel];


                intWSAssertCount++;
            }
            
            //********************************************************************************************************

            // Now we have Client Assertions as well as Web Service Method Assertions
            // So we can now compare the two to check compatibility

            float flWeight = 0;
            float flTotalWeight = 0;
            bool blnCompatible = true;
            bool blnCompatibleOverall = true; 

            if ((intClientAssertCount == 0) || (intWSAssertCount == 0))
            {
                // we should never get here; this would mean there are no assertions in the database for client or web service
                blnCompatible = false;
            }
            else
            {                
                string strCompatibilityMessage = "";
                
                for (int i = 0; i < intClientAssertCount; i++)
                {
                    // check each client resource PR against web service method PP                    
                    blnCompatible = checkClientServiceCompat_ForItem(i, ref flWeight, ref strCompatibilityMessage);
                    strOverallCompatibilityMessage = strOverallCompatibilityMessage + "(" + (i + 1).ToString() + ") " + 
                                                     strCompatibilityMessage + "\n";
                    if (blnCompatible)
                    {
                        // non-mandatory items are always considered "compatible"; but they have a weight of zero
                        flTotalWeight = flTotalWeight + flWeight;
                        flWeight = 0; // initialize 
                    }
                    else
                    {
                        blnCompatibleOverall = false;
                    }
                }
                // need to put in form where we drop imprecise last digits of float
                string strTotalWeight = flTotalWeight.ToString("0.##");
                // put back into float
                bool blnIsFloat = float.TryParse(strTotalWeight, out flTotalWeight);
            }

            //**********************************************   
            
            // if we are compatible overall, check to make sure we have met our threshold number for the sum of weights
            // of all the matching items
            if (blnCompatibleOverall)
            {
                // Priv_Match_Threshold
                float flPrivacyMatchingThreshold = 0;
                bool blnIsFloat = float.TryParse(ArrayAssertions_Client[0, iPriv_Match_Threshold].ToString(), out flPrivacyMatchingThreshold);

                // we should always get a float back for this value
                if (blnIsFloat)
                {
                    if (flTotalWeight >= flPrivacyMatchingThreshold)
                    {  
                        // we have met the threshold set by the client; we are still compatible
                        strOverallCompatibilityMessage = strOverallCompatibilityMessage +
                                                          "We have met threshold set by client: Total Weight (of matched) " +
                                                          flTotalWeight.ToString() + " >= Threshold " + 
                                                          flPrivacyMatchingThreshold.ToString() + 
                                                          "\nAll mismatched items are non-mandatory. Now calling method...";
                    }
                    else
                    {
                        // we have not met the threshold set by the client
                        strOverallCompatibilityMessage = strOverallCompatibilityMessage + 
                                                          " Did not meet threshold set by client: Total Weight " +
                                                          flTotalWeight.ToString() + " < Threshold " +
                                                          flPrivacyMatchingThreshold.ToString();
                        blnCompatibleOverall = false;
                    }
                }
                else
                {
                    // we did not get a float back for this threshold value; we should never get here
                    strOverallCompatibilityMessage = strOverallCompatibilityMessage + " Did not get float back for threshold value";
                    blnCompatibleOverall = false; 
                }
            }

            //**********************************************           

            return blnCompatibleOverall;
        }

        public bool checkClientServiceCompat_ForItem(int intIndexOfResource, ref float flWeight, ref string strCompatibilityMessage)
        {
            getDomainSubsumptionRelationships();

            // need to match: on rule, resource, granularity and "propositional formula"; taking into account rules of subsumption
                        
            bool blnAtLeastOneMatch = false; 
            bool blnSubsumptionMatch = false;
            bool blnOverallMismatch = false;
            string C_strTrue = "True";
            string C_strOtherWebServices = "other web services";
            float flWeightValue = 0;
            int i = intIndexOfResource; // use variable with short name
            string strPartialMatchMessage = "";
            string strMandatoryDescription = "";
            
            //*******************************************

            flWeight = 0; // initialize return parameter
            flWeightValue = 0;
            bool blnWeightIsInt = float.TryParse(ArrayAssertions_Client[i, iWeight].ToString(), out flWeightValue);                               

            //*******************************************

            string strClientDisplayInfo = "";
            string strServiceDisplayInfo = "";
            string strClientServiceDisplayInfo = "";

            // we have a client assertion for this item; now we see if there is a 
            // ws method assertion and, if so, see if they match on important criteria
            for (int x = 0; x < intWSAssertCount; x++)
            { 

                //*********************************************************

                // if we have a client EXP resource, we need to look for a service PR resource
                // (we create a client PR resource string that we will seek to match with service PR resource)
                // if we have a client PR resource, we need to look for a service EXP resource  
                // (we create a client EXP resource string that we will seek to match with service EXP resource)

                bool blnClient_Practices = ArrayAssertions_Client[i, iResource_Name].Contains("_PR");
                bool blnClient_Expectations = ArrayAssertions_Client[i, iResource_Name].Contains("_EXP");
                string strClientEXPVersion = "";
                string strClientPRVersion = "";                
                                 
                if (blnClient_Practices)
                {
                    strClientEXPVersion = ArrayAssertions_Client[i, iResource_Name].Replace("_PR", "_EXP");
                }
                if (blnClient_Expectations)
                {
                    strClientPRVersion = ArrayAssertions_Client[i, iResource_Name].Replace("_EXP", "_PR");
                }

                //*********************************************************

                string strClientScope = ArrayAssertions_Client[i, iScope].ToString().Trim();
                if (strClientScope.Contains("GTE"))
                {
                    strClientScope = strClientScope + " (K-Anon >= " + strClientScope.Substring(3) + ")";
                }

                // for later display purposes

                strClientDisplayInfo = "Topic: " + ArrayAssertions_Client[i, iTopic].ToString() + ", "
                                       + "Level: " + ArrayAssertions_Client[i, iLevel].ToString() + "\n";

                strClientDisplayInfo = strClientDisplayInfo + 
                                       "Client " + "   Resource: " + ArrayAssertions_Client[i, iResource_Name].ToString() + ", "                                                  
                                                  + "Domain: " + ArrayAssertions_Client[i, iDomain_Name].ToString() + ", "
                                                  + "Scope: " + strClientScope 
                                                  + ", Weight: " + flWeightValue.ToString() + " \n";
                
                //*********************************************************

                // Now see if we have a match

                if (
                        // match on Resource (Desc)
                        (
                            (
                                (ArrayAssertions_Client[i, iResource_Name] == ArrayAssertions_WebService[x, iResource_Name])
                                && !(blnClient_Practices)
                                && !(blnClient_Expectations)
                            )
                            ||
                            (blnClient_Practices && (strClientEXPVersion == ArrayAssertions_WebService[x, iResource_Name]))
                            ||
                            (blnClient_Expectations && (strClientPRVersion == ArrayAssertions_WebService[x, iResource_Name]))
                        )

                        // match on Rule ID
                        && (ArrayAssertions_Client[i, iRule_ID] == ArrayAssertions_WebService[x, iRule_ID])

                        // match on Topic ID
                        && (ArrayAssertions_Client[i, iTopic_ID] == ArrayAssertions_WebService[x, iTopic_ID])

                        // match on Level ID
                        && (ArrayAssertions_Client[i, iLevel_ID] == ArrayAssertions_WebService[x, iLevel_ID])

                        // match on Scope ID, OR special K-Anonymity matching
                        &&
                        (
                           (ArrayAssertions_Client[i, iScope_ID] == ArrayAssertions_WebService[x, iScope_ID])  // match on scope_id (granularity)
                           ||
                           (
                             // K-Anon of client either needs to match K-Anon of service (see above condition) or
                             // K-Anon of client needs to be less than K-anon of service (see below condition)
                             (ArrayAssertions_Client[i, iDomain_Name] == C_strOtherWebServices)  // domain name is "other web services"
                             &&
                             (ArrayAssertions_Client[i, iScope].CompareTo(ArrayAssertions_WebService[x, iScope]) == -1)  // compare K-Anon (scope) strings                              
                           )
                        )
                    )
                {   
                    // we have matched on resource, rule_id, and scope_id (granularity)
                    // now we need to get the set of domains on the service side and compare with client domain name 
                    // (here we compare the rule subset (propositional formula) of the client with the service)

                    // our client domain needs to either match the service domain OR
                    // our client domain needs to subsume (be more general than) service domain                    

                    string strWSScope = ArrayAssertions_WebService[x, iScope].ToString().Trim();

                    // for later display purposes
                    strServiceDisplayInfo = "Service " + "  Resource: " + ArrayAssertions_WebService[x, iResource_Name].ToString() + ", "                                            
                                            + "Domain: " + ArrayAssertions_WebService[x, iDomain_Name].ToString() + ", "
                                            + "Scope: " + strWSScope;
                    if (strWSScope.Contains("GTE"))
                    {
                        strServiceDisplayInfo = strServiceDisplayInfo + " (K-Anon >= " + strWSScope.Substring(3) + ")";
                    }

                    strClientServiceDisplayInfo = strClientDisplayInfo + strServiceDisplayInfo + "\n";

                    // For a given Resource:
                    // We may have multiple Domains separated by "AND" as well as corresponding Domain_IDs sep by "AND" as well. 
                    // If there are the same quantity of and values for all domains on both client and WS sides
                    // then we will have an exact match, whether one on both sides or five on both sides
                    // For example:
                    // Ex 1) Client: government,            WS: government
                    // Ex 2) Client: government, research   WS: government, research
                    // 
                    // if one or both sides has an "AND", and not an exact match, then we need to look further                    

                    string strClientDomainID = ArrayAssertions_Client[i, iDomain_ID].ToString();
                    string strWebServiceDomainID = ArrayAssertions_WebService[x, iDomain_ID].ToString();

                    if (strClientDomainID == strWebServiceDomainID)
                    {
                        // exact match on domain
                        // we should always get at least one match; otherwise client did not get set up for
                        // full set of rules found on service side
                        strPartialMatchMessage = strPartialMatchMessage + strClientServiceDisplayInfo;
                        blnAtLeastOneMatch = true;
                    }
                    else
                    {
                        // no exact match on domain
                        if (
                            strClientDomainID.Contains(" AND ")
                            ||
                            strWebServiceDomainID.Contains(" AND ")
                            )
                        {  
                            // check for complex subsumption match
                            bool blnComplexSubsumptionMatch = checkForComplexSubsumptionMatch(strClientDomainID, strWebServiceDomainID, blnClient_Practices);
                            if (blnComplexSubsumptionMatch)
                            {
                                strPartialMatchMessage = "(Complex Subsumption match)\n" + strClientServiceDisplayInfo;
                                blnAtLeastOneMatch = true;
                            }
                            else
                            {
                                blnOverallMismatch = true;
                            }
                        }
                        else
                        {
                            // look to see if the client domain subsumes (is more general than) the service Domain
                            blnSubsumptionMatch = checkForSimpleSubsumptionMatch(strClientDomainID, strWebServiceDomainID, blnClient_Practices);
                            if (blnSubsumptionMatch)
                            {
                                strPartialMatchMessage = "(Subsumption match)\n" + strClientServiceDisplayInfo;
                                blnAtLeastOneMatch = true;
                            }
                            else
                            {
                                // we only need one mismatch on a Mandatory item
                                // to conclude that we have an overall mismatch                                
                                blnOverallMismatch = true;
                            }
                        }
                    }
                }
                else if  (  // see if we have K-Anon on both sides; if we have gotten this far, just have a mismatch on Scope      
                            (ArrayAssertions_Client[i, iDomain_Name] == C_strOtherWebServices)  
                            &&
                            (ArrayAssertions_WebService[x, iDomain_Name] == C_strOtherWebServices)                            
                          )
                {
                    string strWSScope = ArrayAssertions_WebService[x, iScope].ToString().Trim();
                    // for later display purposes
                    strServiceDisplayInfo = strServiceDisplayInfo  
                                            + "Service " + "  Resource: " + ArrayAssertions_WebService[x, iResource_Name].ToString() + ", "                                           
                                            + "Domain: " + ArrayAssertions_WebService[x, iDomain_Name].ToString() + ", "
                                            + "Scope: " + strWSScope;
                    if (strWSScope.Contains("GTE"))
                    {
                        strServiceDisplayInfo = strServiceDisplayInfo + " (K-Anon >= " + strWSScope.Substring(3) + ")";
                    }

                    strServiceDisplayInfo = strServiceDisplayInfo + "\n";
                }
            }

            //********************************************  
            //Endprocessing:                     

            // always tell user if client requirement was mandatory                
            bool blnMandatory;
            if (ArrayAssertions_Client[i, iMandatory_Flag] == C_strTrue)
            {
                blnMandatory = true;
                strMandatoryDescription = ",(Mandatory)";
            }
            else
            {
                blnMandatory = false; 
                strMandatoryDescription = ",(Non-Mandatory)";
            }

            //*****************************************************************************

            if (blnAtLeastOneMatch)
            {   
                // we have at least one match (direct match or via subsumption)
                if (blnOverallMismatch)
                {
                    strCompatibilityMessage = "Overall mismatch (but at least one match)" + 
                                              strMandatoryDescription + 
                                              ": \n" + 
                                              strPartialMatchMessage + "\n";
                    //******************************
                    if (blnMandatory)
                    {  
                        return false;   // a mismatch, and is mandatory: so incompatible
                    }
                    else
                    {
                        return true;    // a mismatch, but not mandatory: so compatible; note: weight will be zero   
                    }
                    //******************************
                }
                else
                {
                    // we have a complete match
                    // set the reference parameter to weight value so we can pass weight back
                    flWeight = flWeightValue;
                    // we have a match; so we can populate the return parameter with the Weight 
                    strCompatibilityMessage = "Overall match" + 
                                              strMandatoryDescription + 
                                              ": \n" + 
                                              strPartialMatchMessage;
                    return true;
                }
            }
            else
            {   // we should not get here; we should always get at least one match; 
                // otherwise client did not get set up for full set of rules found on service side
                strCompatibilityMessage = "Overall mismatch" +                                           
                                          strMandatoryDescription + 
                                          ": \n" +
                                          strClientDisplayInfo + strServiceDisplayInfo + "\n";
                //******************************
                if (blnMandatory)
                {
                    return false;   // a mismatch, and is mandatory: so incompatible
                }
                else
                {
                    return true;    // a mismatch, but not mandatory: so compatible; note: weight will be zero   
                }
                //******************************
            }

            //********************************************
            
        }

        public bool checkForComplexSubsumptionMatch(string strClientDomainID, string strServiceDomainID, bool blnClient_Practices)
        {
            // we don't have an exact match, so look to see if match per complex subsumption
                        
            // 1) Client Expectations, WS Practices:
            //    Client domain needs to subsume (be more general than) the WS Domain
            // 2) WS Expectations, Client Practices:
            //    WS domain needs to subsume (be more general than) the Client domain

            bool blnMatch = false;

            //*******************************

            // each domain on the WS side needs to either match or be more general than at least one on client side

            bool blnMultipleClientDomains = false;
            bool blnMultipleServiceDomains = false;
            string[] arrClientDomains = null;
            string[] arrServiceDomains = null;

            //**************************
            // we need to treat this case opposite from the others; we swap the domain strings
            if (blnClient_Practices)
            {
                string strSwapString = "";
                strSwapString = string.Copy(strClientDomainID);
                strClientDomainID = string.Copy(strServiceDomainID);
                strServiceDomainID = string.Copy(strSwapString);
            }
            //**************************

            if (strClientDomainID.Contains(" AND "))
            {
                blnMultipleClientDomains = true;
                arrClientDomains = Regex.Split(strClientDomainID, " AND ");                
            }
            if (strServiceDomainID.Contains(" AND "))
            {
                blnMultipleServiceDomains = true;
                arrServiceDomains = Regex.Split(strServiceDomainID, " AND ");
            }


            if (blnMultipleClientDomains)
            {
                if (blnMultipleServiceDomains)
                {
                    // mult client domains, mult service domains

                    // EACH SERVICE DOMAIN MUST BE EQUAL TO OR MORE NARROW THAN AT LEAST ONE CLIENT DOMAIN
                    // 2 loops
                    for (int i = 0; i < arrServiceDomains.Count(); i++)
                    {
                        //*************************
                        for (int j = 0; j < arrClientDomains.Count(); j++)
                        {
                            if (arrServiceDomains[i].ToString() == arrClientDomains[j].ToString())
                            {
                                blnMatch = true; // break out of the inner loop
                                break;
                            }
                            else if (checkForSimpleSubsumptionMatch(arrClientDomains[j].ToString(), arrServiceDomains[i].ToString(), blnClient_Practices))
                            {
                                blnMatch = true; // break out of the inner loop
                                break;
                            }                            
                        }

                        if (!blnMatch)
                        {
                            // we were not able to get a match on at least one of the service domains, so we have an overall mismatch
                            // of domains and we are done (we will be returning a false)
                            break;
                        }
                        //*************************
                    }
                }
                else
                {
                    // mult client domains, single service domain

                    // THE SERVICE DOMAIN MUST BE EQUAL TO OR MORE NARROW THAN AT LEAST ONE CLIENT DOMAIN
                    // 1 loop
                    for (int i = 0; i < arrClientDomains.Count(); i++)
                    {
                        if (arrClientDomains[i].ToString() == strServiceDomainID)
                        {
                            blnMatch = true;
                            break;
                        }
                        else if (checkForSimpleSubsumptionMatch(arrClientDomains[i].ToString(), strServiceDomainID, blnClient_Practices))
                        {
                            blnMatch = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (blnMultipleServiceDomains)
                {
                    // single client domain, mult service domains

                    // ALL SERVICE DOMAINS MUST BE EQUAL TO OR MORE NARROW THAN THE CLIENT DOMAIN
                    // 1 loop
                    for (int i = 0; i < arrServiceDomains.Count(); i++)
                    {
                        blnMatch = true; // see if can go through all service domains without breaking

                        if (strClientDomainID == arrServiceDomains[i].ToString())
                        { }
                        else if (checkForSimpleSubsumptionMatch(strClientDomainID, arrServiceDomains[i].ToString(), blnClient_Practices))
                        { }
                        else
                        {
                            blnMatch = false;
                            break;
                        }
                    }                    
                }
                else
                {
                    // single client domain, single service domain                    
                    // *we should never get here since to get here we had to have and "AND" in either client or service string
                }
            }
            
            //*******************************

            return blnMatch;
        }

        public bool checkForSimpleSubsumptionMatch(string strClientDomainID, string strServiceDomainID, bool blnClient_Practices)
        {
            // we don't have an exact match, so look to see if match per subsumption

            // 1) Client Expectations, WS Practices:
            //    Client domain needs to subsume (be more general than) the WS Domain
            // 2) WS Expectations, Client Practices:
            //    WS domain needs to subsume (be more general than) the Client domain

            bool blnMatch = false;

            //*******************************
                        
            for (int z = 0; z < intDomainSubsumpRelationshipsCount; z++)
            {               
                // the Client domain needs to subsume (be more general than) the WS domain 
                if (
                    (strClientDomainID == ArrayDomainSubsumpRelationships[z, iSubsuming_Domain_ID]) &&
                    (strServiceDomainID == ArrayDomainSubsumpRelationships[z, iSubsumed_Domain_ID])
                    )
                {
                    blnMatch = true; // we have a match
                    break;
                }  
            }

            //*******************************

            return blnMatch; 
        }

        public void getDomainSubsumptionRelationships()
        {
            // we go against the stored procedure and populate an array
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spGetDomainSubsumptionRelationships");
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Connection = conn;
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                ArrayDomainSubsumpRelationships = new string[100, 2];
                intDomainSubsumpRelationshipsCount = 0;

                while (reader.Read())
                {
                    ArrayDomainSubsumpRelationships[intDomainSubsumpRelationshipsCount, iSubsumed_Domain_ID] = reader["Subsumed_Domain_ID"].ToString();
                    ArrayDomainSubsumpRelationships[intDomainSubsumpRelationshipsCount, iSubsuming_Domain_ID] = reader["Subsuming_Domain_ID"].ToString();

                    ++intDomainSubsumpRelationshipsCount;
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "Compatibility Class, getDomainSubsumptionRelationships: " + exc.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public void populateClientAssertionsArray(int WS_ID, int Target_WS_ID, string strTargetMethodName)
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["WSProjectConnectionString"].ConnectionString;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("spGetAllAssertionsForClient");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ws_id", SqlDbType.Int)).Value = WS_ID;
                cmd.Parameters.Add(new SqlParameter("@target_ws_id", SqlDbType.Int)).Value = Target_WS_ID;
                cmd.Parameters.Add(new SqlParameter("@target_method_name", SqlDbType.NVarChar)).Value = strTargetMethodName;
                cmd.Connection = conn;
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                ArrayAssertions_Client = new string[50,16];
                intClientAssertCount = 0;

                while (reader.Read())
                {
                    ArrayAssertions_Client[intClientAssertCount, iRule_ID] = reader["Rule_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iRule_Item_ID] = reader["Rule_Item_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iResource_ID] = reader["Resource_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iResource_Name] = reader["Resource_Name"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iWeight] = reader["Weight"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iMandatory_Flag] = reader["Mandatory_Flag"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iDomain_ID] = reader["Domain_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iDomain_Name] = reader["Domain_Name"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iScope_ID] = reader["Scope_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iScope] = reader["Scope"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iClientOrSvcName] = reader["ClientOrSvcName"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iPriv_Match_Threshold] = reader["Priv_Match_Threshold"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iTopic_ID] = reader["Topic_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iTopic] = reader["Topic"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iLevel_ID] = reader["Level_ID"].ToString();
                    ArrayAssertions_Client[intClientAssertCount, iLevel] = reader["Level"].ToString();

                      ++intClientAssertCount;
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                var dataFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt");
                File.AppendAllText(@dataFile, "Compatibility Class, populateClientAssertionsArray: " + exc.Message.ToString());
            }
            finally
            {
                conn.Close();
            }           
        }                
    }
}
