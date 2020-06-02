<?xml version="1.0" encoding="us-ascii"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0" xmlns:sr="http://www.cap.org/pert/2009/01/"
	xmlns:x="urn:ihe:qrph:sdc:2016"
	xmlns:xalan="http://xml.apache.org/xslt"
	xmlns:trig="http://www.ora.com/XSLTCookbook/extend/trig"
	>
	
	<!--xmlns:xalan="http://xml.apache.org/xslt"-->
  <xsl:output encoding="us-ascii" method="html" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
  
  
	<xsl:variable name="show-toc" select="'false'"/>
	<xsl:variable name="debug" select="'false'"/>

	<!-- new code -->
	<xsl:variable name="metadata-display" select="'true'"/>
	<xsl:variable name="change-display" select="'true'"/>
	
	<xsl:template match="/">
		
		<xsl:variable name ="required" select="string(//Header/Property[@type='web_posting_date meta']/@val)"/>
        
        		    
			<title><xsl:value-of select="//x:Header/@title"/></title>
			
			
           <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
		   <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>  		
			
			<nav style="position: fixed; font-size:smaller;" id="navBar">
				<!--<a id="linkxml" href="#" onclick="toggleviewxml()">Show Xml</a>
				<br />-->
			 
               <a href="##" onclick="toggle_metadata();">
                  Toggle Metadata
               </a>
              <br/>
			   
               <a href="##" onclick="toggle_id();">
                  Toggle IDs
               </a>
              <br/>
			  
			  <a style='display:inline-block; max-width:100px' href="##" id='mnuRequired' onclick="toggle_mustImplement();">
                  Toggle optional items
              </a>
			 <br/>
			 
			  <a href="##" class="collapse_all_control">
				Toggle All
			  </a><br/>
				 
			 <a href="##" class="collapse_control">
				Toggle Sections
			 </a><br/>
			 
			 <a href="##"  class="collapse_q_control">
				Toggle Questions
			 </a><br/>

			 <a href="##" onclick="clear_storage();">
                  Clear State
               </a>
              <br/>
            </nav>
            	
            	<!--hidden textbox to store rawxml at run time-->
            	<!--<input type="hidden" id = "rawxml"/>-->
            	<!--<textarea rows="10" style="width:90%;margin:40px;background-color:lightyellow" id = 'rawxml'> 
		               <xsl:copy-of select="node()"/>
		               
         		</textarea>	-->
				
				<div class="BodyGroup">
					<!--
					<xsl:if test="$show-toc='true' and count($template-links/template-link) &gt; 0">
						<xsl:attribute name="style">
							<xsl:text>float:left</xsl:text>
						</xsl:attribute>
					</xsl:if>
					-->
					<div id="confirmBox">
						<div class="message"></div>
						<span class="yes">Yes</span>
						<span class="no">No</span>
					</div>
					
					
					<!--<textarea id="rawxml" />-->
					
					<div id="MessageData" style="display:none;">
						<table class="HeaderGroup" align="center">
							<tr>
								<td>
									
									<div class="TopHeader">
										Structured Report Data
									</div>
									<div id="MessageDataResult" class="MessageDataResult"/>
									
									
									<div class="SubmitButton">
										<input type="button" value="Back" onClick="javascript:closeMessageData()" />
									</div>
								</td>
							</tr>
						</table>
					</div>
					
					<br style="clear:both"/>
						
					<div id="response" style="display:none;">
					</div>
					
					<div id="FormData">
					    
						
						<form id="checklist" name="checklist" method="post" >
							<xsl:attribute name="action">
								<!--<xsl:value-of select="$form-action"/>-->
							</xsl:attribute>
							
							<!--formInstanceURI, formInstanceVersionURI, formPreviousInstanceVersionURI-->
							<div class="form-version">								
								<xsl:if test="//x:FormDesign/@formInstanceURI">
									<p>
										Form Instance: 
										<xsl:value-of select="//x:FormDesign/@formInstanceURI"/>
									</p>
								</xsl:if>
								<xsl:if test="//x:FormDesign/@formInstanceVersionURI">
									<p>
										Version: 
										<xsl:value-of select="//x:FormDesign/@formInstanceVersionURI"/>
									</p>
								</xsl:if>
								<xsl:if test="//x:FormDesign/@formPreviousInstanceVersionURI">
									<p>
										Previous Version: 
										<xsl:value-of select="//x:FormDesign/@formPreviousInstanceVersionURI"/>
									</p>
								</xsl:if>
							</div>
							
							<div class='formTitle'>
								<xsl:value-of select="//x:FormDesign/@formTitle"/>
							</div>
			
							<!--show properties under form-design-->
							<div>
							<div id='columnarblock'>							
								<div id='left-column'>
									<xsl:for-each select="//x:FormDesign/x:Property">
										<xsl:if test="@val">
											<xsl:variable name="textstyle" select="@styleClass"/>
											<xsl:if test='not(@styleClass)'>
												<p class='{$textstyle}'>
												<b><xsl:value-of select="@propName"/></b>
												<xsl:if test="@propName">
													:
												</xsl:if>
												<xsl:value-of select="@val"/>
												</p>
											</xsl:if>
										</xsl:if>
									</xsl:for-each>
								</div>
								<div id='right-column'>
									
										<xsl:for-each select="//x:FormDesign/x:Property">
											<xsl:if test="@val">
												<xsl:variable name="textstyle" select="@styleClass"/>
												<xsl:if test='$textstyle="right" or $textstyle="float-right"'>
													<p class='{$textstyle}'>
													<b><xsl:value-of select="@propName"/></b>
													<xsl:if test="@propName">
														:
													</xsl:if>
													<xsl:value-of select="@val"/>
													</p>
												</xsl:if>
											</xsl:if>
									</xsl:for-each>
								</div>
							</div>
							</div>
			
							<!--show header-->
							<xsl:if test="//x:Header">
								<xsl:variable name="title_style" select="//x:Header/@styleClass"/>
								<xsl:variable name='title_id' select="//x:Header/@ID"/>
								<div ID = '{$title_id}' class="Header_{$title_style}">								
									<xsl:value-of select="//x:Header/@title"/>
								</div>
								<div style="clear:both"/>
								<hr/>
								
								<div>
								<xsl:for-each select="//x:Header/x:Property">
									<xsl:if test="@val">
									
										<xsl:variable name="textstyle" select="@styleClass"/>
										<p class='{$textstyle}'>
											<b><xsl:value-of select="@propName"/></b>
											<xsl:if test="@propName">
												:
											</xsl:if>
											<xsl:value-of select="@val"/>
										</p>
									</xsl:if>								
								</xsl:for-each>
									
								</div>
							</xsl:if>
							<div style="clear:both"></div>
							
							<!--Demo-->		
							<xsl:if test="//x:DemogFormDesign">
								<a style="text-decoration:none;color:black;font-weight:bold;font-size:large" id="demshowhide" href="#" onclick="ShowHideDemo()">+ Demographics</a>
								<div id="divdemo" style="display:none">								
									<xsl:apply-templates select="//x:DemogFormDesign/x:Body/x:ChildItems/x:Section" mode="level1" >
										<xsl:with-param name="required" select="$required" />
										<xsl:with-param name="parentId" select="'*'"/>  
										<xsl:with-param name="defaultStyle" select="'TopHeaderDemo'"/>
									</xsl:apply-templates>
									<xsl:apply-templates select="//x:DemogFormDesign/x:Body/x:ChildItems/x:Question" mode="level2" >
										<xsl:with-param name="required" select="$required" />
										<xsl:with-param name="parentId" select="'*'"/>   
									</xsl:apply-templates>
								</div>
							</xsl:if>
							<hr/>
							<!--show body-->
							<xsl:apply-templates select="//x:FormDesign/x:Body/x:ChildItems/x:Section|//x:FormDesign/x:Body/x:ChildItems/x:DisplayedItem" mode="level1">
								<xsl:with-param name="required" select="$required" />
								<xsl:with-param name="parentId" select="'*'"/> <!--parentId = * for outermost --> 
								<xsl:with-param name="defaultStyle" select="'TopHeader'"/>
							</xsl:apply-templates>
							
							<!--<xsl:apply-templates select="//x:FormDesign/x:Body/x:ChildItems/x:Question" mode="level2" >-->
							<xsl:apply-templates select="//x:FormDesign/x:Body/x:ChildItems/x:Question" mode="level1" >
								<xsl:with-param name="required" select="$required" />
								<xsl:with-param name="parentId" select="'*'"/>  <!--parentId = * for outermost --> 
							</xsl:apply-templates>
							
							<!--<xsl:if test="contains($form-action, 'http') or contains($form-action, 'javascript')">-->
								<!--remove submit button for the desktop verion-->
								<div style="clear:both"/>
								<div class="SubmitButton" style="display:none">
									<input type="submit" id="send" value="Submit" onclick="javascript:openMessageData(1);return false;"/>
								</div>
							<!--</xsl:if>-->
						</form>
					</div>
				</div>
            
    </xsl:template>
   
    <xsl:template match="//x:Header">
       
       
    </xsl:template>
	
	<!--sections within body and other sections directly inside sections-->
	<xsl:template match="x:Section" mode="level1">
		<xsl:param name="parentSectionId"/>	
		<xsl:param name="defaultStyle"/>
		<!--<xsl:if test="string-length(@title) &gt; 0">--> <!--do not show if there is no title-->
			<xsl:if test="not (@visible) or (@visible='true')">
				<xsl:variable name="required" select="true"/>
				<xsl:variable name="style" select="@styleClass"/>
				<!--<xsl:variable name="defaultStyle" select="'TopHeader'"/>-->
				<xsl:variable name="sectionId" select="concat('s',@ID)"/>
				<div> 					
					<xsl:attribute name="id">
						<xsl:value-of select="$sectionId"/>						
					</xsl:attribute>
	  				<xsl:if test="@mustImplement='false'">
						<div class='mustImplement'></div>
					</xsl:if>
					<input id = "maxcardinality" type="hidden">
						<xsl:attribute name="value">
							<xsl:value-of select="@maxCard"/>
						</xsl:attribute>						
					</input>
					
					
					<!-- table is repeated if cardinality is greater than 1 and id value will be incremented-->
					<table class="HeaderTable" align="center">					   
					   <xsl:attribute name="id">
						<xsl:value-of select="$sectionId"/>
					   </xsl:attribute>
						<tr>
							<td>								
								<xsl:choose>
									<xsl:when test="$style!=''">
										<div class="{$style} collapsable">										
											<xsl:value-of select="@title"/>
										</div>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="count(ancestor::x:Section)= 0">
												<div class="{$defaultStyle} collapsable">
													<!-- new code -->
													<xsl:call-template name="ItemWithID">
													   <xsl:with-param name="required" select="required"/>
													</xsl:call-template>
													
													<xsl:value-of select="@title"/>	
													<div style="display:inline" class="MetadataDisplay">
														<!---metadata-->
														<xsl:if test="x:Property/@propName!=''">
															<div class="reportText">
																<xsl:value-of select="x:Property/@propName"/> : 
															</div>
															<div class="reportTextValueSection">
																<xsl:value-of select="x:Property/@val"/> 
															</div>
														</xsl:if>
													</div>													
												</div>
												<div style='clear:both'/>
											</xsl:when>
											<xsl:when test="count(ancestor::x:Section)= 1">
												<div class="{$defaultStyle}2 collapsable">
													<!-- new code -->
													<xsl:call-template name="ItemWithID">
													   <xsl:with-param name="required" select="required"/>
													</xsl:call-template>
													
													<xsl:value-of select="@title"/>	
													<div style="display:inline" class="MetadataDisplay">
														<!---metadata-->
														<xsl:if test="x:Property/@propName!=''">
															<div class="reportText">
																<xsl:value-of select="x:Property/@propName"/> : 
															</div>
															<div class="reportTextValueSection">
																<xsl:value-of select="x:Property/@val"/> 
															</div>
														</xsl:if>
													</div>													
												</div>
												<div style='clear:both'/>
											</xsl:when>
											<xsl:when test="count(ancestor::x:Section)= 2">
												<div class="{$defaultStyle}2 collapsable">
													<!-- new code -->
													<xsl:call-template name="ItemWithID">
													   <xsl:with-param name="required" select="required"/>
													</xsl:call-template>
													
													<xsl:value-of select="@title"/>	
													<div style="display:inline" class="MetadataDisplay">
														<!---metadata-->
														<xsl:if test="x:Property/@propName!=''">
															<div class="reportText">
																<xsl:value-of select="x:Property/@propName"/> : 
															</div>
															<div class="reportTextValueSection">
																<xsl:value-of select="x:Property/@val"/> 
															</div>
														</xsl:if>
													</div>
												</div>
											</xsl:when>
											<xsl:otherwise>
												<div class="{$defaultStyle}2 collapsable">
													<!-- new code -->
													<xsl:call-template name="ItemWithID">
													   <xsl:with-param name="required" select="required"/>
													</xsl:call-template>
													
													<xsl:value-of select="@title"/>	
													<div style="display:inline" class="MetadataDisplay">
														<!---metadata-->
														<xsl:if test="x:Property/@propName!=''">
															<div class="reportText">
																<xsl:value-of select="x:Property/@propName"/> : 
															</div>
															<div class="reportTextValueSection">
																<xsl:value-of select="x:Property/@val"/> 
															</div>
														</xsl:if>
													</div>
												</div>
											</xsl:otherwise>
											
										</xsl:choose>
										
									</xsl:otherwise>
								</xsl:choose>
								<!--show link here?-->
								<xsl:for-each select="x:Link">					
									<xsl:call-template name="handle_link"/>
								</xsl:for-each>

								<xsl:choose>
									<xsl:when test="$required='false'">

									</xsl:when>
									<xsl:otherwise>	
										<xsl:apply-templates select="x:ChildItems/x:Question | x:ChildItems/x:Section | x:ChildItems/x:DisplayedItem" mode="level1" >
											<xsl:with-param name="required" select="'true'"/>
											<xsl:with-param name="parentSectionId" select="$sectionId"/>
											<xsl:with-param name="defaultStyle" select="$defaultStyle"/>
										</xsl:apply-templates>
										<xsl:apply-templates select="x:ChildItems/x:ButtonAction">
											<xsl:with-param name="action" select="onclick"/>
											<xsl:with-param name="functionName" select="//CallFunction/@name"/>
										</xsl:apply-templates>	
									</xsl:otherwise>
								</xsl:choose>
								<div style="clear:both"/>
								
								<xsl:if test="@maxCard&gt;1">
								
									<input type="button" class="btnAdd" onclick="addSection(this)" value="+"/>
									<input type="button" class ="btnRemove" onclick="removeSection(this)" value="-">
										<xsl:attribute name = "style">
											<xsl:value-of select="'visibility:hidden;'"/>
										</xsl:attribute>
									</input>
								</xsl:if>
								
								
							</td>
						</tr>
					</table>
				</div>
			
			</xsl:if>
		<!--</xsl:if>-->
	</xsl:template>
	
	
	<!--SRB: Button Action-->
	<xsl:template match="x:ButtonAction">
	
	    
			 <xsl:param name="action"/>
			 <xsl:param name="name"/>
			 
			  <xsl:if test="@ID='BA1'">
				<xsl:variable name="url" select="x:OnClick//x:Actions/x:CallFunction/x:FunctionURI[1]/@val"/>
			 
			
				 <!--Clin or Path-->
				 
				 <xsl:variable name="TxApproach" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='Tx'][1]/@sourceQuestionName"/>
				 
				 <xsl:variable name="cT" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='cT'][1]/@sourceQuestionName"/>
				 <xsl:variable name="cN" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='cN'][1]/@sourceQuestionName"/>
				 <xsl:variable name="cM" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='cM'][1]/@sourceQuestionName"/>
				 <xsl:variable name="Grade" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='Grade'][1]/@sourceQuestionName"/>
				 <xsl:variable name="ER" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='ER'][1]/@sourceQuestionName"/>
				 <xsl:variable name="PR" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='PR'][1]/@sourceQuestionName"/>
				 <xsl:variable name="HER2.IHC" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='HER2.IHC'][1]/@sourceQuestionName"/>
				 <xsl:variable name="HER2.ISH" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='HER2.ISH'][1]/@sourceQuestionName"/>
				 <xsl:variable name="OncDx" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='OncDx'][1]/@sourceQuestionName"/>
				 
				 
				 
				 
				 <xsl:variable name="pT" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='pT'][1]/@sourceQuestionName"/>
				 <xsl:variable name="pN" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='pN'][1]/@sourceQuestionName"/>
				 <xsl:variable name="pM" select="x:OnClick//x:Actions/x:CallFunction/x:ListItemParameterRef[@paramName='pM'][1]/@sourceQuestionName"/>
			 
				
				 <xsl:variable name="AttributeEvalParam1NameValue" select="concat(name(//x:ConditionalGroupAction/x:AttributeEval/@*[1]),'|',//x:ConditionalGroupAction/x:AttributeEval/@*[1])"/>
								 
				 <xsl:variable name="AttributeEvalParam2NameValue" select="concat(name(//x:ConditionalGroupAction/x:AttributeEval/@*[2]),'|',//x:ConditionalGroupAction/x:AttributeEval/@*[2])"/>
				 				 
				
				
				 <input type="button" onclick="{x:OnClick//x:Actions/x:CallFunction[1]/@name}(
				 '{$url}',
				 '{$TxApproach}',
				 '{$cT}',
				 '{$cN}',
				 '{$cM}',
				 '{$Grade}',
				 '{$ER}',
				 '{$PR}',
				 '{$HER2.IHC}',
				 '{$HER2.ISH}',
				 '{$OncDx}'
				 );
				 {x:OnClick/x:Actions/x:ConditionalGroupAction/x:AttributeEval/x:Actions/x:CallFunction[1]/@name}(
				 '{$url}',
				 '{$TxApproach}',
				 '{$pT}',
				 '{$pN}',
				 '{$pM}',
				 '{$Grade}',
				 '{$ER}',
				 '{$PR}',
				 '{$HER2.IHC}',
				 '{$HER2.ISH}',
				 '{$OncDx}',
				 '{$AttributeEvalParam1NameValue}',
				 '{$AttributeEvalParam2NameValue}'
				 )
				 " value="@title">
					<xsl:attribute name = "value">
						<xsl:value-of select="@title"/>
					</xsl:attribute>
					
				</input>
		 
			</xsl:if>
			
			<xsl:if test="@ID='SubmitInitClinAssess'">
			    
				<input type="button" onclick="{x:OnClick/x:Actions/x:CallFunction/x:LocalFunctionName/@val}('hello')
				 " value="@title">
					<xsl:attribute name = "value">
						<xsl:value-of select="@title"/>
					</xsl:attribute>
					
				</input>
			</xsl:if>
			
			<xsl:if test="@ID='SubmitPathReport'">
			    <xsl:variable name="ElementName" select = "x:OnClick/x:Actions/x:SetAttributeValue[1]/@targetNames"/>
				<xsl:variable name="EnableValue" select = "x:OnClick/x:Actions/x:SetAttributeValue[1]/@actEnable"/>
				<!--<xsl:variable name="SetValue" select = "x:OnClick/x:Actions/x:SetAttributeValue/@actSetVal"/>-->
				
				<input type="button" onclick="EnableDisableElementByName('{$ElementName}','{$EnableValue}');{x:OnClick/x:Actions/x:CallFunction/x:LocalFunctionName/@val}('hello');"				                              
				  value="@title">
					<xsl:attribute name = "value">
						<xsl:value-of select="@title"/>
					</xsl:attribute>
					
				</input>
				
			</xsl:if>
			
			<xsl:if test="@ID='SubmitForm'">
				<input type="button" onclick="{x:OnClick/x:Actions/x:CallFunction/x:LocalFunctionName/@val}('hello')
				 " value="@title">
					<xsl:attribute name = "value">
						<xsl:value-of select="@title"/>
					</xsl:attribute>
					
				</input>
			</xsl:if>
			
	</xsl:template>
	
	<!--section within a list item -->
	<xsl:template match="x:Section" mode="level2">
		<xsl:param name="parentSectionId"/>	
		<xsl:param name="defaultStyle"/>		
		<xsl:if test="not (@visible) or (@visible='true')">
			<xsl:variable name="required" select="true"/>
			<xsl:variable name="style" select="@styleClass"/>
			<!--<xsl:variable name="defaultStyle" select="'TopHeader2'"/>-->
			<xsl:variable name="sectionId" select="concat('s',@ID)"/>
			
			<div class="section_wthin_list"> 				
				<xsl:attribute name="id">
					<xsl:value-of select="$sectionId"/>					
				</xsl:attribute>
				<xsl:if test="@mustImplement='false'">
					<div class='mustImplement'></div>
				</xsl:if>
				<input id = "maxcardinality" type="hidden">
					<xsl:attribute name="value">
						<xsl:value-of select="@maxCard"/>
					</xsl:attribute>					
				</input>
				
				
				<!-- table is repeated if cardinality is greater than 1 and id value will be incremented-->
				<table class="HeaderTableChild" align="center">					
					<xsl:attribute name="id">
						<xsl:value-of select="$sectionId"/>
					</xsl:attribute>
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="$style!=''">
									<div class="{$style}">	
										<!-- new code -->
										<xsl:call-template name="ItemWithID">
										   <xsl:with-param name="required" select="required"/>
										</xsl:call-template>
										
										<xsl:value-of select="@title"/>
										<div style="display:inline" class="MetadataDisplay">
											<!---metadata-->
										</div>
									</div>
								</xsl:when>
								<xsl:otherwise>
									<div class="{$defaultStyle} collapsable">
										<!-- new code -->
										<xsl:call-template name="ItemWithID">
										   <xsl:with-param name="required" select="required"/>
										</xsl:call-template>
										
										<xsl:value-of select="@title"/>
										<div style="display:inline" class="MetadataDisplay">
											<!---metadata-->
										</div>
									</div>
								</xsl:otherwise>
							</xsl:choose>
							<!--show link here?-->
							<xsl:for-each select="x:Link">					
								<xsl:call-template name="handle_link"/>
							</xsl:for-each>

							<xsl:choose>
								<xsl:when test="$required='false'">
									
								</xsl:when>
								<xsl:otherwise>	
									<xsl:apply-templates select="x:ChildItems/x:Question | x:ChildItems/x:Section" mode="level2" >
										<xsl:with-param name="required" select="'true'"/>
										<xsl:with-param name="parentSectionId" select="$sectionId"/>
										<xsl:with-param name="defaultStyle" select="$defaultStyle"/>
									</xsl:apply-templates>
									
								</xsl:otherwise>
							</xsl:choose>
							<div style="clear:both"/>
							<xsl:if test="@maxCard&gt;1">
							
								<input type="button" class="btnAdd" onclick="addQuestion(this)" value="+"/>
								<input type="button" class ="btnRemove" onclick="removeQuestion(this)" value="-">
									<xsl:attribute name = "style">
										<xsl:value-of select="'visibility:hidden;'"/>
									</xsl:attribute>
								</input>
							</xsl:if>
						</td>
					</tr>
				</table>
			</div>
			
		</xsl:if>
	</xsl:template>
	
	<!--question in section-->
	<xsl:template match="x:Question" mode="level1">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>
			<div class='question'>
				<xsl:attribute name="id">
					<xsl:value-of select="$questionId"/>	
				</xsl:attribute>
				
				<input type="hidden" class="TextBox">
					<xsl:attribute name="name">
						<xsl:value-of select="$questionId"/>
					</xsl:attribute>				
					<xsl:attribute name="value">
						<xsl:value-of select="@title"/>
					</xsl:attribute>
				</input>
				<input id = "maxcardinality" type="hidden">
					<xsl:attribute name="value">
						<xsl:value-of select="@maxCard"/>
					</xsl:attribute>					
				</input>
				<div class="QuestionInSection">   <!--two columns-->
					<xsl:if test="@mustImplement='false'">
						<div class='mustImplement'></div>
					</xsl:if>
					<div class="QuestionTitle collapsable_q">
					
						<!-- new code -->
						<xsl:call-template name="ItemWithID">
						   <xsl:with-param name="required" select="required"/>
						</xsl:call-template>
						
						<xsl:value-of select="@title"/> 
						
						<div style="display:inline" class="metadata">
							<!---metadata-->
						</div>
						
						<!-- new code placement -->
						<xsl:call-template name="QuestionSectionAttributes"/>
						
						
						
						
						
						<!--show link here?-->
						<xsl:for-each select="x:Link">					
							<xsl:call-template name="handle_link"/>
						</xsl:for-each>

					</div>
					<xsl:if test="not(x:ResponseField) and not(@readOnly)">
						<a class="QuestionReset">
							<xsl:attribute name="href">
								#
							</xsl:attribute>
							<xsl:attribute name="onclick">
								javascript:resetAnswer('<xsl:value-of select="substring($questionId,2)"/>',event);return false;
							</xsl:attribute>
							<xsl:text>(reset)</xsl:text>
						</a>
					</xsl:if>
					<xsl:if test="x:ResponseField">
						<xsl:call-template name="ResponseFieldAttributes"/>
						<input type="text" class="TextBox">
							<xsl:attribute name="name">
								<xsl:value-of select="substring($questionId,2)"/>
							</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="x:ResponseField/x:Response//@val"/>
							</xsl:attribute>
						</input>
					</xsl:if>
					<div class='command'>
						<xsl:if test="@maxCard&gt;1">				
							<input type="button" class="btnAdd" onclick="addQuestion(this)" value="+"/>
							<input type="button" class ="btnRemove" onclick="removeQuestion(this)" value="-">
								<xsl:attribute name = "style">
									<xsl:value-of select="'visibility:hidden;'"/>
								</xsl:attribute>
							</input>
						</xsl:if>
					</div>
						
					<div style="clear:both;"/>
					
					<xsl:if test="x:ListField">
					  <xsl:apply-templates select="x:ListField" mode="level1">
						  <xsl:with-param name="questionId" select="$questionId" />
						  <xsl:with-param name="parentSectionId" select="$parentSectionId" />
					  </xsl:apply-templates>
					</xsl:if>
					
					<!--11/13/2016: question within question-->
					<div style="clear:both;"/>
					<xsl:if test="x:ChildItems/x:Question">	
					 
						<xsl:apply-templates select="x:ChildItems/x:Question" mode="level3">
							<xsl:with-param name="parentSectionId" select="$parentSectionId" />
						</xsl:apply-templates>
					</xsl:if>
					
				</div>
		</div>
		
	</xsl:template>

	
	<!--question in list item-->
