<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PP.aspx.cs" Inherits="SecuLobbyVMS.PP" %>

<!-- =========================================================
* Sneat - Bootstrap 5 HTML Admin Template - Pro | v1.0.0
==============================================================

* Product Page: https://themeselection.com/products/sneat-bootstrap-html-admin-template/
* Created by: ThemeSelection
* License: You must have a valid license purchased in order to legally use the theme for your project.
* Copyright ThemeSelection (https://themeselection.com)

=========================================================
 -->
<!-- beautify ignore:start -->
<!DOCTYPE html>
<html lang="en" >
	<head>
	  	<meta charset="UTF-8">
	  	<link rel="stylesheet" href="./assets/ppt/style.css">
		<link rel='stylesheet' href='./assets/ppt/bootstrap.min.css'>
		<link rel="stylesheet" type="text/css" href="./assets/ppt/bootstrap-grid.min.css"/>
		<link rel="stylesheet" href="./assets/ppt/jquery.skippr.css">
		<script src="./assets/ppt/jquery-1.8.3.min.js"></script>
		<script src="./assets/ppt/jquery.skippr.js"></script> 
		<script src="./assets/ppt/Scan.js"></script>
		<script src="./assets/ppt/ScanData.js"></script>
		<script src="./assets/ppt/Setting.js"></script>

		<script>
      var bEnableAdvanced = true;
      $(function () {
        $('#skippr0').skippr();
      });
      $(document).ready(function () {
        var rangeSlider = function () {
          var slider = $('.range-slider'),
            range = $('.range-slider input[type="range"]'),
            value = $('.range-value');
          slider.each(function () {
            value.each(function () {
              var value = $(this).prev().attr('value');
              $(this).html(value);
            });
            range.on('input', function () {
              $(this).next(value).html(this.value);
            });
          });
        };
        rangeSlider();
      });

      var Supported_Languages = { 0: "English", 1: "Traditional", 2: "Simplified Chinese" };
      var language = Supported_Languages[0];
		</script>
	</head>
	
	<body style="background: #d5d6d7;">
		<header>		
			<script>
        document.write('<div class="header">');
        document.write('<div class="header__title">' + GetStr(language, "Web FX Scan") + '</div>')
        document.write('<div class="header__picture"><a  href="https://plustek.com/tw/" target="_blank"><img src=" ./assets/ppt/img/background.gif"></a></div>');
        document.write('</div>');
			</script>
			
			<div class="item">
			  <textarea class="copy-area" id="text_cmd_hidden" style=" height:0px; width:0px;">no comment</textarea>
			  <button class="btn-copy" id="btn_cmd_hidden" style=" height:0px; width:0px;"></button>
			</div>
		</header>
		
		<div class="container_all">
			<div class="setting">
			    <!--multisteps-form-->
			    <div class="setting" id="multisteps-form">
			        <!div class="absolute">
					    <!--content inner-->
					  	<div class="content__inner">
					    	<div class="container overflow-hidden">
					        		<div class="row">
					          			<div class="col-12 col-lg-8 ml-auto mr-auto mb-4">
					            			<div class="multisteps-form__progress">
								        		<nav class="menu">
													<a class="menu__item" id="i-0">
														<img class="menu__icon" src="./assets/ppt/img/main.svg"/>
														<script>document.write('<span class="menu__text">' + GetStr(language, "Main Setting") + '</span>')</script>
													</a>

													<a class="menu__item" id="i-1">
														<img class="menu__icon" src="./assets/ppt/img/iman.svg"/>
														<script>document.write('<span class="menu__text">' + GetStr(language, "Image Setting") + '</span>')</script>
													</a>
													<a class="menu__item" id="i-2">
														<img class="menu__icon" src="./assets/ppt/img/OCR.svg"/>
														<script>document.write('<span class="menu__text">' + GetStr(language, "OCR") + '<br>' + GetStr(language, "Setting") + '</span>')</script>
													</a>
													<a class="menu__item" id="i-3">
														<img class="menu__icon" src="./assets/ppt/img/ACTION.svg"/>
														<script>document.write('<span class="menu__text">' + GetStr(language, "Action") + '</span>')</script>
													</a>
												    <div id="active"></div>
												    <div id="active-2"></div>
												    <div id="active-3"></div>
												</nav>
											</div>		
										</div>
									</div>		
					        		<!--form panels-->
					        		<div class="row">
					          			<div class="col-12 col-lg-8 m-auto">
					            			<form class="multisteps-form__form">
					              				<!--single form panel-->
					              				<div class="multisteps-form__panel shadow p-4 rounded bg-white js-active" data-animation="scaleIn">
					              					<script>
                                    document.write('<h3 class="multisteps-form__title">' + GetStr(language, "Main Setting") + '</h3>');
													</script>
					          
					                				<div class="multisteps-form__content">
					                					<!div class="sidebar_top">
					                						<dt>
													        <script>document.write('<div class=label>'
                                      + "&nbsp" + GetStr(language, "DeviceName"))</script>
													        :&nbsp;&nbsp;
													        <select class="select1" name="List" id="List" onchange="ChangeScanData(this.value)">
													        </select>
													        &nbsp;&nbsp;<input type="image" src="./assets/ppt/img/reload.png" style="width: 25px; height: 23px; vertical-align: top;" onclick="GetDevicesList()" title="Reload">
													        <script>document.write('</div>');</script>
													    <!/div>
													    <p></p>	
    													<dt>
															<script>document.write('<div class=label>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "ScanType"))</script>
															:&nbsp;&nbsp;
															<select class="select2" name="sel_source" id="sel_source" onchange="ChangeSource(this.value)">
															</select>
															<script>document.write('</div>');</script>
													    <p></p>
								                        <dt>
								                        
															<script>document.write('<div class=label>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "PageSize"))</script>
															:&nbsp;&nbsp;
															<select class="select2" name="sel_paper-size" id="sel_paper-size">
															</select>
															<script>document.write('</div>');</script>
															<p></p>
														
														<dt>
															<script>document.write('<div class=label>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "ScanMode"))</script>
															:&nbsp;&nbsp;
															<select class="select2" name="sel_mode" id="sel_mode">
															</select>
															<script>document.write('</div>');</script>
															<p></p>
														<dt>
															<script>
                                if (bEnableAdvanced == true) {
                                  document.write('<div class=label>' + "&nbsp" + GetStr(language, "ImageFormat"));
                                  document.write(' : &nbsp;&nbsp;');
                                  document.write('<select class="select2" name="sel_imgfmt" id="sel_imgfmt">');
                                  document.write('</select><br>');
                                  document.write('<p></p>');
                                  document.write('</div>');
                                }
															</script>
														<dt>
															<script>document.write('<div class=label>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "Resolution"))</script>
															:&nbsp;&nbsp;
															<select class="select2" name="sel_resolution" id="sel_resolution">
															</select>
															<script>document.write('</div>');</script>
															<p></p>
														<dt>
														<dt>
															<script>document.write('<div class=label>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "Profile"))</script>
															:&nbsp;&nbsp;
															<select class="select2" name="sel_profile" id="sel_profile">
															</select>
															<script>document.write('</div>');</script>
															<p></p>
														<dt>
														<dt>
														<div class = "row"><div class="button-row d-flex mt-4"></div>
					                  					</div>

														<div class="range_slider_all">
															<script>document.write('<div class=label_slider>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "Brightness") + ' :')
                                document.write('</div>');</script>
															<div class="range-slider">
																<input type="range" value="0" min="-100" max="100" onchange="changeValue(this.value,'text_brightness')" id="range_brightness" style="width: 40%; left:90px; display: inline-block;">
															 	
													            <!span class="range-value"><!/span>
													            <input type="text" id="text_brightness" class="range-value"style=" left:100px;">
													            	
													        </div>   
												        </div>
											
														<div class = "row"><div class="button-row d-flex mt-4"></div> </div>
														<div class = "row"><div class="button-row d-flex mt-4"></div>
					                  					</div>

												        <div class="range_slider_all">
															<script>document.write('<div class=label_slider>' + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" + GetStr(language, "Contrast") + ' :')
                                document.write('</div>');</script>
															<div class="range-slider">
																<input type="range" value="0" min="-100" max="100" onchange="changeValue(this.value,'text_contrast')" id="range_contrast" style="width: 40%; left:90px; display: inline-block;">
															 	
													            <!span class="range-value"><!/span>
													            <input type="text" id="text_contrast" class="range-value"style=" left:100px;">
													            	
													        </div>   
												        </div>   

												        <div class = "row"><div class="button-row d-flex mt-4"></div> </div>
														<div class = "row"><div class="button-row d-flex mt-4"></div>
					                  					</div>
					                  						

												        <div class="range_slider_all">
															<script>document.write('<div class=label_slider>' + "&nbsp" + GetStr(language, "ImageQuality") + ' :')
                                document.write('</div>');</script>
															<div class="range-slider">
																<input type="range" value="75" min="1" max="100" onchange="changeValue(this.value,'text_quality')" id="range_quality" style="width: 40%; left:90px; display: inline-block;">
															 	
													            <!span class="range-value"><!/span>
													            <input type="text" id="text_quality" class="range-value"style=" left:100px;">
													            	
													        </div>   
												        </div>  

												        <div class = "row"><div class="button-row d-flex mt-4"></div>
					                  					</div>   
														
														<div class="button-row d-flex mt-4">
					                    					<script>		
                                          document.write('<button class="btn btn-primary ml-auto js-btn-next" type="button" title="Prev" style="width:58px;height:29px;line-height:15px; font-size: 17px; font-family:  "Calibri Regular";">' + GetStr(language, "Next") + '</button>');
						                      				</script>
					                  					</div>
					                				</div>
					              				</div>
					              				<!--single form panel-->
					              				<div class="multisteps-form__panel shadow p-4 rounded bg-white" data-animation="scaleIn">
					              					<script>
                                    document.write('<h3 class="multisteps-form__title">' + GetStr(language, "Image Setting") + '</h3>');
													</script>
					                				<div class="multisteps-form__content">
					                					<p> </p>
						                					<div class="ckbx-style-15-label">
											                    <script>document.write(GetStr(language, "Auto Crop"))</script>
											                </div>                                            
											                <div class="ckbx-style-15">                                
											                    <input type="checkbox" id="chk_swcrop" value="0" name="chk_swcrop">
											                   <label for="chk_swcrop"></label>
											                </div>
											                <p> </p>	
											                <div class="ckbx-style-16-label">
											                    <script>document.write(GetStr(language, "Auto Deskew"))</script>
											                </div>                                            
											                <div class="ckbx-style-16">                                
											                    <input type="checkbox" id="chk_swdeskew" value="0" name="chk_swdeskew">
											                   <label for="chk_swdeskew"></label>
											                </div>
											                <p> </p>	
											                <div class="ckbx-style-15-label">
											                    <script>document.write(GetStr(language, "Front Eject"))</script>
											                </div>                                            
											                <div class="ckbx-style-15">                                
											                    <input type="checkbox" id="chk_fronteject" value="0" name="chk_fronteject">
											                   <label for="chk_fronteject"></label>
											                </div>

											                <div class="ckbx-style-16-label">
											                    <script>document.write(GetStr(language, "Manual Eject"))</script>
											                </div>                                            
											                <div class="ckbx-style-16">                                
											                    <input type="checkbox" id="chk_manualeject" value="0" name="chk_manualeject">
											                   <label for="chk_manualeject"></label>
											                </div>

											                <div class="ckbx-style-15-label">
											                    <script>document.write(GetStr(language, "Remove Blank Page"))</script>
											                </div>                                            
											                <div class="ckbx-style-15">                                
											                    <input type="checkbox" id="chk_removeblankpage" value="0" name="chk_removeblankpage">
											                   <label for="chk_removeblankpage"></label>
											                </div>
															<script>
                                if (bEnableAdvanced == true) {
                                  document.write('<div class="ckbx-style-16-label">' + GetStr(language, "Remove Black Edges") + '</div>')
                                  document.write('<div class="ckbx-style-16"><input type="checkbox" id="chk_rm_blackedges"  value="0" name="chk_rm_blackedges"><label for = "chk_rm_blackedges"></label></div>')
                                  document.write('<div class="ckbx-style-15-label">' + GetStr(language, "Denoise") + '</div>')
                                  document.write('<div class="ckbx-style-15"><input type="checkbox" id="chk_denoise" value="0" name="chk_denoise"><label for = "chk_denoise"></label></div>')

                                }
															</script>
											                <div class="ckbx-style-16-label">
											                    <script>document.write(GetStr(language, "Multifeed Detect"))</script>
											                </div>

											                <div class="ckbx-style-16">                                
											                    <input type="checkbox" id="chk_multifeeddetect" value="0" name="chk_multifeeddetect">
											                   <label for="chk_multifeeddetect"></label>
											                </div>
	
															
														    <p> </p>		
														    <div class="ckbx-style-15-label">
														        <script>document.write(GetStr(language, "Split"))
														        //style="display:none">
														        </script>
														    </div>                                            
														    <div class="ckbx-style-15">
														        <input type="checkbox" id="chk_split" value="0" name="chk_split">
														        <label for="chk_split"></label>
														    </div>

														    <div class="ckbx-style-16-label" >
														        <script>document.write(GetStr(language, "Duplex Merge"))
                                      style = "display:none" >
														        </script>
														    </div>                                            
														    <div class="ckbx-style-16" >
														        <input type="checkbox" id="chk_duplexmerge" value="0" name="chk_duplexmerge"style="display:none>
														        <label for="chk_duplexmerge"></label>
														    </div>



														    	
														    <br>
															
					                  						<div class = "row">
																<div class="button-row d-flex mt-4"> </div>
					                  						</div>	
					                  						
															
																<div class="button-row d-flex mt-4" style="display:block;">
							                    					

							                    					<script>
                                                document.write('<button class="btn btn-primary js-btn-prev" type="button" title="Prev" style="width:58px;height:29px;line-height:15px; font-size: 17px; font-family:  "Calibri Regular";">' + GetStr(language, "Prev") + '</button>');
                                              document.write('<button class="btn btn-primary ml-auto js-btn-next" type="button" title="Prev" style="width:58px;height:29px;line-height:15px; font-size: 17px; font-family:  "Calibri Regular";">' + GetStr(language, "Next") + '</button>');

						                      						</script>
					                    						</div>
					                				</div>
					              				</div>
					              				<!--single form panel-->
					              				<div class="multisteps-form__panel shadow p-4 rounded bg-white" data-animation="scaleIn">
					              					<script>
                                    document.write('<h3 class="multisteps-form__title">' + GetStr(language, "OCR Setting") + '</h3>');
													</script>
					                				
					                				<div class="multisteps-form__content">
					                					<p> </p>
					                					<dt>
					                						<p></p>
					                						<p></p>
															<script>
                                //if (language != "Simplified Chinese") {
                                document.write('<div class=label>' + GetStr(language, "Recognize Type"));
                                document.write(' : &nbsp;&nbsp;&nbsp;');
                                document.write('<select class="select3" name="sel_recognize_type" id="sel_recognize_type" onchange="ChangeFulltextStatus(this.value)">');
                                document.write('</select><br> </div>');
																//}
															</script>
															<p></p>
														<dt>
															<script>
                                //if (language != "Simplified Chinese") {
                                document.write('<div class=label>' + "&nbsp;&nbsp;&nbsp;&nbsp;" + GetStr(language, "FullText Type"));
                                document.write(' : &nbsp;&nbsp;&nbsp;');
                                document.write('<select disabled class="select3" name="sel_fulltext_type" id="sel_fulltext_type">');
                                document.write('</select><br> </div>');
																//}
															</script>
															<p></p>
														<dt>
															<script>
                                if (language != "Simplified Chinese" && bEnableRegzLang == true) {
                                  document.write('<div class=label>' + GetStr(language, "Recognize Lang"));
                                  document.write(' : &nbsp;&nbsp;&nbsp;');
                                  document.write('<select disabled class="select3" name="sel_recognize_lang" id="sel_recognize_lang">');
                                  document.write('</select><br></div>');
                                }
															</script>
														<dt>
															<div class="row">
						                    					<div class="button-row d-flex mt-4 col-12">	
						                    						<script>
                                              document.write('<button class="btn btn-primary js-btn-prev" type="button" title="Prev" style="width:58px;height:29px;line-height:15px; font-size: 17px; font-family:  "Calibri Regular";">' + GetStr(language, "Prev") + '</button>');
                                              document.write('<button class="btn btn-primary ml-auto js-btn-next" type="button" title="Prev" style="width:58px;height:29px;line-height:15px; font-size: 17px; font-family:  "Calibri Regular";" >' + GetStr(language, "Next") + '</button>');
						                      						</script>
						                    					</div>
						                  					</div>
					                  					<dt>
					                				</div>
					              				</div>
					              				<!--single form panel-->
					              				<div class="multisteps-form__panel shadow p-4 rounded bg-blus" data-animation="scaleIn">
					                				<script>
                                    document.write('<h3 class="multisteps-form__title">' + GetStr(language, "Action") + '</h3>');
													</script>
					                				<div class="multisteps-form__content">
					                					<p> </p>
					                					<p> </p>
					                					<p> </p>
					                					<script>
                                      document.write('<input type="button" class="button" value="' + GetStr(language, "Set Property") + '" style="width: 30%" onclick="SetProperty()">');
														</script>
														<script>
                              document.write('<input type="button" class="button" value="' + GetStr(language, "Scan") + '" style="width: 30%" onclick="Scan()">');
														</script>
														<script>
                              document.write('<input type="button" class="button" value="' + GetStr(language, "FastDownload") + '" style="width: 30%;" onclick="FastDownload()"> ');
														</script>
												 		<p></p>
														<script>
                              document.write('<input type="button" class="button" value="' + GetStr(language, "Manual Eject") + '" style="width: 30%;" onclick="ManualEject()"> ');
														</script>	        
														<script>
                              document.write('<input type="button" class="button" value="' + GetStr(language, "File List") + '" style="width: 30%;" onclick="GetFileList(RecvData)">');
														</script>
														<script>
                              //if (language != "Simplified Chinese") {
                              document.write('<input type="button" class="button" value="' + GetStr(language, "FastOCR") + '" style="width: 30%;" onclick="FastOCR()"> ');
                              document.write('<br>');
														</script>
														<p></p>
														<script>
                              document.write('<input type="button" class="button_1" value="' + GetStr(language, "OCR") + '" style="width: 30%;" onclick="OCR()"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ');
                              document.write('<input type="text" id="text_OCRfilename" style="width: 60%;" placeholder="IMG_523871156_00001">');
															//}
														</script>
														<p></p>
														<script>
                              document.write('<input type="button" class="button_1" id="btn_Preview" value="' + GetStr(language, "Delete File") + '" style="width: 30%;" onclick="DelFile()"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ');
                              document.write('<input type="text" id="text_delfilename" style="width: 60%;" placeholder="IMG_523871156_00001.jpg">');
														</script>
														<p></p>
														<script>
                              document.write('<input type="button" class="button_1" id="btn_getImageFile" value="' + GetStr(language, "Get Image File") + '" style="width: 30%;"  name="btn_getImageFile" onclick="getImageFile(false)"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ');
                              document.write('<input type="text" id="text_Getfilename" style="width: 60%;" placeholder="IMG_523871156_00001.jpg">');
														</script>
														<p></p>
														<script>
                              document.write('<input type="button" class="button_1" id="btn_getImageFile" value="' + GetStr(language, "Get Thumbnail") + '" style="width: 30%;" name="btn_getImageFile" onclick="getImageFile(true)"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ');
                              document.write('<input type="text" id="text_Getthumfilename" style="width: 60%;" placeholder="IMG_523871156_00001_thumbnail.jpg">');
															//document.write('</div>');
														</script>
														<p></p>
														<script>
                              document.write('<input type="button" class="button_1" id="btn_download" value="' + GetStr(language, "Download Image File") + '" style="width: 40%;" name="btn_getImageFile" onclick="DownloadImageFile()"> &nbsp; ');
                              document.write('<input type="text" id="text_dnloadimgfilename" style="width: 56%;" placeholder="IMG_523871156_00001.jpg">');

														</script>
														<dt>
															<div class="row">
						                    					<div class="button-row d-flex mt-4 col-12">
						                    						<script>
                                              document.write('<button class="btn btn-primary js-btn-prev" type="button" title="Prev" style="width:58px;height:29px;line-height:15px; font-size: 17px; font-family:  "Calibri Regular";">' + GetStr(language, "Prev") + '</button>');
						                      						</script>
						                    					</div>
						                  					</div>
					                  					<dt>
					                				</div>
					              				</div>
					            			</form>
					          			</div>
					        		</div>
					      		<!/div>
					    	</div>
					  	</div>
			    </div>
			</div>   <!end of setting>
			 
			<div class="picture" id="picture">	
				<script>
          document.write('<input type="button" class="button_2" id="btn_download" value="' + GetStr(language, "Download File") + '" style="width: 30%;" onclick="DownloadFile()"> &nbsp; ');
          document.write('<input type="button" class="button_3" id="btn_download" value="' + GetStr(language, "Download All File") + '" style="width: 30%;" onclick="DownloadAllFile()"> &nbsp; ');
          document.write('<input type="text" id="picidx" class="pic_info" value="">');
          document.write('<input type="text" id="picallcount" class="pic_info" value=""> <br></br>');
          document.write('<input type="button" class="hidden" id="btn_download_hidden" value=""  onclick="DownloadImageFile_hidden()"> ');
          document.write('<input type="text" id="text_dnloadimgfilename_hidden" class="hidden">');


				</script>	
				<!div id="container">
					<div id="skippr0">
					</div>
					
				<!/div>
		    </div>
		</div>
		<h4 align="center" class="button_cmd" onclick="copycmd()">Copyright : (c)2020 Plustek</h4>
		
		

		<script  src="./assets/ppt/script.js"></script>	
		<script>
      window.onload = function () {
        //setting for menu bar
        indexItem = 0;
        active.style.left = `${(indexItem * 115) + 0}px`;
        active.style.background = colors[indexItem];

        active2.style.left = `${(indexItem * 115) + 15}px`;
        active2.style.background = colors[indexItem];
        active2.classList.add("is-item-animated");

        active3.style.left = `${(indexItem * 115) + 85}px`;
        active3.style.background = colors[indexItem];
        active3.classList.add("is-item-animated");


        initbt = document.querySelector("#i-0");
        initbt.children[0].classList.add("is-icon-visible");
        initbt.children[1].classList.add("is-text-visible");
        PageInit();

      };
		</script>
	</body>
	
</html>

