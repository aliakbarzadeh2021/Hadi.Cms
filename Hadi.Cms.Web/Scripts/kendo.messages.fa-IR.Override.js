/*
* Author : Soheila Rasouli
*/
kendo.ui.Locale = "Persian (fa-IR)";

kendo.ui.FilterMenu.prototype.options.operators =
  $.extend(kendo.ui.FilterMenu.prototype.options.operators, {

  	/* FILTER MENU OPERATORS (for each supported data type) 
	 ****************************************************************************/
  	number: {
  		neq: "نا مساوی"
  	},
  	date: {
  		neq: "نا برابر"
  	}
  	/***************************************************************************/
  });

kendo.ui.Pager.prototype.options.messages =
  $.extend(kendo.ui.Pager.prototype.options.messages, {

  	/* PAGER MESSAGES 
	 ****************************************************************************/
  	first: "اولین صفحه",
  	previous: "صفحه قبلی",
  	next: "صفحه بعدی",
  	last: "آخرین صفحه",
  	refresh: "بازخوانی"
  	/***************************************************************************/
  });