<xsl:template match="x:Question" mode="level2">
	<xsl:param name="parentSectionId"/>
	<xsl:variable name="questionId" select="concat('q',@ID)"/>
	<div class='question'>
		<xsl:attribute name="id">
			<xsl:value-of select="$questionId"/>						
		</xsl:attribute>
		<input type="hidden" class="TextBox">
			<xsl:attribute name="name">
				<xsl:value-of select="$questionId"/>
			</xsl:attribute>
			<xsl:attribute name="value">
				<xsl:value-of select="@title"/>
			</xsl:attribute>
		</input>
		<input id = "maxcardinality" type="hidden">
			<xsl:attribute name="value">
				<xsl:value-of select="@maxCard"/>
			</xsl:attribute>					
		</input>
		<div class="QuestionInListItem"> 	 
			<xsl:if test="@mustImplement='false'">
				<div class='mustImplement'></div>
			</xsl:if>
			<xsl:choose>
				<!--not showing the hidden question-->
				<xsl:when test="string-length(@title)&gt;0">
					<div class="QuestionTitle collapsable_q">
						<div style="display:inline" class="idDisplay">
							<xsl:value-of select="substring-before(@ID, '.')"/> -
						</div>
						
						<xsl:value-of select="@title"/>
						 
						<div style="display:inline" class="MetadataDisplay">			
							<!---metadata-->
							<!---<xsl:if test="x:Property/@propName!=''">
								<div class="reportText">
								<xsl:value-of select="x:Property/@propName"/> : 
							</div>
							<div class="reportTextValue">
								<xsl:value-of select="x:Property/@val"/> 
							</div>
							</xsl:if>-->
						</div>
						
					
						<!-- new code placement -->
						<xsl:call-template name="QuestionSectionAttributes"/>
						
						
						<xsl:if test="x:ResponseField">
							<input type="text" class="TextBox">
								<xsl:attribute name="name">
									<xsl:value-of select="substring($questionId,2)"/> <!--drop q-->
								</xsl:attribute>
								<xsl:attribute name="value">
									<xsl:value-of select="x:ResponseField/x:Response//@val"/>
								</xsl:attribute>
							</input>
						</xsl:if>
					
					</div>
					<xsl:if test="not(x:ResponseField) and not(@readOnly)">
						<a class="QuestionReset">
							<xsl:attribute name="href">
								#
							</xsl:attribute>
							<xsl:attribute name="onclick">
								javascript:resetAnswer('<xsl:value-of select="substring($questionId,2)"/>',event);return false;
							</xsl:attribute>
							<xsl:text>(reset)</xsl:text>
						</a>
					</xsl:if>
				</xsl:when>
				<xsl:otherwise>
					<div class="QuestionTitle collapsable_q">
						<!-- new code -->
						<xsl:call-template name="ItemWithID">
						   <xsl:with-param name="required" select="required"/>
						</xsl:call-template>
						
						<div style="display:inline" class="MetadataDisplay">
							<!---metadata-->
							<xsl:if test="x:Property/@propName!=''">
							<div class="reportText">
							<xsl:value-of select="x:Property/@propName"/> : 
							</div>
							<div class="reportTextValue">
							<xsl:value-of select="x:Property/@val"/> 
							</div>
							</xsl:if>
						</div>
					</div>
					<xsl:if test="not(x:ResponseField) and not(@readOnly)">
						<a class="QuestionReset">
							<xsl:attribute name="href">
								#
							</xsl:attribute>
							<xsl:attribute name="onclick">
								javascript:resetAnswer('<xsl:value-of select="substring($questionId,2)"/>',event);return false;
							</xsl:attribute>
							<xsl:text>(reset)</xsl:text>
						</a>
					</xsl:if>
					<!--reset for hidden field-->
					
					
					<xsl:if test="x:ResponseField">
						<input type="text" class="TextBox">
							<xsl:attribute name="name">
								<xsl:value-of select="substring($questionId,2)"/>
							</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="x:ResponseField/x:Response//@val"/>
							</xsl:attribute>
						</input>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>		
		
			<div class='command'>
				<xsl:if test="@maxCard&gt;1">				
					<input type="button" class="btnAdd" onclick="addQuestion(this)" value="+"/>
					<input type="button" class ="btnRemove" onclick="removeQuestion(this)" value="-">
						<xsl:attribute name = "style">
							<xsl:value-of select="'visibility:hidden;'"/>
						</xsl:attribute>
					</input>
				</xsl:if>
			</div>
					
			<div style="clear:both;"/>
			<xsl:if test="x:ListField">
				<xsl:apply-templates select="x:ListField" mode="level1">
					<xsl:with-param name="questionId" select="$questionId" />
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
		
			<!--11/13/2016: question within question-->
			<!--<xsl:if test="x:ChildItems/x:Section">			-->
				<xsl:apply-templates select="x:ChildItems/x:Section | x:ChildItems/x:Question" mode="level2">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
					<xsl:with-param name="defaultStyle" select="'TopHeader2'"/>
				</xsl:apply-templates>
			<!--</xsl:if>-->
		
		</div>
	</div>
