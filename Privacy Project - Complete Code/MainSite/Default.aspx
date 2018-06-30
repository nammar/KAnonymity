<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
        <table border="0" align="left">
        <tr>
              <td class="style1"><font size="5"><b>Web Service Methods - Check Compatibility and Call</b></font></td>
          </tr>
          <tr><td></td></tr>
          <tr>    
             <td><font color="red">
                  <b><asp:Label ID="lblMessageToUser" runat="server" Text=""></asp:Label></b>
                  </font>
                  </td>  
             <td></td>         
          </tr> 
          <tr><td>&nbsp;</td></tr> 
          <tr>
             <td><b>Please select a web service method for the client to call:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>    
             <td></td>            
          </tr> 
          <tr><td>&nbsp;</td></tr> 
          <tr>
             <td>
              <table border="0" align=left>
                 <tr>
                 <td class="style2"><b>Web Services:</b></td>
                 <td><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Methods:</b></td>
                 <td><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Parameters:</b></td>
                 </tr>
              </table>
             </td>                
          </tr>          
          <tr>
              <td class="style1">
                <asp:ListBox ID="lstWebServices" runat="server" Height="86px" Width="246px" 
                      AutoPostBack="True" 
                      onselectedindexchanged="lstWebServices_SelectedIndexChanged" 
                      BackColor="LightCyan">
                </asp:ListBox>
                 &nbsp;&nbsp;&nbsp;
                <asp:ListBox ID="lstMethods" runat="server" Height="86px" Width="244px" 
                      AutoPostBack="True" 
                      onselectedindexchanged="lstMethods_SelectedIndexChanged" 
                      BackColor="LightCyan">
                </asp:ListBox>
                &nbsp;&nbsp;&nbsp;
                <asp:ListBox ID="lstParam" runat="server" Height="86px" Width="260px" 
                      onselectedindexchanged="lstWebServices_SelectedIndexChanged" 
                      BackColor="LightCyan">
                </asp:ListBox>
                  <br />
                </td>
              <td>
                  &nbsp;</td>
              </td>      
          </tr>
          <tr><td class="style1">&nbsp;<br />
                    <b><asp:Label ID="lblParamSpecified" runat="server" 
                        Text="Please specify all input parameters, delimited by commas (any output parameters entered are ignored):"></asp:Label></b>                   
                </td>  
          </tr>
          <tr>
                <td class="style1">
                    <asp:TextBox ID="txtParamSpecified" runat="server" Width="550px" 
                        BackColor="LightCyan"></asp:TextBox>
                </td>  
          </tr>
           <tr><td class="style1">&nbsp;</td></tr>
          <tr>
             <td class="style3"><asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                     onclick="btnSubmit_Click" /></td>
          </tr> 
          <tr><td class="style1">&nbsp;</td></tr>
          <tr>
                <td class="style1"><font color="red"><b><asp:Label ID="lblCompatCheckResults" runat="server"></asp:Label></b></font></td>
          </tr> 
          <tr><td>&nbsp;</td></tr>  
           <tr> <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                      
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         &nbsp;&nbsp;&nbsp;
                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <b>
                  <asp:Label ID="Label3" runat="server" 
                  Text="Client Threshold (0 to 1), Submit Different Number In Order to Change:       "></asp:Label></b>     
                  <asp:TextBox ID="txtClientThreshold" runat="server" Width="42px" 
                   BackColor="LightCyan" style="text-align:right; margin-left: 7px;"></asp:TextBox>  
                </td>  
          </tr>       
           <tr> <td>&nbsp;<b><asp:Label ID="lblCompatMessage" runat="server" 
                        Text="Compatibility Check - Results:"></asp:Label></b>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <b>
                  <asp:Label ID="Label2" runat="server" 
                  Text="Unique Identifier for Resubmission: "></asp:Label></b>     
                  <asp:TextBox ID="txtUniqueIdentifierForResubmit" runat="server" 
                   Width="287px" BackColor="LightCyan"></asp:TextBox>  
                </td>  
          </tr>
          <tr><td class="style1">
              <asp:TextBox ID="txtCompatMessage" runat="server" Height="419px" 
                  Width="913px" TextMode="MultiLine" BackColor="LightCyan" Enabled="False"></asp:TextBox>
              </td>
          </tr>          
          <tr>  <td class="style1">&nbsp;<br />
                   <b><asp:Label ID="lblResults" runat="server" Text="Method Execution - Results:"></asp:Label></b>                   
                </td>  
          </tr>
          <tr>
                <td class="style1">
                    <asp:TextBox ID="txtResults" runat="server" Width="913px" 
                        Height="99px" TextMode="MultiLine" BackColor="LightCyan" Enabled="False"></asp:TextBox>
                </td>  
          </tr>  
          <tr><td class="style1">&nbsp;</td></tr>   
          <tr><td class="style1">&nbsp;</td></tr>          
        </table>     

        <table border="0" align=left>
          <tr>
             <td>&nbsp;</td>               
          </tr> 
        </table> 
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 797px;
        }
        .style2
        {
            width: 85px;
        }
        .style3
        {
            width: 797px;
            height: 30px;
        }
    </style>
</asp:Content>


