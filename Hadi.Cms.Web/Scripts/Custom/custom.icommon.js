Namespace('custom').common = new function () {

	this.loadCss = function (path) {
		var cssId = path;
		if (!document.getElementById(cssId)) {
			var head = document.getElementsByTagName('head')[0];
			var link = document.createElement('link');
			link.id = cssId;
			link.rel = 'stylesheet';
			link.type = 'text/css';
			link.href = path;
			link.media = 'all';
			head.appendChild(link);
		}
	}

	this.loadJs = function (path) {
		var jsId = path;

		if (!document.getElementById(jsId)) {

			var fileref = document.createElement('script');
			fileref.setAttribute("type", "text/javascript");
			fileref.setAttribute("src", path);
			fileref.setAttribute("id", path);
			document.getElementsByTagName("head")[0].appendChild(fileref)
		}
	}

	this.alert = function (title, message) {

		dialogHtml = String.format('<div title="{0}"><p>{1}</p></div>', title, message);
		$(dialogHtml).dialog({
			buttons: [
				{
					text: 'OK',
					click: function () {
						$(this).dialog("close");
						$(this).dialog("destroy");
					}
				},
			],
			modal: true,
			resizable: false
		});

	}

	this.inputBox = function (title, message) {

		dialogHtml = String.format('<div title="{0}"><p>{1}</p></div>', title, message);
		$(dialogHtml).dialog({
			buttons: [
				{
					text: 'OK',
					click: function () {
						$(this).dialog("close");
						$(this).dialog("destroy");
					}
				},
			],
			modal: true,
			resizable: false
		});

	}

	this.confirm = function (title, message, buttons, handler) {

		dialogHtml = String.format('<div title="{0}"><p>{1}</p></div>', title, message);
		$(dialogHtml).dialog({
			buttons: [
				{
					text: 'Yes',
					click: function () {
						$(this).dialog("close");
						$(this).dialog("destroy");
						if (handler)
							handler(true);
					}
				},
				{
					text: 'No',
					click: function () {
						$(this).dialog("close");
						$(this).dialog("destroy");
						if (handler)
							handler(false);
					}
				}
			],
			modal: true,
			resizable: false
		});

	}
};