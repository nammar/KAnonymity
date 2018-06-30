<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="WSCLAdmin.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">           
        <table border="0" align=left>
          <tr>
              <td ><font size="5"><b>Web Service Admin Site - WSCL Processing</b></font></td>
          </tr>
          <tr><td></td></tr>
          <tr>
              <td class="style1">
                   User selects <b>WSCL Conversation Transitions File</b>, clicks Submit, and 3 Intermediate Tables are populated.</td>
          </tr>
           <tr>
              <td>
                    The last intermediate table is potentially used to populate <b>WS Privacy Ruleset Items</b>.
              </td>     
          </tr>
          <tr><td>&nbsp</td></tr> 
           <tr>
              <td>
                    <b><u>Diagrams for Input Files of 5 and 9 Methods</u>. In this Visual Studio project, the actual client has references to 5 WS methods.  </b>
              </td>     
          </tr> 
          <tr>
              <td>
                    <b>The first diagram reflects these methods and their relation to one another. The second is included for comparison purposes.</b>
              </td>     
          </tr> 
          <tr><td>&nbsp</td></tr> 
          <tr>
              <td>
                    
                  <asp:Image ID="Image1" src = "Five Method Choreography.jpg" runat="server" 
                      Width="268px" BorderColor="#99FFCC" BorderStyle="Solid" BorderWidth="2px" 
                      Height="214px" style="margin-top: 0px" />&nbsp;&nbsp;&nbsp;
                  <asp:Image ID="Image2" src = "Nine Method Choreography.jpg" runat="server" 
                       Width="218px" Height="214px" style="margin-top: 2px" 
                      BorderColor="#66FFCC" BorderStyle="Solid" BorderWidth="2px" />
                    
              </td>     
          </tr>
          <tr><td>&nbsp</td></tr> 
          <tr>             
              <td> <font color="red"><asp:Label ID="lblMessageToUser" runat="server" Text=""></asp:Label>
                  <br />
                  </font></td>
          </tr> 
           <tr><td>&nbsp</td></tr>   
           <tr><td>
                 <b>Two possible WSCL files can be selected on this page. They correspond to the above diagrams.</b>
              </td>
          </tr>
          <tr>
              <td>
              <asp:Label ID="lblFileSelected" runat="server" Text=""></asp:Label>  
              </td>
          </tr>                            
          <tr>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" Width="284px" style="width: 90px; margin-left: 3px;" 
                        onchange="this.form.submit()"/>
                </td> 
          </tr>         
           <tr><td>&nbsp</td></tr>  
          <tr><td><b>Choose K-Anonymity Type</b></td></tr>    
          <tr>
             <td>
              <asp:RadioButtonList ID="rblKAnonType" runat="server">
                  <asp:ListItem Selected="True" Value="AllMethPassThroughCounted">All WSCL Methods Can Be Called (Pass-Through Methods Counted)</asp:ListItem>
                  <asp:ListItem Value="AllMethPassThroughNotCounted">All WSCL Methods Can Be Called (Pass-Through Methods Not Counted)</asp:ListItem>
                  <asp:ListItem Value="OnlyEndpointMeth">Only Endpoint WSCL Methods Can be Called</asp:ListItem>
              </asp:RadioButtonList>
              </td>
          </tr> 
           <tr><td>&nbsp</td></tr>  
          <tr><td>
                    <asp:Button ID="btnClearTables" runat="server" Text="Clear Tables" onclick="btnClearTables_Click" />
                </td>
           </tr>
           <tr><td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Process File" onclick="btnSubmit_Click" />
                </td>
           </tr>
           <tr><td>&nbsp</td></tr>  
           <tr><td><b>WSCL ConversationTransitions</b></td></tr>    
          <tr><td>
              <asp:TextBox ID="txtWSCLDisplay" runat="server" Height="317px" 
                  TextMode="MultiLine" Width="643px" Enabled="False"></asp:TextBox>
              </td></tr>
          <tr><td>&nbsp</td></tr>  
           <tr><td><b>Conversation Transitions (table: WSCL_Transitions)</b></td></tr>  
           <tr><td><b>* Reflection of WSCL conversation transitions in table form</b></td></tr>  
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource1" Width="650px">
                  <Columns>
                      <asp:BoundField DataField="source_name" HeaderText="source_name" 
                          SortExpression="source_name" />
                      <asp:BoundField DataField="destination_name" HeaderText="destination_name" 
                          SortExpression="destination_name" />
                  </Columns>
                  <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  SelectCommand="spGet_WSCL_Transitions" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
              </td></tr>
          <tr><td>&nbsp</td></tr>    
           <tr><td><b>Conversation Transitions - All Possible Routes Plus KAnonymity (table: 
               WSCL_Transitions_KAnonymity)</b></td></tr>  
           <tr><td><b>* Expansion of previous table information, indicating all possible end-to-end routes and K-Anonymity GTE values.</b></td></tr> 
           <tr><td><b>* GTE = "greater than equal". The GTE number indicates that K-Anonymity will be at least as large as this value.</b></td></tr> 
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource2" Width="650px">
                  <Columns>
                      <asp:BoundField DataField="source_name" HeaderText="source_name" 
                          SortExpression="source_name" />
                      <asp:BoundField DataField="destination_name" HeaderText="destination_name" 
                          SortExpression="destination_name" />
                      <asp:BoundField DataField="KAnon" HeaderText="KAnon" SortExpression="KAnon" />
                      <asp:BoundField DataField="Scope_Desc" HeaderText="Scope_Desc" 
                          SortExpression="Scope_Desc" />
                  </Columns>
                  <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  SelectCommand="spGet_WSCL_Transitions_AllPossibleRoutes_KAnonymity" 
                  SelectCommandType="StoredProcedure"></asp:SqlDataSource>
              </td></tr>
          <tr><td>&nbsp</td></tr> 
          <tr><td><b>Possible Rows to Add to WS Privacy Ruleset Items (table: 
              WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd)</b></td></tr>  
          <tr><td><b>* We choose the smallest GTE values for each particular web service method.</b></td></tr>  
          <tr><td><b>* These values define a potential K-Anonymity Privacy Policy rule item for each method.</b></td></tr>  
          <tr><td><b>* They will be added to the available rule items if not there already.</b></td></tr>  
           <tr><td>&nbsp</td></tr>  
          <tr><td>
              <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                  DataSourceID="SqlDataSource3" Width="780px">
                  <Columns>
                      <asp:BoundField DataField="ws_id" HeaderText="ws_id" 
                          SortExpression="ws_id" />
                      <asp:BoundField DataField="rule_name" HeaderText="rule_name" 
                          SortExpression="rule_name" />
                      <asp:BoundField DataField="Rule_ID" HeaderText="Rule_ID" 
                          SortExpression="Rule_ID" InsertVisible="False" ReadOnly="True" />
                      <asp:BoundField DataField="rule_item_id" HeaderText="rule_item_id" 
                          InsertVisible="False" ReadOnly="True" SortExpression="rule_item_id" />
                      <asp:BoundField DataField="topic_id" HeaderText="topic_id" 
                          SortExpression="topic_id" />
                      <asp:BoundField DataField="topic_desc" HeaderText="topic_desc" 
                          SortExpression="topic_desc" />
                      <asp:BoundField DataField="level_id" HeaderText="level_id" 
                          SortExpression="level_id" />
                      <asp:BoundField DataField="level_desc" HeaderText="level_desc" 
                          SortExpression="level_desc" />
                      <asp:BoundField DataField="domain_id" HeaderText="domain_id" 
                          SortExpression="domain_id" />
                      <asp:BoundField DataField="domain_desc" HeaderText="domain_desc" 
                          SortExpression="domain_desc" />
                      <asp:BoundField DataField="scope_id" HeaderText="scope_id" 
                          SortExpression="scope_id" />
                      <asp:BoundField DataField="scope_desc" HeaderText="scope_desc" 
                          SortExpression="scope_desc" />
                  </Columns>
                  <rowstyle backcolor="LightCyan"  
                       forecolor="DarkBlue"
                       font-italic="false"/>
                        <alternatingrowstyle backcolor="PaleTurquoise"  
                          forecolor="DarkBlue"
                          font-italic="false"/>
              </asp:GridView>
              </td></tr>
          <tr><td>
              <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                  ConnectionString="<%$ ConnectionStrings:WSProjectConnectionString %>" 
                  
                  
                  SelectCommand="spGetWSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd" 
                  SelectCommandType="StoredProcedure"></asp:SqlDataSource>
              </td></tr> 
          <tr><td>&nbsp</td></tr>  
          <tr><td>&nbsp</td></tr>  
        </table>
        <table border="0" align=left>
          <tr>
             <td>&nbsp;</td>               
          </tr> 
        </table>    

</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    </asp:Content>


