

var xmlDoc;
var repeatIndex = 0;   //used to generate unique ids, names in repeated elements
var _debug = false;
var $xml;
var toggle_all;	
var toggle_section;	
var toggle_question;
				
$(document).ready(function () {

	//link CSS based on whether runing from disk or from server
	
	if (window.location.protocol=='file:')
	{
		
		$('<link type="text/css" rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>').appendTo('head');
		
		
	}
	else{
		
		$("<link rel='stylesheet' href='Transforms/working/sdctemplate.css' type='text/css' />").appendTo('head');
	}
  }
)


  
$(document).ready(function () {
	
	// 2018-08-02
	// alert($('#checklist').length);
								
	//hide all notRequired
	//toggle_notRequired();
	
	

	jQuery.support.cors = true;  //not sure if needed because cors setting is on the server
	
							
	
	
	var endpoints;
	var successIndex = 0;
	
	
	
	/*
		save original xml in jquery variable  
		server or xslt puts original xml in #rawxml, issue with xslt putting in xml is copy-of function decodes special characters, thus 
		making xml invalid
	*/
	
	var xmlstring = $("#rawxml").val();   
	
	
	//load into xml dom
	try{
		
		
		xmlDoc = $.parseXML(xmlstring);
		
		$xml = $(xmlDoc);

		//allow submit
		if($("#allowsubmit").val()=='no')
		{
			$("#send").css("display","none");
			
		}
		
		
	}
	catch(err){
		alert('Error loading xml: ' + err.message);
	}
	
	
	//disable all fill-in boxes
	/*
	var $fillins = $xml.find("ListItemResponseField")
	$fillins.each(function(){
		var answerid = $(this).parent().attr("ID");
		var $answerElement = getAnswerItemByID(answerid, $("html"))
		if(!$answerElement.prop('checked'))
			$answerElement.parent().find('.AnswerTextBox').prop('disabled',true);
		
	})
	*/
});

function BindEvents()
{
	//toggle events
 
 $('.mustImplement').parent().toggle();

	toggle_all = true;
	$(".collapse_all_control").click(function(){
		$(".collapsable_q").each(function(){
			if ($(this).siblings().is( ":visible" ) && toggle_all === true) {
			   $(this).siblings().toggle(false);
			   $(this).toggleClass("Question collapsed_q");
			} else if (!$(this).siblings().is( ":visible" ) && toggle_all === false) {
			   $(this).siblings().toggle(true);
			   $(this).toggleClass("Question collapsed_q");
			}
		});
		
		$(".collapsable").each(function(){
			if ($(this).siblings().is( ":visible" ) && toggle_all === true) {
				$(this).siblings().toggle(false);
				$(this).toggleClass("HeaderGroup collapsed");
			} else if (!$(this).siblings().is( ":visible" ) && toggle_all === false) {
				$(this).siblings().toggle(true);
				$(this).toggleClass("HeaderGroup collapsed");
			}
		});
		
		if (toggle_all) {
			toggle_all = false;
		} else {
			toggle_all = true;
		}
	});
	
	$(".collapsable").click(function(){
		$(this).siblings().toggle();
		$(this).toggleClass("HeaderGroup collapsed");									
	});	
	
 toggle_section = true;
	$(".collapse_control").click(function(){
		alert('here');
		$(".collapsable").each(function(){
			if ($(this).siblings().is( ":visible" ) && toggle_section === true) {
				$(this).siblings().toggle(false);
				$(this).toggleClass("HeaderGroup collapsed");
			} else if (!$(this).siblings().is( ":visible" ) && toggle_section === false) {
				$(this).siblings().toggle(true);
				$(this).toggleClass("HeaderGroup collapsed");
			}
		});
		
		if (toggle_section) {
			toggle_section = false;
		} else {
			toggle_section = true;
		}
	});
	
	toggle_question  = true;
	$(".collapse_q_control").click(function(){
		$(".collapsable_q").each(function(){
			if ($(this).siblings().is( ":visible" ) && toggle_question === true) {
			   $(this).siblings().toggle(false);
			   $(this).toggleClass("Question collapsed_q");
			} else if (!$(this).siblings().is( ":visible" ) && toggle_question === false) {
			   $(this).siblings().toggle(true);
			   $(this).toggleClass("Question collapsed_q");
			}
		});
		
		if (toggle_question)
			toggle_question = false;
		else
			toggle_question = true;
	});
	
	//support toggle - sections
	$(".collapsable").click(function(){
		$(this).siblings().toggle();
		$(this).toggleClass("HeaderGroup collapsed");									
	});	

	// support toggle - questions
	$(".collapsable_q").click(function(){
		$(this).siblings().toggle();
		$(this).toggleClass("Question collapsed_q");									
	});	
	
	toggle_mustImplement();
	
	$("#btnParameters").click(function(){
		$("#options").toggle();
	});
	
	$("#btnParameters").click();
	
	//support toggle
	$(".collapsable").click(function(){
		$(this).siblings().toggle();
		$(this).toggleClass("HeaderGroup collapsed");									
	});	
}
function collapseSection(sectionId)
{	
	collapsableDiv = $("[id='" + sectionId + "']").eq(1).find(".TopHeader");
	$(collapsableDiv).siblings().toggle();
	$(collapsableDiv).toggleClass("HeaderGroup collapsed");	
}

function clear_storage()
{
  if (localStorage.length > 0 ) {
    localStorage.clear(); 
  }
  
   document.location.reload(true);
}
function initialize()
{
						
							//S_Onc_Clin_Assess_43969
							//S_PathReport_49193	- s49193.100004300
							//S_Staging_41550		- s49193.100004300					
							
							
							if(isLocalMode())
							{
								
								if(localStorage.getItem("SavedState")==null)
								{
									
									
									$('#rawxml').remove();
									$('<textarea id="rawxml" rows="20" style="-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;width: 100%;"/>').appendTo('#example');
		
									$('#rawxml').text(localStorage.getItem("xml"));
									
									xmlDoc = $.parseXML(localStorage.getItem("xml"));
									$xml = $(xmlDoc);
									
									
									
									//console.log($('#rawxml').text());
								}
								else{
									
									$('#rawxml').remove();
									$('<textarea id="rawxml" rows="20" style="-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;width: 100%;"/>').appendTo('#example');
		
									$('#rawxml').text(localStorage.getItem("xml"));
									
									xmlDoc = $.parseXML(localStorage.getItem("xml"));
									$xml = $(xmlDoc);
									console.log($('#rawxml').text());
									
								}
							}
							else{
								alert('not local');
							}
							
							if(_debug)
							{
							
								$('#rawxml').show();
							}
							else{
								
								$('#rawxml').hide();
							}

							
							//if($('#packageInstanceId').text()==$('#MessageId').text())
							//	localStorage.removeItem("SavedState");
							
							
							collapseSection('s43969.100004300');
							collapseSection('s49193.100004300');
							collapseSection('s41550.100004300');						
					
							
							
							if(localStorage.getItem("SavedState")==null)
							{									
									collapseSection('s43969.100004300');									
									EnableDisableElementById('s49193.100004300','false');
									EnableDisableElementById('s41550.100004300','false');									
							}
							
							if(localStorage.getItem("SavedState")=="ClinAssess")
							{									
									collapseSection('s49193.100004300');
									EnableDisableElementById('s43969.100004300','false');
									EnableDisableElementById('s41550.100004300','false');
							}	
                            if(localStorage.getItem("SavedState")=="PathRpt")
							{									
									collapseSection('s41550.100004300');
									EnableDisableElementById('s43969.100004300','false');
									EnableDisableElementById('s49193.100004300','false');
							}	
                            if(localStorage.getItem("SavedState")=="Staged")
							{
									
									collapseSection('s41550.100004300');
									EnableDisableElementById('s43969.100004300','false');
									EnableDisableElementById('s49193.100004300','false');
							}	

							
}
function validateDate(value)
{
	//2015-1-11 13:57:24
	var pattern = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01]) (00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])$/g;
	
}
function doConfirm(msg, yesFn, noFn)
{
	
	var confirmBox = $("#confirmBox");
	confirmBox.find(".message").text(msg);
	confirmBox.find(".yes,.no").unbind().click(function()
	{
		confirmBox.hide();
	});
	confirmBox.find(".yes").click(yesFn);
	confirmBox.find(".no").click(noFn);
	confirmBox.show();
}

function isLocalMode()
{
	if (window.location.protocol=='file:')
	{
		
		return true;
	}
	else
	{
		return false;
	}
}


function SubmitForm()
{
	
	if(isLocalMode()) 
	{
		openMessageData(0,"Staged");
		
		localStorage.setItem("flatXml",flatXml);
		$(xmlDoc).find('Property[propName="SaveState"]').attr("val","Staged");
		localStorage.setItem("xml",xmlToString(xmlDoc));
		
		//initialize dialog
		$( function() {
			$( "#dialog-message" ).dialog({
			  autoOpen: false,
			  modal: true,
			  width: 800,
			  height: 500,
			  buttons: {
				Ok: function() {
					localStorage.setItem("SavedState","Staged");
					document.location.reload(true);	
				  $( this ).dialog( "close" );
				}
			  }
			});
		  } );
		
		//now open
		//
		
		
		$("#dialog-message").html("<p>Final report with stage calculation is saved.</p>" 
		                           + "<p>Here is the summary</p><hr/>");
		FormatSummaryReport();						   
		$("#dialog-message").dialog("open");
	}
	else
	    openMessageData(1,"Staged");
		
	
	return false;
}

function SubmitPathReport()
{
    if(isLocalMode()) 
	{
		openMessageData(0,"PathRpt");
		localStorage.setItem("flatXml",flatXml);
				
		
		$(xmlDoc).find('Property[propName="SaveState"]').attr("val","PathRpt");
		var test = xmlToString(xmlDoc);
		localStorage.setItem("xml",test);
		
		//initialize dialog
		$( function() {
			$( "#dialog-message" ).dialog({
			  autoOpen: false,
			  modal: true,			 
			  width: 800,
			  height: 500,
			  buttons: {
				Ok: function() {
					localStorage.setItem("SavedState","PathRpt");
					document.location.reload(true);	
				  $( this ).dialog( "close" );
				}
			  }
			});
		  } );
		
		//now open
		$("#dialog-message").html("<p>Pathology Report is saved.");
		FormatSummaryReport();
		
		$("#dialog-message").dialog("open");
	}
	else
	    openMessageData(1,"PathRpt");
		
	
	return false;
}
function FormatSummaryReport()
{
	var table = $('<table></table>');
	var tr = $('<tr></tr>') //creates row
	var td = $('<td></td>') //creates table cells
	
	var xml = localStorage.getItem('flatXml');
	var $FlatDoc = $($.parseXML(xml));
	
	$FlatDoc.find('question').each(function() {
		var row = tr.clone();
		row.append(td.clone().text($(this).attr('display-name')));	
        $(this).find('answer').each(function() {
			row.append(td.clone().text($(this).attr('display-name')));
		});
		table.append(row);
	});
	
	$('#dialog-message').append(table);
}