</xsl:template>

	<!--question in question-->
	<xsl:template match="x:Question" mode="level3">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>	
		<div class='question'>
		<xsl:attribute name="id">
			<xsl:value-of select="$questionId"/>						
		</xsl:attribute>
		<input type="hidden" class="TextBox">
			<xsl:attribute name="name">
				<xsl:value-of select="$questionId"/>
			</xsl:attribute>				
			<xsl:attribute name="value">
				<xsl:value-of select="@title"/>
			</xsl:attribute>
		</input>
		
		<input id = "maxcardinality" type="hidden">
			<xsl:attribute name="value">
				<xsl:value-of select="@maxCard"/>
			</xsl:attribute>					
		</input>
		
		<div class="QuestionInQuestion"> 
			<xsl:if test="@mustImplement='false'">
				<div class='mustImplement'></div>
			</xsl:if>		
			<div class="QuestionTitle collapsable_q">
				<!-- new code -->
				<xsl:call-template name="ItemWithID">
				   <xsl:with-param name="required" select="required"/>
				</xsl:call-template>
						
				<xsl:value-of select="@title"/>
				<div style="display:inline" class="MetadataDisplay">
					<!---metadata-->
					<xsl:if test="x:Property/@propName!=''">
						<div class="reportText">
							<xsl:value-of select="x:Property/@propName"/> : 
						</div>
						<div class="reportTextValue">
							<xsl:value-of select="x:Property/@val"/> 
						</div>
					</xsl:if>
				</div>
				
				<!-- new code placement -->
				<xsl:call-template name="QuestionSectionAttributes"/>
						
						
				<xsl:if test="x:ResponseField"> 
					<input type="text" class="TextBox">
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="x:ResponseField/x:Response//@val"/>
							<!--<xsl:value-of select="substring($expandedId,2)"/>-->
						</xsl:attribute>
					</input>
				</xsl:if>
				
				
				
				<!--show link here?-->
				<xsl:for-each select="x:Link">					
					<xsl:call-template name="handle_link"/>
				</xsl:for-each>

			</div>
			<xsl:if test="not(x:ResponseField) and not(@readOnly)">
				<a class="QuestionReset">
					<xsl:attribute name="href">
						#
					</xsl:attribute>
					<xsl:attribute name="onclick">
						javascript:resetAnswer('<xsl:value-of select="substring($questionId,2)"/>',event);return false;
					</xsl:attribute>
					<xsl:text>(reset)</xsl:text>
				</a>
			</xsl:if>		
			<div style="clear:both;"/>
			
			<xsl:if test="x:ListField">
				<xsl:apply-templates select="x:ListField" mode="level1">
					<xsl:with-param name="questionId" select="$questionId" />
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
			<div class='command'>
				<xsl:if test="@maxCard&gt;1">				
					<input type="button" class="btnAdd" onclick="addQuestion(this)" value="+"/>
					<input type="button" class ="btnRemove" onclick="removeQuestion(this)" value="-">
						<xsl:attribute name = "style">
							<xsl:value-of select="'visibility:hidden;'"/>
						</xsl:attribute>
					</input>
				</xsl:if>
			</div>
				
			<!--11/13/2016: question within question-->
			<div style="clear:both;"/>
			<xsl:if test="x:ChildItems/x:Question">				
				<xsl:apply-templates select="x:ChildItems/x:Question" mode="level3">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
		</div>
		</div>
	</xsl:template>
	
	
	<!--question in body-->
	<xsl:template match="x:Question" mode="level5">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>	
		<div class='question'>
		<xsl:attribute name="id">
			<xsl:value-of select="$questionId"/>						
		</xsl:attribute>
		
		<!-- new code -->
		<xsl:call-template name="ItemWithID">
		   <xsl:with-param name="required" select="required"/>
		</xsl:call-template>
		
		<input type="hidden" class="TextBox">
			<xsl:attribute name="name">
				<xsl:value-of select="$questionId"/>
			</xsl:attribute>				
			<xsl:attribute name="value">
				<xsl:value-of select="@title"/>
			</xsl:attribute>
		</input>
		
		<input id = "maxcardinality" type="hidden">
			<xsl:attribute name="value">
				<xsl:value-of select="@maxCard"/>
			</xsl:attribute>					
		</input>
		
		<div class="QuestionInQuestion">  
            <xsl:if test="@mustImplement='false'">
				<div class='mustImplement'></div>
			</xsl:if>		
			<div class="QuestionTitle collapsable_q">
				<!-- new code -->
				<xsl:call-template name="ItemWithID">
				   <xsl:with-param name="required" select="required"/>
				</xsl:call-template>
				
				<xsl:value-of select="@title"/>
				<div style="display:inline" class="MetadataDisplay">
					<!---metadata-->
					<xsl:if test="x:Property/@propName!=''">
						<div class="reportText">
							<xsl:value-of select="x:Property/@propName"/> : 
						</div>
						<div class="reportTextValue">
							<xsl:value-of select="x:Property/@val"/> 
						</div>
					</xsl:if>
				</div>
							
				<xsl:if test="x:ResponseField"> 
					<input type="text" class="TextBox">
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="x:ResponseField/x:Response//@val"/>
							<!--<xsl:value-of select="substring($expandedId,2)"/>-->
						</xsl:attribute>
					</input>
				</xsl:if>
				
				
				<!--show link here?-->
				<xsl:for-each select="x:Link">					
					<xsl:call-template name="handle_link"/>
				</xsl:for-each>

			</div>
			<xsl:if test="not(x:ResponseField) and not(@readOnly)">
				<a class="QuestionReset">
					<xsl:attribute name="href">
						#
					</xsl:attribute>
					
					<xsl:attribute name="onclick">
						javascript:resetAnswer('<xsl:value-of select="substring($questionId,2)"/>',event);return false;
					</xsl:attribute>
					<xsl:text>(reset)</xsl:text>
				</a>
			</xsl:if>	
			<div class='command'>
				<xsl:if test="@maxCard&gt;1">				
					<input type="button" class="btnAdd" onclick="addQuestion(this)" value="+"/>
					<input type="button" class ="btnRemove" onclick="removeQuestion(this)" value="-">
						<xsl:attribute name = "style">
							<xsl:value-of select="'visibility:hidden;'"/>
						</xsl:attribute>
					</input>
				</xsl:if>
			</div>
				
			<div style="clear:both;"/>
			
			<xsl:if test="x:ListField">
				<xsl:apply-templates select="x:ListField" mode="level1">
					<xsl:with-param name="questionId" select="$questionId" />
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
			<!--11/13/2016: question within question-->
			<div style="clear:both;"/>
			<xsl:if test="x:ChildItems/x:Question">				
				<xsl:apply-templates select="x:ChildItems/x:Question" mode="level3">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
		</div>
		</div>
	</xsl:template>