function getReportHtml(xml)
{
	var table = $('<table></table>');
	var tr = $('<tr></tr>') //creates row
	var td = $('<td></td>') //creates table cells
	
	var reportXml = xml;
	var $FlatDoc = $($.parseXML(reportXml));
	
	$FlatDoc.find('question').each(function() {
		var row = tr.clone();
		row.append(td.clone().text($(this).attr('display-name')));	
        $(this).find('answer').each(function() {
			row.append(td.clone().text($(this).attr('display-name')));
		});
		table.append(row);
	});
	
	return  table.prop('outerHTML')
}

function SubmitIntClinAssessment()
{
    
		
	if(isLocalMode()) 
	{
		openMessageData(0,"ClinAssess");
		localStorage.setItem("flatXml",flatXml);
		
		$(xmlDoc).find('Property[propName="SaveState"]').attr("val","ClinAssess");
		var test = xmlToString(xmlDoc);
		localStorage.setItem("xml",test);
		
		
		//initialize dialog
		$( function() {
			$( "#dialog-message" ).dialog({
			  autoOpen: false,
			  modal: true,
			  width: 800,
			  height: 500,
			  buttons: {
				Ok: function() {
					localStorage.setItem("SavedState","ClinAssess");
					document.location.reload(true);	
				  $( this ).dialog( "close" );
				}
			  }
			});
		  } );
		
		//now open
		$("#dialog-message").html("<p>Initial Clinical Assessment is saved.");
		FormatSummaryReport();
		$("#dialog-message").dialog("open");
	}
	else
	    openMessageData(1,"ClinAssess");
		
	
	
	return false;
}

function downloadXml()
{
   var xml = localStorage.getItem("xml");
   download("FullResponse.xml",xml);   
}

function downloadReport()
{
	 var xml = localStorage.getItem("flatXml");
	 var table = getReportHtml(xml);
	html = "<html><body>" + table + "</html></body>";
     download("Report.html",html);   
}
function download(filename, text)
{

 var pom = document.createElement('a');
 pom.setAttribute('href','data:text/plain;charset=utf-8,' + encodeURIComponent(text));
 pom.setAttribute('download', filename);
 
 if(document.createEvent) {
	var event = document.createEvent('MouseEvents');
	event.initEvent('click', true);
	pom.dispatchEvent(event);
 }
 else{
    pom.click();
 }
}


function callCalculatorService(stageInput, type)
{



	
	var serviceUrl;
	var StageQuestion;
	if(type==1)
	{
		//serviceUrl = "http://localhost:32511/api/clinicalStage";
		serviceUrl = "https://ajcccalculator.azurewebsites.net/AJCCCalculator/api/clinicalStage";
		StageQuestion = "Q_cStage_40471"
		 console.log("Computing Clinical");

		}
	else
	{
		//serviceUrl = "http://localhost:32511/api/pathologicalStage";
		serviceUrl = "https://ajcccalculator.azurewebsites.net/AJCCCalculator/api/pathologicalStage";
		StageQuestion = "Q_pStage_53106"
		 console.log("Computing Pathological");

		}
	
	


try
		{
			$.ajax({
			type: "post",		
			data: {StageInput:stageInput},
            url: serviceUrl,
			ContentType: 'application/json;utf-8',
            datatype:'json',
			success: function (response) {
			  console.log(response.Type);		  
			  
			  if(response.Stage!='')
			  {
			      console.log(response.Stage);
				  $id = $xml.find('Question[name=' + StageQuestion + ']').find('ListItem[associatedValue=' + response.Stage + ']').attr('ID');
				  $(document).find(':input[value^="' + $id + '"]').prop('checked',true);
			  
			  }
			  else{
					console.log("not stageable");
					$id = $xml.find('Question[name=' + StageQuestion + ']').find('ListItem[associatedValue="NA"]').attr('ID');
				    $(document).find(':input[value^="' + $id + '"]').prop('checked',true);
			  }

			  },
			error: function (xhr, message, exception ) {alert('error');	}
			});
		}

		catch(err)
		{
			alert("Error when posting submit request to " + serviceUrl + ": " + err);
		}
		
	
	for ( var i in stageInput) {
		var id = stageInput[i].ColumnName;
		var name = stageInput[i].ColumnValue;
		console.log(id + ':' + name);
		
	}
	
	//now refrwsh browser to reload xml in rawxml
	//location.reload();
	
	
	
	
}