<xsl:template match="x:ListField" mode="level1">
   <xsl:param name="questionId" />
   <xsl:param name="parentSectionId" />  
	<xsl:choose>
		<!--single select-->
		<xsl:when test="@maxSelections='1' or not (@maxSelections)" >
			<xsl:apply-templates select= "x:List/x:ListItem|x:List/x:DisplayedItem" mode="singleselect">
				<xsl:with-param  name="questionId" select ="$questionId"/>
				<xsl:with-param  name="parentSectionId" select ="$parentSectionId"/>	
			</xsl:apply-templates>			
		</xsl:when>	
		<!--multi select-->
		<xsl:otherwise>			
			<xsl:apply-templates select= "x:List/x:ListItem|x:List/x:DisplayedItem" mode="multiselect">
				<xsl:with-param  name="questionId" select ="$questionId"/>
				<xsl:with-param  name="parentSectionId" select ="$parentSectionId"/>	
			</xsl:apply-templates>			
		</xsl:otherwise>		
	</xsl:choose>
</xsl:template>
	
	<xsl:template match="x:ListItem" mode="singleselect">
		<xsl:param name="questionId" />
		<xsl:param name="parentSectionId" />  		
			<div class="Answer">
				<xsl:if test="@mustImplement='false'">
					<div class='mustImplement'></div>
				</xsl:if>
				<input type="radio" style="float:left">
					<xsl:attribute name="name">
						<xsl:value-of select="substring($questionId,2)"/>
					</xsl:attribute>
					<xsl:if test="@selected='true'">
						<xsl:attribute name="checked">
						</xsl:attribute>
					</xsl:if>					
					<xsl:attribute name="value">
						<xsl:value-of select="@ID"/>,<xsl:value-of select="@title"/>
					</xsl:attribute>
					<xsl:attribute name="onchange">	
                        <xsl:if test="x:OnSelect">				    
							javascript:SelectUnselectTarget('<xsl:value-of select="x:OnSelect/x:Actions/x:SetAttributeValue/@targetNames"/>', 
															'<xsl:value-of select="x:OnSelect/x:Actions/x:SetAttributeValue/@actSelect"/>');						
						</xsl:if>
						javascript:SelectUnselectParents('<xsl:value-of select="$questionId" />', this);					
					
					</xsl:attribute>
					<xsl:attribute name="onblur">
						javascript:OnChoiceBlur('<xsl:value-of select="@ID" />', this)
					</xsl:attribute>
					
				</input> 
				<!-- new code -->
				<xsl:call-template name="ItemWithID">
				   <xsl:with-param name="required" select="required"/>
				</xsl:call-template>
				
				<xsl:value-of select="@title"/>
				<!--show link here?-->
				
				<xsl:for-each select="x:Link">	
				
					<xsl:call-template name="handle_link"/>
				</xsl:for-each>
		
				<!-- new code placement -->
				<xsl:call-template name="ListItemAttributes"/>
				<xsl:call-template name="CommonAttributes"/>
				
				<div style="display:inline" class="MetadataDisplay">
					<!---metadata-->
				</div>
				<!--answer fillin-->
				<xsl:if test="x:ListItemResponseField">
					<xsl:call-template name="ResponseFieldAttributes"/>
					<input type="text" class="AnswerTextBox">
						<xsl:attribute name="width">
							<xsl:value-of select="100"/>
						</xsl:attribute>
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="x:ListItemResponseField/x:Response//@val"/>
						</xsl:attribute>
						
						<xsl:if test="x:ListItemResponseField/x:AfterChange//x:Actions/x:SetAttributeValue/@actSetValFromRef">
							<xsl:attribute name="onchange">							
								javascript:SetValueOnTextChange('<xsl:value-of select="x:ListItemResponseField/x:AfterChange/x:Actions/x:SetAttributeValue/@targetNames"/>', 
								                                '<xsl:value-of select="x:ListItemResponseField/x:AfterChange/x:Actions/x:SetAttributeValue/@actSetValFromRef"/>',
																'<xsl:value-of select="concat(name(x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[1]),
																':',x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[1],',',name(x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[2]),
																':',x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[2])"/>');	
												
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="x:ListItemResponseField/x:AfterChange//x:Actions/x:SetAttributeValue/@actSelect">
						     
							<xsl:attribute name="onchange">	
							
								javascript:SelectUnselectTarget('<xsl:value-of select="x:ListItemResponseField/x:AfterChange//x:Actions/x:SetAttributeValue/@targetNames"/>', 
																'<xsl:value-of select="x:ListItemResponseField/x:AfterChange//x:Actions/x:SetAttributeValue/@actSelect"/>',
                                                                '<xsl:value-of select="concat('{',name(x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[1]),
																':',x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[1],',',name(x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[2]),
																':',x:ListItemResponseField/x:AfterChange/x:AttributeEval/@*[2],'}')"/>',
																'<xsl:value-of select="concat('{', x:ListItemResponseField/x:AfterChange//x:Else/x:Actions/x:SetAttributeValue/@targetNames, ':', x:ListItemResponseField/x:AfterChange//x:Else/x:Actions/x:SetAttributeValue/@actSelect,'}')"/>');
																																
																
												
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="onkeydown">
							javascript:SelectChoiceOnKeyPress('<xsl:value-of select="@ID" />', this, event)
						</xsl:attribute>
						<xsl:attribute name="onblur">
							javascript:SelectUnselectChoiceOnBlur('<xsl:value-of select="@ID" />', this)
						</xsl:attribute>
						
					</input>
					
					&#xA0;<xsl:value-of select="x:ListItemResponseField/x:TextAfterResponse/@val"/>
					<!--&#xA0;<xsl:value-of select="x:ListItemResponseField/x:ResponseUnits/@val"/>-->
				</xsl:if>
			</div>
			
			<!--property-->
		<!--<xsl:if test="x:Property">
				<div class="property">
					<xsl:value-of select="@name"/>
					<xsl:value-of select="@type"/>
					<xsl:value-of select="@val"/>
				</div>
			</xsl:if>			
			-->
			<!--SRB: 12/18/2016 - handle section within listitem -->
			<xsl:apply-templates select="x:ChildItems/x:Section" mode="level2">
				<xsl:with-param name="parentSectionId" select="$parentSectionId" />
			</xsl:apply-templates>
			<!--question within list-->
			<xsl:apply-templates select="x:ChildItems/x:Question" mode="level2">
				<xsl:with-param name="parentSectionId" select="$parentSectionId" />
			</xsl:apply-templates>	
		<xsl:apply-templates select="x:ChildItems/x:DisplayedItem" mode="singleselect">
			<xsl:with-param name="parentSectionId" select="$parentSectionId" />
		</xsl:apply-templates>	
			
	</xsl:template>
	
	<xsl:template name="handle_link">
	
		<!--show link here?-->
		
			&#160;
			<xsl:element name="a">
				<xsl:attribute name="href">
					<xsl:value-of select="x:LinkURI/@val"/>
				</xsl:attribute>
				<xsl:attribute name="target">
					_blank
				</xsl:attribute>
				<xsl:attribute name="title">
					<xsl:value-of select="x:LinkText/x:Property[@propClass='tooltip']/@val"/>							
				</xsl:attribute>
				<xsl:value-of select="x:LinkText/@val"/>
			</xsl:element>
		
	</xsl:template>
	
	<xsl:template match="x:ListItem" mode="multiselect">
		<xsl:param name="questionId" />
		<xsl:param name="parentSectionId" />  		
			<div class="Answer">
				<xsl:if test="@mustImplement='false'">
					<div class='mustImplement'></div>
				</xsl:if>
				<input type="checkbox" style="float:left;">
					<xsl:attribute name="name">
						<xsl:value-of select="substring($questionId,2)"/>
					</xsl:attribute>
					<xsl:attribute name="value">
						<xsl:value-of select="@ID"/>,<xsl:value-of select="@title"/>
					</xsl:attribute>
					<xsl:if test="@selected='true'">
						<xsl:attribute name="checked">
						</xsl:attribute>
					</xsl:if>
					<xsl:attribute name="onchange">
						javascript:SelectUnselectParents('<xsl:value-of select="$questionId" />', this)
					</xsl:attribute>
					<xsl:attribute name="onblur">
						javascript:OnChoiceBlur('<xsl:value-of select="@ID" />', this)
					</xsl:attribute>
				</input>
				<!-- new code -->
				<xsl:call-template name="ItemWithID">
				   <xsl:with-param name="required" select="required"/>
				</xsl:call-template>
				
				<xsl:value-of select="@title"/>
				<!--show link here?-->
				
				<xsl:for-each select="x:Link">					
					<xsl:call-template name="handle_link"/>
				</xsl:for-each>
				
				<!-- new code placement, want to inline this information inside Answer -->
				<xsl:call-template name="ListItemAttributes"/>
				<xsl:call-template name="CommonAttributes"/>
				
				<div style="display:inline" class="MetadataDisplay">
					<!---metadata-->
				</div>
				<xsl:if test="x:ListItemResponseField">
					<xsl:call-template name="ResponseFieldAttributes"/>
					<input type="text" class="AnswerTextBox">
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="x:ListItemResponseField/x:Response//@val"/>
						</xsl:attribute>
						
						<xsl:attribute name="onkeydown">
							javascript:SelectChoiceOnKeyPress('<xsl:value-of select="@ID" />', this, event)
						</xsl:attribute>
						<xsl:attribute name="onblur">
							javascript:SelectUnselectChoiceOnBlur('<xsl:value-of select="@ID" />', this)
						</xsl:attribute>
						
					</input>
				</xsl:if>
			</div>
			<!--SRB: 12/18/2016 - handle section within listitem -->
			<xsl:apply-templates select="x:ChildItems/x:Section" mode="level2">
				<xsl:with-param name="parentSectionId" select="$parentSectionId" />
			</xsl:apply-templates>
			<!--question within list-->
			<xsl:apply-templates select="x:ChildItems/x:Question" mode="level2">
				<xsl:with-param name="parentSectionId" select="$parentSectionId" />
			</xsl:apply-templates>
		<xsl:apply-templates select="x:ChildItems/x:DisplayedItem" mode="multiselect">
			<xsl:with-param name="parentSectionId" select="$parentSectionId" />
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="*|/" mode="DisplayedItem">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>
		<xsl:if test="x:DisplayedItem">
			<div> 
				<xsl:if test="@mustImplement='false'">
					<div class='mustImplement'></div>
				</xsl:if>	
				<div class="DisplayedItem">
					<xsl:value-of select="x:DisplayedItem/@title"/> 				
				</div>		
				
				<div style="clear:both;"/>			
			</div>	
		</xsl:if>			
	</xsl:template>
	
	<xsl:template match="x:DisplayedItem" mode="level1">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>
			<div>  
				<xsl:if test="@mustImplement='false'">
					<div class='mustImplement'></div>
				</xsl:if>
				<div class="NoteText">
					<!-- new code -->
					<xsl:call-template name="ItemWithID">
					   <xsl:with-param name="required" select="required"/>
					</xsl:call-template>
					
					<xsl:value-of select="@title"/> 
					<div style="display:inline" class="MetadataDisplay">
						<!---metadata-->
					</div>					
				</div>			
				<div style="clear:both;"/>			
			</div>	
					
	</xsl:template>
	
	<xsl:template match="x:DisplayedItem" mode="singleselect">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>
		<div>  
			<xsl:if test="@mustImplement='false'">
				<div class='mustImplement'></div>
			</xsl:if>
			<div class="ListNote" style="margin-left:15px;">
				<!-- new code -->
				<!---metadata-->
				<xsl:call-template name="ItemWithID">
				   <xsl:with-param name="required" select="required"/>
				</xsl:call-template>
				
				<xsl:value-of select="@title"/>
				  
				<div style="display:inline" class="MetadataDisplay">
					<xsl:if test="x:Property/@propName!=''">
						<div class="reportText">
							<xsl:value-of select="x:Property/@propName"/> : 
						</div>
						<div class="reportTextValue">
							<xsl:value-of select="x:Property/@val"/> 
						</div>
					</xsl:if>
					
				</div>
			</div>			
			<div style="clear:both;"/>			
		</div>		
	</xsl:template>
	
	
	<xsl:template match="x:DisplayedItem" mode='multiselect'>
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID)"/>
		
		<div>  
			<xsl:if test="@mustImplement='false'">
				<div class='mustImplement'></div>
			</xsl:if>
			<div class="ListNote" style="margin-left:15px;">
				<!-- new code -->
				<xsl:call-template name="ItemWithID">
				   <xsl:with-param name="required" select="required"/>
				</xsl:call-template>
				<xsl:value-of select="@title"/>   
				<div style="display:inline" class="MetadataDisplay">
					<!---metadata-->
					<xsl:if test="x:Property/@propName!=''">
						<div class="reportText">
							<xsl:value-of select="x:Property/@propName"/> : 
						</div>
						<div class="reportTextValue">
							<xsl:value-of select="x:Property/@val"/> 
						</div>
					</xsl:if>
				</div>
			</div>			
			<div style="clear:both;"/>			
		</div>
		
	</xsl:template>

	<!-- new code -->
	<xsl:template name="SectionAttributes">
		<xsl:param name="metadataClass" select="'MetadataDisplay'"/>
		
		 <xsl:if test="change/@change_type='newItem'">
			 <!--&#160;<span class="sdc">sdc* </span>			 -->
			 <xsl:apply-templates select="change" mode="label">
				<xsl:with-param name="changeAttribute" select="'newItem'"/>
				<xsl:with-param name="label" select="'new*'"/>
				<xsl:with-param name="tooltip" select="concat('NEW ITEM ADDED. New: ', change/@change_type)"/>
				
			</xsl:apply-templates>
		 </xsl:if>
		 
		 <xsl:if test="change/@change_type='parent'">
			 <xsl:apply-templates select="change" mode="label">
				<xsl:with-param name="changeAttribute" select="'parent'"/>
				<xsl:with-param name="label" select="'parent*'"/>
				<xsl:with-param name="tooltip" select="concat('NEW PARENT ITEM:: ', change/@change_type)"/>
			 </xsl:apply-templates>
		 </xsl:if>
				
		 <xsl:if test="change/@change_type='prevSibling'">
			 <xsl:apply-templates select="change" mode="label">
				<xsl:with-param name="changeAttribute" select="'prevSibling'"/>
				<xsl:with-param name="label" select="'prevSibling*'"/>
				<xsl:with-param name="tooltip" select="concat('NEW PREV SIBLING:: ', change/@change_type)"/>
			 </xsl:apply-templates>
		 </xsl:if>
	</xsl:template>
	
	<!-- new code -->
	<xsl:template name="HeaderSectionAttributes">
      <xsl:param name="metadataClass" select="'MetadataDisplay'"/>
	
		<xsl:if test="$change-display = 'true'">
			<xsl:if test="change/@change_type='templateTitle'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'templateTitle'"/>
					<xsl:with-param name="label" select="'templateTitle*'"/>
					<xsl:with-param name="tooltip" select="concat('new: ', change/@templateTitle-new,', old: ', change/@templateTitle-old)"/>
				</xsl:apply-templates>
			</xsl:if>
			
			<xsl:if test="change/@change_type='figo'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'figo'"/>
					<xsl:with-param name="label" select="'figo*'"/>
					<xsl:with-param name="tooltip" select="concat('new: ', change/@figo-new,', old: ', change/@figo-old)"/>
				</xsl:apply-templates>
			</xsl:if>	
			
			<xsl:if test="change/@change_type='cs'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'cs'"/>
					<xsl:with-param name="label" select="'cs*'"/>
					<xsl:with-param name="tooltip" select="concat('new: ', change/@cs-new,', old: ', change/@cs-old)"/>
				</xsl:apply-templates>
			</xsl:if>	
		</xsl:if>
	</xsl:template>
	
	<!-- new code -->
	<xsl:template name="QuestionSectionAttributes">
      <xsl:if test="$metadata-display = 'true'">
         <div class="MetadataDisplay">
            <xsl:if test="string(@locked) = 'true'">
               &#160;<div class="locked">locked</div>
            </xsl:if>
            <xsl:if test="string(@minCard) != ''">
               &#160;<div class="minCard">min: </div>
               <xsl:value-of select="@minCard"/>
            </xsl:if>
            <xsl:if test="string(@maxCard) != ''">
               &#160;<div class="maxCard">max: </div>:
               <xsl:value-of select="@maxCard"/>
            </xsl:if>
            <!--<xsl:call-template name="CommonAttributes">
               <xsl:with-param name="metadataClass" select="$metadataClass"/>
            </xsl:call-template>-->
         </div>
      </xsl:if>
	
      
	
         <xsl:call-template name="CommonAttributes"/>
				   <!-- <xsl:with-param name="metadataClass" select="$metadataClass"/> -->
		 <!-- </xsl:call-template> -->
      
   </xsl:template>
   
   <!-- new code -->
   <xsl:template name="ResponseFieldAttributes">
      <xsl:if test="$metadata-display = 'true'">
         <div class="MetadataDisplay">
            <xsl:if test="string(@datatype) != ''">
               &#160;<div class="dataType">dt: </div>
               <xsl:value-of select="@datatype"/>
            </xsl:if>
			
            <xsl:if test="string(x:ListItemResponseField/x:ResponseUnits/@val) != ''">
			<!-- GOT HERE LI-1			 -->
				<!-- <xsl:value-of select ="local-name()"/> -->
               &#160;<div class="ResponseUnits">un: </div>
               <xsl:value-of select="x:ListItemResponseField/x:ResponseUnits/@val"/>
            </xsl:if>
            <xsl:if test="string(x:ListItemResponseField/x:TextAfterResponse/@val) != ''">
			<!-- GOT HERE LI-2 -->
               &#160;<div class="TextAfterResponse">txtAft: </div>
               <xsl:value-of select="x:ListItemResponseField/x:TextAfterResponse/@val"/>
            </xsl:if>
            <xsl:if test="string(x:ListItemResponseField/@responseRequired) = 'true'">
			<!-- GOT HERE LI-3 -->
               &#160;<div class="responseRequired">respReq: </div>
			   <xsl:value-of select="x:ListItemResponseField/@responseRequired"/>
            </xsl:if>  

            <xsl:if test="string(x:ResponseField/x:ResponseUnits/@val) != ''">
			<!-- GOT HERE R-1 -->
               &#160;<div class="ResponseUnits">un: </div>
               <xsl:value-of select="x:ResponseField/x:ResponseUnits/@val"/>
            </xsl:if>
            <xsl:if test="string(x:ResponseField/x:TextAfterResponse/@val) != ''">
			<!-- GOT HERE R-2 -->
               &#160;<div class="TextAfterResponse">txtAft: </div>
               <xsl:value-of select="x:ResponseField/x:TextAfterResponse/@val"/>
            </xsl:if>
            <xsl:if test="string(x:ResponseField/@responseRequired) = 'true'">
			<!-- GOT HERE R-3 -->
               &#160;<div class="responseRequired">respReq: </div>
			   <xsl:value-of select="x:ResponseField/@responseRequired"/>
            </xsl:if>			
         </div>
      </xsl:if>
   <xsl:if test="$change-display = 'true'">
         <div class="ChangeDisplay">      
            <xsl:if test="change/@change_type='dataType'">
				<xsl:apply-templates select="change" mode="label">
				   <xsl:with-param name="changeAttribute" select="'dataType'"/>
				   <xsl:with-param name="label" select="'dt*'"/>
				   <xsl:with-param name="tooltip" select="concat('DataType Changed. New: ', change/@dataType-new,', Old: ', change/@dataType-old)"/>
				</xsl:apply-templates>
            </xsl:if>
			 <xsl:if test="change/@change_type='answerUnits'">
				 <xsl:apply-templates select="change" mode="label">
					 <xsl:with-param name="changeAttribute" select="'answerUnits'"/>
					 <xsl:with-param name="label" select="'un*'"/>
					 <xsl:with-param name="tooltip" select="concat('Answer Units Changed. New: ', change/@answerUnits-new,', Old: ', change/@answerUnits-old)"/>
				 </xsl:apply-templates>
			 </xsl:if>
           <xsl:if test="change/@change_type='length'">
				<xsl:apply-templates select="change" mode="label">
                  <xsl:with-param name="changeAttribute" select="'length'"/>
				  <xsl:with-param name="label" select="'len*'"/>
				  <xsl:with-param name="tooltip" select="concat('new: ', change/@length-new,', old: ', change/@length-old)"/>
               </xsl:apply-templates> 
			</xsl:if>   
            <!--display change if exists-->
			 
            <!--<xsl:apply-templates select="change" mode="label">
               <xsl:with-param name="changeAttribute" select="'textAfterResponse'"/>
               <xsl:with-param name="label" select="'txtAft'"/>
            </xsl:apply-templates>-->
            
            <!--display change if exists-->
			 <xsl:if test="change/@change_type='responseRequired'">
				 <xsl:apply-templates select="change" mode="label">
					 <xsl:with-param name="changeAttribute" select="'responseRequired'"/>
					 <xsl:with-param name="label" select="'respReq*'"/>
					 <xsl:with-param name="tooltip" select="concat('Fill-in Response is Required. New: ', change/@responseRequired-new,', Old: ', change/@responseRequired-old)"/>
				 </xsl:apply-templates>
			 </xsl:if>
         </div>
      </xsl:if>
   </xsl:template>
   
   <!-- new code -->
   <xsl:template name="ListItemAttributes">
      <xsl:if test="$metadata-display = 'true'">
         <div class="MetadataDisplay">
            <xsl:if test="@selectionDisablesChildren = 'true'">
               &#160;<div class="sdc">sdc </div>
            </xsl:if>
            <xsl:if test="string(@omitWhenSelected) = 'true'">
               &#160;<div class="omitWhenSel">omitWhenSel </div>
            </xsl:if>
            <xsl:if test="string(@locked) = 'true'">
               &#160;<div class="locked">locked</div>
            </xsl:if>
            <xsl:if test="@selectionDeselectsSiblings = 'true'">
               &#160;<div class="sds">sds </div>
            </xsl:if>
            <xsl:if test="string(@selected) = 'true'">
               &#160;<div class="selected">sel: </div>
            </xsl:if>

         </div>
      </xsl:if>
	   
	  <xsl:if test="$change-display = 'true'">
         <div class="ChangeDisplay">
			 <xsl:if test="change/@change_type='sdc'">
				 <!--&#160;<span class="sdc">sdc* </span>			 -->
				 <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'sdc'"/>
					<xsl:with-param name="label" select="'sdc*'"/>
					<xsl:with-param name="tooltip" select="concat('Selection Disables All Children Changed. New: ', change/@sdc-new,', Old: ', change/@sdc-old)"/>
					
				</xsl:apply-templates>
			 </xsl:if>
			 
			<xsl:if test="change/@change_type='parent'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'parent'"/>
					<xsl:with-param name="label" select="'parent*'"/>
					<xsl:with-param name="tooltip" select="concat('NEW PARENT ITEM:: ', change/@change_type)"/>
				</xsl:apply-templates>
			</xsl:if>
		
		    <xsl:if test="change/@change_type='prevSibling'">
			    <xsl:apply-templates select="change" mode="label">
				    <xsl:with-param name="changeAttribute" select="'prevSibling'"/>
				    <xsl:with-param name="label" select="'prevSibling*'"/>
				    <xsl:with-param name="tooltip" select="concat('NEW PREV SIBLING:: ', change/@change_type)"/>
			    </xsl:apply-templates>
		    </xsl:if>
		 
			 <xsl:if test="change/@change_type='newItem'">
				 <!--&#160;<span class="sdc">sdc* </span>			 -->
				 <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'newItem'"/>
					<xsl:with-param name="label" select="'new*'"/>
					<xsl:with-param name="tooltip" select="concat('NEW ITEM ADDED. New: ', change/@change_type)"/>
					
				</xsl:apply-templates>
			 </xsl:if> 
			 
			 <xsl:if test="change/@change_type='omitWhenSelected'">            
               <!--&#160;<span class="omitWhenSel">omitWhenSel* </span>-->
			   <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'omitWhenSelected'"/>
					<xsl:with-param name="label" select="'omitWhenSel*'"/>
					<xsl:with-param name="tooltip" select="concat('Omit When Selected Changed. New: ', change/@omitWhenSelected-new,', Old: ', change/@omitWhenSelected-old)"/>
					
				</xsl:apply-templates>
            </xsl:if>			 
            <xsl:if test="change/@change_type='locked'">
               <!--&#160;<span class="locked">locked*</span>-->
			   <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'locked'"/>
					<xsl:with-param name="label" select="'locked*'"/>
					<xsl:with-param name="tooltip" select="concat('Locked Changed. New: ', change/@locked-new,', Old: ', change/@locked-old)"/>
					
				</xsl:apply-templates>
            </xsl:if>
            <xsl:if test="change/@change_type = 'sds'">
               <!--&#160;<span class="sds">sds* </span>-->
			   <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'sds'"/>
					<xsl:with-param name="label" select="'sds*'"/>
					<xsl:with-param name="tooltip" select="concat('Selection Deselects Siblings Changed. New: ', change/@sds-new,', old: ', change/@sds-old)"/>
					
				</xsl:apply-templates>
            </xsl:if>
            <xsl:if test="change/@change_type='selected'">
               <!--&#160;<span class="selected">sel* </span>-->
			   <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'sel'"/>
					<xsl:with-param name="label" select="'sel*'"/>
					<xsl:with-param name="tooltip" select="concat('new: ', change/@selected-new,', old: ', change/@selected-old)"/>
					
				</xsl:apply-templates>
            </xsl:if>
			<xsl:if test="change/@change_type='itemType'">
               <!--&#160;<span class="itemType">itemType* </span>-->
			   <xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'itemType'"/>
					<xsl:with-param name="label" select="'itemType*'"/>
					<xsl:with-param name="tooltip" select="concat('Item Type Changed. New: ', change/@itemType-new,', Old: ', change/@itemType-old)"/>
					
				</xsl:apply-templates>
            </xsl:if>
			
         </div>
      </xsl:if>
   </xsl:template>
   
   <!-- new code -->
   <xsl:template name="CommonAttributes">
      <xsl:param name="metadataClass" select="'MetadataDisplay'"/>
      <xsl:if test="$metadata-display = 'true'">
         <div class="{string($metadataClass)}">
		    <!-- unnecessary logging -->
            <xsl:if test="string(@mustImplement) = 'false'">
               &#160;<div class="mustImplement">mI: </div>
               <xsl:value-of select="@mustImplement"/>
            </xsl:if>
			
            <xsl:if test="string(@showInReport) = 'false'">
               &#160;<div class="showInReport">inRpt: </div>
               <xsl:value-of select="@showInReport"/>
            </xsl:if>
			
            <!-- <xsl:if test="string(@reportText) != ''"> -->
               <!-- &#160;<div class="reportText">rpt: </div> -->
               <!-- <xsl:value-of select="@reportText"/> -->
            <!-- </xsl:if> -->
			         	
         	<xsl:if test="string(x:Property/@propName) != '' and string(x:Property/@propName) != '{no text}'">
         		&#160;<div class="reportText">rpt: </div>
         		<xsl:value-of select="x:Property/@val"/>
         	</xsl:if>
         	
            <xsl:if test="string(@alt-text) != ''">
               &#160;<div class="altText">alt: </div>
               <xsl:value-of select="@alt-text"/>
            </xsl:if>
         	<xsl:if test="string(@name) != ''">
         		&#160;
         			<div class="name">name: </div>
         			
         				<xsl:value-of select="@name"/>
         			
         		
         	</xsl:if>
            <xsl:if test="string(@visible) = 'false'">
               &#160;<div class="visible">vis: </div>
               <xsl:value-of select="@visible"/>
            </xsl:if>

            <xsl:if test="string(@enabled) != ''">
               &#160;<div class="enabled">enabled: </div>
               <xsl:value-of select="@enabled"/>
            </xsl:if>
            <xsl:if test="string(@styleClass) != ''">
               &#160;<div class="styleClass">class: </div>
               <xsl:value-of select="@styleClass"/>
            </xsl:if>
            <xsl:if test="string(@type) != ''">
               &#160;<div class="type">type: </div>
               <xsl:value-of select="@type"/>
            </xsl:if>

            <xsl:if test="string(@tooltip) != ''">
               &#160;<div class="tooltip">tooltip: </div>
               <xsl:value-of select="@tooltip"/>
            </xsl:if>
            <xsl:if test="string(@linkText) != ''">
               &#160;<div class="linkText">link: </div>
               <xsl:value-of select="@linkText"/>
            </xsl:if>
            <xsl:if test="string(@linkText2) != ''">
               &#160;<div class="linkText2">link2: </div>
               <xsl:value-of select="@linkText2"/>
            </xsl:if>
         </div>
      </xsl:if>
	   
	 <xsl:if test="$change-display = 'true'">
         <div class="ChangeDisplay">
			
            <xsl:if test="change/@change_type='mustImplement'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'mustImplement'"/>
					<xsl:with-param name="label" select="'mI*'"/>
					<xsl:with-param name="tooltip" select="concat('Must Implement Changed. New: ', change/@mustImplement-new,', Old: ', change/@mustImplement-old)"/>
				</xsl:apply-templates>              
            </xsl:if>
            <xsl:if test="change/@change_type='showInReport'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'showInReport'"/>
					<xsl:with-param name="label" select="'inRpt*'"/>
					<xsl:with-param name="tooltip" select="concat('new: ', change/@showInReport-new,', old: ', change/@showInReport-old)"/>
				</xsl:apply-templates> 
            </xsl:if>
            <xsl:if test="change/@change_type='reportText'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'reportText'"/>
					<xsl:with-param name="label" select="'rpt*'"/>
					<xsl:with-param name="tooltip" select="concat('Report Text Changed. New: ', change/@reportText-new,', Old: ', change/@reportText-old)"/>
				</xsl:apply-templates> 
            </xsl:if>
            <xsl:if test="change/@change_type='altText'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'altText'"/>
					<xsl:with-param name="label" select="'alt*'"/>
					<xsl:with-param name="tooltip" select="concat('Hidden Text Changed. New: ', change/@altText-new,', Old: ', change/@altText-old)"/>
					
				</xsl:apply-templates>              
            </xsl:if>
            <xsl:if test="change/@change_type='name'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'name'"/>
					<xsl:with-param name="label" select="'name*'"/>
					
				</xsl:apply-templates>  
            </xsl:if>
            <xsl:if test="change/@change_type='visible'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'visible'"/>
					<xsl:with-param name="label" select="'vis*'"/>
					
				</xsl:apply-templates>
			
            </xsl:if>

            <xsl:if test="change/@change_type='enabled'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'enabled'"/>
					<xsl:with-param name="label" select="'enabled*'"/>
					
				</xsl:apply-templates>
            </xsl:if>
            <xsl:if test="change/@change_type='class'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'styleClass'"/>
					<xsl:with-param name="label" select="'class*'"/>
					
				</xsl:apply-templates>
            </xsl:if>
            <xsl:if test="change/@change_type='type'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'type'"/>
					<xsl:with-param name="label" select="'type*'"/>
					
				</xsl:apply-templates>
             </xsl:if>

            <xsl:if test="change/@change_type='tooltip'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'tooltip'"/>
					<xsl:with-param name="label" select="'tooltip*'"/>
					
				</xsl:apply-templates>              
            </xsl:if>
            <xsl:if test="change/@change_type='linkText'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'linkText'"/>
					<xsl:with-param name="label" select="'link*'"/>
					
				</xsl:apply-templates>              
            </xsl:if>
            <xsl:if test="change/@change_type='linkText2'">
				<xsl:apply-templates select="change" mode="label">
					<xsl:with-param name="changeAttribute" select="'linkText2'"/>
					<xsl:with-param name="label" select="'link2*'"/>
					
				</xsl:apply-templates>
            </xsl:if>
         </div>
      </xsl:if>

   </xsl:template>
   
   <!-- new code -->
   <xsl:template name="ItemWithID">
      <xsl:param name="required"/>
      <xsl:if test="$required = 'false' or @mustImplement = 'false'">
         <div class="notRequired">+</div>
      </xsl:if>

      <xsl:if test="$metadata-display = 'true'">
         <div style="display:inline" class="idDisplay">
            <a onClick="javascript:syncItem(this)">
				<xsl:attribute name="name">
					<xsl:value-of select="substring-before(@ID, '.')"/>
				</xsl:attribute>				
				<xsl:value-of select="substring-before(@ID, '.')"/>
            	-  
			</a>
         	<!--<xsl:value-of select="x:Property/@val"/>  -->
         </div>
      </xsl:if>

      <xsl:choose>
         <xsl:when test="count(display/property) > 1">
            <xsl:apply-templates select="display"/>
         </xsl:when>
      </xsl:choose>
   </xsl:template>
   
	<!-- new code -->
	<!--puts a label on change-->
   <xsl:template match="change" mode="label">
      <xsl:param name="changeAttribute"/>
      <xsl:param name="label"/>
	  <xsl:param name="tooltip"/>
	  
      <xsl:variable name="currentChangeAttriibute" select="@change_type"/>
	  <!--<xsl:variable name="tooltip" select="@oldText"/>-->
	  
	   <div class="ChangeDisplay">
		   <xsl:choose>
			 <xsl:when test="$currentChangeAttriibute != '' and $currentChangeAttriibute=$changeAttribute">
			 	  <!-- GOT CHANGE -->
				  <!-- <xsl:value-of select="$changeAttribute" /> -->
				  <!-- <xsl:value-of select="$currentChangeAttriibute" /> -->
				  <!-- <xsl:value-of select="$tooltip" /> -->
				  <!-- <xsl:value-of select="$label" /> -->
				<span class="{$currentChangeAttriibute}" title="{$tooltip}">
				   <xsl:choose>
					  <xsl:when test="$label=''">
						  <xsl:value-of select="$currentChangeAttriibute"/>* 
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$label"/> 
					  </xsl:otherwise>
				   </xsl:choose>
				</span>
			 </xsl:when>         
		  </xsl:choose>  
	   </div>
       
   </xsl:template>
</xsl:stylesheet>