function func_calcClinStage(url, TxApproach, cT, cN, cM, Grade, ER, PR, HER2_IHC, HER2_ISC, OncDx)
{
   
   //openMessageData(0);
   
   $('#rawxml').val(xmlToString(xmlDoc));
   
   console.log('in func_calcClinStage, url = ' + url);
      
   var clinInput='';
   var stageInput = [];   

	pair = {"ColumnName":"site", "ColumnValue":"C500"}
	stageInput.push(pair);
		
	pair = {"ColumnName":"histology", "ColumnValue":"8500"}
	stageInput.push(pair);
	
	pair = {"ColumnName":"age", "ColumnValue":"30"}
	stageInput.push(pair);
	
	pair = {"ColumnName":"sex", "ColumnValue":"2"}
	stageInput.push(pair);
		
	pair = {"ColumnName":"behavior", "ColumnValue":"3"}
	stageInput.push(pair);
		
	pair = {"ColumnName":"disc1", "ColumnValue":""}
	stageInput.push(pair);
		
	pair = {"ColumnName":"disc2", "ColumnValue":""}
	stageInput.push(pair);
		
	
	
    var id = $xml.find('Question[name="' + TxApproach + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + TxApproach + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
		
     }
   
       
	
	var $t = $xml.find('Question[name="' + cT + '"]:first').find("ListItem[selected='true']:first");
	
	
	//new method to find selected item
	//T
	var id = $xml.find('Question[name="' + cT + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + cT + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var tval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"t", "ColumnValue":tval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"t", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }    
	
	
	//N
	var id = $xml.find('Question[name="' + cN + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + cN + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var nval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"n", "ColumnValue":nval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"n", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }    
	 
	 //M
	var id = $xml.find('Question[name="' + cM + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + cM + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var mval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"m", "ColumnValue":mval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"m", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	
	   
	 
	 
	var id = $xml.find('Question[name="' + OncDx + '"]:first').attr("ID");
	 var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	 
		
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + OncDx + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var oncval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"oncotypedx", "ColumnValue":oncval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"oncotypedx", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	
	
	//grade
	var id = $xml.find('Question[name="' + Grade + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + Grade + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var gradeval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-g", "ColumnValue":gradeval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-g", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	 
	 
    //her2 ihc
	
	var id = $xml.find('Question[name="' + HER2_IHC + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + HER2_IHC + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var her2ihcval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":her2ihcval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	 
       
	

	/* Only one her2
	var $her2isc = $xml.find('Question[name="' + HER2_ISC + '"]:first').find("ListItem[selected='true']:first");
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + HER2_ISC + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
    if($her2isc.length==1)
	{
		clinInput = clinInput + '\nHER2.ISC value:' + $her2isc.attr("associatedValue");		
		
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":$her2isc.attr("associatedValue")};
		stageInput.push(pair);
	}

	else{
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":"N/A"}
		stageInput.push(pair);
	}	
	*/
	
	//er
	var id = $xml.find('Question[name="' + ER + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + ER + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var erval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-er", "ColumnValue":erval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-er", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }
	 
	//pr
    var id = $xml.find('Question[name="' + PR + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + PR + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var prval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-pr", "ColumnValue":prval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-pr", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }
	 

	callCalculatorService(stageInput,1);   
   
	
  
}

function func_calcPathStage(url, TxApproach, pT, pN, pM, Grade, ER, PR, HER2_IHC, HER2_ISC, OncDx, EvalParam1NameValue, EvalParam2NameValue)
{
  
   //evaluate condition
   //get the item name
   var calculate = true;
   if(EvalParam1NameValue!='' && EvalParam2NameValue!='')
   {
	    var itemName = EvalParam1NameValue.split('|')[1];
		
		//find ID from xml
		var id = $xml.find("*[name='" + itemName + "']").attr("ID");
		//find it in DOM
		
		if($(document).find(':input[value^="' + id + '"]').prop('checked')!=true)
			calculate = false;
		
   }
  
   
   
   
   console.log('calculate = ' + calculate);
   
  
   
   var pathInput='';
   
   var stageInput = [];
   
    pair = {"ColumnName":"site", "ColumnValue":"C500"}
	stageInput.push(pair);
		
	pair = {"ColumnName":"histology", "ColumnValue":"8500"}
	stageInput.push(pair);
	
	pair = {"ColumnName":"age", "ColumnValue":"30"}
	stageInput.push(pair);
	
	pair = {"ColumnName":"sex", "ColumnValue":"2"}
	stageInput.push(pair);
		
	pair = {"ColumnName":"behavior", "ColumnValue":"3"}
	stageInput.push(pair);
		
	pair = {"ColumnName":"disc1", "ColumnValue":""}
	stageInput.push(pair);
		
	pair = {"ColumnName":"disc2", "ColumnValue":""}
	stageInput.push(pair);
   
    var id = $xml.find('Question[name="' + TxApproach + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + TxApproach + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
		
     }
	
	//new method to find selected item
	//T
	var id = $xml.find('Question[name="' + pT + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + pT + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var tval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"t", "ColumnValue":tval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"t", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }    
	
	
	//N
	var id = $xml.find('Question[name="' + pN + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + pN + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var nval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"n", "ColumnValue":nval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"n", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }    
	 
	 //M
	var id = $xml.find('Question[name="' + pM + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + pM + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var mval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"m", "ColumnValue":mval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"m", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	
	   
	 //onctype
	 var id = $xml.find('Question[name="' + OncDx + '"]:first').attr("ID");
	 var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	 
		
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + OncDx + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var oncval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"oncotypedx", "ColumnValue":oncval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"oncotypedx", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	
	
	//grade
	var id = $xml.find('Question[name="' + Grade + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + Grade + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var gradeval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-g", "ColumnValue":gradeval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-g", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	 
	 
    //her2 ihc
	
	var id = $xml.find('Question[name="' + HER2_IHC + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + HER2_IHC + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var her2ihcval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":her2ihcval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     } 
	 
       
	

	/* Only one her2
	var $her2isc = $xml.find('Question[name="' + HER2_ISC + '"]:first').find("ListItem[selected='true']:first");
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + HER2_ISC + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
    if($her2isc.length==1)
	{
		clinInput = clinInput + '\nHER2.ISC value:' + $her2isc.attr("associatedValue");		
		
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":$her2isc.attr("associatedValue")};
		stageInput.push(pair);
	}

	else{
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
		pair = {"ColumnName":"ajccfactor-her2", "ColumnValue":"N/A"}
		stageInput.push(pair);
	}	
	*/
	
	//er
	var id = $xml.find('Question[name="' + ER + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + ER + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var erval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-er", "ColumnValue":erval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-er", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }
	 
	//pr
    var id = $xml.find('Question[name="' + PR + '"]:first').attr("ID");
	var selectedItem = $(document).find(':input[name*="' + id + '"]:checked');
	
	var $selectIf = $xml.find('SelectIf').find("AttributeEval[itemNames='" + PR + "']");	
	var listitem = $selectIf.closest("ListItem").attr('ID');
	
	if(selectedItem.length>0)
	{
	    var itemValue = selectedItem.attr("value");
		var array = itemValue.split(',');		
		var prval = $xml.find('ListItem[ID="' + array[0] + '"]').attr("associatedValue");
		pair = {"ColumnName":"ajccfactor-pr", "ColumnValue":prval}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',true);
	}
	else{
		pair = {"ColumnName":"ajccfactor-pr", "ColumnValue":"N/A"}
		stageInput.push(pair);
		$(document).find(':input[value^="' + listitem + '"]').prop('checked',false);
     }
	   	
       
	   
	 if(calculate) 
		callCalculatorService(stageInput,2);      
   
}




function loadXml(){
	
	if (isLocalMode())
	{
		// 2018-08-02 change: 
	    // alert('local');
		
		$('#rawxml').remove();
		$('<textarea id="rawxml" rows="20" style="-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;width: 100%;"/>').appendTo('body');
		readTextFile(window.location.href);
									
	}
}

function readTextFile(file)
{
		
	var rawFile = new XMLHttpRequest();
	rawFile.open("GET", file, false);
	rawFile.onreadystatechange = function ()
	{
		if(rawFile.readyState === 4)
		{
			if(rawFile.status === 200 || rawFile.status == 0)
			{
				var allText = rawFile.responseText;
				
				$('#rawxml').val(allText);
				
			}
		}
	}
	rawFile.send(null);
}

function sayHello(name) {
	alert("sayHello function in javascripts says - hello, " + name);
	return window.external.ShowMessage("If you can see this message sayHello successfully called ShowMessage function in this desktop client app.");

}

function toggle_metadata() {

   var divsMD = document.getElementsByClassName('MetadataDisplay')
   var divsMDH = document.getElementsByClassName('MetadataDisplayHeader')           
	  
	  var display = 'none'
	  if (divsMD[0].style.display)
	  {                  
		 if (divsMD[0].style.display == 'inline' )
		 { display = 'none' }
			else
		 { display = 'inline' }
	  }
	  
	  for (var i = 0; i < divsMD.length; i++)
	  { divsMD[i].style.display = display }
	  for (var i = 0; i < divsMDH.length; i++) 
	  { divsMDH[i].style.display = display } 
	  
	  
	  //Toggle ids too
	  var divs = document.getElementsByClassName('idDisplay')   
	  for (var i = 0; i < divs.length; i++)
	  { 
		 divs[i].style.display = display 
	  }
	  
	  //Toggle deprecated items                 
	  var dis = document.getElementsByClassName('TopHeader')
	  var searchText = "(Deprecated Items)"
	  
	  for (var i = 0; i < dis.length; i++) 
	  { 
	  if (dis[i].textContent.indexOf(searchText) >=0) 
		 {
			var divHeader = dis[i].parentElement.parentElement.parentElement //the tbody element
			if (display == 'inline') {display = ''}
			divHeader.style.display = display
			break;                         
		 }                     
	  }                

   }
 
function toggle_mustImplement()
{
	//alert('here');
	// $('.mustImplement').parent().toggle();
	if($('.mustImplement').parent().is(":visible")){
		$('#mnuRequired').text('Toggle optional items');
		$('.mustImplement').parent().toggle();
	}
	else{
		$('#mnuRequired').text('Hide optional items');
		$('.mustImplement').parent().toggle();
	}

}
function toggle_id() {
		  var divs = document.getElementsByClassName('idDisplay');
			var display = 'none';
			
		  if (divs[0].style.display)
		  {                  
			  if (divs[0].style.display == 'inline' )
				{ display = 'none'; }
			  else
				{ display = 'inline'; }
		  }
		  
		  for (var i = 0; i < divs.length; i++)
		  { divs[i].style.display = display; }
		  
	   }
	   
function resetAnswer(questionId, event) {
		/*will it work with repeated sections?*/
	  
	  var answers = document.getElementsByName(questionId);
	  
	   //alert(questionId.split("..")[0]);
	   for (var i = 0; i < answers.length; i++) {
			var selecttype = $(answers[i]).attr('type');
			if(selecttype=="checkbox" || selecttype=="radio")
			{
				answers[i].checked = false;
				
				// fire onchange on this answer choice	
				
				//answers[i].onchange();
			}
		}
		
		return false;
		
	}
	
function ShowHideDemo() {
	$('#divdemo').toggle();
	if (($('#divdemo')).css("display")=='none')
		$('#demshowhide').text('+ Demographics');
	else
		$('#demshowhide').text('- Demographics');
	
}


//adds a new repeat of a section
function addSection(obj) {
	//obj is btnAdd
	/*
		UI: Clone the block
			Get new guid for block (section)
			Change names of each element (textbox, hiddenbox, checkbox, radio) in the block to original id + ":" + blockguid
		XML:    
			Clone the current section in xml
			Add new attribute called Guid = blockguid to the top level element
			Add new attribute called ParentGuid and set it equal to blockguid
			Change Id of each child to original id + ":" + blockguid
			
		Each question and answer choices in repeated block will have their ids changed to their original id + ": " + blockguid 
		
	*/
	
	
	
	//we need to clone table, so get table
	var td = obj.parentElement;
	var table = td.parentElement  //tr
				   .parentElement  //tbody
				   .parentElement //table
	
	/*if current section is the first occurrence, it's ID is from the xml
	if current section is a repeat it's ID = ID from the xml + Guid*/
	var currentSectionId = table.id; 
	
	var blockGuid = generateShortUid();  // generateGuid();  //to distinguish each repeat of parent element which is section for now
	repeatIndex++;
	
	
	var max = table.parentElement.firstChild.value;  //maxcardinality								
	
	try{
		var parentTable =  table.parentElement.
						   parentElement.
						   parentElement.
						   parentElement.
						   parentElement;
		
	}
	catch(err)
	{
		alert("Error when getting parent table: " + err);
		return;
	}
	
	if(countSectionRepeats(parentTable,currentSectionId.split("..")[0])==max)
	{
		alert("max repeat = " + max + " reached ");
		return;
	}
	
	var newtable = table.cloneNode(true);    							
	
	//newtable.id = currentSectionId.split(":")[0] + ":" + blockGuid;   //each repeated section id has the same ID from xml + blockGuid							
	newtable.id = currentSectionId.split("..")[0] + ".." + repeatIndex
	
								
	//set new ids to each nested table 
	var newtableitems = newtable.getElementsByTagName('*');										
	for(i=0; i< newtableitems.length; i++)			
		if(newtableitems[i].tagName=="TABLE")
			//newtableitems[i].id = newtableitems[i].id.split(":")[0] + ":" + blockGuid;
			newtableitems[i].id = newtableitems[i].id.split("..")[0] + ".." + repeatIndex;

	var trace = 0;
	var newname;
	var i;
	
	var ID;

	//add the new repeat
	try {
		
		/*find section in xml corresponding to this block (ID=currentSectionId.substring(1)) and clone it, then assign new ID*/
		//alert(currentSectionId.substring(1));
		var $sectionCurrent = $xml.find('Section[ID="' + currentSectionId.substring(1) + '"]:first');  //first is redundant since there is only one section with this ID
		if($sectionCurrent.length==0)
		{
			alert("Section ID = " + currentSectionId.substring(1) + " not found");
			return;
		}
		
		
		var $sectionNew = $sectionCurrent.clone(true);
		
		//$sectionNew.attr('ID',currentSectionId.split(":")[0].substring(1)+":" + blockGuid);
		$sectionNew.attr('ID',currentSectionId.split("..")[0].substring(1)+".." + repeatIndex);								
		
		//xml: set IDs of all children sections
		$sectionNew.find('Section').each(function(index){
			//var secid = $(this).attr("ID").split(":")[0] + ":" + blockGuid;	
			var secid = $(this).attr("ID").split("..")[0] + ".." + repeatIndex;	
			$(this).attr("ID",secid);
			
			
		});
		
		
											
		var oldtableitems = td.getElementsByTagName("input");  //get hidden input, radio buttons, checkboxes and input text boxes
		
		//iterate through oldtableitems and assign new unique ids to them
		for (i = 0; i < oldtableitems.length; i++) {
			
			if (oldtableitems[i].type == "hidden" || oldtableitems[i].type == "text" || oldtableitems[i].type=="radio") {
				oldname = oldtableitems[i].name;  //name of the first instance is ID from xml, repeats have ID + ":" + Guid

				if(oldtableitems[i].id=="maxcardinality")
					  continue;

				if(oldtableitems[i].name=="")
				{
					alert("error: a " + oldtableitems[i].type + " box without name is found at " + i);
					continue;
				}
					   
				//newname = oldtableitems[i].name.split(":")[0] + ':' + blockGuid;
				newname = oldtableitems[i].name.split("..")[0] + '..' + repeatIndex;
															
				//find the element in the new table
				
				newtableitems = newtable.getElementsByTagName('*');										
				
				
				
				for(k=0;k<newtableitems.length;k++)
				{											
					
				   if(newtableitems[k].name == oldtableitems[i].name)
				   {
						newtableitems[k].name = newname;													
						
						
						if(newtableitems[k].type=="hidden")   //question will have Q as the first letter
						{  
							
						   //find question in xml fragment and change ID
						   
						   $question = $sectionNew.find('Question[ID="' + oldtableitems[i].name.substring(1) + '"]');
						   
						   if($question.length==0)
						   {
								alert("Qusetion ID = " + oldtableitems[i].name.substring(1) + " not found.");
								$sectionNew.find('Question').each(function(index){
									alert($(this).attr("ID"));
								})
								return;
							}
							else{
								
								//$question.attr("ID", newtableitems[k].name.split(":")[0].substring(1) + ':' + blockGuid); 
								$question.attr("ID", newtableitems[k].name.split("..")[0].substring(1) + '..' + repeatIndex); 
								
							}
						   
						   
						   /* 12/18/2016
						   New constraints
						   Property name, ResponseField name and Value name have to be unique 
						   */
						   
						   if (typeof $question.find("Property").attr("name") != 'undefined')
						   {
								//new property name
								//var propname = $question.find("Property").attr("name") + "_" + blockGuid; // repeat;
								var propname = $question.find("Property").attr("name").split('..')[0] + ".." + repeatIndex;
								$question.find("Property").attr("name",propname);
						   }
						   
						   if (typeof $question.find("ResponseField").attr("name") != 'undefined')
						   {
								//new response name
								//propname = $question.find("ResponseField").attr("name") + "_" + blockGuid;  // repeat;
								propname = $question.find("ResponseField").attr("name").split('..')[0] + ".." + repeatIndex;  // repeat;
								$question.find("ResponseField").attr("name",propname);
						   }
						   if (typeof $question.find("Response").children(0).attr("name") != 'undefined')
						   {
								//new name on value field
								//propname = $question.find("Response").children(0).attr("name") + "_" + blockGuid;  // repeat;
								propname = $question.find("Response").children(0).attr("name").split('..')[0] + ".." + repeatIndex;  // repeat;
								$question.find("Response").children(0).attr("name", propname);
						   }
						   
						}
						else {                   //answers do not have Q											
								if(newtableitems[k].type=="radio" || newtableitems[k].type == "checkbox")
								{
									 newtableitems[k].checked = false;														 
									 
								}
								 else
								{
									 newtableitems[k].value = "";
								}
						}												   
					}
				}
					
			}
		}

		//better to append new table after setting properties of individual controls
		table.parentElement.appendChild(newtable);

											
		//insert newsec after last section
		
		//$xml.find('Section[ID="' + table.id.substring(1) + '"]').after($sectionNew);
		
		var $orgsecid = table.id.substring(1).split('..')[0];
		
		var $lastindex = $xml.find('Section[ID*="' + $orgsecid + '"]').length - 1;									
		
		if($lastindex>=0)
		{
			$xml.find('Section[ID*="' + $orgsecid + '"]').last().after($sectionNew);
			
		}
		else
		{
			alert("error adding section repeat");
			return;
		}
			
			
		//remove all nested repeats
		newtable = removeNestedTableRepeats(newtable);
	
		//update rawxml for view							
		$('#rawxml').val(xmlToString(xmlDoc));												
		
		repeat = countSectionRepeats(parentTable, currentSectionId.split("..")[0])																	
		
		showHideButtons(newtable);	
		
		//make sure + is visible on the first repeat of nested section
		nestedtables = getChildTables(newtable);
		
		
		for(i=0;i<nestedtables.length;i++)
		{
			elements = nestedtables[i].getElementsByTagName('*');	
			for(j=0;j<elements.length;j++)
			{
				if(elements[j].className=="btnAdd")
					elements[j].style.visibility="visible";
			}
		}
			
		
	}
	catch (err) {
		alert(err.message + "\n" + trace + "\n" + newname + "n" + i);
	}

}

//adds a new repeat of a question
function addQuestion(obj) {
	//obj is btnAdd inside command div
	/*
		UI: Clone .question div, update id of this div to original id + '..' + repeatIndex
		change names of children to name + '..' + repeatIndex 
			
			
		XML:    
			Clone the current question node in xml
			Change Id of the question and its children to original id + ":" + blockguid
			
		Each question and answer choices in repeated block will have their ids changed to their original id + ": " + blockguid 
		
	*/
	
	
	try{
	
		
		var button = $(obj);
		
		//find question div
		var questionToRepeat = button.closest('.question');  //closest parent div with class='question'
				
		
			//alert(questionToRepeat.html())
			
		var currentQuestionId = questionToRepeat.attr('id');
		
		currentQuestionId = currentQuestionId.substring(1).split('.')[0];
		
		var max = questionToRepeat.find("input[id='maxcardinality']").first().val();  //maxcardinality	
		
		//alert(max);
	     var repeats = countQuestionRepeats(currentQuestionId);
				
		if(repeats> max)
		{
			alert('max value ' + max + ' reached.');
			//repeatIndex--;
			return;
		}
				
		var clonedBlock = questionToRepeat.clone();
		//alert(currentQuestionId);
		clonedBlock.attr("id", 'q' + currentQuestionId + '..' + repeatIndex);
		
		//alert(clonedBlock.html());
		
		//find the top level question node in xml and clone it
		var elementToClone = $xml.find("Question[ID='" + currentQuestionId + "']");
		var clonedXml = elementToClone.clone();
		
		//change name attributes 
		clonedXml.find("Property").each(function(){
				var prop = $(this);
				if(typeof prop.attr("name") != "undefined")
					prop.attr("name",prop.attr("name").split('..')[0] + ".." + repeatIndex);
		});
		
		clonedXml.find("ResponseField").each(function(){
				var temp = $(this);
				if(typeof temp.attr("name") != "undefined")
				temp.attr("name",temp.attr("name").split('..')[0] + ".." + repeatIndex);
		});
		
		clonedXml.find("Response").each(function(){
				var temp = $(this).children(0);
				if(typeof temp.attr("name") != "undefined")
				temp.attr("name",temp.attr("name").split('..')[0] + ".." + repeatIndex);
		});
		
		clonedXml.find("ListItemResponseField").each(function(){
				var temp = $(this);
				if(typeof temp.attr("name") != "undefined")
				temp.attr("name",temp.attr("name").split('..')[0] + ".." + repeatIndex);
		});
		
		
		
		//update names of cloned elements
		var clonedItems = clonedBlock.find("input");  //get hidden input, radio buttons, checkboxes and input text boxes
		
		
		//iterate through clonedItems and assign new unique ids to them
		clonedItems.each(function(){
			var item = $(this);
			var name = item.attr('name');
			
			if(typeof name != 'undefined')
			{
				
						   
				if(name.substring(0,1)=='q')
				{
					var questionId = name.substring(1).split('..')[0];
					item.attr('name','q' + questionId + '..' + repeatIndex);
					//alert(questionId + ':' + clonedXml.find("Question[ID='" + questionId + "']").length);
					if(clonedXml.attr("ID")==questionId)
					{
						//alert('here')
						clonedXml.attr("ID",questionId + '..' + repeatIndex);	
					}
					else{
						clonedXml.find("Question[ID='" + name.substring(1) + "']").attr('ID',questionId + '..' + repeatIndex);	
						//alert(questionId + '..' + repeatIndex);
					}
					
				}
				else{
					//answer choices do not have 'q'
					var questionId = name.split('..')[0];
					item.attr('name',questionId + '..' + repeatIndex);
					//alert(item.attr('type'));
					if(item.attr('type')=='text')
					{
						item.val('');
					}
					
					item.attr('checked',false);
				}
			}
				
		});
		
		//append cloned node to xml
		clonedXml.appendTo(elementToClone.parent());
		
		
		
		
		//append cloned UI block in display
		clonedBlock.find('.btnRemove').css('visibility', 'visible');  //show does not work
		//clonedBlock.appendTo(questionToRepeat.after());   
		questionToRepeat.after(	clonedBlock);	
		
		
		
		
		
		
	}
	catch (err) {
		alert(err.message );
	}

}


function removeQuestion(obj)
{

	try {
		var button = $(obj);
		
		//find question div
		var questionToRemove = button.closest('.question');  //closest parent div with class='question'
																				
		var currentQuestionId = questionToRemove.attr('id').substring(1);
		
		//remove
		questionToRemove.remove();
		//alert('now removing ' + currentQuestionId);
		//alert(countQuestionRepeats(currentQuestionId));
		if($xml.find("Question [ID='" + currentQuestionId + "']").length==0)
		{
			alert("ID = " + currentQuestionId + " not found");
		}
		
		$xml.find("Question [ID='" + currentQuestionId + "']").remove();
	}
	
	catch(err)
	{
		alert(err.message);
	}

}


function getRepeats(id)
{
	var id = id.split('_')[0];
	var count = 0;
	$('input').each(function(){
		var name = $(this).attr('name');
		if(name==id)
		{
			count++;
		}
	});
	
	return count;
	
}

function generateShortUid() {
		return ("0000" + (Math.random()*Math.pow(36,4) << 0).toString(36)).slice(-4)
}

function generateGuid() {
	var result, i, j;
	result = '';
	for (j = 0; j < 32; j++) {
		if (j == 8 || j == 12 || j == 16 || j == 20)
			result = result + '-';
		i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
		result = result + i;
	}
	return result;
}


/*
Counts the number of repeats of a block (table)
Each repeated block (table) has id that has two parts
1. id that is the same for each repeat.
2. a guid that is different for each repeat 
*/
function countSectionRepeats(parentT, sectionid) {
	
	
	var tables = parentT.getElementsByTagName('TABLE');
	var count = 0;
	for(i=0; i<tables.length; i++)
	{
	   checkid = tables[i].id.split("..")[0];
	   if(checkid == sectionid) count++;
	}
   
	return count;

}


/*
count question repeats 
*/
function countQuestionRepeats(questionid) {

	questionid = questionid.split('.')[0];
	
	//alert(questionid + ':' + $xml.find('Question[ID^="' + questionid + '"]').length);
	return $xml.find('Question[ID^="' + questionid + '"]').length;
	

}


function getMaxCount(sectionid)
{
	alert(document.getElementById(sectionid).length);

}


function getSiblingTables(parentT) {								
	
	return tables = parentT.getElementsByTagName('TABLE');								

}



function getChildTables(table)
{
	return table = table.getElementsByTagName('TABLE');
}

function getLastRepeat(sectionid) {
	var section = document.getElementById(sectionid);
	var tables = section.parentElement.getElementsByTagName('TABLE');
	var ret = null;
	for(i=0;i<tables.length;i++)
	{
	   if(tables[i].id.split("..")[0]==sectionid)
		 ret = tables[i];
	}
	return ret;
}

function getFirstRepeat(sectionid) {
	var section = document.getElementById(sectionid);
	var tables = section.parentElement.getElementsByTagName('TABLE');
	var ret = null;
	for(i=0;i<tables.length;i++)
	{
	   if(tables[i].id.split("..")[0]==sectionid)
	   {
		 ret = tables[i];
		 break;
		}
	}
	return ret;
}

function removeNestedTableRepeats(table)
{
	
	var all = table.getElementsByTagName("*");
	for(i=0; i<all.length-1; i++)
	{
		if(all[i].id.indexOf("s")==0 & all[i].tagName=="TABLE") //nested table
		{										
			var id = all[i].id;
			//alert("delete id = " + id);
			for(j=i+1; j<all.length-1; j++)
			{
				
				if(all[j].id.split("..")[0]==id.split("..")[0])
				{
					
					v = all[j].id;
					
					//remove table
					all[j].parentElement.removeChild(all[j]);
					
					//remove xmlnode
					
					$j = $xml.find('Section[ID="' + v.substring(1) + '"]');
					
					
					if($j.length==0)
						alert("removeNestedTableRepeats - not found: " + v.substring(1));
					
					if($j.length > 1 )
					{
						try
						{
							$j.slice(1).remove();  //remove from index = 1 down
							
						}
						catch(err)
						{
							alert("Error in removeNestedTableRepeats: " + err);
						}
					}
					removeNestedTableRepeats(table);
				}
			}
		}
	}
	return table;
}

//gets the id parentSection of +, - buttons
function getParentSectionId(button)
{
	if(button.parentElement.parentElement.parentElement.parentElement.tagName=="TABLE")
		return button.parentElement.parentElement.parentElement.parentElement.id;
	else
		alert("Unexpected tagName");

}

function getParentTable(table)
{
	//get parentTable
	try{
	var parentTable =  table.parentElement.
						   parentElement.
						   parentElement.
						   parentElement.
						   parentElement;
	return parentTable;
	}
	catch(err)
	{
		alert("Error in getParentTable: " + err);
		return;
	}
}

function showHideButtons(table)
{
	//get parentTable
	
	var parentTable =  getParentTable(table)
	
	//show/hide buttons
	
	//get all siblings of this table
	var siblings = getSiblingTables(parentTable);
	
	//get max repeat for this table - get parent which is DIV and the firstChild of DIV is maxcount
	var max = table.parentElement.firstChild.value;  
	
	//how many repeats are there for this table currently
	var repeat = countSectionRepeats(parentTable, table.id.split("..")[0]);
	
	
	var inputs = "";
	if(siblings.length==0)
	{
		alert("error in getting siblings");
		return;
	}
	

	
	if(repeat<max)   //
	{
		
		for (k=0;k<siblings.length; k++)
		{										
			if(siblings[k].id.split("..")[0]==table.id.split("..")[0])
			{	
				inputs = siblings[k].getElementsByTagName('*');
		
				for(m=0;m<inputs.length;m++)
				{
					if(inputs[m].className=="btnAdd")
					{														
						//which section does it belong?
						sectionid = getParentSectionId(inputs[m]);
						
						if(table.id.split("..")[0] != sectionid.split("..")[0])
						{
							//alert(table.id);
							//alert(sectionid);
							continue;
						}													
						
						
						if(k>0)
						{
							inputs[m].nextSibling.style.visibility = "visible";
							inputs[m].style.visibility = "visible";
						}
						else
						{
							
							inputs[m].nextSibling.style.visibility = "hidden";
							inputs[m].style.visibility = "visible";
						}											
						
					}
					
				}
			}
		}
	}
	else
	{
		
		for (k=0;k<siblings.length; k++)
		{
			if(siblings[k].id.split("..")[0]==table.id.split("..")[0])
			{
				inputs = siblings[k].getElementsByTagName('*');
				for(m=0;m<inputs.length;m++)
				{
					if(inputs[m].className=="btnAdd")
					{
						
						inputs[m].style.visibility = "hidden";
						
						if(k>0)
							inputs[m].nextSibling.style.visibility = "visible";
					}
				}
			}
		}				
	}								
}
							
function removeSection(obj) {
	td = obj.parentElement;
	tr = td.parentElement;
	tbody = tr.parentElement;
	table = tbody.parentElement;
	var section = table.parentElement;
	var id = table.id;
	
	parentTable = getParentTable(table);
	
						   
	//do not let user remove the first instance
	if(table.id.indexOf("..")==-1)
	{
		alert("Cannot remove the first instance.");
		return;
	}
	section.removeChild(table);

									
	id = section.id;
									
	$todelete = $xml.find('Section[ID="' + table.id.substring(1) + '"]');
	if($todelete.length==0)
	{
		alert("Could not find section with ID = " + table.id.substring(1) + " to delete");
		return;
	}
	
	$todelete.remove();
	
	$todelete = $xml.find('Section[ID="' + table.id.substring(1) + '"]');
	if($todelete.length!=0)
	{
		alert("Could not delete Section ID = " + table.id.substring(1));									
	}
	
	//update rawxml
	$('#rawxml').val(xmlToString(xmlDoc));
								
	//current table is deleted, so get the first table by going upto the parent, then the first Table
	table = document.getElementById(parentTable.id);
	
	if(table.tagName=="DIV")   //first table is inside DIV element
	{
		table = table.childNodes[1];  //parentTable
		table = table.getElementsByTagName("TABLE")[0]  //firstChild table																		
		
	}
	else  //subsequent repeats are nested inside parent TABLE directly
	{
		
		table = table.getElementsByTagName("TABLE")[0]  //first child table
	}
								
	
	showHideButtons(table);
		
}


/*
Helper functions
*/
function trim(input) {
input = input.replace(/^\s+|\s+$/g, '');
return input;
}

function findElementById(parentId, Id) {
   //finds an element among descedants of a given node
   var parent = document.getElementById(parentId);

   var children = parent.getElementsByTagName('*');


   for (i = 0; i < children.length; i++) {

	  if (children[i].id == Id) {
		 return children[i];
	  }
   }

}

function findElementByName(parentName, Name) {
  //finds an element among descedants of a given node
  var parent = document.getElementById(parentName);
  var children = parent.getElementsByTagName('*');
  
  for (i = 0; i < children.length; i++) {
	 if (children[i].name == Name) {
		 return children[i];
	 }
  }
}

function validateSubmit()
{
	//alert('in validate');
	$('input').removeClass('error');
	document.getElementById('navBar').style.display = 'none';
	var retval = false;
	var $allinputs = $("#FormData").find(":input:not([type=hidden], [type=button], [type=submit])")
	$allinputs.each(function() {
		var $test = $(this);
		
		if($test.is(':checkbox') || $test.is(':radio'))
		{
			if($test.prop('checked')==true)
			{
					//alert($test.attr('id'));
				retval = true;
			}
		}
		else
		{
			if($test.val()!='')
			{
				retval = true;
				//alert($test.attr('id'));
			}
		}
	});
	
	//alert(retval);
	if(!retval)
	{
		alert('Error: You have not selected/entered any response on the form.');
		return false; //so 
	}
	
	retval = true;
	
	
	
	var $fillins = $(xmlDoc).find("ListItemResponseField")
	
	$fillins.each(function(){
		var answerid = $(this).parent().attr("ID");
		var $answerElement = getAnswerItemByID(answerid, $("#FormData"));
		
		if(!$answerElement.prop('checked') & $answerElement.parent().find(".AnswerTextBox").val()!=''){
				//alert('error');
				$answerElement.parent().find('.AnswerTextBox').addClass('error');
				retval = false;
		}
		
	})
	//alert(retval);
	if(!retval)
	{
		alert('Error: You have entered text in fill-in box)(es) without selecting the corresponding choice(s)');
		return false;
	}
	
	var requiredResponses = $xml.find("ListItemResponseField[responseRequired='true']");
	retval = true;
	requiredResponses.each(function(){
		var test = $(this);
		var id = test.parent().attr('ID');
		
		
		
		var response = test.find('>Response');
		
		if(response.length>0)
		{
			var box = $(document).find(':input[value^="' + id + '"]'); //check/option 
			if(box.length>0)
			{
			
				var testval = box.siblings('input').val();									
			
			
				if(testval=='')
				{												
					
					if(box.is(':checked'))
					{
						box.siblings('input').addClass('error');
						retval = false;
					}													
					
				}
			
			}
													
				
		}
	});
	
	if(!retval)
	{
		alert('Error: You have not answered all required questions.');
		return false
	}
	return true;
}


//utility functions
//Utility function
function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};


function xmlToString(xmlData) {

	var xmlString;
	
	xmlString = (new XMLSerializer()).serializeToString(xmlData);
	
	return xmlString;
}

//helper functions end

//submit form calls this function
/*
Builds flatXml, updates the original xml with answers.
Note that new section nodes for repeat sections have already been added (upon clicking btnAdd - addSection function) 
*/
var flatXml;
function openMessageData(submit,saveState) {
	
	var sb = "";
	var answer = "";
	
	try {
	if(document.getElementById("checklist")==null)
	{
       alert('checklist not found');	
	   //return;
	}
	if(document.getElementById("FormData")==null)
	{
       alert('checklist not found');	
	   return;
	}
	var elem = document.getElementById("checklist").elements;
	//var elem = document.getElementById("FormData").elements;
	var response = "<response>";
	var html = "";
	
	
	
	for (var i = 0; i < elem.length; i++) {
		html = "";
		var name = elem[i].name;

		var value;

		
		var instanceGuid = '';
		
		var id = name;
		var guid = "";
		if (name.indexOf("q") == 0) {
			
			value = elem[i].value;
			
			//make answer xml safe
			answer = GetAnswer(name.substring(1));

			if (answer != "") {
				
				
				
				response += "<question ID=\"" + id + "\" display-name=\"" + value.replace(/</g, "&lt;").replace(/>/g, "&gt;") 
						 + "\">";
				response += answer + "</question>";               

				
				newid = id.split('..')[0].substring(1);									
				
				if(id.split('..').length==2)
					guid=id.split('..')[1]
				


				//html += "<div class=\"MessageDataQuestion\">&lt;question ID=\"" + id + "\" guid=\"" + guid + "\" display-name=\"" + value + "";
				html += "<div class=\"MessageDataQuestion\">&lt;question ID=\"" + id.substring(1) +  "\" display-name=\"" + value + "";
				html += "&gt;<br><div class=\"MessageDataAnswer\">" + answer.replace(/</g,"&lt;").replace(/>/g,"&gt;") + "</div>&lt;/question&gt;</div>";

				
				
			}
			sb += html;
			answer = "";
		}
	}

	
	response = response.replace(/<br>/g, "");
	response = response + "</response>";															
	flatXml = response;


	sb = "<div style='font-weight:bold; color:purple'>Flat Xml response</div>" 
		 + "<div class=\"MessageDataChecklist\">&lt;response&gt;" + sb + "&lt;/response&gt;</div>"
		 + "<br/><div style='font-weight:bold; color:purple'>Response xml sent to web service.</div>"
	

	//update Xml with answers
	updateXml();
	
	//10/8/2018
	if (submit==0)
	{
		
		var test = xmlToString(xmlDoc);
		
		//document.getElementById('rawxml').innerText = test;
		$('#rawxml').val(test);
		return;
	}	
	
	/* disable validation
	if(!validateSubmit())
	{
	
		if (confirm('Do you want to submit this form with errors? Click Ok to submit or Cancel to fix errors.')==false)
		{
			return;
		}
	}								
	*/
	document.getElementById('MessageDataResult').innerHTML = sb;
	document.getElementById('MessageData').style.display = 'block';
	document.getElementById('FormData').style.display = 'none';
	
	//if running from disk 
	if(isLocalMode())
	{
		//Ajax call to web service
		CallSoapSubmit2(xmlToString(xmlDoc),saveState);
		return false;
	}
	
	

	var test = xmlToString(xmlDoc);
	//document.getElementById('rawxml').innerText = test;
	$('#rawxml').val(test);
	
	
	if($('#scriptsubmit').is(':checked'))
	{
		//Ajax call to web service
		CallSoapSubmit2(xmlToString(xmlDoc),saveState);
		return false;
	}
	else
	{
		//call formreceiver from server side code
		//alert('calling server submit');
		ServerSubmit();

	}

	}
	catch(err)
	{
		alert(err);
	}
}

/* start of functions to call .NET serverside methods*/

function ServerSubmit()
{
	//alert('trace1');
	var xml = document.getElementById("rawxml").value;
	var submiturls = document.getElementById("submiturl").value
	//alert(submiturls);
	//alert('trace2');
	var responsetext = PageMethods.submitform(xml, submiturls, OnServerSucceed, OnServerError);
	//alert('trace3');
	return false;
}

function OnServerSucceed(result)
{
	//alert('Server submit succeded.');
   //server response includes SoapResponse and SoapRequest strings delimited by #!#2#3
	var response = result.split('#!#2#3')[0];
	var request = result.split('#!#2#3')[1];

	
	
	//request = formatXml(request);
	$("#submitsoap").val(request);

	response = formatXml(response);
	xml_escaped = response.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/ /g, '&nbsp;').replace(/\n/g, '<br />');
	
  
	$("#response").html("<PRE>" + xml_escaped + '</PRE>');
	$("#response").css("background-color", "yellow");
	$("#response").css("display", "block");								

   
}

function OnServerError(error)
{
	var result = error.get_message();
	alert("Error on server submit: " + result);
	document.getElementById("response").innerText = result;
	document.getElementById("response").style.backgroundColor = "yellow";
	document.getElementById("response").style.color = "red";
}

/* end of functions to call .NET serverside methods */


function closeMessageData() {
	document.getElementById('MessageData').style.display = 'none';
	document.getElementById('response').style.display = 'none';
	document.getElementById('FormData').style.display = 'block';
	document.getElementById('navBar').style.display = 'block';
}

function GetAnswer(qCkey) {
	
	var elem = document.getElementById("checklist").elements;
	var str = "";
	var name, value;

	for (var i = 0; i < elem.length; i++) {
		name = elem[i].name;
		value = elem[i].value;

		//if (name.indexOf(qCkey) == 0) {
		if (name==qCkey) {	
			if (elem[i].checked || (elem[i].type == "text" && value != "")) {

				{
					
					var k = value.split(',');

					if (elem[i].type == "text" && value != "") {
						value = value.replace(/&/g,"&amp;")
									 .replace(/</g,"&lt;")
									 .replace(/>/g,"&gt;")
									 .replace(/"/g,"&quot;")
									 .replace(/'/g,"&apos;");
												 
					//alert(answer);
						
						str += "<answer value=\"" + value + "\"/><br>";
					}
					else if (elem[i].type != "text") {
						//str += "&lt;answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/&gt;<br>";
						str += "<answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/><br>";
					}
				}
			}
		}
	}
	return str;
}

function GetDisplayName(value) {
	
	var strArray = value.split(',');
	var returnStr = "";
	if (strArray.length > 1) {
		for (var i = 1; i < strArray.length; i++) {
			if (i != strArray.length) {
				returnStr += strArray[i] + ",";
			}
			else {
				returnStr += strArray[i];
			}
		}
	}
	returnStr = returnStr.replace(/</g,"&lt;").replace(/>/g,"&gt;");
	return returnStr.substr(0, returnStr.length - 1);
}


//updates answers in full xml
function updateXml() {
	var $xml = $(xmlDoc);  //full xml
	FlatDoc = $.parseXML(flatXml);
	$xmlFlatDoc = $(FlatDoc);
	$xmlFlatDoc.find('question').each(function () {
		var $question = $(this);
		var questionid = $question.attr("ID");

		

		questionid = questionid.substring(1);

		var repeat = 0;
		


		//there may be multiple answers per question
		$question.find('answer').each(function () {
			var $test = $(this);
			var id = $test.attr("ID");
			var val = $test.attr("value");

			
			var $targetQuestion = $(xmlDoc).find("Question[ID='" + questionid + "']");
			var targetQuestionId = $targetQuestion.attr("ID");


			if (id != null) {
				 
				var $targetAnswer = $targetQuestion.find("ListItem[ID='" + id + "']");
				$targetAnswer.attr("selected", "true");
				//alert("set selected to true");
				if ($targetAnswer.find("ListItemResponseField") != null) {
					val = $question.find('answer').next().attr("value");
					$response = $targetAnswer.find("Response").children(0);
					$response.attr("val", val);
				}

			}
			else {  //free response
				
				$targetAnswer = $targetQuestion.find("ResponseField").find("Response");
				$targetAnswer.children(0).attr("val", val);
			}
		});

	});
	
   

}

function padLeft (str, max) {
  str = str.toString();
  return str.length < max ? padLeft("0" + str, max) : str;
}


//soap 1.2
function CallSoapSubmit2(data, saveState) {	
	
	/*
	if($('#scriptsubmit').length == 0 | (!$('#scriptsubmit').is(':checked')))
	{
		alert("Script Submit is not supported.");
		//serverSubmit(data);
		return;
	}
    */
	
	try{
		//get DemogFormDesign and FormDesign element only
	xmlDoc = $.parseXML(data);
	$xml = $(xmlDoc);
	
	var $formdesignelement = $xml.find('FormDesign');
			   
	if($xml.find('DemogFormDesign'))
	{
		
		$demog = $xml.find('DemogFormDesign'); 
		$demogNew = $demog.clone(true);
	}

	var $designNew = $formdesignelement.clone(true);
	newDoc = $.parseXML("<SDCSubmissionPackage xmlns='urn:ihe:qrph:sdc:2016'></SDCSubmissionPackage>")
	test = $(newDoc).find("SDCSubmissionPackage");
	
	
	
	//add 2018 - pkgInstanceURI, pkgInstanceVersionURI, pkgPreviousInstanceVersionURI, pkgDateTimeStamp
	//read packageInstanceURI from parameter
	
	//var MsgId = getUrlParameter('MessageId');
	MsgId =  $('#packageInstanceId').text()
	if(MsgId!='')
	  test.attr("pkgInstanceURI",MsgId);
	//else
	//   test.attr("pkgInstanceURI","1223456");	
	   
	//savedstate passed as parameter   
	if(saveState!=null)
		test.attr("pkgInstanceVersionURI",saveState);
	else
	    test.attr("pkgInstanceVersionURI","");
	
	test.attr("pkgPreviousInstanceVersionURI","1223456");  //not used
	
	var d = new Date();

	var month = padLeft(d.getMonth()+1,2);
	var day = padLeft(d.getDate(),2);
	var year = d.getFullYear();
	var hr = padLeft(d.getHours(),2);
	var min = padLeft(d.getMinutes(),2);
	var sec = padLeft(d.getSeconds(),2);
	var fulldate = year + '-' + month + '-' + day + 'T' + hr + ':' + min + ':' + sec;
	//alert(fulldate);
	var dt = new Date("30 July 2010 15:05 UTC");
	//document.write(dt.toISOString());
	//test.attr("pkgDateTimeStamp","2017-11-18T07:53:01");
	//2017-11-18T8:7:34
	test.attr("pkgDateTimeStamp",fulldate);
	test.append($designNew);
	if($demogNew)
	{
		test.prepend($demogNew);						
		
	}

	data = xmlToString(newDoc);

	//read destination url from xml if present
	var webServiceURL = ''
	//var webServiceURL = "http://ajcccalculator.azurewebsites.net/RFDServices/Services/FormReceiver.asmx";

	
	//if webServiceURL is hard-coded here, use that
	if(webServiceURL=='')
	{
		$destinations = $xml.find('Destination');
		
		if($destinations.length>0)								
		{
			$.each($destinations, function(){
				
				webServiceURL = webServiceURL + "|" + $(this).find('Endpoint').attr('val');
			});
		
			webServiceURL = webServiceURL.substring(1);															
			
		}
		else
		{
			alert('submiturl = ' + $("#submiturl").val());
			webServiceURL = $("#submiturl").val();
			if(webServiceURL=='')
			{
				alert("destination not found.");
				return;
				}
		}
	
	}
	else{
		alert("submitting form to: " + webServiceURL);
    }	
	
									
	var ns = 'urn:ihe:iti:rfd:2007';

	$.support.cors = true;
	var xmldata = encodeURIComponent(data);
	
	var soapRequest =
						'<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope"' + 
						' xmlns:urn="' + ns + '">'  + 
						'<soap:Header/>' +
							' <soap:Body>' +
									' <urn:SubmitFormRequest>' +
									 data +
									'</urn:SubmitFormRequest>' +
							' </soap:Body>' +
						' </soap:Envelope>';
								

	$("#submitsoap").val(soapRequest);
	endpoints = webServiceURL.split('|')  //multiple endpoints are separated by |
	numEndpoints = endpoints.length;
	
	//clear before submit
	$("#response").html('');
	
	var currEndpoint='';
	for(i=0;i<numEndpoints;i++)
	{
		currEndpoint = endpoints[i].trim();
		try
		{
			$.ajax({
			type: "POST",
			context:{test:currEndpoint},  //test is the value when call was made and is available in success and error
			url: currEndpoint,
			//contentType: "application/soap+xml;charset=utf-8;",
			contentType: "application/soap+xml;",
			dataType: "xml",
			processData: false,									
			data: soapRequest,
			success: function (response) {OnSuccess(response,this.test)},
			error: function (xhr, message, exception ) {OnError(xhr, message, exception, this.test)}
			});
		}

		catch(err)
		{
			alert("Error when posting submit request to " + currEndpoint + ": " + err);
		}
	
	}
	

	return false;
	}
	
	catch (err)
	{
		alert(err.message);
		return false;
	}

	
	
}


//soap 1.1
function CallSoapSubmit1(data) {
	
	
	$("#response").val("************");
	

	//get DemogFormDesign and FormDesign element only
	xmlDoc = $.parseXML(data);
	$xml = $(xmlDoc);

	var $formdesignelement = $xml.find('FormDesign');
			   
	if($xml.find('DemogFormDesign'))
	{
		
		$demog = $xml.find('DemogFormDesign'); 
		$demogNew = $demog.clone(true);
	}

	var $designNew = $formdesignelement.clone(true);
	newDoc = $.parseXML("<SDCSubmissionPackage xmlns='urn:ihe:qrph:sdc:2016'></SDCSubmissionPackage>")
	test = $(newDoc).find("SDCSubmissionPackage");
	test.append($designNew);
	if($demogNew)
	{
		test.prepend($demogNew);						
		
	}

	data = xmlToString(newDoc);

	//read destination url from xml if present
	var webServiceURL = "";

	//webServiceURLFromPackage = $xml.find('Destination').find('Endpoint').attr('val');
	$destinations = $xml.find('Destination');
	
	if($destinations.length>0)								
	{
		for(i=0;i<$destinations.length;i++)
		{
			webServiceURL = webServiceURL + "|" + $destinations.find('Endpoint').attr('val');									
			
		}
		webServiceURL = webServiceURL.substring(1);															
		
	}
	else
	{
		alert('1.1');
		alert("destination not found.");
		return;
	}
	
	if(webServiceURL!="" & $("#submiturl").val()=="")
		$("#submiturl").val(webServiceURL);
	

	webServiceURL = $("#submiturl").val();  
	
	
	var ns = $("#submitnamespace").val();
	
	$.support.cors = true;
	var xmldata = encodeURIComponent(data);
	
	
	var soapRequest =
						'<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"' + 
						' xmlns:urn="' + ns + '">'  + 
						'<soap:Header/>' +
							' <soap:Body>' +
									' <urn:SubmitFormRequest>' +
									//' <SDCSubmissionPackage xmlns="urn:ihe:qrph:sdc:2016">' +
									 data +
									// ' </SDCSubmissionPackage>' +
									'</urn:SubmitFormRequest>' +
							' </soap:Body>' +
						' </soap:Envelope>';

	
	$("#submitsoap").val(soapRequest);

	//soapAction = "SubmitForm";  
	soapAction = $("#submitaction").val();
	
	endpoints = webServiceURL.split('|')  //multiple endpoints are separated by |
	numEndpoints = endpoints.length;
	
	var currEndpoint='';
	
	//clear before submit
	$("#response")='';
	
	//soapRequest='<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:urn="urn:ihe:iti:rfd:2007"> <soap:Header/><soap:Body><urn:SubmitFormRequest>test</urn:SubmitFormRequest> </soap:Body></soap:Envelope>'
	//alert(soapRequest);
	for(i=0;i<numEndpoints;i++)
	{
		currEndpoint = endpoints[i].trim();
		
		$.ajax({
		type: "POST",
		context:{test:currEndpoint},  //test is the value when call was made and is available in success and error
		url: currEndpoint,
		contentType: "text/xml",
		dataType: "xml",
		processData: false,
		headers: {
			"SOAPAction": soapAction  
		},
		data: soapRequest,
		success: function (response) {OnSuccess(response,this.test)},
		error: function (response) {OnError(response, this.test)}
	});
	
	}
	

	

	return false;
	
}




function OnSuccess(data, url) {
	
	
	var xmlstring = xmlToString(data);

	xmlstring = formatXml(xmlstring);
	xml_escaped = xmlstring.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/ /g, '&nbsp;').replace(/\n/g,'<br />');

	

	if (document.getElementById("response") != null) {       
		
		$("#response").append("Received Response from " + url  + " - <PRE>" + xml_escaped + '</PRE>');
		$("#response").css("background-color", "yellow");
		$("#response").css("display", "block");
		
		
	}
	
	
}

function OnError(xhr, textStatus, errThrown, url) {
	//CORS error can only be see in Chrome 
	var xmlstring = xhr.responseText;
	

	
	if (document.getElementById("response") != null) {       
		$("#response").append("Receiver Response from " + url + " - <PRE>" + xmlstring + '</PRE>');
		$("#response").css("background-color", "white");
		$("#response").css("color", "red");
		$("#response").css("display", "block");
	}
	
}

//https://gist.github.com/sente/1083506
function formatXml(xml) {
	var formatted = '';
	var reg = /(>)(<)(\/*)/g;
	xml = xml.replace(reg, '$1\r\n$2$3');
	var pad = 0;
	jQuery.each(xml.split('\r\n'), function(index, node) {
		var indent = 0;
		if (node.match( /.+<\/\w[^>]*>$/ )) {
			indent = 0;
		} else if (node.match( /^<\/\w/ )) {
			if (pad != 0) {
				pad -= 1;
			}
		} else if (node.match( /^<\w([^>]*[^\/])?>.*$/ )) {
			indent = 1;
		} else {
			indent = 0;
		}

		var padding = '';
		for (var i = 0; i < pad; i++) {
			padding += '  ';
		}

		formatted += padding + node + '\r\n';
		pad += indent;
	});

	return formatted;
}



function getAnswerItemByID(ID, container)
{
	//finds an answer item within container element
	try{
		//alert(ID + ':' + $(container).find(':input[value^="' + ID + '"]').length);
		return $(container).find(':input[value^="' + ID + '"]');
		}
	catch(e)
	{
		alert(e + ", ID = " + ID);
	}

}


function getAnswerID(answerElement)
{
	
	var id = $(answerElement).attr('value');
	if (id === undefined || id === null) {
		 // do something 
		 alert("Error in getAnswerID: value attribute not found.");
		 alert($(answerElement).length);
	}
	if(id.indexOf(',')>0)
		id = id.substring(0, id.indexOf(','));
	return id;
}
/*
function getQuestionID(answerElement)
{
	var id = answerElement.attr('name');
	return id;
}
*/

function getSectionID(answerElement)
{
	var $section = $(answerElement).parentsUntil('table').parent();
	//alert($section.get(0).nodeName);
	var sectionid = '';
	if($section.length>0)
	{
		sectionid = $section.get(0).id.substring(1);
	}
	return sectionid;
}

function getAnswerSection(answerElement)
{
   //returns DOM element
	var $section = $(answerElement).parentsUntil('table').parent();
	
	//return the first in the list
	return $($section.get(0));
}

function isSDSAnswer(answerElement)
{
	//alert('isSDS');
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	var attr = $answer.attr('selectionDeselectsSiblings');

	// For some browsers, 'attr' is undefined; for others,
	// 'attr' is false.  Check for both.
	if (typeof attr !== typeof undefined && attr !== false) {
		return true;
	}
	else{
		return false;
	}
}

function isFillinAnswerChoice(answerElement)
{
//checks if the checkbox or radio button has fillin-abswer box
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	//alert($answer.child().attr(
	if ($answer.has('> ListItemResponseField').length>0)
		return true;
	else
		return false;	
}

function isFillinInput(answerElement)
{
	//checks if the input box is a fill-in answer input box
	alert($(answerElement).prev().type());
}


function isSDCAnswer(answerElement)
{
	//alert('isSDC');
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	var attr = $answer.attr('selectionDisablesChildren');

	// For some browsers, 'attr' is undefined; for others,
	// 'attr' is false.  Check for both.
	if (typeof attr !== typeof undefined && attr !== false) {
		return true;
	}
	else{
		return false;
	}		
}

function isNestedAnswer(answerElement)
{
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	if($answer.length==0)
		alert('Error in isNestedAnswer');
	var $parent = $answer.parentsUntil('Question').parent();
	if($parent.length==0)
		alert('Error in isNestedAnswer');
	
	if($parent.parent().get(0).nodeName=='ChildItems')
	{
		if($parent.parent().parent().get(0).nodeName=='ListItem')
			return true;
		else
			return false;
	}
	else
	{
		return false;
	}
}

function getAnswerSiblings(answerElement)
{
	//returns xml nodes
	
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	return $answer.siblings('ListItem');
}

function getNestedAnswers(answerElement)
{
	//return xml nodes
	
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	return $answer.children("ChildItems").children("Question").children("ListField").children("List").children("ListItem");
}

function getParentAnswerID(answerElement)
{
	//finds answer in xml and returns parentAnswerID
	var answerid = getAnswerID(answerElement);
	var sectionid = getSectionID(answerElement);
	var $answer = $xml.find('Section[ID="' + sectionid + '"]').find('ListItem[ID="' + answerid + '"]');
	if($answer.length==0)
	{
		alert('Error in getParentAnswerID: answerElement not found.answerid=' + answerid);
	}
	var $parent = $answer.parentsUntil('Question').parent();
	if($parent.length==0)
		alert('Error in getParentAnswerID: Question element not found.');
	if($parent.parent().get(0).nodeName=='ChildItems')
	{
		if($parent.parent().parent().get(0).nodeName=='ListItem')
		{
			return $parent.parent().parent().attr('ID');
		}
		else{
			//no parent answer
			return "";
		}
	}
	else
		alert('Error in getParentAnswerID');

}


function isAnswerSelected(answerElement)
{
	return $(answerElement).is(':checked');

}

function SelectAnswer(answerElement, section)
{
	$(answerElement).prop('checked',true);
	$(answerElement).parent().find('.AnswerTextBox').prop('disabled',false);
	
	//if there are any SDS answers they will need to be unchecked
	var $siblings = getAnswerSiblings(answerElement);
	$siblings.each(function() {
		var id = $(this).attr("ID");
		var testElement = getAnswerItemByID(id, section);
		if(isSDSAnswer($(answerElement)))  //if this answer element is SDS, unselect all siblings
		{
			UnSelectAnswer(testElement, section);
		}
		else if(isSDSAnswer(testElement))//if this sibling is SDS 
		{
		   UnSelectAnswer(testElement, section);
		}
		else if ($(answerElement).is(":radio"))  //if this is a single select make sure the siblings and their children are unselected
		{
			
			UnSelectAnswer(testElement, section);
		}
	}
	)
	
}

function UnSelectAnswer(answerElement, section)
{
	//unselects answerElement and its children recursively
	if(!$(answerElement).is(":checkbox, :radio")) return;
	
		$(answerElement).prop('checked',false);
		
	//unselect all child answers
	var $childanswers = getNestedAnswers(answerElement);
	
	$childanswers.each(function(){
		var childid = $(this).attr("ID");									
		
		var childelement = getAnswerItemByID(childid, section);
		
		UnSelectAnswer(childelement,section);
	})
		

}

function DisableAnswer(answerElement)
{
	$(answerElement).prop('disabled',true);
	//disble children
	$(answerElement).find('*').prop('disabled', true);
	
	
	
}

function EnableAnswer(answerElement)
{
	$(answerElement).prop('disabled',false);
}

function getSelectedListItems(listitems, section)
{
	var $selecteditems = [];
	
	listitems.each(function() {
		var $answeritem = getAnswerItemByID($(this).attr('ID'),section);
		if(($answeritem).is(':checked'))
			$selecteditems.push($answeritem);
	});
	
	return $selecteditems;
}



function UncheckChildAnswers(currentInput)
{
	var test;
	
	$(currentInput).find(':input:checked').prop('checked', false);
}



function UncheckSiblings(currentInput)
{

}
function getSelectedSiblings(currentInput)
{
	var count =0 ;
	var siblings = $(currentInput).parent().siblings();
	siblings.each(function() {
		if($(this).get(0).className=='Answer')
		{
			
			if ($(this).find(':input').is(':checked'))
				count++;
		}
			
	});
	if($(currentInput).is(':checked'))
		count++;
	return count;
}

function SelectUnselectDescendents(parentQuestion, event)
{

}

function SelectUnselectChoiceOnBlur(choiceID, element)
{
	var $section = getAnswerSection(element);
	
	var $answeritem = getAnswerItemByID(choiceID, $section);
	
	return;
	
	if($(element).val()!='')
		SelectAnswer($answeritem, $section);
	else
		UnSelectAnswer($answeritem, $section);
}

function OnChoiceBlur(choiceID, element)
{
	var $section = getAnswerSection(element);
	
	//alert(choiceID);
	var $answeritem = getAnswerItemByID(choiceID, $section);
    	
	
	var $input = $answeritem.parent().find('.AnswerTextBox');
	
	$input.removeClass('error');
	
	if(!$answeritem.is(':checked'))
	{
		
		if($input.val()!='')
		{
			
			//$answeritem.siblings('input').addClass('error');
			//$input.removeClass('error');
			//$input.addClass('error');
			//alert($input.val());
		}
		//$input.prop('disabled',true);
		
	}
}

//changing fill-in value calls this function
function SelectChoiceOnKeyPress(choiceID, element, event)
{
	
	//if control characters return
	var keycode = (event.keyCode ? event.keyCode : event.which);
	
	if(keycode==8) return;
		
	
	var $answeritem = $(element).parent().find('input:radio, input:checkbox');
	
	$($answeritem).prop('checked',true);
	
	var $section = getAnswerSection($answeritem.get(0));
	var parentid = getParentAnswerID($answeritem.get(0));
	if(parentid==null || parentid=='')
		return;
	
	var parent = getAnswerItemByID(parentid, $section);
	
	SelectAnswer(parent, $section);	
}


/*
1. compares value to a compare value passed as part of condition
2. condition contains the name of the compare function and the value to compare to
3. returns true or false
*/
function Evaluate(value, condition)
{
  
    if(condition.indexOf("hasValueLT")>=0)
	{
		var compareValue = condition.split(':')[1];
		if(parseInt(value)<parseInt(compareValue))
		{
			
			return true;
		}
		   
		else
		{
			
			return false;
		}
			
	}
	else if (condition.indexOf("hasSelectionsGTE")>=0)
	{
		alert('not implemented');
		return false;
	}
}
/*
selects/unselects an option
target - radio option to set value to (true or false) if either no evalClause condition is specified or if evalClause condition is true if evalClause is supplied
setValue - boolean value to set
evalClause - optional - if supplied contains attribute names (targetNames, and actSelect) and associated values of SetAttributeValue node under AttributeEval node
elseEval - optional - if supplied contains attribute names (targetNames, and actSelect) and associated values of SetAttributeValue node under Attribute/Else node 
*/
function SelectUnselectTarget(target, setValue, evalClause, elseEval)
{
		
   if(target!='')  {		
		
		if(evalClause!=null)
		{			
			eval_parms = evalClause.split(',');
			eval_target = eval_parms[0].split(':')[1];			
			eval_function = eval_parms[1];
			
			//get question id of source from xml	
			$eval_targetOptionId = $xml.find("[name='" + eval_target + "']").closest("ListItem").attr("ID");
			
			$eval_target_value = $(document).find(':input[value^="' + $eval_targetOptionId + '"]').siblings(":input").val();
			
			
			if(Evaluate($eval_target_value,eval_function))
			{
				$id = $xml.find('ListItem[name=' + target + ']').attr('ID');		
				$(document).find(':input[value^="' + $id + '"]').prop('checked',setValue);
			}
			else
			{	
				elseTarget = elseEval.split(':')[0].replace("{","");
				elseValue = elseEval.split(':')[1].replace("{","").replace("}","");
				
				$id = $xml.find('ListItem[name=' + elseTarget + ']').attr('ID');
				
				$(document).find(':input[value^="' + $id + '"]').prop('checked',elseValue);
			}
		}
		else
		{
		
			$id = $xml.find('ListItem[name=' + target + ']').attr('ID');
			
			$(document).find(':input[value^="' + $id + '"]').prop('checked',setValue);
		
		}
		
   }
}

function EnableDisableElementById(target, value)
{
  $table = $(document).find("[id*='" + target + "']").first();
   $table.find('input').attr("disabled",true);  
}

function EnableDisableElementByName(target, value)
{
   
   var id = $xml.find("[name='" + target + "']").attr('ID');  
   $table = $(document).find("[id*='" + id + "']").first();
   $table.find('input').attr("disabled",true);  
   
  			
   
}

//10/25/2018 - completed and tested
/*
eval parameter is not implemented yet
*/
function SetValueOnTextChange(target, source, eval)
{
  
	if(source=='' || target=='')
		return; 
		
	
	
	//get question id of source from xml	
	$sourceQNodeId = $xml.find("[name='" + source + "']").closest("Question").attr("ID");
	
	
    //get text value to copy from DOM
	var sourceVal = $(document).find(':input[name^="' + $sourceQNodeId + '"]:checked').siblings(":input").val();

	    
 	//first get target listitem ID from xml  -- list item is the parent of response field (target)
 	$targetListItemNodeId = $xml.find("[name='" + target + "']").closest("ListItem").attr("ID");
	
	//now find the target listitem in DOM and check it if sourceVal !=''
	var $targetOption = $(document).find(':input[value^="' + $targetListItemNodeId + '"]'); 
	if($targetOption.length==1)  //found the target listitem 
	{	
		if(sourceVal!='')
		{
			$targetOption.prop('checked',true);
			
			//now find the textbox that is sibling of the list item in DOM and set sourceVal
			$targetOption.siblings(":input").val(sourceVal);
		}
		else
		{
			$targetOption.prop('checked',false);
			
			//now find the textbox that is sibling of the list item in DOM and set val to empty
			$targetOption.siblings(":input").val('');
		}
	}
	else{
		alert("No target option found");
		return;
	}
	
}



function SelectUnselectParents(parentQuestion, element)
{
	
	/* parentQuestion: questionid of the answer clicked */
	if (!$(element).is(":checkbox, :radio"))  //only if the element is a radio or a checkbox
	{
		
		return;
	}
	var issingleselect = $(element).is(":radio");
	var answerid = getAnswerID(element);  //$(event.target).attr("name");
	
	var selecttype = $(element).attr('type');
	var $section = getAnswerSection(element);
	var $answeritem = getAnswerItemByID(answerid, $section);
	var selected = $answeritem.is(':checked');  //value is true or false
	var sectionId = getSectionID(element);
	
	
	var $siblings = getAnswerSiblings(element);
	var $childanswers = getNestedAnswers(element);
	
	
	var parentid = getParentAnswerID(element);
	
	
	
	var selectedsiblings = $(getSelectedListItems($siblings,$section)).length;
	
	/*
	SELECT/UNSELECT PARENT ANSWER
	if current item or one or more of its siblings are selected
	  select the parent answer
	*/
	if( selected || (selectedsiblings > 0))
	{
		//select parent answer
		
		if(parentid=="")
		{
			//alert('no parent answer');
		}
		else{
			var parent = getAnswerItemByID(parentid, $section);	
			
			//if parent is SDC, don't select it 
			if(!isSDCAnswer(parent))
			{	
				SelectAnswer(parent, $section);											
			}
		}									
		
	}
	else  //neither this answer nor any of its siblings are selected, unselect parent
	{
		//unselect parent answer
		UnSelectAnswer(getAnswerItemByID(parentid, $section),$section);
	
	}
	
	/*
	 UNSELECT all child answers if current answer is not selected
	*/
	
	if(selected==false){
		//unckeck all children
		
		$childanswers.each(function(){
			var childid = $(this).attr("ID");
			UnSelectAnswer(getAnswerItemByID(childid, $section),$section);
		})
	}
	
	
	//selection disables children
	if (isSDCAnswer(element) & selected)
	{
	   try{
			//go to the Answer level and disable all answer choices at first
			$(element).parent().parent().find('* [type=checkbox], [type=radio], [type=text]').prop('disabled', true);
			//uncheck all answers
			$(element).parent().parent().find('* [type=checkbox],[type=radio]').prop('checked', false);
			//enable just this answer
			EnableAnswer(element);  //EnableAnswer will enable checkbox/radio and fill-in box 
			//check just this answer
			SelectAnswer(element, $section);
	   }
	   
	   catch(err)
	   {
			alert(err);
	   }
		
		
	}
	else 
	{
		//enable children
										
		try{
			$(element).parent().parent().find('* [type=checkbox], [type=radio]').prop('disabled', false);
			
	   }
	   
	   catch(err)
	   {
			alert(err);
	   }
	
	}
	
	
	//selection deselects siblings
	if(isSDSAnswer(element) & selected)
	{
		//unselect all siblings
		$siblings.each(function() {
	
			var id = $(this).attr("ID");
			UnSelectAnswer(getAnswerItemByID(id, $section), $section);  //unselect will disable fill-in also
		})
	}
	else if(!isSDSAnswer(element) & selected)
	{
		//all SDSAnswers must be unselected
		$siblings.each(function() {
			
			var id = $(this).attr("ID");
			var testElement = getAnswerItemByID(id, $section);
			
			if(isSDSAnswer(testElement))
			   UnSelectAnswer(testElement, $section);    //unselect will disable fill-in also
			
		})
	
	}
	
	
	
	//enable/disable fillin boxes
	if(selected & isFillinAnswerChoice(element))
	{
		//enable fillin box
		//var $input = $(element).parent().find('.AnswerTextBox');
		//$input.prop('disabled',false);
		
	}
	else if (!selected & isFillinAnswerChoice(element))
	{
		//disable fillin box
		//var $input = $(element).parent().find('.AnswerTextBox');
		//$input.prop('disabled',true);
		
	}
	
	if(selected & $(element).is(":radio"))
	{ 
		$siblings.each(function(){
			var sibling = $(this);
		UnSelectAnswer(sibling, $section);
				
		});
	}

	
}

